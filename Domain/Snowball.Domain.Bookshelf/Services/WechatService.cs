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

        public string BuildSearchReplayMessage(string fromUser, string toUser, IEnumerable<BookDto> books)
        {
            if (string.IsNullOrWhiteSpace(fromUser)
                   || string.IsNullOrWhiteSpace(toUser))
            {
                return string.Empty;
            }

            string content = BuildSearchReplayContent(books);
            return BuildNormalReplayMessage(fromUser, toUser, content);
        }

        private string BuildSearchReplayContent(IEnumerable<BookDto> books)
        {
            if (books == null
                || !books.Any())
            {
                return "尴尬了，没有你想要的书籍！";
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
            content.AppendLine("编辑发送【2:编号】就可以获取书籍下载链接啦!");
            return content.ToString();
        }

        public string BuildDownloadReplayMessage(string fromUser, string toUser, BookDto book)
        {
            if (string.IsNullOrWhiteSpace(fromUser)
                      || string.IsNullOrWhiteSpace(toUser))
            {
                return string.Empty;
            }

            string content = BuildDownloadContent(book);
            return BuildNormalReplayMessage(fromUser, toUser, content);
        }

        private string BuildDownloadContent(BookDto book)
        {
            if (book == null)
            {
                return "很遗憾，没有找到您想要的书籍！";
            }

            StringBuilder content = new StringBuilder();
            content.AppendLine($"下载地址：{book.DownloadUrl}");
            content.AppendLine($"提 取 码：{book.ExtractionCode}");
            content.AppendLine("很高兴能帮到您，祝您投资路上一片坦途！");
            return content.ToString();
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
