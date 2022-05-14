using System;

namespace Snowball.Domain.Wechat.Entities
{
    public class WechatSuggestionEntity
    {
        /// <summary>
        /// 自增长ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 建议
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
    }
}
