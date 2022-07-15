using Microsoft.AspNetCore.Mvc;
using Snowball.Api.Dtos;
using Snowball.Core;
using Snowball.Core.Cache;
using System.Threading.Tasks;

namespace Snowball.Api.Controllers
{
    /// <summary>
    /// 全局配置管理
    /// </summary>
    [ApiController]
    [Route("v1/api/[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly ICache _cache;

        public ConfigController(ICache cache)
        {
            this._cache = cache;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ConfigDto>> Get()
        {
            var config = await this._cache.GetAsync<ConfigDto>(GlobalConstant.Config);
            return Ok(config);
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<bool>> Post(ConfigDto config)
        {
            bool success = await this._cache.SetAsync(GlobalConstant.Config, config);
            return Ok(success);
        }
    }
}
