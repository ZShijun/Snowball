using Snowball.Domain.Stock.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Snowball.Domain.Stock
{
    public interface IIndexValuationService
    {
        /// <summary>
        /// 查询所有估值数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IndexValuationDto>> GetAllAsync();
    }
}
