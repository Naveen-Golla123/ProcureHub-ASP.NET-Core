using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using Microsoft.OpenApi.Any;
using Newtonsoft.Json;
using ProcureHub_ASP.NET_Core.Filters;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using StackExchange.Redis;

namespace ProcureHub_ASP.NET_Core.SignalR
{
    [ExtractInfoFilter]
    public class AuctionHub : Hub
    {
        private readonly IRedisConnection _connection;
        private readonly ILiveService _liveService;
        private readonly IServiceProvider serviceProvider;
        private readonly IChatService _chatService;
        public AuctionHub(IRedisConnection _redisConnection, ILiveService liveService, IServiceProvider _serviceProvider, IChatService chatService) 
        {
            _connection = _redisConnection;
            _liveService = liveService;
            serviceProvider = _serviceProvider;
            _chatService = chatService;
        }

        public async Task<Task> BidPlaced(SupplierLots supplierLot)
        {
            Eventdetails eventDetails_ = await _liveService.PlaceBid(supplierLot);
            return Clients.Group("event_" + supplierLot.EventId.ToString()).SendAsync("BidPlaced", eventDetails_);
        }


        // This method is for debugging the connection.
        public Task Send(string message)
        {
            return Clients.All.SendAsync("Send", "hello");
        }

        public async Task AcknowledgeConnection(ConnectionParams params_)
        {
            var httpCtx = Context.GetHttpContext();
            var headers = httpCtx.Request.Headers;
            params_.connectionId = Context.ConnectionId;
            AddToGroups(params_);
            _ = Clients.Groups("event_" + params_.EventId.ToString() + "_buyers").SendAsync("connections", await AddToActiveUsers(params_));
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            
            IDatabase database = _connection.GetDatabase();
            var httpCtx = Context.GetHttpContext();
            var eventId = httpCtx!.Request!.Query["EventId"];
            var result = await database.StringGetAsync(eventId.ToString() + "_connectedUsers");
            var _connections = JsonConvert.DeserializeObject<List<SignalRConnection>>(result);
            var item = _connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                _connections.Remove(item);
            }
            _ = _connection.SetString(eventId.ToString() + "_connectedUsers", _connections);
            _ = Clients.Groups("event_" + eventId.ToString() + "_buyers").SendAsync("connections", _connections);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<SignalRConnection>> GetOnlineUsers(string eventId)
        {
            IDatabase database = _connection.GetDatabase();
            var result = await database.StringGetAsync(eventId.ToString() + "_connectedUsers");
            var _connections = JsonConvert.DeserializeObject<List<SignalRConnection>>(result);
            return _connections;
        }

        // Chat Functionality
        [ExtractInfoFilter]
        public async Task<Message> SendMessage(Message message)
        {
            IUserContext userContext = serviceProvider.GetService<IUserContext>();
            var headers= Context.GetHttpContext().Request.Headers;
            var userId =message.SentBy;

            Message message_ = await _chatService.SendMessage(message);
            _ = Clients.Groups("event_" + message.EventId.ToString() + "_" + message.SentTo).SendAsync("recieveMessage", message_);
            return message_;
        }

        public async Task BroadCastMessage()
        {

        }

        private async void AddToGroups(ConnectionParams params_)
        {
            IUserContext userContext = serviceProvider.GetService<IUserContext>();
            var userId = params_.UserId;
            bool isSupplier = userContext.GetIsSupplier();
            // Event Level Group
            await Groups.AddToGroupAsync(params_.connectionId, "event_" + params_.EventId.ToString());
            //Individual Group
            await Groups.AddToGroupAsync(params_.connectionId, "event_" + params_.EventId.ToString() + "_" + userId);
            
            if (isSupplier)
            {
                //Supplier Group
                await Groups.AddToGroupAsync(params_.connectionId, "event_" + params_.EventId.ToString() + "_suppliers");
            }
            else
            {
                // Buyer Group 
                await Groups.AddToGroupAsync(params_.connectionId, "event_" + params_.EventId.ToString() + "_buyers");
            }
        }

        private async Task<List<SignalRConnection>> AddToActiveUsers(ConnectionParams params_)
        {
            IDatabase database = _connection.GetDatabase();
            var result = await database.StringGetAsync(params_.EventId.ToString() + "_connectedUsers");
            List<SignalRConnection> _connections;
            if (result.IsNullOrEmpty)
            {
                _connections = new List<SignalRConnection>();
            }
            else
            {
                _connections = JsonConvert.DeserializeObject<List<SignalRConnection>>(result);
            }
            
            if (_connections!.Count(x => x.UserId == params_.UserId) == 0)
            {
                _connections!.Add(new SignalRConnection { ConnectionId = params_.connectionId, UserId = params_.UserId, eventId = params_.EventId });
            }
            _ = _connection.SetString(params_.EventId.ToString() + "_connectedUsers", _connections);
            return _connections;
        }
    }
}
