using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Snowball.Application;
using System.IO;
using System.Threading.Tasks;

namespace Snowball.Api.Controllers
{
    /// <summary>
    /// 微信回调
    /// </summary>
    [ApiController]
    [Route("v1/api/[controller]")]
    public class WechatController : ControllerBase
    {
        private readonly IWechatAppService _wechatAppService;
        private readonly ILogger<WechatController> _logger;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="wechatAppService"></param>
        /// <param name="logger"></param>
        public WechatController(IWechatAppService wechatAppService,
                                ILogger<WechatController> logger)
        {
            this._wechatAppService = wechatAppService;
            this._logger = logger;
        }

        /// <summary>
        /// 开发者通过检验signature对请求进行校验。
        /// 若确认此次GET请求来自微信服务器，请原样返回echostr参数内容，否则接入失败。
        /// </summary>
        /// <remarks>
        /// 加密/校验流程如下：
        /// 
        ///     1）将token、timestamp、nonce三个参数进行字典序排序 
        ///     2）将三个参数字符串拼接成一个字符串进行sha1加密
        ///     3）开发者获得加密后的字符串可与signature对比，标识该请求来源于微信
        /// </remarks>
        /// <param name="echoStr">随机字符串</param>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<string> Validation(string echoStr, string signature, string timestamp, string nonce)
        {
            bool success = this._wechatAppService.CheckSignature(signature, timestamp, nonce);
            if (success)
            {
                return Ok(echoStr);
            }

            return Ok();
        }

        /// <summary>
        /// 微信回调统一接口
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="encryptType">加密类型</param>
        /// <param name="msgSignature">消息体签名</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> Callback(string signature,
                                           string timestamp,
                                           string nonce,
                                           [FromQuery(Name = "encrypt_type")] string encryptType,
                                           [FromQuery(Name = "msg_signature")] string msgSignature)
        {
            bool success = this._wechatAppService.CheckSignature(signature, timestamp, nonce);
            if (!success)
            {
                return Ok();
            }

            string messageBody = await GetMessageBodyAsync(Request.Body);
            var result = await this._wechatAppService.ReplyAsync(messageBody, encryptType, timestamp, nonce, msgSignature);
            this._logger.LogInformation("Replay:" + result);
            return Ok(result);
        }

        private Task<string> GetMessageBodyAsync(Stream body)
        {
            using var reader = new StreamReader(body);
            return reader.ReadToEndAsync();
        }
    }
}
