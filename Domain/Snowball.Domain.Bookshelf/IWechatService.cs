using System.Threading.Tasks;

namespace Snowball.Domain.Bookshelf
{
    public interface IWechatService
    {
        /// <summary>
        /// 检验signature
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        bool CheckSignature(string signature, string timestamp, string nonce);

        /// <summary>
        /// 获取回应微信的消息
        /// </summary>
        /// <param name="messageBody">消息体</param>
        /// <param name="encryptType">加密类型</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="msgSignature">消息体签名</param>
        /// <returns></returns>
        Task<string> ReplyAsync(string messageBody, string encryptType, string timestamp, string nonce, string msgSignature);
    }
}
