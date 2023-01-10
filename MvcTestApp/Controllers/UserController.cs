using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcTestApp.Models;

namespace MvcTestApp.Controllers
{
    public class UserController : Controller
    {

        private UserDBHandle dbHandler = new UserDBHandle();
        // GET: User
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //GET: Login User
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ViewCategory", "Category");
            }
            return View();
        }

        //Verify User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel user)
        {
            if (dbHandler.IsValidUser(user.EmailAddress, user.Password))
            {
                FormsAuthentication.SetAuthCookie(user.EmailAddress, false);
                ViewBag.Message = "Login Successful";
                return RedirectToAction("ViewCategory", "Category");
            }
            else
            {
                ViewBag.Message = "Invalid Email Address or Password";
            }
            return View();
        }

        //GET: SignUp USer
        public ActionResult SignUp()
        {
            return View();
        }

        //POST: SignUp User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!dbHandler.IfUserExist(user.EmailAddress))
                    {
                        if (dbHandler.SignUp(user))
                        {
                            FormsAuthentication.SetAuthCookie(user.EmailAddress, false);
                            ViewBag.Message = "User Inserted successfully";
                            ModelState.Clear();
                            return RedirectToAction("Login");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Something went wrong");
                        }
                    }
                    else
                    {
                        ViewBag.Message = "User already exists";

                        ModelState.Clear();
                        ModelState.AddModelError("", "User already exists");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Data");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View();
        }

        //GET: Logout User
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        }
    }
}