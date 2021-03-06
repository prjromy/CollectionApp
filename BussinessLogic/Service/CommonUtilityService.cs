﻿using BussinessLogic.Repository;
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

        public static SelectList MonthList()
        {
            var Month= uow.Repository<Month>().GetAll().ToList();
            return new SelectList(Month, "Id", "Name");
        }
       
        public static SelectList CustomerStatusList()
        {
            List<SelectListItem> objAccountType = new List<SelectListItem>();
            objAccountType.Add(new SelectListItem { Text = "Active", Value = "1" });
            objAccountType.Add(new SelectListItem { Text = "Disabled", Value = "0" });
      
            return new SelectList(objAccountType, "Value", "Text");
        }
        public static SelectList EntryTypeList()
        {
            List<SelectListItem> objAccountType = new List<SelectListItem>();
            objAccountType.Add(new SelectListItem { Text = "Mobile" });
            objAccountType.Add(new SelectListItem { Text = "Web" });

            return new SelectList(objAccountType, "Value", "Text");
        }
        
        public static SelectList VerifiedUnverifiedList()
        {
            List<SelectListItem> objAccountType = new List<SelectListItem>();
            objAccountType.Add(new SelectListItem { Text = "Verified", Value = "1" });
            objAccountType.Add(new SelectListItem { Text = "Unverified", Value = "2" });

            return new SelectList(objAccountType, "Value", "Text");
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
        public static bool AllowPasswordChange(int menuId)
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
        public static SelectList SubscriptionSearchOption()
        {
            List<SelectListItem> objCustomerSrchOption = new List<SelectListItem>();
            objCustomerSrchOption.Add(new SelectListItem { Text = "Customer Name", Value = "Customer Name" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Customer No", Value = "Customer No" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Subscription No", Value = "Subscription No" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Mobile Number", Value = "Mobile No" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Address", Value = "Address" });
            //objCustomerSrchOption.Add(new SelectListItem { Text = "Customer Type", Value = "Customer Type" });
            return new SelectList(objCustomerSrchOption, "Value", "Text");
        }

        public static SelectList Printed()
        {
            List<SelectListItem> printed = new List<SelectListItem>();
      
            printed.Add(new SelectListItem { Text = "New", Value = "New" });
            printed.Add(new SelectListItem { Text = "Printed", Value = "Printed" });
            //objCustomerSrchOption.Add(new SelectListItem { Text = "Customer Type", Value = "Customer Type" });
            return new SelectList(printed, "Value", "Text");
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
        public static string UserName(int? userid)
        {

            var LocationList = uow.Repository<User>().FindBy(x => x.UserId == userid).Select(x => x.UserName).SingleOrDefault();

            return LocationList;
        }
        public static string getCustomerName(int? customerId)
        {
            var userName = uow.Repository<Customer>().FindBy(x => x.Cid == customerId).Select(x => x.CustomerName).SingleOrDefault();
            return userName;
        }
        public static string getCollectorName(int? collectorid)
        {
            var userName = uow.Repository<DataAccess.DatabaseModel.Employee>().FindBy(x => x.EmployeeId == collectorid).Select(x => x.EmployeeName).SingleOrDefault();
            return userName;
        }

        public static SelectList Year()
        {
            var yearList = uow.Repository<YearListofAD>().GetAll().ToList();
            return new SelectList(yearList,"Id",  "Yr");
        }

        public static SelectList Month()
        {
            var monthList = uow.Repository<MonthAD>().GetAll().ToList();
            return new SelectList(monthList, "Id", "Month");
        }

    }
}
