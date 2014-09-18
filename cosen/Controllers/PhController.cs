using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using cosen.Models;
namespace cosen.Controllers
{
    public class PhController : ApiController
    {
        [Ninject.Inject]
        private ILogicModel logicModel { get; set; }
        // GET api/ph
        public IEnumerable<ZhiZaoDanInfo> Get(string zdid)
        {
            return logicModel.GetZDHById(zdid);
        }
        /// <summary>
        /// 获取所有的待配货款号信息
        /// </summary>
        /// <param name="dh"></param>
        /// <returns></returns>
        [HttpPost]
        // POST api/ph
        public IEnumerable<ZhiZaoDanInfo> GetZDHById(string dh)
        {
            return logicModel.GetZDHById(dh);
        }
        /// <summary>
        /// 获取某一款的配货信息
        /// </summary>
        /// <param name="zdid"></param>
        /// <param name="style"></param>
        /// <returns></returns>
        [HttpPost]
        public IEnumerable<GetPeiHuo_ProcResult> GetPeiHuos(string zdid, string style)
        {
            return logicModel.GetPeiHuos(zdid, style);
        }
        /// <summary>
        ///获取店铺列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<dianpu> GetDianpus()
        {
            return logicModel.GetDianpuList();
        }

        /// <summary>
        /// 保存配货信息
        /// </summary>
        /// <param name="ph">配货信息</param>
        /// <returns></returns>
        [HttpPost]
        public string SavePh(GetPeiHuo_ProcResult ph)
        {
            return logicModel.SavePh(ph);
        }

        /// <summary>
        /// 删除配货信息
        /// </summary>
        /// <param name="style">款式+颜色</param>
        /// <param name="use_id">店铺id</param>
        /// <returns></returns>
        [HttpPost]
        public string DelPh(string style, string use_id)
        {
            return logicModel.DelPh(style, use_id);
        }

        [HttpGet]
        public IList<string> Get_Man_No()
        {
            return logicModel.Get_Man_No();
        }
    }
}
