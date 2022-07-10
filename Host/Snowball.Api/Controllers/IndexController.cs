using Microsoft.AspNetCore.Mvc;
using Snowball.Application;
using Snowball.Application.Dtos;
using Snowball.Domain.Stock;
using System.Threading.Tasks;

namespace Snowball.Api.Controllers
{
    /// <summary>
    /// 指数
    /// </summary>
    [ApiController]
    [Route("v1/api/[controller]")]
    public class IndexController : ControllerBase
    {
        private readonly IStockAppService _stockAppService;

        public IndexController(IStockAppService stockAppService)
        {
            this._stockAppService = stockAppService;
        }

        /// <summary>
        /// 查询所有估值数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IndexValuationOutputDto>> GetAllAsync()
        {
            var output = await this._stockAppService.GetAllAsync();
            return Ok(output);
        }
    }
}
