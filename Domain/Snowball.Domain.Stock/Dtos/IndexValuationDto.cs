﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Snowball.Domain.Stock.Dtos
{
    public class IndexValuationDto
    {
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
        public decimal PE { get; set; }

        /// <summary>
        /// 市盈率百分位
        /// </summary>
        public decimal PEPercentile { get; set; }

        /// <summary>
        /// 市净率
        /// </summary>
        public decimal PB { get; set; }

        /// <summary>
        /// 市净率百分位
        /// </summary>
        public decimal PBPercentile { get; set; }

        /// <summary>
        /// 净资产收益率
        /// </summary>
        public decimal ROE { get; set; }

        /// <summary>
        /// 股息率
        /// </summary>
        public decimal DividendYield { get; set; }

        /// <summary>
        /// 估值水平
        ///     1:严重低估[0-15),
        ///     2:低估[15-35)
        ///     3:适中[35-65],
        ///     4:高估(65-85],
        ///     5:严重高估(85-100]
        /// </summary>
        public int ValuationLevel { get; set; }
    }
}
