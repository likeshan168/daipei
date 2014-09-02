using cosen.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace cosen.Controllers
{
    public class AccountController : Controller
    {

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogOn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="loginModel"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]//防止csrf攻击
        public ActionResult LogOn(LoginModel loginModel, string returnUrl)
        {
            
            
            if (ModelState.IsValid && Membership.ValidateUser(loginModel.UserName,loginModel.Password))
            {
                //没有对cookie设置过期时间的话，关闭浏览器就会退出登录
                FormsAuthentication.SetAuthCookie(loginModel.UserName, false);

                //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(loginModel.UserName,false,30);
                //string hashCookie=FormsAuthentication.Encrypt(ticket);
                //HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookie);
                ////cookie.Expires = DateTime.Now.AddSeconds(0);
                //Response.Cookies.Add(cookie);
                return RedirectToLocal(returnUrl);
            }
            ModelState.AddModelError("", "用户名或者密码不正确。");
            return View(loginModel);
        }
        
       

        [HttpGet]
        public ActionResult AddUser(LoginModel model)
        {
            return View();
        }
        /// <summary>
        /// 注销不退出浏览器
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]//只有登录过的用户才能进行注销操作
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 退出浏览器
        /// </summary>
        [HttpGet]
         public string Exit()
        {
            FormsAuthentication.SignOut();
            return "ok";
        }

        public ActionResult GetRoles(LoginModel model)
        {
            string[] roles = Roles.GetAllRoles();
            //string[] users = Membership.GetAllUsers();
            return View(roles);
        }
        //帮助程序
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
