using Microsoft.Extensions.Logging;
using Snowball.Core;
using Snowball.Domain.Bookshelf;
using Snowball.Domain.Bookshelf.Dtos;
using System;
using System.Threading.Tasks;

namespace Snowball.Application
{
    public class WechatAppService : IWechatAppService
    {
        private readonly IBookService _bookService;
        private readonly IWechatService _wechatService;
        private readonly ILogger<WechatAppService> _logger;

        public WechatAppService(IBookService bookService,
                                IWechatService wechatService, ILogger<WechatAppService> logger)
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
            return await HandleTextMessageAsync(plaintextMessage);
        }

        private async Task<string> HandleTextMessageAsync(WechatPlaintextMessage message)
        {
            if (message.MsgType != "text"
                || string.IsNullOrWhiteSpace(message.Content))
            {
                return string.Empty;
            }

            string replay = string.Empty;
            try
            {
                var command = new WechatCommand(message.Content);
                switch (command.CommandType)
                {
                    case WechatCommandType.BookSearch:
                        var books = await this._bookService.FuzzySearchByNameAsync(command.Content);
                        replay = this._wechatService.BuildSearchReplayTextMessage(message.ToUserName, message.FromUserName, books);
                        break;
                    case WechatCommandType.Download:
                        if (int.TryParse(command.Content, out int bookId))
                        {
                            var book = await this._bookService.GetAsync(bookId);
                            replay = this._wechatService.BuildDownloadTextMessage(message.ToUserName, message.FromUserName, book);
                        }

                        break;
                    case WechatCommandType.Advice:
                    case WechatCommandType.Unknow:
                    default:
                        replay = this._wechatService.BuildDefaultReplayMessage(message.ToUserName, message.FromUserName);
                        break;
                }
            }
            catch (BizException bizEx)
            {
                replay = this._wechatService.BuildNormalTextMessage(message.ToUserName, message.FromUserName, bizEx.Message);
            }

            return replay;
        }

        /// <summary>
        /// 是否通过Aes加密
        /// </summary>
        /// <param name="encryptType">加密类型</param>
        /// <returns></returns>
        private bool IsAesEncrypt(string encryptType)
        {
            return !string.IsNullOrEmpty(encryptType)
                && string.Equals(encryptType, "aes", StringComparison.OrdinalIgnoreCase);
        }
    }
}
