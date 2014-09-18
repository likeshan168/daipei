using cosen.Models;
using System;
using System.Text;
using System.Web.Mvc;

namespace cosen.Controllers
{
    public class ReportController : Controller
    {
        [Ninject.Inject]
        private ILogicModel logicModel { get; set; }
        //public ReportController()
        //{
        //    this.logicModel = new LogicModel();
        //}
        [MyAuthorize]
        public ActionResult Index()
        {
            DateTime endDate = DateTime.Now;

            DateTime startDate = endDate.AddMonths(-1);

            ViewData["startDate"] = startDate.ToString("yyyy-MM-dd");
            ViewData["endDate"] = endDate.ToString("yyyy-MM-dd");
            int sm = startDate.Month;
            int em = endDate.Month;
            ViewData["startMonth"] = sm < 10 ? "0" + sm.ToString() : sm.ToString();
            ViewData["endMonth"] = em < 10 ? "0" + em.ToString() : em.ToString();
            return View();
        }
        [MyAuthorize]
        [HttpPost]
        public JsonResult Search(string sltType, string dps, int pageNum, string startDate, string endDate, string styleNo, string sort, string sortT)
        {
            //return Content(dps+":"+pageNum+":"+startDate+":"+endDate);
            //LogicModel logic = new LogicModel();

            return Json(logicModel.GetReportsInfo(sltType, dps, pageNum, startDate, endDate, styleNo, sort, sortT));
        }
        /// <summary>
        /// 改上市日期
        /// </summary>
        /// <param name="styleNo">款式</param>
        /// <param name="date">上市日期</param>
        /// <returns></returns>
        [HttpPost]
        [MyAuthorize(Roles = "system")]//只有系统管理员才有权限进行日期的修改
        public ContentResult UpdateDate(string styleNo, string date, int? cxinfo)
        {
            return Content(logicModel.UpdateDate(styleNo, date, cxinfo));
        }
        [MyAuthorize(Roles = "system")]//只有管理员才能有权限导出excel
        public FileResult OutExcel()
        {
            //LogicModel logic = new LogicModel();
            //byte[] fileContents = Encoding.Default.GetBytes(logic.OutExcel());
            //return File(fileContents, "application/ms-excel", "fileContents.xls");
            return null;


        }

    }
}
