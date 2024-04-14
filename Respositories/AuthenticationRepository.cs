using Neo4j.Driver;
using ProcureHub_ASP.NET_Core.Models;
using ProcureHub_ASP.NET_Core.Respositories.Interfaces;
using ProcureHub_ASP.NET_Core.Services.Interfaces;
using System.Text;
using System.Xml.Linq;

namespace ProcureHub_ASP.NET_Core.Respositories
{
    public class AuthenticationRepository: IAuthenticationRepository
    {

        private readonly IConnectionDriver _connectionDriver;

        public AuthenticationRepository(IConnectionDriver driver) 
        {
            _connectionDriver = driver;
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            IDriver driver = _connectionDriver.GetConnection();
            var session = driver.AsyncSession();
            try
            {         
                var tx = await session.BeginTransactionAsync();
                var reader = await tx.RunAsync("MATCH (n:user{email: $email}) RETURN n", new { email = email });
                var results = await reader.FetchAsync();
                return results;
            }
            catch (Exception ex) 
            {
                throw;
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task<bool> RegisterUser(UserDetails userDetails)
        {
            IDriver connection = _connectionDriver.GetConnection();
            var session = connection.AsyncSession();
            try
            {
                var statementText = new StringBuilder();
                string query = "CREATE (person:user {name: $name, email: $email, password: $password, hashSalt: $hashSalt, mobile: $mobile, isApproved: true, isSupplier: false, isAdmin: true})";
                var statementParameters = new Dictionary<string, object>
                {
                    { "name", userDetails.name },
                    { "email", userDetails.email},
                    { "password", userDetails.hashedPassword},
                    { "hashSalt", userDetails.hashSalt },
                    { "mobile", userDetails.mobile },
                    { "isApproved", userDetails.isApproved },
                    { "isAdmin", userDetails.isAdmin },
                    { "isBuyer", userDetails.isBuyer }
                };
                var tx = await session.ExecuteWriteAsync(tx => tx.RunAsync(query, statementParameters));
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserDetails> GetUserDetails(string email)
        {
            IDriver driver = _connectionDriver.GetConnection();
            var session = driver.AsyncSession();
            UserDetails userDetails = new UserDetails();
            try
            {
                var statementText = new StringBuilder();
                string query = "MATCH(n:user {email: $email}) return n";
                Dictionary<string, Object> statementParameters = new Dictionary<string, Object>
                {
                    {"email", email }
                };
                var tx = await session.BeginTransactionAsync();
                var reader = await tx.RunAsync(query, statementParameters);

                while(await reader.FetchAsync())
                {
                    var node = reader.Current["n"].As<INode>();
                    userDetails.email = node.Properties["email"].As<string>();
                    userDetails.mobile = node.Properties["mobile"].As<string>();
                    userDetails.hashedPassword = node.Properties["password"].As<byte[]>();
                    userDetails.hashSalt = node.Properties["hashSalt"].As<byte[]>();
                    userDetails.isSupplier = node.Properties["isSupplier"].As<bool>();
                    userDetails.isApproved = node.Properties["isApproved"].As<bool>();
                    userDetails.isAdmin = node.Properties["isAdmin"].As<bool>();
                    userDetails.name = node.Properties["name"].As<string>();
                    userDetails.id = (int)node.Id;
                }
                return userDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<string> SignIn(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }

     //while (await reader.FetchAsync())
     //           // Each current read in buffer can be reached via Current
     //       {
     //           products.Add(reader.Current[0].ToString());
     //       }
}
