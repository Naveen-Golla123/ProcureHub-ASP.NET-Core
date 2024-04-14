using StackExchange.Redis;

namespace ProcureHub_ASP.NET_Core.Helper
{
    public interface IRedisConnection
    {
        public Task<string> GetString(string key);
        public Task<bool> SetString<T>(string key, T value);
        public IDatabase GetDatabase();
    }
}
