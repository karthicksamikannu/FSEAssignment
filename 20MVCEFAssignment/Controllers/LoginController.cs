using _20MVCEFAssignment.Models;
using _20MVCEFAssignment.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace _20MVCEFAssignment.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppRepository repo = new Repository.AppRepository();
            Person person = repo.AuthenticateUser(model.UserName, model.Password);
            if (person != null)
            {
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                Session["UserName"] = model.UserName;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid Username or Password!");
                return View(model);
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Register()
        {
            ViewBag.Message = "";
            return View();
        }

        [HttpPost]
        public ActionResult Register(PersonModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppRepository repo = new Repository.AppRepository();
            if(repo.UserAlreadyExists(model.User_Id))
            {
                ModelState.AddModelError("", "User Already Exists!");
                return View(model);
            }
            try
            {
                if (repo.RegisterOrUpdateUser(model))
                {
                    ViewBag.Message = "User created Successfully! Please Log in to continue....";
                }
                else
                {
                    ViewBag.Message = "";
                    ModelState.AddModelError("", "Unable to create user now. Please try again later!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to create user now. Please try again later!");
            }
            return View(model);
        }
    }
}