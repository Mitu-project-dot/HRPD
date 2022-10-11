using Chevron.HRPD.BusinessComponent;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.BusinessEntities.T;
using Chevron.HRPD.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Chevron.HRPD.UI.MVC4.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ErrorController()
        {
            CurrentUser currentUserInfo = new CurrentUser();

            currentUserInfo = CurrentUserAD.GetCurrentUserInfo();

            ViewData["CurrentUser"] = " " + currentUserInfo.DisplayName.ToString();

            ViewData["SessionTimeOutvalue"] = (System.Web.HttpContext.Current.Session.Timeout * 1000 * 60) - 2; /// deducted 2 second to get the session name, value. if session is expired, session id and other value will aslo expired.

            ViewData["UserType"] = "UnAuthorized";
        }


        public ActionResult Index()
        {
            return View("Error");
        }

        public ActionResult Unauthorized()
        {
            return View("NotAuthorized");
            //return View();
        }
        public ActionResult NotPermisible()
        {
            return View("NotPermisible");
            //return View();
        }

        public ActionResult SessionExpired()
        {
            CurrentUser currentUserInfo = new CurrentUser();

            currentUserInfo = CurrentUserAD.GetCurrentUserInfo();

            ViewData["CurrentUser"] = " " + currentUserInfo.DisplayName.ToString();

            ViewData["UserType"] = currentUserInfo.isHRPDAdmin;


            ViewData["SessionTimeOutvalue"] = (System.Web.HttpContext.Current.Session.Timeout * 1000 * 60) - 2; /// deducted 2 second to get the session name, value. if session is expired, session id and other value will aslo expired.



            if (System.Web.HttpContext.Current.Session["HRPDUserSessionID"] != null)
            {
                string iCurrentSessionID = System.Web.HttpContext.Current.Session["HRPDUserSessionID"].ToString();

                if (!string.IsNullOrEmpty(iCurrentSessionID))
                {

                    UserLogInLogComponent ULILComp = new UserLogInLogComponent();

                    UserLogInLog entityUserLogInLog = ULILComp.FindBySessionID(iCurrentSessionID); //new UserLogInLog();


                    entityUserLogInLog.CAI = currentUserInfo.CAI;
                    //entityUserLogInLog.LoginDate = DateTime.Now;                
                    entityUserLogInLog.SessionID = iCurrentSessionID;
                    entityUserLogInLog.IPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    entityUserLogInLog.SessionExpires = DateTime.Now;
                    entityUserLogInLog.LogoutTime = DateTime.Now;
                    ULILComp.Persist(entityUserLogInLog);
                }
            }

            return View("SessionExpired");
        }

    }
}
