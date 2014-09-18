using System;
using System.Collections.Generic;
namespace cosen.Models
{
    public interface ILogicModel
    {
        void AddPromotionInfo(cosen.promotions pro);
        System.Net.Http.HttpResponseMessage DelImg(string imgType, string imgName, string dpid, System.Net.Http.HttpRequestMessage request);
        string DelPh(string style, string use_id);
        void DelPromotionInfo(int id);
        System.Collections.Generic.IList<string> Get_Man_No();
        DaPeiInfo GetDaiPei(string sltDp, int pageNum, string tjDate, string filterType, string filterValue, System.Web.HttpContext context);
        System.Collections.Generic.IList<cosen.dianpu> GetDianpuList();
        System.Collections.Generic.IList<System.Web.Mvc.SelectListItem> GetDianpus();
        System.Linq.IQueryable GetDpByMaster(string style, string dpType, string gtype);
        System.Collections.Generic.IList<cosen.GetPeiHuo_ProcResult> GetPeiHuos(string zdid, string style);
        System.Collections.Generic.IList<cosen.promotions> GetPromotionInfo();
        cosen.promotions GetPromotionInfoById(int? id);
        System.Collections.Generic.IList<ReportInfo> GetReportsInfo(string sltType, string dps, int pageNum, string startDate, string endDate, string styleNo, string sort, string sortT);
        StylesJsonData GetStyleJson(System.Web.HttpContext context);
        System.Collections.Generic.IList<string> GetUp4Images(string styleNo);
        System.Collections.Generic.IList<ZhiZaoDanInfo> GetZDHById(string zdid);
        System.Linq.IQueryable LookAllTjDp();
        System.Linq.IQueryable LookAllTjDp(string dpId);
        System.Collections.Generic.IList<DaPei> LookUpTjDp(string dpid, string tjdate);
        string PhExcel(string zdid);
        string SaveDp(System.Web.HttpContext httpContext);
        string SaveImgName(string oldName, string newName, string imgType, string dpid, System.Web.HttpContext context);
        string SavePh(cosen.GetPeiHuo_ProcResult ph);
        string SaveTuiJianDp(string dpid, string tjdate, string tjdp, string remark);
        System.Collections.Generic.IList<ImageInfo> SearchImg(string imgName, string imgType, int imgPage, string dpid, System.Web.HttpContext context);
        System.Data.Linq.Binary ShowImage(string styleNo);
        bool ThumbnailCallback();
        string UpdateDate(string styleNo, string date, int? cxinfo);
        void UpdatePromotionInfo(cosen.promotions pro);
        IList<ImageInfo> UploadImg(System.Web.HttpContext context, System.Net.Http.HttpRequestMessage request);

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        bool ValidateUser(string userName, string password);
        /// <summary>
        /// 获取用户信息列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="pageCount">页行数</param>
        /// <param name="user_name">用户名</param>
        /// <returns></returns>
        IList<get_user_infosResult> Get_User_List(int page, int pageCount, string user_name);


    }
}
