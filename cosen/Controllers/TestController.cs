using System.Web;
using System.Web.Mvc;
using cosen.Models;

using System.Net;
namespace cosen.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        private LogicModel logic = null;
        public ActionResult Index()
        {
            logic = new LogicModel();
            return View(logic.GetDianpus());
        }
        [AcceptVerbs(HttpVerbs.Get)]

        public ActionResult Json()
        {
            //Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //Response.StatusCode = (int)HttpStatusCode.NotFound;
            //throw new HttpException((int)HttpStatusCode.InternalServerError, "自己抛出的异常");
            throw new HttpException((int)HttpStatusCode.NotFound, "没有找到页码");
            //return View();
        }

        public ActionResult Knockout()
        {

            //throw new HttpException((int)HttpStatusCode.InternalServerError, "服务器内部异常");
            return View();
        }


        public ActionResult KnockoutTest()
        {
            return View();
        }



        //public ContentResult ServiceTest()
        //{
        //    ProductionWebServiceClient client = new ProductionWebServiceClient();
        //    String ps = "{\"type\":{\"TYPE\":\"UPDATE_PRODUCTPUBLISH_STOCK\",\"DOCTYPE\":\"P\",\"STORENO\":\"HZ01\",\"SHOPPEID\":\"00432\",\"OPERATOR\":\"001\",\"TERMINALID\":\"7461724198439971\",\"MEMO\":\"\"},\"operdata\":[{\"ID\":\"tt013260\",\"ADDSTOCK\":\"40.0\"},{\"ID\":\"tt011230\",\"ADDSTOCK\":\"38.0\"}]}";
        //    string result = client.exChangeData(ps);

        //    return Content(result);
        //    //string[] names = { "andylau", "cosen lee", "jackchen" };
        //    //return Content("hello world");
        //}

        public ActionResult Knockout1()
        {
            return View();
        }


        public ActionResult Chart()
        {
            return View();
        }

        public ContentResult MD5()
        {
            return Content(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("cosen", "MD5"));
        }

    }
}
