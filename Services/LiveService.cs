using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NReJSON;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using StackExchange.Redis;
using System;
using System.Runtime.InteropServices;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class LiveService : ILiveService
    {
        private readonly IRedisConnection _redisConnection;
        private readonly IEventService _eventService;
        private readonly ISupplierService _supplierService;
        private readonly ILotsService _lotsService;

        public LiveService(IRedisConnection redisConnection, IEventService eventService, ISupplierService supplierService, ILotsService lotService) 
        {
            _redisConnection = redisConnection;
            _eventService = eventService;
            _supplierService = supplierService;
            _lotsService = lotService;
        }

        public async Task<bool> LoadData(int eventId)
        {
            // call redis call
            Event event_ = await _eventService.GetEventById(eventId);
            List<Lot> lots = await _lotsService.GetAllLots(eventId);
            List<User> suppliers =  await _supplierService.GetAddedSuppliers(eventId);

            Dictionary<string, Lot> lotsDic = new Dictionary<string, Lot>();
            Dictionary<string, SupplierLots> supplierDic = new Dictionary<string, SupplierLots>();

            float TotalBid = 0;

            foreach (Lot lot in lots)
            {
                TotalBid += lot.TotalPrice;
                lotsDic.Add(lot._id.ToString(), lot);
            }

            Dictionary<string,RankDetails> ranks = new Dictionary<string, RankDetails>();

            foreach(User user in suppliers)
            {
                SupplierLots supplierLot = new SupplierLots()
                {
                    id = user.id,
                    name= user.name,
                    email = user.email,
                    isAdmin = user.isAdmin,
                    isSupplier = user.isSupplier,
                    isApproved = user.isApproved,
                    mobile = user.mobile
                };
                supplierLot.supplierLots = lotsDic;
                supplierLot.CurrentBid = TotalBid;
                supplierLot.EventId = eventId;
                ranks.Add(supplierLot.id.ToString(), new RankDetails
                {
                    Rank = 1,
                    TotalBid = TotalBid,
                    Supplier = user,
                });
                supplierLot.BidTracker = new List<Bid>
                {
                    new Bid()
                    {
                        BidAmount = TotalBid,
                        UserId = supplierLot.id,
                        SupplierName = supplierLot.name,
                        BidTime = new DateTime()
                    }
                };
                supplierDic.Add(user.id.ToString(), supplierLot);
            }

            Eventdetails eventDetails = new Eventdetails()
            {
                EventInfo = event_,
                Lots = lotsDic,
                Suppliers = supplierDic,
                Ranks = ranks,
                TopBiders = new List<Bid>()
            };

            try
            {
                //IDatabase database = _redisConnection.GetDatabase();
                await _redisConnection.SetString<Eventdetails>(eventId.ToString(), eventDetails);
                await _redisConnection.SetString(eventId.ToString() + "_connectedUsers", new List<SignalRConnection>());
            }
            catch
            {
                throw;
            }
            
            //await _redisConnection.SetString<Eventdetails>(eventId.ToString(), eventDetails);
            return true;
        }

        public async Task<Eventdetails> GetSupplierDashBoardData(int eventId)
        {
            try
            {
                IDatabase database = _redisConnection.GetDatabase();
                string[] params_ = { "."};
                var result = await database.StringGetAsync(eventId.ToString());
                var eventDetails = JsonConvert.DeserializeObject<Eventdetails>(result);
                return eventDetails;
            } catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<Eventdetails> PlaceBid(SupplierLots supplierLot)
        {
            IDatabase database = _redisConnection.GetDatabase();
            string[] params_ = { "." };
            var result = await database.StringGetAsync(supplierLot.EventId.ToString());
            var eventDetails = JsonConvert.DeserializeObject<Eventdetails>(result);
            supplierLot.BidTracker.Add(new Bid()
            {
                UserId = supplierLot.id,
                BidAmount = supplierLot.CurrentBid,
                BidTime = new DateTime()
            });
            string bestBider = "";
            eventDetails.Suppliers[supplierLot.id.ToString()] = supplierLot;
            eventDetails.Ranks = eventDetails.Suppliers.OrderBy(obj => obj.Value.CurrentBid).Select((result,index) => {
                bestBider = index == 0 ? result.Value.id.ToString() : bestBider;
                return new
                {
                    Key = result.Key,
                    Value = new RankDetails
                    {
                        Rank = index + 1,
                        Supplier = result.Value,
                        TotalBid = result.Value.CurrentBid
                    }
                };
            }).ToDictionary(obj=>obj.Key, obj=>obj.Value);
            eventDetails.TopBiders.Add(new Bid
            {
                BidAmount = eventDetails.Ranks[bestBider].TotalBid,
                BidTime = DateTime.Now,
                SupplierName = eventDetails.Suppliers[bestBider].name,
                UserId = eventDetails.Suppliers[bestBider].id,
            });
            //eventDetails
            await _redisConnection.SetString(supplierLot.EventId.ToString(), eventDetails);
            return eventDetails;
        }
    }
}