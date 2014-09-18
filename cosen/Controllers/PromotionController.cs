using cosen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cosen.Controllers
{
    public class PromotionController : Controller
    {
        [Ninject.Inject]
        private ILogicModel logicModel { get; set; }
        //public PromotionController()
        //{
        //    this.logicModel = new LogicModel();
        //}
        public ActionResult Index()
        {
            return View(logicModel.GetPromotionInfo());
        }

        [MyAuthorize(Roles = "system")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize(Roles = "system")]
        public ActionResult Create(promotions pro)
        {
          
            logicModel.AddPromotionInfo(pro);
            return RedirectToAction("index");
        }

        [HttpGet]
        [MyAuthorize(Roles = "system")]
        public ActionResult Edit(int? id)
        {
           

            return View(logicModel.GetPromotionInfoById(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize(Roles = "system")]
        public ActionResult Edit(promotions pro)
        {
          
            logicModel.UpdatePromotionInfo(pro);
            return RedirectToAction("index");
        }
        [HttpGet]
        [MyAuthorize(Roles = "system")]
        public ActionResult Delete(int? id)
        {
           
            return View(logicModel.GetPromotionInfoById(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [MyAuthorize(Roles = "system")]
        public ActionResult Delete(int id)
        {
           
            logicModel.DelPromotionInfo(id);
            return RedirectToAction("index");
        }


        public JsonResult GetPromotionJson()
        {
            return Json(logicModel.GetPromotionInfo(), JsonRequestBehavior.AllowGet);
        }
    }
}
