using _20MVCEFAssignment.Models;
using _20MVCEFAssignment.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _20MVCEFAssignment.Controllers
{
    public class ProfileController : Controller
    {
        public ActionResult Edit(string id)
        {
            ViewBag.Message = "";
            AppRepository repo = new AppRepository();
            PersonModel model = new PersonModel();
            model = repo.GetProfile(id);
            model.Password = null;
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PersonModel model)
        {
            ViewBag.Message = "";
            try
            {
                AppRepository repo = new AppRepository();
                repo.RegisterOrUpdateUser(model);
                ViewBag.Message = "Profile has been updated Successfully!";
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "Something went wrong! Please try again later.");
                return View(model);
            }
        }

    }
}
