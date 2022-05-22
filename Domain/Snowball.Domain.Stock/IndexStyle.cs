using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Snowball.Domain.Stock
{
    public enum IndexStyle
    {
        [Display(Name = "宽基")]
        WideBase = 1,
        [Display(Name = "行业")]
        Theme = 2,
        [Display(Name = "策略")]
        Strategy = 3,
        [Display(Name = "全球")]
        Global = 4
    }
}
