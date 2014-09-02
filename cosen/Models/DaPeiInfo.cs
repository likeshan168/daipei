using System.Collections.Generic;

namespace cosen.Models
{
    /// <summary>
    /// 搭配详情表
    /// </summary>
    public class DaPei
    {
        /// <summary>
        /// 主打款的存款
        /// </summary>
        public string mqu { get; set; }
        /// <summary>
        /// 主打款款式
        /// </summary>
        public string masterstyle { get; set; }
        /// <summary>
        /// 上市日期
        /// </summary>
        public string mdate { get; set; }
        /// <summary>
        /// 内搭一库存
        /// </summary>
        public string lqu1 { get; set; }
        /// <summary>
        /// 内搭一款式
        /// </summary>
        public string legging1 { get; set; }
        /// <summary>
        /// 上市日期
        /// </summary>
        public string datel1 { get; set; }
        /// <summary>
        /// 内搭二库存
        /// </summary>
        public string lqu2 { get; set; }
        /// <summary>
        /// 内搭二款式
        /// </summary>
        public string legging2 { get; set; }
        /// <summary>
        /// 上市日期
        /// </summary>
        public string datel2 { get; set; }
        /// <summary>
        /// 下身一库存
        /// </summary>
        public string bqu1 { get; set; }
        /// <summary>
        /// 下身一款式
        /// </summary>
        public string bottom1 { get; set; }
        /// <summary>
        /// 上市日期
        /// </summary>
        public string dateb1 { get; set; }
        /// <summary>
        /// 下身二库存
        /// </summary>
        public string bqu2 { get; set; }
        /// <summary>
        /// 下身二款式
        /// </summary>
        public string bottom2 { get; set; }
        /// <summary>
        /// 上市日期
        /// </summary>
        public string dateb2 { get; set; }
        /// <summary>
        /// 配饰一库存
        /// </summary>
        public string aqu1 { get; set; }
        /// <summary>
        /// 配饰款式
        /// </summary>
        public string accessory1 { get; set; }
        /// <summary>
        /// 上市日期
        /// </summary>
        public string datea1 { get; set; }
        /// <summary>
        /// 配饰二库存
        /// </summary>
        public string aqu2 { get; set; }
        /// <summary>
        /// 配饰二款式
        /// </summary>
        public string accessory2 { get; set; }
        /// <summary>
        /// 上市日期
        /// </summary>
        public string datea2 { get; set; }
        /// <summary>
        /// 组合图编号
        /// </summary>
        public string cbpicture { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }



    }

    public class DaPeiInfo
    {
        public IEnumerable<getdpResult> rows { get; set; }
        public IEnumerable<TjDateInfo> tjrows { get; set; }
        public int total { get; set; }

    }

    public class TjDateInfo
    {
        public string tjdate { get; set; }
    }
}