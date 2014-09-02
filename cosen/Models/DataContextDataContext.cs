using cosen.Models;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

namespace cosen
{
    /// <summary>
    /// 自己扩展的，因为自动生成的不符合要求
    /// </summary>
    partial class DataContextDataContext : System.Data.Linq.DataContext
    {

        partial void OnCreated()
        {
            this.CommandTimeout = 1000 * 10 * 6;
        }
        /// <summary>
        /// 重新改写
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="of"></param>
        /// <returns></returns>
        [ResultType(typeof(DaPeiInfo))]
        [ResultType(typeof(int))]
        [ResultType(typeof(TjDateInfo))]
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.getdp")]
        public IMultipleResults getdp2([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(30)")] string uid, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(30)")] string dt1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(30)")] string dt2, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "Int")] System.Nullable<int> of)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), uid, dt1, dt2, of);
            return ((IMultipleResults)(result.ReturnValue));
        }
        /// <summary>
        /// 重新改写了
        /// </summary>
        /// <param name="sltType">查询类型</param>
        /// <param name="dt1">开始日期</param>
        /// <param name="dt2">结束日期</param>
        /// <param name="cond">店铺</param>
        /// <returns></returns>
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.report")]
        public ISingleResult<ReportInfo> report2([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(10)")] string sltType, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(20)")] string dt1, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(20)")] string dt2, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(1000)")] string cond)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), sltType, dt1, dt2, cond);
            return (ISingleResult<ReportInfo>)(result.ReturnValue);
        }

        [ResultType(typeof(ChuHuoDetails))]
        [ResultType(typeof(DiaoBoDetails))]
        [ResultType(typeof(TuiHuoDetails))]
        [global::System.Data.Linq.Mapping.FunctionAttribute(Name = "dbo.getchdbdetail")]
        public IMultipleResults getchdbdetail2([global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(30)")] string startdate, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(30)")] string enddate, [global::System.Data.Linq.Mapping.ParameterAttribute(DbType = "VarChar(1000)")] string cond)
        {
            IExecuteResult result = this.ExecuteMethodCall(this, ((MethodInfo)(MethodInfo.GetCurrentMethod())), startdate, enddate, cond);
            return ((IMultipleResults)(result.ReturnValue));
        }

    }
}