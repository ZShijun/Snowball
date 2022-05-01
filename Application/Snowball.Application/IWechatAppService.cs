using System.Threading.Tasks;

namespace Snowball.Application
{
    public interface IWechatAppService
    {
        bool CheckSignature(string signature, string timestamp, string nonce);

        Task<string> ReplyAsync(string messageBody, string encryptType, string timestamp, string nonce, string msgSignature);
    }
}
