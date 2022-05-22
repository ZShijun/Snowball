using System;

namespace Snowball.Domain.Stock.Entities
{
    /// <summary>
    /// 指数估值
    /// </summary>
    public class IndexValuationEntity
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
        /// 指数简介
        /// </summary>
        public string Description { get; set; }

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
        /// 指数编制时间
        /// </summary>
        public DateTime PreparationTime { get; set; }

        /// <summary>
        /// 是否使用市净率估值
        /// </summary>
        public bool UsePB { get; set; }

        /// <summary>
        /// 记录修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
    }
}
