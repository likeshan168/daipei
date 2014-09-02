
namespace cosen.Models
{
    public class ReportInfo
    {

        /// <summary>
        /// 序号
        /// </summary>
        //public int Xh { get; set; }
        /// <summary>
        /// 款式+颜色
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "VarChar(50)")]
        public string nsty_no { get; set; }
        /// <summary>
        /// 款式名称
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "VarChar(50)")]
        public string com_nm { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        //public string ImageUrl { get; set; }

        /// <summary>
        /// 上市日期
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "VarChar(50)")]
        public string editionhandle { get; set; }

        /// <summary>
        /// 促销信息
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Int")]
        public int? p_id { get; set; }
        /// <summary>
        /// 入库数
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Decimal(38,0) NOT NULL")]
        public float rknum { get; set; }
        /// <summary>
        /// 出货数（出货指的是给加盟商的发货）
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Decimal(38,0) NOT NULL")]
        public float chnum { get; set; }

        /// <summary>
        /// 调拨数（调拨指的是：给直营店的发货）
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Decimal(38,0) NOT NULL")]
        public float dbnum { get; set; }

        /// <summary>
        /// 退货数
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Decimal(38,0) NOT NULL")]
        public float thnum { get; set; }
        /// <summary>
        /// 销售数
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Decimal(38,0) NOT NULL")]
        public float xsnum { get; set; }
        /// <summary>
        /// 库存数
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Decimal(38,0) NOT NULL")]
        public float cknum { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Decimal(38,0) NOT NULL")]
        public float unitprice { get; set; }
        /// <summary>
        /// 销售总金额
        /// </summary>
        [global::System.Data.Linq.Mapping.ColumnAttribute(DbType = "Decimal(38,0) NOT NULL")]
        public float xsmoney { get; set; }

    }
}