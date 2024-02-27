using Neo4j.Driver;

namespace ProcureHub_ASP.NET_Core.Services.Interfaces
{
    public interface IConnectionDriver
    {
        public IDriver GetConnection();
    }
}
