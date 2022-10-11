//using Chevron.HRPD.Security;
//using System.Web.Mvc;



/// <summary>     
///  Added by       : Umme Salma Mitu
///  Date           : 10.11.2018
///  Purpose        : This controller will handle all common functionality for all other controls. All other 
///                   controls will inherit the default property of this controls. For an example : display 
///                   log in user name, show version number etc.
///                   
/// 
/// </summary>

using System;
using System.Web.Mvc;
using Chevron.HRPD.Common.Helpers;
using Chevron.HRPD.UI.MVC4.Common;
using Chevron.HRPD.BusinessEntities.T;
using System.Web.SessionState;
using Chevron.HRPD.BusinessComponent;
using Chevron.HRPD.BusinessEntities;
using System.Web;
using System.Web.UI;

namespace Chevron.HRPD.UI.MVC4.Controllers
{
    [HandleError]    
    //[AuthorizeAD(Groups = ConstantsAD.HRPDAdminGroup + "," + ConstantsAD.HRPDSupervisorGroup + "," + ConstantsAD.HRPDEmployeeGroup)]
    [AuthorizeAD(Groups = ConstantsAD.HRPDAdminGroup + "," + ConstantsAD.HRPDSupervisorGroup + "," + ConstantsAD.HRPDEmployeeGroup)]
    public class BaseController : Controller
    {

        CurrentUser currentUserInfo = new CurrentUser();

