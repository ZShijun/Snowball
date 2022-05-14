using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Domain.Wechat.Dtos
{
    public class WechatSuggestionDto
    {
        /// <summary>
        /// 微信OpenId
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 建议
        /// </summary>
        public string Content { get; set; }
    }
}
