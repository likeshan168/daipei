using System;
using System.Web.Mvc;
using System.Web.Http.Filters;
namespace cosen.Controllers
{
    /// <summary>
    /// 自定义异常处理应用于控制器级别的(这个是针对mvc的，但是它不会处理web api 中的异常)
    /// </summary>
    public class MyHandleErrorAttribute : HandleErrorAttribute
    {
        //添加错误日志去数据库中
        public override void OnException(ExceptionContext filterContext)
        {
            using (DataContextDataContext dataContext = new DataContextDataContext())
            {
                dataContext.ExceptionLog.InsertOnSubmit(new ExceptionLog() 
                { 
                    ErrorDate=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Message=filterContext.Exception.Message,
                    ExceptionType = ExceptionType.Name,
                    StackTrace=filterContext.Exception.StackTrace
                });
                dataContext.SubmitChanges();
                //filterContext.ExceptionHandled = true;//我们这里不要设置，因为我还要显示错误的页面出来

            }
        }
    }
    /// <summary>
    /// 自定义web api 异常处理(注意不能处理HttpResponseException异常)
    /// </summary>
    public class MyWebApiHandlerErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            using (DataContextDataContext dataContext = new DataContextDataContext())
            {
                dataContext.ExceptionLog.InsertOnSubmit(new ExceptionLog()
                {
                    ErrorDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Message = actionExecutedContext.Exception.Message,
                    ExceptionType = "webapi",
                    StackTrace = actionExecutedContext.Exception.StackTrace
                });
                dataContext.SubmitChanges();
                //filterContext.ExceptionHandled = true;//我们这里不要设置，因为我还要显示错误的页面出来

            }
        }
    }
}
