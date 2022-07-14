using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Core.Cache
{
    public class HybirdCache : ICache, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(string key)
        {
            throw new NotImplementedException();
        }

        public TValue Get<TValue>(string key)
        {
            throw new NotImplementedException();
        }

        public Task<TValue> GetAsync<TValue>(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public long RemoveAll(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task<long> RemoveAllAsync(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        public bool Set<TValue>(string key, TValue value, TimeSpan? expiry = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAsync<TValue>(string key, TValue value, TimeSpan? expiry = null)
        {
            throw new NotImplementedException();
        }
    }
}
