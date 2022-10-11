using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chevron.HRPD.UI.MVC4;

namespace Chevron.HRPD.UI.Areas.Admin.Controllers
{
    [AuthorizeRedirect(Roles="Administrator")]
    //[AuthorizeRedirect]
    public abstract class AdminBaseController : Controller
    {
        
    }
}
