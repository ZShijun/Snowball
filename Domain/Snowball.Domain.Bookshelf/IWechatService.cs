using Snowball.Domain.Bookshelf.Dtos;
using System.Collections.Generic;

namespace Snowball.Domain.Bookshelf
{
    public interface IWechatService
    {
        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        bool CheckSignature(string signature, string timestamp, string nonce);

        /// <summary>
        /// 签名校验
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="encryptMessage">加密消息体</param>
        /// <returns></returns>
        bool CheckSignature(string signature, string timestamp, string nonce, string encryptMessage);

        /// <summary>
        /// 从消息体中获取加密消息
        /// </summary>
        /// <param name="messageBody">消息体</param>
        /// <returns></returns>
        string GetEncryptMessage(string messageBody);

        /// <summary>
        /// 解密密文，获取明文消息
        /// </summary>
        /// <param name="encrypted">AES加密后的消息</param>
        /// <returns></returns>
        WechatPlaintextMessage GetPlaintextMessage(string encrypted);

        /// <summary>
        /// 创建一般的文本消息响应体
        /// </summary>
        /// <param name="fromUser">开发者微信号</param>
        /// <param name="toUser">接收方帐号（收到的OpenID）</param>
        /// <param name="content">回复的消息内容</param>
        /// <returns></returns>
        string BuildNormalTextMessage(string fromUser, string toUser, string content);

        string BuildSearchReplayTextMessage(string fromUser, string toUser, IEnumerable<BookDto> books);

        string BuildDownloadTextMessage(string fromUser, string toUser, BookDto book);

        string BuildDefaultReplayMessage(string fromUser, string toUser);
    }
}
