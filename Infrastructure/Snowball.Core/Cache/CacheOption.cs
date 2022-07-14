using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Core.Cache
{
    public class CacheOption
    {
        /// <summary>
        /// 缓存类型
        /// </summary>
        public CacheType CacheType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
