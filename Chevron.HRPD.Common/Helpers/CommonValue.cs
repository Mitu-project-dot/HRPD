


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;


namespace Chevron.HRPD.Common.Helpers
{
    public static class CommonValue
    {

        public static string GetLogINUSer()
        {
            string userLogin = System.Security.Principal.WindowsIdentity.GetCurrent().Name;// Request.ServerVariables["LOGON_USER"].ToString();

            return userLogin;
        }
    }
}

