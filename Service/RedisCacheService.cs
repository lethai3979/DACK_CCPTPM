using StackExchange.Redis;
using System.Text.Json;

namespace GoWheels_WebAPI.Service
{
    public class RedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public List<string> GetAllKeysAsync(string pattern = "*")
        {
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            return server.Keys(pattern: pattern).Select(k => k.ToString()).ToList();
        }

        public async Task SetDataAsync(string key, string value, TimeSpan expiry)
        {
            var db = _redis.GetDatabase();
            var jsonData = JsonSerializer.Serialize(value);
            await db.StringSetAsync(key, jsonData, expiry);
        }

        public async Task<string?> GetDataAsync(string key)
        {
            var db = _redis.GetDatabase();
            var jsonData = await db.StringGetAsync(key);
            return jsonData.IsNullOrEmpty ? default : JsonSerializer.Deserialize<string>(jsonData!);
        }

        public async Task DeleteDataAsync(string key)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
