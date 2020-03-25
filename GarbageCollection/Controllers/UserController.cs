using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GarbageCollection.Models;
using Microsoft.AspNet.Identity.Owin;

namespace GarbageCollection.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public ActionResult Create(int UserId=0)
        {
            RegisterViewModel registerView = new RegisterViewModel();
            ApplicationUser userView = new ApplicationUser();
            if (UserId != 0)
            {
               // userView=UserManager.FindByIdAsync
            }
            return View(registerView);
        }
        //[HttpPost]
        //public ActionResult Create(RegisterViewModel registerViewModel )
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
                    

        //        }
        //        catch (Exception ex)
        //        {
                   
        //            throw;
        //        }
        //    }
        //}
    }
}