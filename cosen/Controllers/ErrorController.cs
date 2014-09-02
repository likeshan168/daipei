using System.Web.Mvc;

namespace cosen.Controllers
{
    public class ErrorController : Controller
    {
        //
        // 500

        public ActionResult HttpError()
        {
            return View("Error");//显示error视图
        }
        //404
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Index()
        {
            return RedirectToAction("Home", "Index");
        }
    }
}
