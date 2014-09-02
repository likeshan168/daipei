using System.Web;
using System.Web.Mvc;

namespace cosen3
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());//指示所有的controller 的500错误
        }
    }
}