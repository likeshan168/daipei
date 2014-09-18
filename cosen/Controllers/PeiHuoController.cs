using cosen.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace cosen.Controllers
{
    public class PeiHuoController : Controller
    {
        [Ninject.Inject]
        private ILogicModel logicModel { get; set; }

        //public PeiHuoController()
        //{
        //    this.logicModel = new LogicModel();
        //}
        //
        // GET: /PeiHuo/
        [MyAuthorize(Roles = "system")]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuthorize]
        public FileResult OutputExcel(string zdid)
        {
            //LogicModel logic = new LogicModel();
            //byte[] fileContents = Encoding.Default.GetBytes(logic.PhExcel());
            //return File(fileContents, "application/ms-excel", "peihuo.xls");
            //string file = new LogicModel().PhExcel(zdid);
            string file = logicModel.PhExcel(zdid);
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            return File(fs, "application/ms-excel", "peihuo.xls");

        }
    }
}
