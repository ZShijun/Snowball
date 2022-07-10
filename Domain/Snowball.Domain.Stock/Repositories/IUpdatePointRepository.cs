using Snowball.Domain.Stock.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Domain.Stock.Repositories
{
    public interface IUpdatePointRepository
    {
        Task<UpdatePointEntity> GetAsync(string key);

        Task<IEnumerable<UpdatePointEntity>> GetAllAsync();

        Task<bool> UpdateAsync(UpdatePointEntity entity);
    }
}
