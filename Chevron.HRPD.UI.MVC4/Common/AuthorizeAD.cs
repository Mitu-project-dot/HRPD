//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace Chevron.HRPD.UI.MVC4.Common
//{
//    public class AuthorizeAD
//    {
//    }
//}\





using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices.AccountManagement;
using Chevron.HRPD.Common.Helpers;



namespace Chevron.HRPD.UI.MVC4.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAD : AuthorizeAttribute
    {
        private const string IS_AUTHORIZED = "isAuthorized";

        public string RedirectUrl = "~/error/unauthorized";

        public string Groups { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                if (base.AuthorizeCore(httpContext))
                {
                    /* Return true immediately if the authorization is not 
                    locked down to any particular AD group */
                    if (String.IsNullOrEmpty(Groups))
                        return true;

                    // Get the AD groups
                    var groups = Groups.Split(',').ToList();

                    // Verify that the user is in the given AD group (if any)
                    var context = new PrincipalContext(
                                          ContextType.Domain, ConstantsAD.Domain
                                          );

                    var userPrincipal = UserPrincipal.FindByIdentity(
                                           context,
                                           IdentityType.SamAccountName,
                                           httpContext.User.Identity.Name);

                    foreach (var group in groups)
                        try
                        {
                            if (userPrincipal.IsMemberOf(context,
                                 IdentityType.Name,
                                 group))
                            {
                                //Guid groupguid = group.Guid;
                                return true;
                            }
                        }
                        catch (Exception exc)
                        {
                            return false;
                        }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(
        AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                //var result = new ViewResult();
                //result.ViewName = "NotAuthorized";
                //result.MasterName = "_Layout";
                //filterContext.Result = result;

                filterContext.RequestContext.HttpContext.Response.Redirect(RedirectUrl);
            }
            else
                base.HandleUnauthorizedRequest(filterContext);
        }
    }
}