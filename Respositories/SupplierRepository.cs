using Neo4j.Driver;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;

namespace ProcureHub_ASP.NET_Core.Respositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly IConnectionDriver _connectionDriver;
        public SupplierRepository(IConnectionDriver _driver) 
        {
            _connectionDriver = _driver;
        }

        public Task<bool> AddSuppliersToEvent()
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAddedSuppliers(int eventId)
        {
            IDriver driver = _connectionDriver.GetConnection();

            IAsyncSession session = driver.AsyncSession();
            List<User> suppliers = new List<User>();
            try
            {
                string query = "MATCH (u:user)-[r:ADDED_TO]->(event:Event) where ID(event) = $eventId return u";
                var tx = await session.BeginTransactionAsync();
                var reader = await tx.RunAsync(query, new
                {
                    eventId = eventId
                });

                while (await reader.FetchAsync())
                {
                    INode node = reader.Current["u"].As<INode>();
                    User user = new User();
                    user.id = (int)node.Id;
                    user.name = node.Properties["name"].As<string>();
                    user.email = node.Properties["email"].As<string>();
                    user.isAdmin = node.Properties["isAdmin"].As<bool>();
                    user.isBuyer = !(node.Properties["isSupplier"].As<bool>());
                    user.isAdmin = node.Properties["isAdmin"].As<bool>();
                    user.mobile = node.Properties["mobile"].As<string>();
                    suppliers.Add(user);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return suppliers;
        }

        public async Task<List<User>> GetSuppliers()
        {
            IDriver driver = _connectionDriver.GetConnection();
            IAsyncSession session = driver.AsyncSession();
            List<User> suppliers = new List<User>();
            try
            {
                string query = "MATCH (n:user { isSupplier : true }) RETURN n";
                var tx = await session.BeginTransactionAsync();
                var reader = await tx.RunAsync(query);

                while(await reader.FetchAsync())
                {
                    INode node = reader.Current["n"].As<INode>();
                    User user = new User();
                    user.id = (int)node.Id;
                    user.name = node.Properties["name"].As<string>();
                    user.email = node.Properties["email"].As<string>();
                    user.isAdmin = node.Properties["isAdmin"].As<bool>();
                    user.isBuyer = !(node.Properties["isSupplier"].As<bool>());
                    user.isAdmin = node.Properties["isAdmin"].As<bool>();
                    user.mobile = node.Properties["mobile"].As<string>();
                    suppliers.Add(user);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return suppliers;
        }

        public Task<bool> RemoveSupplierFromEvent()
        {
            throw new NotImplementedException();
        }

        public Task<bool> TriggerInviteToSupplier()
        {
            throw new NotImplementedException();
        }
    }
}
