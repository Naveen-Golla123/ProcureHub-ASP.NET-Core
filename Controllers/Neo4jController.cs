using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4j.Driver;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using System.Text;

namespace ProcureHub_ASP.NET_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Neo4jController : ControllerBase
    {
        private readonly IDriver _driver;

        public Neo4jController(IConnectionDriver connection)
        {
            _driver = connection.GetConnection();
            //_driver = GraphDatabase.Driver("neo4j://localhost:7687", AuthTokens.Basic("neo4j", "123456789"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateNode(string name)
        {
            var statementText = new StringBuilder();
            statementText.Append("CREATE (person:Person {name: $name})");
            var statementParameters = new Dictionary<string, object>
        {
            {"name", name }
        };

            var session = this._driver.AsyncSession();
            var result = await session.WriteTransactionAsync(tx => tx.RunAsync(statementText.ToString(), statementParameters));
            return StatusCode(201, "Node has been created in the database");
        }
    }
}
