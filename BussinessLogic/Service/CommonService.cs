using BuisnessObject.ViewModel;
using BussinessLogic.Repository;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{

    public class CommonService
    {
        public  GenericUnitOfWork uow = null;
        public CommonService()
        {
            uow = new GenericUnitOfWork();
        }
        public TaskVerificationList GetUserAssignMenu(int menuId, int userId)
        {
            var userAssignMenu = uow.Repository<TaskVerificationList>().SqlQuery("select * from fin.FgetCheckUserAssignMenu(@menuID) where UserId=" + userId
                , new SqlParameter("@menuID", menuId)).FirstOrDefault();
            return userAssignMenu;
        }
    }
}