using Snowball.Application.Dtos;
using Snowball.Core;
using Snowball.Core.Cache;
using Snowball.Domain.Stock;
using System;
using System.Threading.Tasks;

namespace Snowball.Application
{
    public class StockAppService : IStockAppService
    {
        private readonly IIndexValuationService _indexValuationService;
        private readonly ICache _cache;

        public StockAppService(IIndexValuationService indexValuationService,
                               ICache cache)
        {
            this._indexValuationService = indexValuationService;
            this._cache = cache;
        }

        public async Task<IndexValuationOutputDto> GetAllAsync()
        {
            var lastUpdateTime = await this._cache.GetAsync<DateTime>(GlobalConstant.IndexValuationUpdateTime);
            var valuations = await this._indexValuationService.GetAllAsync();
            return new IndexValuationOutputDto
            {
                LastUpdateTime = lastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Valuations = valuations
            };
        }
    }
}
