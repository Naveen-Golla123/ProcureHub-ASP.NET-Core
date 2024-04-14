using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using StackExchange.Redis;
using NReJSON;
using System.Dynamic;

namespace ProcureHub_ASP.NET_Core.Helper
{
    public class RedisConnection : IRedisConnection
    {
        private readonly ConnectionMultiplexer connection;
        public RedisConnection() 
        {
            connection = ConnectionMultiplexer.Connect("localhost");
        }

        public IDatabase GetDatabase()
        {
            return connection.GetDatabase();
        }

        public async Task<string> GetString(string key)
        {
            IDatabase database = GetDatabase();
            return await database.StringGetAsync(key);
        }

        public async Task<bool> SetString<T>(string key, T value)
        {
            IDatabase database = GetDatabase();
            string str = JsonConvert.SerializeObject(value);
            try
            {
                return await database.StringSetAsync(key, str);
            }
            catch
            {
                throw;
            }
            
        }
    }
}
