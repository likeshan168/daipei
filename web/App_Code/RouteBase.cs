using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace web.App_Code
{
    public abstract class RouteBase
    {
        public abstract RouteData GetRouteData(HttpContextBase httpContext);
    }
}