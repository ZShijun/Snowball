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

        public Task<IEnumerable<IndexValuationDto>> GetAllAsync()
        {
            return this._indexValuationService.GetAllAsync();
        }
    }
}
