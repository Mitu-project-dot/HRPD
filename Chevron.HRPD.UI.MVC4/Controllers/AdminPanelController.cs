using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chevron.HRPD.BusinessComponent;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.DataAccess;

namespace Chevron.HRPD.UI.MVC4.Controllers
{
    public class AdminPanelController : BaseController
    {
        private BaseContext db = new BaseContext();

       //OverTimeApplyComponent ot = new OverTimeApplyComponent();

        public ActionResult Index()
        {
           //   List<OverTimeApply> OverTimeApply = new List<OverTimeApply>();

           //OverTimeApply = ot.Find();

            //return View(OverTimeApply);
            return View();
        }

        //
        // GET: /AdminPanel/Details/5

        public ActionResult Details(string id = null)
        {
            //OverTimeApply overtimeapply = ot.FindByID(Convert.ToInt32(id));
            //if (overtimeapply == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(overtimeapply);
            return View();
        }

        //
        // GET: /AdminPanel/Create

       
    }
}