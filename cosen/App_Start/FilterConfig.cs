using cosen.Controllers;
using System.Web;
using System.Web.Mvc;

namespace cosen
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());//异常处理
            filters.Add(new MyHandleErrorAttribute());//添加自定义的错误处理
        }
    }
}