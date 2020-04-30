using BuisnessObject.ViewModel;
using BussinessLogic.Service;
using DataAccess.DatabaseModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GarbageCollection.Controllers
{
    public class CustomerUserController : Controller
    {
        private CustomerUserService customerUserService = null;
        public CustomerUserController()
        {
            customerUserService = new CustomerUserService();
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
                
                var returnmessage = customerUserService.SaveCustomerUser(customerUserViewModel);
                return Json(returnmessage, JsonRequestBehavior.AllowGet);
            }
            catch  (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
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


        public ActionResult _List(string name="", int pageNo = 1, int pageSize = 10)
        {
            CustomerUserViewModel customerViewModel = new CustomerUserViewModel();
            var customerList = customerUserService.getUserCustomerList(name, 1, 10);
            customerViewModel.customeruserPagedList = new StaticPagedList<CustomerUserViewModel>(customerList, pageNo, pageSize, (customerList.Count == 0) ? 0 : customerList.FirstOrDefault().TotalCount);
            return PartialView(customerViewModel);
        }
    }
}