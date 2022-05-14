using System;

namespace Snowball.Domain.Wechat.Entities
{
    public class WechatSubscriptionEntity
    {
        /// <summary>
        /// 自增长ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 服务提供者(公众号原始ID)
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// 订阅者(微信OpenId)
        /// </summary>
        public string Subscriber { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 订阅时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否取消订阅
        /// </summary>
        public bool IsDel { get; set; }
    }
}
