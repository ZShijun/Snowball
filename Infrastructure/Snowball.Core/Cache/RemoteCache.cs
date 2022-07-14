using Snowball.Core.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Snowball.Core.Cache
{
    internal class RemoteCache : ICache, IDisposable
    {
        private readonly IDatabase _db;
        private readonly ConnectionMultiplexer _connector;

        internal RemoteCache(string host)
        {
            this._connector = ConnectionMultiplexer.Connect(host);
            this._db = this._connector.GetDatabase();
        }

        public bool Exists(string key)
        {
            return this._db.KeyExists(key);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return this._db.KeyExistsAsync(key);
        }

        public bool Remove(string key)
        {
            return this._db.KeyDelete(key);
        }

        public Task<bool> RemoveAsync(string key)
        {
            return this._db.KeyDeleteAsync(key);
        }

        public long RemoveAll(IEnumerable<string> keys)
        {
            var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
            return this._db.KeyDelete(redisKeys);
        }

        public Task<long> RemoveAllAsync(IEnumerable<string> keys)
        {
            var redisKeys = keys.Select(x => (RedisKey)x).ToArray();
            return this._db.KeyDeleteAsync(redisKeys);
        }


        public bool Set<TValue>(string key, TValue value, TimeSpan? expiry = null)
        {
            return this._db.StringSet(key, JsonUtil.Serialize(value), expiry);
        }

        public Task<bool> SetAsync<TValue>(string key, TValue value, TimeSpan? expiry = null)
        {
            return this._db.StringSetAsync(key, JsonUtil.Serialize(value), expiry);
        }

        public TValue Get<TValue>(string key)
        {
           var value = this._db.StringGet(key);
            return JsonUtil.ToObject<TValue>(value);
        }

        public async Task<TValue> GetAsync<TValue>(string key)
        {
            var value = await this._db.StringGetAsync(key);
            return JsonUtil.ToObject<TValue>(value);
        }

        public void Dispose()
        {
            var endPoints = this._db.Multiplexer.GetEndPoints();
            foreach (EndPoint endpoint in endPoints)
            {
                var server = this._db.Multiplexer.GetServer(endpoint);
                server.FlushDatabase();
            }

            this._connector.Close();
        }

    }
}
