using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Domain.Stock.Entities
{
    /// <summary>
    /// 数据更新点记录
    /// </summary>
    public class UpdatePointEntity
    {
        /// <summary>
        /// 数据更新键
        /// </summary>
        public string UpdateKey { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
    }
}
