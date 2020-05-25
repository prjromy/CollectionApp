using BuisnessObject.ViewModel;
using BussinessLogic.Service;
using DataAccess.DatabaseModel;
using Loader.App_Start;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace GarbageCollection.Controllers
{
    public class CustomerUserController : Controller
    {
        private CustomerUserService customerUserService = null;
        private ApplicationCustomerManager _userManager;
        private ApplicationCustomerSignInManager _signInManager;
       
       
        public CustomerUserController()
        {
            customerUserService = new CustomerUserService();
           

        }

        //public CustomerUserController(ApplicationCustomerManager userManager, ApplicationCustomerSignInManager signInManager)
        //{
           

        //    UserManager = userManager;
        //    SignInManager = signInManager;
            
        //}
        
        public ApplicationCustomerManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationCustomerManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationCustomerSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationCustomerSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        // GET: CustomerUser

        public JsonResult CheckPassword(string ReEnterPassword, string PasswordHash)
        {
            if (ReEnterPassword == PasswordHash)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Create(int? userid)
        {
            CustomerUserViewModel user = new CustomerUserViewModel();
            if (userid != 0&&userid!=null)
            {
               
                   user = customerUserService.getSingleCustomerList(userid);
                ViewBag.Customer = customerUserService.getCustomerName(user.CustomerId);
                ViewBag.Id = userid;
            }
            else
            {
                ViewBag.Id = 0;
            }
          
            return PartialView(user);
        }


        public ActionResult CustomerUserSave(CustomerUserViewModel customerUserViewModel)
        {
            try
            {
                if (customerUserViewModel.UserId == 0)
                {
                    PasswordHasher pass = new PasswordHasher();
                    customerUserViewModel.PasswordHash = pass.HashPassword(customerUserViewModel.PasswordHash);
                }
              
                var message = customerUserService.SaveCustomerUser(customerUserViewModel);
                return Json(message, JsonRequestBehavior.AllowGet);
            }
           catch(Exception ex)
            {
                throw ex;
            }

        }
        //public async Task<ActionResult> CustomerUserSave(CustomerUserViewModel customerUserViewModel)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {


        //            var user = new Loader.Models.CustomerUser { UserName = customerUserViewModel.Email, Email = customerUserViewModel.Email };
        //            var result = await UserManager.CreateAsync(user, customerUserViewModel.PasswordHash);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //                // Send an email with this link
        //                // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //                // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //                // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

        //                return RedirectToAction("Create", "CustomerUser");
        //            }


        //            else
        //            {

        //                return JavaScript(result.Errors.First().ToString());

        //            }

        //            //return Json(returnmessage, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            var err = ModelState.Values.SelectMany(v => v.Errors);
        //            return JavaScript(err.ToString());

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return JavaScript(ex.Message);
        //    }
        //}
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [HttpPost]
        public JsonResult _GetCustomerAutoCompleteTreePopUp(string term)
        {


            List<Customer> tree = new List<Customer>();
            tree = customerUserService.GetCustomerAutoCompleteBranchGroupTree(term);
            return Json(tree);

        }
        public ActionResult List()
        {
            try
            {
                CustomerUserViewModel customerViewModel = new CustomerUserViewModel();
                var customerList = customerUserService.getUserCustomerList("",  1, 10);
                customerViewModel.customeruserPagedList = new StaticPagedList<CustomerUserViewModel>(customerList, 1, 10, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);

                //foreach (var item in customerList)
                //{
                //    customerViewModel.customerViewModelList.Add(item);
                //}

                return PartialView(customerViewModel);


            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public JsonResult CheckUsernameAvailable(string UserName, int UserId = 0)
        {
            bool isExists = customerUserService.CheckUsername(UserName, UserId);
            return Json(isExists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _List(string name="", int pageNo = 1, int pageSize = 10)
        {
            CustomerUserViewModel customerViewModel = new CustomerUserViewModel();
            var customerList = customerUserService.getUserCustomerList(name, pageNo, pageSize);
            customerViewModel.customeruserPagedList = new StaticPagedList<CustomerUserViewModel>(customerList, pageNo, pageSize, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);
            return PartialView(customerViewModel);
        }
    }
}