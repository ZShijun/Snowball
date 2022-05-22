using Microsoft.AspNetCore.Mvc;
using Snowball.Domain.Stock;
using Snowball.Domain.Stock.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IIndexValuationService _indexValuationService;

        public IndexController(IIndexValuationService indexValuationService)
        {
            this._indexValuationService = indexValuationService;
        }

        /// <summary>
        /// 查询所有估值数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IndexValuationDto>>> GetAllAsync()
        {
            var dtos = await this._indexValuationService.GetAllAsync();
            return Ok(dtos);
        }
    }
}
