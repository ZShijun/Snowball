using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Domain.Stock.Dtos
{
    public class IndexValuationDto
    {
        /// <summary>
        /// 指数编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 指数标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 指数风格
        /// </summary>
        public IndexStyle Style { get; set; }

        /// <summary>
        /// 市盈率
        /// </summary>
        public string PE { get; set; }

        /// <summary>
        /// 市盈率百分位
        /// </summary>
        public string PEPercentile { get; set; }

        /// <summary>
        /// 市净率
        /// </summary>
        public string PB { get; set; }

        /// <summary>
        /// 市净率百分位
        /// </summary>
        public string PBPercentile { get; set; }

        /// <summary>
        /// 市盈率/市净率合并后的百分位
        /// </summary>
        public decimal Percentile { get; set; }

        /// <summary>
        /// 净资产收益率
        /// </summary>
        public string ROE { get; set; }

        /// <summary>
        /// 股息率
        /// </summary>
        public string DividendYield { get; set; }

        /// <summary>
        /// 估值水平颜色
        /// </summary>
        public string ValuationColor
        {
            get
            {
                int red = 0XFF;
                int green = 0XFF;
                if (this.Percentile <= 50)
                {
                    red = (int)(red * this.Percentile / 50);
                }
                else if(this.Percentile <= 100)
                {
                    green = (int)(green * (100 - this.Percentile) / 50);
                }
                else
                {
                    return "#FFF";
                }

                return $"#{red:X2}{green:X2}00";
            }
        }
    }
}
