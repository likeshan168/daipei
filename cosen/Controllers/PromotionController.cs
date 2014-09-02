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

        public ActionResult Index()
        {
            return View(new LogicModel().GetPromotionInfo());
        }

        [Authorize(Roles = "system")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "system")]
        public ActionResult Create(promotions pro)
        {
            LogicModel logic = new LogicModel();
            logic.AddPromotionInfo(pro);
            return RedirectToAction("index");
        }

        [HttpGet]
        [Authorize(Roles = "system")]
        public ActionResult Edit(int? id)
        {
            LogicModel logic = new LogicModel();

            return View(logic.GetPromotionInfoById(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "system")]
        public ActionResult Edit(promotions pro)
        {
            LogicModel logic = new LogicModel();
            logic.UpdatePromotionInfo(pro);
            return RedirectToAction("index");
        }
        [HttpGet]
        [Authorize(Roles = "system")]
        public ActionResult Delete(int? id)
        {
            LogicModel logic = new LogicModel();

            return View(logic.GetPromotionInfoById(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "system")]
        public ActionResult Delete(int id)
        {
            LogicModel logic = new LogicModel();
            logic.DelPromotionInfo(id);
            return RedirectToAction("index");
        }


        public JsonResult GetPromotionJson()
        {
            return Json(new LogicModel().GetPromotionInfo(), JsonRequestBehavior.AllowGet);
        }
    }
}
