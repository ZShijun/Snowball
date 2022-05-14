using Microsoft.Extensions.Logging;
using Snowball.Application.Dtos;
using Snowball.Core;
using Snowball.Domain.Bookshelf;
using Snowball.Domain.Bookshelf.Dtos;
using Snowball.Domain.Wechat;
using Snowball.Domain.Wechat.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Application
{
    public class WechatAppService : IWechatAppService
    {
        private readonly IBookService _bookService;
        private readonly IWechatService _wechatService;
        private readonly ILogger<WechatAppService> _logger;

        public WechatAppService(IBookService bookService,
                                IWechatService wechatService,
                                ILogger<WechatAppService> logger)
        {
            this._bookService = bookService;
            this._wechatService = wechatService;
            this._logger = logger;
        }

        public bool CheckSignature(string signature, string timestamp, string nonce)
        {
            return this._wechatService.CheckSignature(signature, timestamp, nonce);
        }

        public async Task<string> ReplyAsync(string messageBody, string encryptType, string timestamp, string nonce, string msgSignature)
        {
            this._logger.LogInformation("EncryptMessage:" + messageBody);
            if (!IsAesEncrypt(encryptType))
            {
                return string.Empty;
            }

            var encryptMessage = this._wechatService.GetEncryptMessage(messageBody);
            if (string.IsNullOrEmpty(encryptMessage))
            {
                return string.Empty;
            }

            if (!this._wechatService.CheckSignature(msgSignature, timestamp, nonce, encryptMessage))
            {
                return string.Empty;
            }

            var plaintextMessage = this._wechatService.GetPlaintextMessage(encryptMessage);
            return await ReplyAsync(plaintextMessage);
        }

        private bool IsAesEncrypt(string encryptType)
        {
            return !string.IsNullOrEmpty(encryptType)
                && string.Equals(encryptType, "aes", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<string> ReplyAsync(WechatPlaintextMessage message)
        {
            if (message.MsgType == "text")
            {
                return await ReplyTextMessageAsync(message);
            }
            else if (message.MsgType == "event")
            {
                return await ReplyEventMessageAsync(message);
            }
            else
            {
                return string.Empty;
            }
        }

        #region 回复文本消息
        private async Task<string> ReplyTextMessageAsync(WechatPlaintextMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Content))
            {
                return string.Empty;
            }

            string replay;
            try
            {
                var command = new WechatCommand(message.Content);
                replay = command.CommandType switch
                {
                    WechatCommandType.BookSearch => await BuildSearchReplay(message.ToUserName, message.FromUserName, command.Content),
                    WechatCommandType.Download => await BuildDownloadReplay(message.ToUserName, message.FromUserName, command.Content),
                    WechatCommandType.Suggest => await AddSuggestionAsync(message.ToUserName, message.FromUserName, command.Content),
                    _ => this._wechatService.BuildDefaultReplayMessage(message.ToUserName, message.FromUserName),
                };
            }
            catch (BizException bizEx)
            {
                replay = this._wechatService.BuildNormalReplayMessage(message.ToUserName, message.FromUserName, bizEx.Message);
            }

            return replay;
        }

        private async Task<string> BuildSearchReplay(string fromUser, string toUser, string searchKey)
        {
            var books = await this._bookService.FuzzySearchByNameAsync(searchKey);
            string messageContent = BuildSearchReplayContent(books);
            return this._wechatService.BuildNormalReplayMessage(fromUser, toUser, messageContent);
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

        private async Task<string> BuildDownloadReplay(string fromUser, string toUser, string bookId)
        {
            if (!int.TryParse(bookId, out int id))
            {
                return string.Empty;
            }

            var book = await this._bookService.GetAsync(id);
            string messageContent = BuildDownloadReplayContent(book);
            return this._wechatService.BuildNormalReplayMessage(fromUser, toUser, messageContent);
        }

        private string BuildDownloadReplayContent(BookDto book)
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

        private async Task<string> AddSuggestionAsync(string fromUser, string toUser, string suggestion)
        {
            bool success = await this._wechatService.AddSuggestionAsync(new WechatSuggestionDto
            {
                OpenId = fromUser,
                Content = suggestion
            });

            if (!success)
            {
                return string.Empty;
            }

            const string suggestionReplayContent = "收到，感谢您的宝贵意见！";
            return this._wechatService.BuildNormalReplayMessage(fromUser, toUser, suggestionReplayContent);
        }
        #endregion

        #region 回复事件消息
        private async Task<string> ReplyEventMessageAsync(WechatPlaintextMessage message)
        {
            if (message.Event == "subscribe")
            {
                return await SubscribeAsync(message);
            }

            if (message.Event == "unsubscribe")
            {
                await this._wechatService.UnsubscribeAsync(message.FromUserName);
            }

            return string.Empty;
        }

        private async Task<string> SubscribeAsync(WechatPlaintextMessage message)
        {
            bool success = await this._wechatService.SubscribeAsync(message.FromUserName, message.ToUserName);
            if (!success)
            {
                return string.Empty;
            }

            string content = BuildSubscribeReplayContent();
            return this._wechatService.BuildNormalReplayMessage(message.ToUserName, message.FromUserName, content);
        }

        private string BuildSubscribeReplayContent()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Hi，亲爱的投资者朋友，你终于来了！");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine("希望在接下来的日子里，我能陪伴你穿越牛熊，慢慢变富……");
            return stringBuilder.ToString();
        } 
        #endregion
    }
}
