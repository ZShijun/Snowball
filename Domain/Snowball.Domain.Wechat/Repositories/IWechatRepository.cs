using Snowball.Domain.Wechat.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Snowball.Domain.Wechat.Repositories
{
    public interface IWechatRepository
    {
        /// <summary>
        /// 订阅是否已存在
        /// </summary>
        /// <param name="subscriber">订阅者</param>
        /// <returns></returns>
        Task<bool> ExistsSubscriptionAsync(string subscriber);

        /// <summary>
        /// 添加订阅关系
        /// </summary>
        /// <param name="entity">订阅实体</param>
        /// <returns></returns>
        Task<bool> AddSubscriptionAsync(WechatSubscriptionEntity entity);

        /// <summary>
        /// 删除订阅关系
        /// </summary>
        /// <param name="subscriber">订阅者</param>
        /// <returns></returns>
        Task<bool> RemoveSubscriptionAsync(string subscriber);

        /// <summary>
        /// 添加建议
        /// </summary>
        /// <param name="entity">建议实体</param>
        /// <returns></returns>
        Task<bool> AddSuggestionAsync(WechatSuggestionEntity entity);
    }
}
