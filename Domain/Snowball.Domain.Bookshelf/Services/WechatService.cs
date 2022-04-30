using Microsoft.Extensions.Options;
using Snowball.Core.Utils;
using Snowball.Domain.Bookshelf.Dtos;
using Snowball.Domain.Bookshelf.Dtos.Options;
using System;
using System.Text;
using System.Xml.Linq;
using Snowball.Domain.Bookshelf.Dtos.Wechat;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Snowball.Domain.Bookshelf.Services
{
    public class WechatService : IWechatService
    {
        private readonly WechatOption _wechatOption;
        private readonly ILogger<WechatService> _logger;

        public WechatService(IOptions<WechatOption> options, ILogger<WechatService> logger)
        {
            this._wechatOption = options.Value;
            this._logger = logger;
        }

        public bool CheckSignature(string signature, string timestamp, string nonce)
        {
            string[] args = { this._wechatOption.Token, timestamp, nonce };
            return WechatMessageHelper.CheckSign(signature, args);
        }

        public async Task<string> ReplyAsync(string messageBody, string encryptType, string timestamp, string nonce, string msgSignature)
        {
            this._logger.LogInformation("EncryptMessage:" + messageBody);
            if (!WechatMessageHelper.IsAesEncrypt(encryptType))
            {
                return string.Empty;
            }

            var encryptMessage = WechatMessageHelper.GetEncryptMessage(messageBody);
            if (string.IsNullOrEmpty(encryptMessage))
            {
                return string.Empty;
            }

            string[] args = { this._wechatOption.Token, timestamp, nonce, encryptMessage };  
            if (!WechatMessageHelper.CheckSign(msgSignature, args))
            {
                return string.Empty;
            }

            var plaintextMessage = WechatMessageHelper.GetPlaintextMessage(encryptMessage, this._wechatOption.EncodingAESKey);
            this._logger.LogInformation("PlaintextMessage:" + JsonUtil.Serialize(plaintextMessage));
            if (plaintextMessage.MsgType == "text")
            {
                return WechatMessageHelper.BuildTextMessage(plaintextMessage.ToUserName, plaintextMessage.FromUserName, "Hello \r\n World!");
            }

            return string.Empty;
        }
    }
}
