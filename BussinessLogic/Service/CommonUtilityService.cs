using BussinessLogic.Repository;
using DataAccess.DatabaseModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BussinessLogic.Service
{
    public static class CommonUtilityService
    {
        public static GenericUnitOfWork uow = null;
        public static CommonService commonService = null;
        static CommonUtilityService()
        {
            uow = new GenericUnitOfWork();
            commonService = new CommonService();
        }
        public static SelectList CustomerTypeList()
        {
            var customerType = uow.Repository<CustomerType>().FindBy(x=>x.status==1).ToList();
            return new SelectList(customerType, "ID", "RFUniDesc");
        }
        public static bool AllowEdit(int menuId)
        {
            var AllowDeposit = commonService.GetUserAssignMenu(menuId, Global.UserId);
            if (AllowDeposit != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AllowStatusChange(int menuId)
        {
            var AllowDeposit = commonService.GetUserAssignMenu(menuId, Global.UserId);
            if (AllowDeposit != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static SelectList LedgerList()
        {
            var ledgerList = uow.Repository<BuisnessObject.ViewModel.MainViewModel.LedgerViewModel>().SqlQuery("select * from [dbo].[fgetledger]()").ToList();
            return new SelectList(ledgerList, "LedgerId", "LedgerName");
        }
        public static SelectList LocationList()
        {
            var LocationList = uow.Repository<LocationMaster>().GetAll().ToList();
            return new SelectList(LocationList, "Lid", "LocationName");
        }
        public static SelectList CustomerSearchOption()
        {
            List<SelectListItem> objCustomerSrchOption = new List<SelectListItem>();
            objCustomerSrchOption.Add(new SelectListItem { Text = "Customer Name", Value = "Customer Name" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Customer No", Value = "Customer No" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Mobile Number", Value = "Mobile No" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Address", Value = "Address" });
            //objCustomerSrchOption.Add(new SelectListItem { Text = "Customer Type", Value = "Customer Type" });
            return new SelectList(objCustomerSrchOption, "Value", "Text");
        }

        public static string CustomerType(int? cid)
        {
            var customer = uow.Repository<CustomerType>().FindBy(x=>x.Id==cid).Select(x=>x.RFUniDesc).SingleOrDefault();
            return customer;
        }

        public static string LedgerName(int? lid)
        {

            var ledgerList = uow.Repository<string>().SqlQuery("select LedgerName from [dbo].[fgetledger]() where ledgerId="+ lid).SingleOrDefault();
          
            return ledgerList;
        }


        public static string LocationName(int? lid)
        {

            var LocationList = uow.Repository<LocationMaster>().FindBy(x => x.Lid == lid).Select(x => x.LocationName).SingleOrDefault();

            return LocationList;
        }
    }
}
