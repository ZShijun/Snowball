using Snowball.Core.Utils;
using Snowball.Domain.Bookshelf.Dtos.Wechat;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace Snowball.Domain.Bookshelf.Services
{
    internal static class WechatMessageHelper
    {
        /// <summary>
        /// 微信签名
        /// </summary>
        /// <param name="args">签名参数</param>
        /// <returns></returns>
        public static string MakeSign(params string[] args)
        {
            //1. 字典序排序
            Array.Sort(args);

            // 2. 拼接字符串
            string tempStr = string.Join("", args);

            // 3. 进行sha1签名
            return SecurityUtil.Sha1(tempStr);
        }

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="signature">签名</param>
        /// <param name="args">签名参数</param>
        /// <returns></returns>
        public static bool CheckSign(string signature, params string[] args)
        {
            var mySign = MakeSign(args);
            return mySign.Equals(signature, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 是否通过Aes加密
        /// </summary>
        /// <param name="encryptType">加密类型</param>
        /// <returns></returns>
        public static bool IsAesEncrypt(string encryptType)
        {
            return !string.IsNullOrEmpty(encryptType)
                && string.Equals(encryptType, "aes", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetEncryptMessage(string messageBody)
        {
            XDocument doc = XDocument.Parse(messageBody);
            return GetElementValue(doc, "Encrypt");
        }

        public static WechatPlaintextMessage GetPlaintextMessage(string encrypted, string encodingAESKey)
        {
            var decryptedBuffer = SecurityUtil.AesDecrypt(encrypted, encodingAESKey);
            int msgLen = BitConverter.ToInt32(decryptedBuffer, 16);
            msgLen = IPAddress.NetworkToHostOrder(msgLen);
            string plaintext = Encoding.UTF8.GetString(decryptedBuffer, 20, msgLen);
            XDocument doc = XDocument.Parse(plaintext);
            return new WechatPlaintextMessage
            {
                FromUserName = GetElementValue(doc, "FromUserName"),
                ToUserName = GetElementValue(doc, "ToUserName"),
                MsgType = GetElementValue(doc, "MsgType"),
                Event = GetElementValue(doc, "Event"),
                EventKey = GetElementValue(doc, "EventKey"),
                CreateTime = GetElementValue(doc, "CreateTime")
            };
        }

        public static string BuildTextMessage(string fromUser, string toUser, string content)
        {
            long timestamp = DateTime.Now.ToSecondTimeStamp();
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

        private static string GetElementValue(XDocument doc, string elementKey)
        {
            if (doc == null)
            {
                return null;
            }

            var element = doc.Root.Element(elementKey);
            if (element == null)
            {
                return null;
            }

            return element.Value;
        }
    }
}
