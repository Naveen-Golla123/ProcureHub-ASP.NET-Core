using Neo4j.Driver;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class Neo4jDriver : IConnectionDriver
    {
        private readonly IDriver _driver;

        public Neo4jDriver(IConfiguration configuration) 
        {
            _driver = GraphDatabase.Driver("neo4j://54.90.168.10:7687", AuthTokens.Basic("neo4j", "Golla@189"));
        }

        public IDriver GetConnection()
        {
            return _driver;
        }
    }
}
