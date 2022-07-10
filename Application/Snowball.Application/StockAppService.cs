using Snowball.Application.Dtos;
using Snowball.Domain.Stock;
using Snowball.Domain.Stock.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Application
{
    public class StockAppService : IStockAppService
    {
        private readonly IIndexValuationService _indexValuationService;

        public StockAppService(IIndexValuationService indexValuationService)
        {
            this._indexValuationService = indexValuationService;
        }

        public async Task<IndexValuationOutputDto> GetAllAsync()
        {
            var lastUpdateTime = await this._indexValuationService.GetLastUpdateTimeAsync();
            var valuations = await this._indexValuationService.GetAllAsync();
            return new IndexValuationOutputDto
            {
                LastUpdateTime = lastUpdateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Valuations = valuations
            };
        }
    }
}
