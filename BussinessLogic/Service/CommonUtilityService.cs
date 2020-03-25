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
        public static bool AllowEdit()
        {
            var AllowDeposit = commonService.GetUserAssignMenu(25 ,Global.UserId);
            if (AllowDeposit != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool AllowStatusChange()
        {
            var AllowDeposit = commonService.GetUserAssignMenu(26, Global.UserId);
            if (AllowDeposit != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
