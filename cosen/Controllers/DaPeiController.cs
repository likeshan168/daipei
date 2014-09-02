using cosen.Models;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;
namespace cosen.Controllers
{
    public class DaPeiController : Controller
    {

        /// <summary>
        /// 进行搭配
        /// </summary>
        /// <returns></returns>
        [Authorize]//如果没有登录的话，就会跳转到web.config中设置的登录界面中
        public ActionResult Arrange()
        {
            return View();
        }
        /// <summary>
        /// 推荐搭配
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult SuggestPairs()
        {
            LogicModel logic = new LogicModel();

            ViewData["list"] = logic.GetDianpus();
            return View();
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UploadImg()
        {
            LogicModel logic = new LogicModel();

            ViewData["list"] = logic.GetDianpus();
            return View();
        }
        /// <summary>
        /// 查看搭配
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult LookPairs()
        {
            return View();
        }

        /// <summary>
        /// UP4 软件中的图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult Up4Image()
        {
            return View(new List<string>());
        }
        /// <summary>
        /// 获取所有符合条件的款式
        /// </summary>
        /// <param name="styleNo"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult Up4Image(string styleNo)
        {
            LogicModel logic = new LogicModel();
            ViewData["styleNo"] = styleNo;
            return View(logic.GetUp4Images(styleNo));
        }
        /// <summary>
        /// 显示图片
        /// </summary>
        /// <param name="styleNo">款式代码</param>
        /// <returns></returns>
        [Authorize]
        public FileResult ShowImage(string styleNo)
        {
            LogicModel logic = new LogicModel();

            return File(logic.ShowImage(styleNo).ToArray(), "image/jpg");

        }
        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ActionResult Users()
        {
            return View();
        }
    }
}
