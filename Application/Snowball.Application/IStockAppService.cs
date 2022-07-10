using Snowball.Application.Dtos;
using System.Threading.Tasks;

namespace Snowball.Application
{
    public interface IStockAppService
    {
        /// <summary>
        /// 查询所有估值数据
        /// </summary>
        /// <returns></returns>
        Task<IndexValuationOutputDto> GetAllAsync();
    }
}
