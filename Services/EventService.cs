using Hangfire;
using Microsoft.AspNetCore.Mvc;
using ProcureHub_ASP.NET_Core.Enums;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using System.Globalization;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;
        private readonly IUserContext context;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILotsService _lotService;
        private readonly ISupplierService _supplierService;
        public EventService(IEventRepository _eventRepository, 
            IUserContext context_, 
            ILotsService lotService,
            ISupplierService supplierService,
            IServiceProvider serviceProvider) 
        { 
            eventRepository = _eventRepository;
            context = context_;
            _lotService = lotService;
            _supplierService = supplierService;
            _serviceProvider = serviceProvider;
        }

        public async Task<Event> CreateEvent(Event _event)
        {
            return await eventRepository.CreateEvent(_event);
        }

        public async Task<List<Event>> GetAllEvents()
        {
            var service = _serviceProvider.GetService<IUserContext>();
            return await eventRepository.GetAllEvents();
        }

        public async Task<Event> GetEventById(int id)
        {
            return await eventRepository.GetEventById(id);
        }

        public async Task<Event> UpdateEvent(Event _event)
        {
            return await eventRepository.UpdateEvent(_event);
        }

        public async Task<bool> AddSuppliers(List<int> supplierIds, int eventId)
        {
            return await eventRepository.AddSuppliers(supplierIds, eventId);
        }

        public async Task<SubmitAuctionResponse> SubmitAuction(int eventId)
        {
            var submitAuction = new SubmitAuctionResponse
            {
                errors = new List<string>(),
                isSubmitted = true,
            };

            try
            {
               var service = _serviceProvider.GetService<ILiveService>();
                Event event_ = await eventRepository.GetEventById(eventId);
                event_.lots = await _lotService.GetAllLots(eventId);
                event_.suppliers = await _supplierService.GetAddedSuppliers(eventId);
                if (event_ != null)
                {
                    validateAuctionSchedule(event_, ref submitAuction);

                    if (event_.lots != null && event_.lots.Count > 0)
                    {

                        foreach (var lot in event_.lots)
                        {
                            if (lot.has_item == null || lot.has_item.Count <= 0)
                            {
                                submitAuction.errors.Add("Add atleast Item to Lot " + lot.name);
                            }
                        }
                    } 
                    else
                    {
                        submitAuction.errors.Add("Add atleast one lot to submit the Auction");
                    }

                    if(event_.suppliers == null || event_.suppliers.Count == 0)
                    {
                        submitAuction.errors.Add("Invite atlease one supplier to the Auction");
                    }
                }
                else
                {
                    submitAuction.errors.Add("Issue in submitting Auction");
                }

                if(submitAuction.errors.Count > 0)
                {
                    submitAuction.isSubmitted = false;
                }
                else
                {
                    var response = await ChangeAuctionStatus(eventId,EventStatus.Invited);
                    string oneMinuteEarlier = GetOneMiniuteEarlyTime(event_.Starttime);
                    var selectedStartDate = DateTimeOffset.Parse(event_.Startdate + " " + event_.Starttime + ":00 -7:00");
                    var earlyStartTimeTime = DateTimeOffset.Parse(event_.Startdate + " " + oneMinuteEarlier + ":00 -7:00");

                    // Jobs to Start the Auction
                    BackgroundJob.Schedule(() => this.eventRepository.ChangeAuctionStatus(event_.id, EventStatus.Live), selectedStartDate);
                    BackgroundJob.Schedule(() => service.LoadData(eventId), earlyStartTimeTime);

                    // Jobs to End the Auction
                    var selectedEndDate = DateTimeOffset.Parse(event_.Enddate + " " + event_.Endtime + ":00 -7:00");
                    BackgroundJob.Schedule(() => this.eventRepository.ChangeAuctionStatus(event_.id, EventStatus.Completed), selectedEndDate);
                    submitAuction.isSubmitted = true;
                }
                
            }
            catch
            {
                throw;
            }
            return submitAuction;
        }

        private void validateAuctionSchedule(Event event_, ref SubmitAuctionResponse submitAuction)
        {
            DateTime startDate = DateTime.ParseExact(event_.Startdate + " " + event_.Starttime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            DateTime enddate = DateTime.ParseExact(event_.Enddate + " " + event_.Endtime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            if(startDate >= enddate)
            {
                submitAuction.errors.Append("Auction Start date and time should be later to end time and date");
            }
        }

        private string GetOneMiniuteEarlyTime(string time)
        {
            int hours = Int32.Parse(time.Substring(0, 2));
            int mins = Int32.Parse(time.Substring(3));

            if (mins == 0)
            {
                hours = hours - 1;
                mins = 59;
            }
            else
            {
                mins = mins - 1;
            }
            string updatedTime = hours.ToString() + ":" + (mins < 10 ? "0" : "") + mins.ToString();
            return updatedTime;
        }

        public async Task<bool> ChangeAuctionStatus(int eventId, EventStatus eventStatus)
        {
            return await eventRepository.ChangeAuctionStatus(eventId,eventStatus);
        }

        public async Task<List<SupplierEvent>> GetInvitedEvents()
        {
            return await eventRepository.GetInvitedEvents();
        }

        public async Task<bool> TestHangfire()
        {
            // test hang fire here.
            var backgroudClient = new BackgroundJobClient();

            var selecteDate = DateTimeOffset.Parse("2024-03-22 09:08:00");
            BackgroundJob.Schedule(() => this.eventRepository.ChangeAuctionStatus(127, EventStatus.Completed), selecteDate);
            //await this.eventRepository.ChangeAuctionStatus(127, EventStatus.Completed);
            return true;
        }

        public async Task<int> AcceptEvent(int eventId)
        {
            return await this.eventRepository.AcceptEvent(eventId);
        }

        public async Task<int> RejectEvent(int eventId)
        {
            return await this.eventRepository.RejectEvent(eventId);
        }
    }
}
