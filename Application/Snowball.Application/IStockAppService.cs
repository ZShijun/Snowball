using Snowball.Domain.Stock.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Application
{
    public interface IStockAppService
    {
        /// <summary>
        /// 查询所有估值数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IndexValuationDto>> GetAllAsync();
    }
}
