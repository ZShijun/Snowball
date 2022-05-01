using Microsoft.Extensions.Options;
using Snowball.Core.Utils;
using Snowball.Domain.Bookshelf.Dtos;
using Snowball.Domain.Bookshelf.Dtos.Options;
using System;
using System.Text;
using System.Xml.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

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

        public WechatPlaintextMessage GetPlaintextMessage(string encrypted)
        {
            var encodingAESKey = this._wechatOption.EncodingAESKey;
            var decryptedBuffer = SecurityUtil.AesDecrypt(encrypted, encodingAESKey);
            int msgLen = BitConverter.ToInt32(decryptedBuffer, 16);
            msgLen = IPAddress.NetworkToHostOrder(msgLen);
            string plaintext = Encoding.UTF8.GetString(decryptedBuffer, 20, msgLen);
            this._logger.LogInformation("PlaintextMessage:" + JsonUtil.Serialize(plaintext));
            XDocument doc = XDocument.Parse(plaintext);

            return new WechatPlaintextMessage
            {
                FromUserName = GetElementValue(doc, "FromUserName"),
                ToUserName = GetElementValue(doc, "ToUserName"),
                MsgType = GetElementValue(doc, "MsgType"),
                Event = GetElementValue(doc, "Event"),
                EventKey = GetElementValue(doc, "EventKey"),
                Content = GetElementValue(doc, "Content"),
                CreateTime = GetElementValue(doc, "CreateTime")
            };
        }
        #endregion

        #region 构造响应消息
        public string BuildNormalTextMessage(string fromUser, string toUser, string content)
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

        public string BuildSearchReplayTextMessage(string fromUser, string toUser, IEnumerable<BookDto> books)
        {
            string content = BuildSearchReplayContent(books);
            return BuildNormalTextMessage(fromUser, toUser, content);
        }

        private string BuildSearchReplayContent(IEnumerable<BookDto> books)
        {
            if (books == null
                || !books.Any())
            {
                return "这就尴尬了，没有你想要的书籍哦！";
            }

            StringBuilder content = new StringBuilder();
            content.AppendLine("我们帮你找到了如下书籍：");
            foreach (var book in books)
            {
                content.Append($"编　号：{book.Id}");
                if (book.DoubanScore != 0)
                {
                    content.AppendLine($"\t评　分：{book.DoubanScore}");
                }
                else
                {
                    content.AppendLine();
                }

                content.AppendLine($"书　名：{book.Name}");
                if (!string.IsNullOrWhiteSpace(book.OriginalName))
                {
                    content.AppendLine($"原书名：{book.OriginalName}");
                }
                content.AppendLine($"作　者：{book.Author}");
                if (!string.IsNullOrWhiteSpace(book.Translator))
                {
                    content.AppendLine($"译　者：{book.Translator}");
                }
                content.AppendLine();
            }
            content.AppendLine("编辑发送“2:编号”就可以获取你想要的书籍下载链接啦!");
            content.AppendLine("记得要以“2:”开头哦！");
            return content.ToString();
        }

        public string BuildDownloadTextMessage(string fromUser, string toUser, BookDto book)
        {
            string content = BuildDownloadContent(book);
            return BuildNormalTextMessage(fromUser, toUser, content);
        }

        private string BuildDownloadContent(BookDto book)
        {
            if (book == null)
            {
                return "很抱歉，没有找到您想要的书籍！";
            }

            StringBuilder content = new StringBuilder();
            content.AppendLine("拿走不谢，很高兴能够帮到您！");
            content.AppendLine($"下载地址：{book.DownloadUrl}");
            content.AppendLine($"提 取 码：{book.ExtractionCode}");
            return content.ToString();
        }

        public string BuildDefaultReplayMessage(string fromUser, string toUser)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("亲爱的投资者朋友，您的指令真的让我很为难，你可以尝试按如下形式发送指令：");
            content.AppendLine("1、【1:书籍名称】：按书籍名称搜索相关书籍，不记得全名也能搜索哦！");
            content.AppendLine("2、【2:书籍编号】：根据书籍编号，获取书籍下载地址，书籍编号可以通过【1:书籍名称】查询。");
            // content.AppendLine("0、【0:意见内容】：有好的意见或建议都可以通过该指令发送给我，感谢您的支持！");
            content.AppendLine("感谢您的支持，您的宝贵意见会让我变得更强！");
            return BuildNormalTextMessage(fromUser, toUser, content.ToString());
        }
        #endregion
    }
}
