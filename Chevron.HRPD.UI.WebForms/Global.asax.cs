using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Chevron.HRPD.Common.Interfaces;
using Chevron.HRPD.Common.Helpers;

namespace Chevron.HRPD.UI.WebForms
{
	public class Global : System.Web.HttpApplication
	{
		#region Application Events

		void Application_Start(object sender, EventArgs e)
		{
			//BuildContainer();
		}

		void Application_End(object sender, EventArgs e)
		{
			//CleanUp();
		}

        protected void Application_EndRequest(Object sender, EventArgs e)
        { 
            HttpContext context = HttpContext.Current;

            //Handles access denied error
            if (context.Response.Status.Substring(0,3).Equals("401"))
            {
                Response.Redirect(String.Format("~/ErrorPage.aspx?Message={0}","Access Denied."));
            } 
        }

		void Application_Error(object sender, EventArgs e)
		{
            /*
            string errorPolicy = "Global Policy";
         
            Exception exceptionToThrow = new Exception();
            
            Exception currentException = HttpContext.Current.Server.GetLastError();
            
            ExceptionManager exceptionManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();

            if (currentException is HttpUnhandledException)
            {
                currentException = currentException.InnerException;
            }
            
            if (exceptionManager.HandleException(currentException, errorPolicy, out exceptionToThrow))
            {
            
                if (exceptionToThrow == null)
                {
                    exceptionToThrow = currentException;
                }
                
                Response.Redirect(String.Format("~/ErrorPage.aspx?Message={0}", Uri.EscapeDataString(exceptionToThrow.Message)));
            }
            
            else
            {
                Server.ClearError();
                
                Server.Transfer(HttpContext.Current.Request.Url.PathAndQuery.ToString(), false);
            }
            */
		}

		#endregion

		#region Session Events

		void Session_Start(object sender, EventArgs e)
		{
			// Code that runs when a new session is started

		}

		void Session_End(object sender, EventArgs e)
		{
			// Code that runs when a session ends. 
			// Note: The Session_End event is raised only when the sessionstate mode
			// is set to InProc in the Web.config file. If session mode is set to StateServer 
			// or SQLServer, the event is not raised.

		}

		#endregion

		//#region Unity Container Constructor

		///// <summary>
		///// Build UnityContainer
		///// </summary>
		//private static void BuildContainer()
		//{
		//    IUnityContainer unityContainer = new UnityContainer();
		//    IUnityConfigurator configurator = new UnityConfiguratorHelper();
		//    configurator.Configure(unityContainer);
		//    HttpContext.Current.Application["UnityContainer"] = unityContainer;
		//}

		///// <summary>
		///// CleanUp Unity references
		///// </summary>
		//private static void CleanUp()
		//{
		//    if (HttpContext.Current.Application["MyContainer"] != null)
		//    {
		//        IUnityContainer unityContainer = HttpContext.Current.Application["MyContainer"] as IUnityContainer;
		//        unityContainer.Dispose();
		//        HttpContext.Current.Application["MyContainer"] = null;
		//    }
		//}

		//#endregion

	}
}
