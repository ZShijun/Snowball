namespace Snowball.Domain.Bookshelf.Dtos
{
    /// <summary>
    /// 明文消息
    /// </summary>
    public class WechatPlaintextMessage
    {
        /// <summary>
        /// 接收方帐号（收到的OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息类型，event、text
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 事件类型，subscribe(订阅)、unsubscribe(取消订阅)
        /// </summary>
        public string Event { get; set; }

        public string Content { get; set; }

        public string CreateTime { get; set; }
    }
}
