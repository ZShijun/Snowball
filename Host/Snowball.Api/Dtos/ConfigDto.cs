using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snowball.Api.Dtos
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public class ConfigDto
    {
        /// <summary>
        /// 是否审核通过
        /// </summary>
        public bool Passed { get; set; }
    }
}
