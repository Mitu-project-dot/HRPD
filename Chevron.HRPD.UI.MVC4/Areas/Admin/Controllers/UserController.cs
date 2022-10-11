using Chevron.HRPD.BusinessComponent;
using Chevron.HRPD.BusinessEntities;
using Chevron.HRPD.Common.Helpers;
using Chevron.HRPD.UI.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Chevron.HRPD.UI.Areas.Admin.Controllers
{
    //public class UserController : AdminBaseController
    //{
    //    private UserComponent userComponent = new UserComponent();

    //    public ActionResult Index()
    //    {
    //        List<User> users = userComponent.Find();
    //        return View(users);
    //    }

    //    public ActionResult Create()
    //    {
    //        var viewModel = new UserViewModel
    //        {
    //            user = new User(),
    //            roles = userComponent.GetRoles()
    //        };
    //        return View(viewModel);
    //    }

    //    [HttpPost]
    //    public ActionResult Create(UserViewModel uservm, FormCollection formVals)
    //    {
    //        User newUser = uservm.user;
    //        if (ModelState.IsValid)
    //        {
    //            User loginUser = userComponent.FindByCAI(CurrentUserAD.GetCurrentUser().CAI);


    //            newUser.CreatedBy = CurrentUser.GetCurrentUser().SysUserID;
    //            newUser.CreatedBy = (int)loginUser.ID;

    //            newUser.CreateDate = DateTime.Now;
    //            newUser.CAI = newUser.CAI.ToUpper();

    //            if (!string.IsNullOrEmpty(formVals["role"]))
    //                userComponent.UpdateRoles(newUser, formVals["role"]);

    //            userComponent.Persist(newUser);
    //            return RedirectToAction("Index");
    //        }

    //        var viewModel = new UserViewModel
    //        {
    //            user = newUser,
    //            roles = userComponent.GetRoles()
    //        };
    //        return View(viewModel);
    //    }

    //    public ActionResult Edit(string cai)
    //    {
    //        User u = userComponent.FindByCAI(cai);

    //        var viewModel = new UserViewModel
    //        {
    //            user = u,
    //            roles = userComponent.GetRoles()
    //        };

    //        return View(viewModel);
    //    }

    //    [HttpPost]
    //    public ActionResult Edit(User user, FormCollection formVals)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            user.UpdatedBy = CurrentUserAD.GetCurrentUser().SysUserID;
    //            user.UpdateDate = DateTime.Now;

    //            if (!string.IsNullOrEmpty(formVals["role"]))
    //                userComponent.UpdateRoles(user, formVals["role"]);

    //            userComponent.Persist(user);
    //            return RedirectToAction("Index");
    //        }

    //        var viewModel = new UserViewModel
    //        {
    //            user = user,
    //            roles = userComponent.GetRoles()
    //        };
    //        return View(viewModel);
    //    }

    //    [HttpPost]
    //    public JsonResult PersonSearch(string Cai, string FirstName, string LastName)
    //    {
    //        List<dynamic> retVal = new Security.GPDA.PeopleSearch().FindUsers(Cai, FirstName, LastName);

    //        return Json(retVal, JsonRequestBehavior.DenyGet);
    //    }

    //    public ActionResult Delete(string cai)
    //    {
    //        User u = userComponent.FindByCAI(cai);

    //        return View(u);
    //    }

    //    [HttpPost, ActionName("Delete")]
    //    public ActionResult DeleteConfirmed(string cai)
    //    {
    //        User u = userComponent.FindByCAI(cai);

    //        if (u.ID.HasValue)
    //            userComponent.Delete(u.ID.Value);

    //        return RedirectToAction("Index");
    //    }
    //}



}
