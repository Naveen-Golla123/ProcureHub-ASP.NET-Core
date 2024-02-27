using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace ProcureHub_ASP.NET_Core.Respositories
{
    public class EventRepository : IEventRepository
    {
        private readonly IConnectionDriver driver;
        public EventRepository(IConnectionDriver _driver)
        {
            driver = _driver;
        }

        public async Task<bool> SaveEvent(Event _event)
        {
            IDriver connection = driver.GetConnection();
            var session = connection.AsyncSession();
            try
            {
                string query = "MATCH(creator:user{email:'naveeng@csu.fullerton.edu'}) CREATE(event:Event{name:$eventName, description: $eventDescription })-[r:CREATED_BY]->(creator) return creator, r, event";
                var statementParameters = new Dictionary<string, object>
                {
                    { "eventName" , "naveeng@csu.fullerton.edu" }, {"eventDescription", "event description"}
                };
                var tx = await session.ExecuteWriteAsync(tx => tx.RunAsync(query, statementParameters));
                while (await tx.FetchAsync())
                {

                }
                
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
