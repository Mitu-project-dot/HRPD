using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Chevron.HRPD.UI.WebForms
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If it's running under DEBUG mode it will show the error description, If not, user friendly message will be displayed
            #if DEBUG
                if (string.IsNullOrEmpty(Request.QueryString["Message"]))
                {
                    ErrorMessage.Text = "Unhandled exception";
                }
                else
                {
                    ErrorMessage.Text = Request.QueryString["Message"].ToString();
                }
            #else
                ErrorMessage.Text = "The system encountered an unexpected error - please contact support.";
#endif
        }
        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            Master.Layout = "layout-16 welcome";
        }
    }
}