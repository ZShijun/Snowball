using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Snowball.Core.Utils;
using Snowball.Domain.Wechat.Dtos;
using System;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Snowball.Domain.Wechat.Services
{
    public class WechatService : IWechatService
    {
        private readonly TimeProvider _timeProvider;
        private readonly WechatOption _wechatOption;
        private readonly ILogger<WechatService> _logger;

        public WechatService(TimeProvider timeProvider,
                             IOptions<WechatOption> options,
                             ILogger<WechatService> logger)
        {
            this._timeProvider = timeProvider;
            this._wechatOption = options.Value;
            this._logger = logger;
        }

        #region 签名校验
        public bool CheckSignature(string signature, string timestamp, string nonce)
        {
            string[] args = { this._wechatOption.Token, timestamp, nonce };
            return CheckSign(signature, args);
        }

        public bool CheckSignature(string signature, string timestamp, string nonce, string encryptMessage)
        {
            string[] args = { this._wechatOption.Token, timestamp, nonce, encryptMessage };
            return CheckSign(signature, args);
        }

        private bool CheckSign(string signature, params string[] args)
        {
            var mySign = MakeSign(args);
            return mySign.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }

        private string MakeSign(params string[] args)
        {
            //1. 字典序排序
            Array.Sort(args);

            // 2. 拼接字符串
            string tempStr = string.Join("", args);

            // 3. 进行sha1签名
            return SecurityUtil.Sha1(tempStr);
        }
        #endregion

        #region 处理请求消息
        public string GetEncryptMessage(string messageBody)
        {
            XDocument doc = XDocument.Parse(messageBody);
            return GetElementValue(doc, "Encrypt");
        }

        private string GetElementValue(XDocument doc, string elementKey)
        {
            var element = doc.Root.Element(elementKey);
            if (element == null)
            {
                return null;
            }

            return element.Value;
        }

        public WechatPlaintextMessage GetPlaintextMessage(string encrypted)
        {
            var encodingAESKey = this._wechatOption.EncodingAESKey;
            var decryptedBuffer = SecurityUtil.AesDecrypt(encrypted, encodingAESKey);
            int msgLen = BitConverter.ToInt32(decryptedBuffer, 16);
            msgLen = IPAddress.NetworkToHostOrder(msgLen);
            string plaintext = Encoding.UTF8.GetString(decryptedBuffer, 20, msgLen);
            this._logger.LogInformation("PlaintextMessage:" + plaintext);
            XDocument doc = XDocument.Parse(plaintext);

            return new WechatPlaintextMessage
            {
                FromUserName = GetElementValue(doc, "FromUserName"),
                ToUserName = GetElementValue(doc, "ToUserName"),
                MsgType = GetElementValue(doc, "MsgType"),
                Event = GetElementValue(doc, "Event"),
                Content = GetElementValue(doc, "Content"),
                CreateTime = GetElementValue(doc, "CreateTime")
            };
        }
        #endregion

        #region 构造响应消息
        public string BuildNormalReplayMessage(string fromUser, string toUser, string content)
        {
            if (string.IsNullOrWhiteSpace(fromUser)
                || string.IsNullOrWhiteSpace(toUser)
                || string.IsNullOrWhiteSpace(content))
            {
                return string.Empty;
            }

            long timestamp = this._timeProvider.Now.ToSecondTimeStamp();
            StringBuilder templateBuilder = new StringBuilder();
            templateBuilder.Append("<xml>");
            templateBuilder.Append($"<ToUserName><![CDATA[{toUser}]]></ToUserName>");
            templateBuilder.Append($"<FromUserName><![CDATA[{fromUser}]]></FromUserName>");
            templateBuilder.Append($"<CreateTime>{timestamp}</CreateTime>");
            templateBuilder.Append("<MsgType><![CDATA[text]]></MsgType>");
            templateBuilder.Append($"<Content><![CDATA[{content}]]></Content>");
            templateBuilder.Append("</xml>");
            return templateBuilder.ToString();
        }

        public string BuildDefaultReplayMessage(string fromUser, string toUser)
        {
            if (string.IsNullOrWhiteSpace(fromUser)
                      || string.IsNullOrWhiteSpace(toUser))
            {
                return string.Empty;
            }

            StringBuilder content = new StringBuilder();
            content.AppendLine("您的指令让我有些为难，你可以尝试如下形式：");
            content.AppendLine("1、【1:书籍名称】：按书籍名称搜索相关书籍，不记得全名也能搜哦！");
            content.AppendLine("2、【2:书籍编号】：获取书籍下载地址，编号可以通过【1:书籍名称】查询！");
            content.AppendLine("0、【0:强烈建议】：别太难为人哦！");
            content.AppendLine("祝您生活愉快，每天都能跳着踢踏舞去工作！");
            return BuildNormalReplayMessage(fromUser, toUser, content.ToString());
        }
        #endregion
    }
}
