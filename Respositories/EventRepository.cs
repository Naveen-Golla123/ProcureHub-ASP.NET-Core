using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Neo4j.Driver;
using ProcureHub_ASP.NET_Core.Enums;
using ProcureHub_ASP.NET_Core.Helper;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace ProcureHub_ASP.NET_Core.Respositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IConnectionDriver driver;
        private IUserContext context_;
        private readonly IServiceProvider _serviceProvider;

        public EventRepository(IConnectionDriver _driver, IServiceProvider serviceProvider)
        {
            driver = _driver;
            _serviceProvider = serviceProvider;
        }

        public async Task<Event> UpdateEvent(Event _event)
        {
            IDriver connection = driver.GetConnection();
            var session = connection.AsyncSession();
            try
            {
                string query = "MATCH (event:Event) WHERE ID(event) = $id SET event.description = $description, event.statusCode = $statusCode, event.startTime = $startTime, event.endTime= $endTime, event.startDate = $startDate, event.endDate = $endDate return {id: ID(event), name: event.name, description: event.description, statusCode:event.statusCode, Startdate: event.startDate , Starttime: event.startTime , Enddate: event.endDate , Endtime: event.endTime  } as event";
                var statementParameters = new Dictionary<string, object>
                {
                    {"id", _event.id }, { "eventName" , _event.name }, {"description", _event.description}, {"statusCode", (int) EventStatus.Draft}, {"startDate" , _event.Startdate}, { "endDate" , _event.Enddate}, { "endTime" , _event.Endtime}, {"startTime", _event.Starttime}
            };
                return await session.ExecuteWriteAsync(

                    async tx =>
                {
                    var reader = await tx.RunAsync(query, statementParameters);
                    Event createdEvent = new Event();
                    
                    if(await reader.FetchAsync()){
                        createdEvent = await this.GetEventById(_event.id);
                    }
                    
                    return createdEvent;
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Event> CreateEvent(Event _event)
        {
            IDriver connection = driver.GetConnection();
            var session = connection.AsyncSession();
            try
            {
                string query = "MATCH(creator:user{email:'naveeng@csu.fullerton.edu'}) CREATE(event:Event{name:$eventName, description: $eventDescription, statusCode: $statusCode, startTime : $startTime, endTime: $endTime, startDate : $startDate, endDate : $endDate })-[r:CREATED_BY]->(creator) return creator, r, event";
                var statementParameters = new Dictionary<string, object>
                {
                    { "eventName" , _event.name }, {"eventDescription", "event description"}, {"statusCode", (int) EventStatus.Draft}, {"startDate" , _event.Startdate}, { "endDate" , _event.Enddate}, { "endTime" , _event.Endtime}, {"startTime", _event.Startdate}
            };
                return await session.ExecuteWriteAsync(async tx => 
                { 
                    var reader = await tx.RunAsync(query, statementParameters);
                    Event createdEvent = new Event();
                    while (await reader.FetchAsync())
                    {
                        var node = reader.Current["event"].As<INode>();
                        createdEvent.name = node.Properties["name"].As<string>();
                        createdEvent.statusCode = (EventStatus)node.Properties["statusCode"].As<int>();
                        createdEvent.Startdate = node.Properties["startDate"].As<string>();
                        createdEvent.Starttime = node.Properties["startTime"].As<string>();
                        createdEvent.Enddate = node.Properties["endDate"].As<string>();
                        createdEvent.Endtime = node.Properties["endTime"].As<string>();
                        createdEvent.id = (int)node.Id;
                    }
                    return createdEvent;
                });                
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Event>> GetAllEvents()
        {
            var userContext = _serviceProvider.GetService<IUserContext>();
            var email = userContext.GetEmail();

            IDriver connection = driver.GetConnection();
            var session = connection.AsyncSession();
            List<Event> events = new List<Event>();
            try
            {
                var query = "match (event:Event)-[r:CREATED_BY]-(n:user{email:$email})  return event";
                var reader = await session.RunAsync(query, new
                {
                   email = email
                });
                
                while(await reader.FetchAsync())
                {
                    Event ev_ = new Event();
                    var node = reader.Current["event"].As<INode>();
                    ev_.name = node.Properties["name"].As<string>();
                    ev_.statusCode = (EventStatus)node.Properties["statusCode"].As<int>();
                    ev_.Startdate = node.Properties.ContainsKey("startDate") ? node.Properties["startDate"].As<string>() : null;
                    ev_.Starttime = node.Properties.ContainsKey("startTime") ? node.Properties["startTime"].As<string>() : null;
                    ev_.Enddate = node.Properties.ContainsKey("endDate") ? node.Properties["endDate"].As<string>() : null;
                    ev_.Endtime = node.Properties.ContainsKey("endTime") ? node.Properties["endTime"].As<string>() : null;
                    ev_.id = (int)node.Id;
                    events.Add(ev_);
                }
            }
            catch
            {
                throw;
            }

            return events; 
        }

        public async Task<Event> GetEventById(int id)
        {
            // DB Query
            IDriver connection = driver.GetConnection();
            var session = connection.AsyncSession();
            try
            {
                string query = "match (e:Event) where id(e) = $eventId return {id: ID(e), name: e.name, description: e.description, statusCode:e.statusCode, Startdate: e.startDate , Starttime: e.startTime , Enddate: e.endDate , Endtime: e.endTime  } as event";
                var tx = await session.RunAsync(query, new
                {
                    eventId = id
                });
                Event event_ = new Event();
                while(await  tx.FetchAsync())
                {
                    var node = tx.Current;
                    var temp = JsonSerializer.Serialize(node.Values["event"]);
                    event_ = JsonSerializer.Deserialize<Event>(temp);
                    //event_.name = node.Properties["name"].As<string>();
                    //event_.description = node.Properties["description"].As<string>();
                    //event_.statusCode = (EventStatus)node.Properties["statusCode"].As<int>();
                }
                return event_;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddSuppliers(List<int> supplierIds, int eventId)
        {
            IDriver connection = driver.GetConnection();
            var session = connection.AsyncSession();
            try
            {
                string query = "UNWIND $supplierList as supplierId MATCH (supplier:user) where ID(supplier) = supplierId match (event:Event)  where  Id(event) = $eventId with supplier, event merge (supplier)-[r:ADDED_TO]->(event) return supplier, event";
                return await session.ExecuteWriteAsync(async tx =>
                {
                    var reader = await tx.RunAsync(query, new
                    {
                        supplierList = supplierIds,
                        eventId = eventId
                    });

                    if(await reader.FetchAsync())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
            }
            catch
            {
                throw;
            }
        }


        public async Task<Event> GetEventInfo(int eventId)
        {
            IDriver connection = driver.GetConnection();
            var session = connection.AsyncSession();
            try
            {
                Event event_ = new Event();
                string query = "MATCH path = (e)-[*0..]->(lot) where ID(e) = $eventId WITH e,path MATCH (u:user)-[r:ADDED_TO]->(e) WITH collect(path) as paths,e,u CALL apoc.convert.toTree(paths) yield value WITH value,e,collect({id:Id(u), name: u.name, email:u.email}) as us RETURN { id: ID(e),name: e.name,description: e.description,Startdate: e.startDate,Enddate: e.endDate,Starttime: e.startTime, Endtime: e.endTime,Lots: value.has_lot,suppliers: us} as result;";
                return await session.ExecuteWriteAsync(async tx =>
                {
                    var reader = await tx.RunAsync(query, new
                    {
                        eventId = eventId
                    });

                    while(await reader.FetchAsync())
                    {
                        var node_ = reader.Current;
                        string temp = JsonSerializer.Serialize(node_.Values["result"]);
                        event_ = JsonSerializer.Deserialize<Event>(temp);
                    }
                    return event_; 
                });
            }
            catch 
            {
                throw;
            }
        }

        public async Task<bool> ChangeAuctionStatus(int eventId, EventStatus eventStatus)
        {
            IDriver connection = driver.GetConnection();
            var session = connection.AsyncSession();
            try
            {
                string query = "MATCH (e:Event) where ID(e)=$eventId SET e.statusCode = $eventStatus return e";
                return await session.ExecuteWriteAsync(async tx =>
                {
                    var reader = await tx.RunAsync(query, new
                    {
                        eventId = eventId,
                        eventStatus = (int)eventStatus
                    });
                    var result = await reader.FetchAsync();
                    return result;
                });

            }
            catch
            {
                throw;
            }
        }
    }
}


//Match(n: user{ email: "danala@gmail.com"})
//MERGE(event:Event{ name: "event3"})- [r:CREATED_BY]->(n) set event.description = "event description" return event