        public BaseController()
        {
            try
            {
                HttpSessionStateBase session = new HttpSessionStateWrapper(System.Web.HttpContext.Current.Session);
                var stimout = session.Timeout;

                currentUserInfo = CurrentUserAD.GetCurrentUserInfo();

                System.Web.HttpContext.Current.Session["CAI"] = currentUserInfo.CAI;
                ViewData["CAI"] = currentUserInfo.CAI;

                ViewData["CurrentUser"] = " " + currentUserInfo.DisplayName.ToString();

                ViewData["isHRPDEmployee"] = currentUserInfo.isHRPDEmployee;
                ViewData["isHRPDSupervisorGroup"] = currentUserInfo.isHRPDSupervisor;
                ViewData["isHRPDAdminGroup"] = currentUserInfo.isHRPDAdmin;


                //ViewData["isHRPDEmployee"] = true;
                //ViewData["isHRPDSupervisorGroup"] = true;
                //ViewData["isHRPDAdminGroup"] = true;


                ViewData["SessionTimeOutvalue"] = (System.Web.HttpContext.Current.Session.Timeout * 1000 * 60) - 2; /// deducted 2 second to get the session name, value. if session is expired, session id and other value will aslo expired.

                if (session.IsNewSession)
                    ViewData["IsNewSession"] = true;
                else
                    ViewData["IsNewSession"] = false;

                //*** To Get Application Version information from Database


                string HRPDVersionInfo = string.Empty;
                string HRPDVersionReleaseDate = string.Empty;

                //*************Session Management****************//

                // string newSessionId = System.Web.HttpContext.Current.Session.SessionID;

                try
                {
                    if (session.IsNewSession || System.Web.HttpContext.Current.Session["HRPDUserSessionID"] == null)
                    {
                        System.Web.HttpContext.Current.Session["HRPDUserSessionID"] = session.SessionID;

                        UserLogInLogComponent ULILComp = new UserLogInLogComponent();
                        UserLogInLog entityUserLogInLog = new UserLogInLog();

                        entityUserLogInLog.CAI = currentUserInfo.CAI;
                        entityUserLogInLog.LoginDate = DateTime.Now;
                        entityUserLogInLog.LoginTime = DateTime.Now;
                        //entityUserLogInLog.LogoutTime = DateTime.Now.AddMinutes(stimout);
                        entityUserLogInLog.SessionID = session.SessionID.ToString();
                        entityUserLogInLog.IPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                        entityUserLogInLog.SessionLastHit = DateTime.Now;
                        entityUserLogInLog.SessionExpires = DateTime.Now.AddMinutes(stimout);

                        ULILComp.Persist(entityUserLogInLog);
                    }
                    else
                    {
                        string iCurrentSessionID = System.Web.HttpContext.Current.Session["HRPDUserSessionID"].ToString();

                        if (session.SessionID.ToString() == iCurrentSessionID)
                        {
                            UserLogInLogComponent ULILComp = new UserLogInLogComponent();

                            UserLogInLog entityUserLogInLog = ULILComp.FindBySessionID(iCurrentSessionID); //new UserLogInLog();

                            entityUserLogInLog.CAI = currentUserInfo.CAI;
                            entityUserLogInLog.IPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            entityUserLogInLog.SessionLastHit = DateTime.Now;
                            entityUserLogInLog.SessionExpires = DateTime.Now.AddMinutes(stimout);
                            ULILComp.Persist(entityUserLogInLog);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception Ex)
            {
                
            }
            //*************END of Session Management****************//
        }


        /// <summary>     
        ///  Added by       : Soumitra Bain
        ///  Date           : 10-Feb-2016
        ///  Purpose        : This methods will trace user Activity over the application.
        ///                   
        ///  Updated By     : Soumitra Bain
        ///  Date           : 14-Feb-2016
        ///  Purpose        : Implement User Log in Activity log with sessionID
        ///  
        ///  Updated By     : 
        ///  Date           : 
        ///  Purpose        : 
        /// </summary>
        

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
           
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerDescriptor.ControllerName;


            var actionParametrs = filterContext.ActionParameters;


            var valued = filterContext.Controller.ValueProvider;//.GetValue("ID").AttemptedValue.ToString();

            var routData = filterContext.RouteData;

            var testData = filterContext.Result;

            try
            {
                if(((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.ReturnType == typeof(ActionResult))
                {
                    string iRemarks =string.Empty;

                    foreach (var rmk in actionParametrs)
                    {                        
                        object obj = rmk.Value;
                        if ((obj != null))
                        {
                            try
                            {
                                iRemarks = "ID: " + obj.GetType().GetProperty("ID").GetValue(obj, null);
                            }
                            catch (Exception ex)
                            {
 
                            }
                        }
                    }

                    if (actionName == "Index")
                        iRemarks = "View " + controllerName + " Dashboard";

                    UserActivityLogComponent UALComp = new UserActivityLogComponent();

                    UserActivityLog UAL = new UserActivityLog();

                    UAL.ActionName = actionName;
                    UAL.ControllerName = controllerName;
                    UAL.CAI = currentUserInfo.CAI;
                    UAL.ActivityTime = DateTime.Now;
                    UAL.SessionID = session.SessionID;
                    UAL.Remarks = iRemarks;

                    UALComp.Persist(UAL);
                }

                // Only effective when the actionName is not duplicated in the controller.
               //// Type returnType = controllerType.GetMethod(actionName).ReturnType;
               // if (returnType.Name == "ActionResult")
               // {

               //     UserActivityLogComponent UALComp = new UserActivityLogComponent();

               //     UserActivityLog UAL = new UserActivityLog();

               //     UAL.ActionName = actionName;
               //     UAL.ControllerName = controllerName;
               //     UAL.CAI = currentUserInfo.CAI;
               //     UAL.ActivityTime = DateTime.Now;
               //     UAL.SessionID = session.SessionID;

               //     UALComp.Persist(UAL);
               // }
            }

            catch (Exception ex)
            {
 
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var descriptor = filterContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            var controllerName = descriptor.ControllerDescriptor.ControllerName;


            //var actionParametrs = filterContext.;


            var valued = filterContext.Controller.ValueProvider;//.GetValue("ID").AttemptedValue.ToString();

            var routData = filterContext.RouteData;

            var testData = filterContext.Result;
     

        }


        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            var cache = GetCache(filterContext);

            cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            cache.SetValidUntilExpires(false);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Get the reponse cache
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        protected virtual HttpCachePolicyBase GetCache(ResultExecutingContext filterContext)
        {
            return filterContext.HttpContext.Response.Cache;
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{

        //    Exception ex = filterContext.Exception;
        //    filterContext.ExceptionHandled = true;
        //    var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");


        //    ExceptionLogComponent expLogCom = new ExceptionLogComponent();
        //    ExceptionLog expEntity = new ExceptionLog();

        //    expEntity.CAI = currentUserInfo.CAI;
        //    expEntity.ControllerName = model.ControllerName;
        //    expEntity.ActionName = model.ActionName;
        //    expEntity.ExceptionTime = DateTime.Now;
        //    expEntity.ExceptionDetails = model.Exception.ToString();

        //    expLogCom.Persist(expEntity);

        //    //base.OnException(filterContext);
        //}
               
    }
}