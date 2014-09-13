using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cosen.Models
{
    /// <summary>
    /// 制造单信息表
    /// </summary>
    public class ZhiZaoDanInfo
    {
        /// <summary>
        /// 款式
        /// </summary>
        public string sty_no { get; set; }
        /// <summary>
        /// 颜色代码
        /// </summary>
        public string col_no { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string com_nm { get; set; }
        /// <summary>
        /// 颜色名称
        /// </summary>
        public string col_dr { get; set; }
        /// <summary>
        /// 尺寸：S
        /// </summary>
        public decimal s105 { get; set; }
        /// <summary>
        /// 尺寸：M
        /// </summary>
        public decimal m120 { get; set; }
        /// <summary>
        /// 尺寸：L
        /// </summary>
        public decimal l130 { get; set; }
        /// <summary>
        /// 尺寸：X
        /// </summary>
        public decimal xl140 { get; set; }
        /// <summary>
        /// 尺寸：XXL
        /// </summary>
        public decimal xxl155 { get; set; }
        /// <summary>
        ///  总数量
        /// </summary>
        public decimal com_qu { get; set; }
        /// <summary>
        /// 吊牌价
        /// </summary>
        public decimal? unt_pr { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal? com_pr { get; set; }
    }
}