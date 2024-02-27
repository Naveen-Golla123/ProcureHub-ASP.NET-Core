using Neo4j.Driver;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Services
{
    public class Neo4jDriver : IConnectionDriver
    {
        private readonly IDriver _driver;

        public Neo4jDriver(IConfiguration configuration) 
        {
            _driver = GraphDatabase.Driver("neo4j://localhost:7687", AuthTokens.Basic("neo4j", "123456789"));
        }

        public IDriver GetConnection()
        {
            return _driver;
        }
    }
}
