using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Chevron.HRPD.Security;


namespace Chevron.HRPD.UI.WebForms
{
    public partial class _Default1 : System.Web.UI.Page
    {

        //Sets desired IBS layout
        protected void Page_LoadComplete(object sender, EventArgs e)
        { 
            Master.Layout = "layout-124 band-small welcome home search";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                lblWelcomeMessage.Text = "Hello " + CurrentUser.GetCurrentUser().FullName;
            }
        }
    }
}
