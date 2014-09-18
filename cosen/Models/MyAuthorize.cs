using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace cosen.Models
{
    /// <summary>
    /// 自定义验证
    /// </summary>
    public class MyAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            IPrincipal user = httpContext.User;//获取用户对象
            if (!user.Identity.IsAuthenticated)//判断是否已经通过验证
                return false;

            //接下来看角色符不符合要求
            /*
             *1.获取当前用户的角色
             *2.和指定的角色进行比较
             *3.和指定的用户名称进行比较
             */

            if (!string.IsNullOrEmpty(Users))
            {
                return Users.Split(',').Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase);
            }

            if (!string.IsNullOrEmpty(Roles))
            {
                get_user_roleResult role;
                using (DataContextDataContext db = new DataContextDataContext())
                {
                    role = db.get_user_role(user.Identity.Name).FirstOrDefault();
                }
                if (role == null) return false;
                return Roles.Split(',').Contains(role.role_name, StringComparer.OrdinalIgnoreCase);
            }
            return true;
        }
    }
}