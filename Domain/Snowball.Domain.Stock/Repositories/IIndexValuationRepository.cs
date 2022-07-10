using Snowball.Domain.Stock.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Domain.Stock.Repositories
{
    public interface IIndexValuationRepository
    {
        Task<bool> InsertAsync(IndexValuationEntity entity);

        Task<bool> UpdateAsync(IndexValuationEntity entity);

        /// <summary>
        /// 查询所有估值数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<IndexValuationEntity>> GetAllAsync();
    }
}
