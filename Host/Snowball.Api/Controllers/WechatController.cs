using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Snowball.Domain.Bookshelf;
using System.IO;
using System.Threading.Tasks;

namespace Snowball.Api.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    public class WechatController : ControllerBase
    {
        private readonly IWechatService _wechatService;
        private readonly ILogger<WechatController> _logger;

        public WechatController(IWechatService wechatService,
                                ILogger<WechatController> logger)
        {
            this._wechatService = wechatService;
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
            bool success = this._wechatService.CheckSignature(signature, timestamp, nonce);
            if (success)
            {
                return Content(echoStr);
            }

            return NoContent();
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
            bool success = this._wechatService.CheckSignature(signature, timestamp, nonce);
            if (!success)
            {
                return Ok( string.Empty);
            }
            string messageBody = await GetMessageBodyAsync(Request.Body);
            var result = await this._wechatService.ReplyAsync(messageBody, encryptType, timestamp, nonce, msgSignature);
            _logger.LogInformation("Result:" + result);
            return Ok(result);
        }

        private Task<string> GetMessageBodyAsync(Stream body)
        {
            using (var reader = new StreamReader(body))
            {
                return reader.ReadToEndAsync();
            }
        }
    }
}
