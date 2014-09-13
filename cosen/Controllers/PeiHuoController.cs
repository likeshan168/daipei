using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cosen.Controllers
{
    public class PeiHuoController : Controller
    {
        //
        // GET: /PeiHuo/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
