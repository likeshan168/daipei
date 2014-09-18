using cosen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace cosen.Controllers
{
    public class DpApiController : ApiController
    {
        [Ninject.Inject]
        private ILogicModel logic { get; set; }

        //private ILogicModel logic;
        //public DpApiController(ILogicModel logic)
        //{
        //    this.logic = logic;
        //}

        [HttpGet]
        public StylesJsonData GetStyles()
        {
            //throw new HttpResponseException(HttpStatusCode.InternalServerError);//不能被过滤器捕捉
            //throw new Exception("web api 异常");//能被过滤器捕捉
            return logic.GetStyleJson(HttpContext.Current);
        }
        [HttpPost]
        public string SaveDp()
        {
            return logic.SaveDp(HttpContext.Current);

        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //[ActionName("Post")]
        //public Task<IList<ImageInfo>> UploadImg()
        //{

        //    logic = new LogicModel();
        //    return logic.UploadImg(HttpContext.Current, Request);

        //}
        [HttpPost]
        public IList<ImageInfo> UploadImg()
        {
            return logic.UploadImg(HttpContext.Current, Request);

        }
        /// <summary>
        /// 删除图片文件
        /// </summary>
        /// <param name="imgType">图片类型（单款图还是组合图还是场地图）</param>
        /// <param name="imgName">图片名称</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DelImg(string imgType, string imgName, string dpid)
        {
            return logic.DelImg(imgType, imgName, dpid, Request);
        }
        /// <summary>
        /// 查询图片
        /// </summary>
        /// <param name="imgName">图片名称</param>
        /// <param name="imgType">图片类型（单款，组合，场地图）</param>
        /// <param name="imgPage">页码（分页显示用的）</param>
        /// <returns></returns>
        [HttpPost]
        //[ActionName("Post")]
        public ImageViewInfo SearchImg(string imgName, string imgType, int imgPage, string dpid)
        {
            return new ImageViewInfo { files = logic.SearchImg(imgName, imgType, imgPage, dpid, HttpContext.Current) };
        }
        /// <summary>
        /// 修改图片名称
        /// </summary>
        /// <param name="oldName">原来的名称</param>
        /// <param name="newName">新名称</param>
        /// <param name="imgType">图片类型</param>
        /// <returns></returns>
        [HttpPost]
        public string SaveImgName(string oldName, string newName, string imgType, string dpid)
        {
            return logic.SaveImgName(oldName, newName, imgType, dpid, HttpContext.Current);
        }
        /// <summary>
        /// 获取搭配
        /// </summary>
        /// <param name="sltDp">推荐店铺</param>
        /// <param name="pageNum">页码</param>
        /// <param name="tjDate">推荐时间</param>
        /// <returns></returns>
        [HttpGet]
        public DaPeiInfo GetDaiPei(string sltDp, int pageNum, string tjDate, string filterType, string filterValue)
        {
            DaPeiInfo rst = logic.GetDaiPei(sltDp, pageNum, tjDate, filterType, filterValue, HttpContext.Current);
            return rst;
        }
        /// <summary>
        /// 保存店铺的推荐搭配
        /// </summary>
        /// <param name="dpid">店铺id</param>
        /// <param name="tjdate">推荐日期</param>
        /// <param name="tjdp">推荐内容</param>
        /// <returns></returns>
        [HttpPost]
        public string SaveTuiJianDp(string dpid, string tjdate, string tjdp, string remark)
        {
            return logic.SaveTuiJianDp(dpid, tjdate, tjdp, remark);

        }
        /// <summary>
        /// 查询搭配信息
        /// </summary>
        /// <param name="dpid">店铺id</param>
        /// <param name="tjdate">推荐日期</param>
        /// <returns></returns>
        [HttpPost]
        public IList<DaPei> LookUpTjDp(string dpid, string tjdate)
        {
            return logic.LookUpTjDp(dpid, tjdate);
        }

        /// <summary>
        /// 查询所有的店铺推荐信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IQueryable LookAllTjDp()
        {
            return logic.LookAllTjDp();
        }
        /// <summary>
        /// 查询某店铺的推荐日期
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IQueryable LookAllTjDp(string dpId)
        {
            return logic.LookAllTjDp(dpId);
        }
        /// <summary>
        /// 根据主打款号，或者组合图获取搭配图片信息
        /// </summary>
        /// <param name="style">主打款号，或者组合图的名称</param>
        /// <param name="dpType">搭配类型</param>
        /// <param name="gtype">标志：single表示根据主打款查询，compose表示根据组合图进行查询</param>
        /// <returns></returns>
        [HttpPost]
        public IQueryable GetDpByMaster(string style, string dpType, string gtype)
        {
            return logic.GetDpByMaster(style, dpType, gtype);

        }
        [HttpGet]
        public IList<dianpu> GetDianpus()
        {
            return logic.GetDianpuList();

        }



    }
}
