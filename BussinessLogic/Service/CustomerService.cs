using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuisnessObject.ViewModel;
using BussinessLogic.Repository;
using System.Data.SqlClient;
using Loader.Models;
using DataAccess.DatabaseModel;

namespace BussinessLogic.Service
{
    public class CustomerService
    {
        ReturnBaseMessageModel returnMessage = null;

        private GenericUnitOfWork uow = null;

        public CustomerService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();

        }

        public ReturnBaseMessageModel Save(MainViewModel.CustomerViewModel customer)
        {
            try
            {
                if (customer.PanNo == null){
                    customer.PanNo = "0";
                }
                uow.ExecWithStoreProcedure("[dbo].[PCreateCustomer] @Cid,@CustomerName,@CustomerTypeId,@PhoneNo,@MobileNo,@Address,@Email,@PanNo,@PostedBy",
                                                new SqlParameter("@Cid", customer.Cid),
                                                new SqlParameter("@CustomerName", customer.CustomerName),
                                                 new SqlParameter("@CustomerTypeId", customer.CustomerTypeId),
                                                new SqlParameter("@PhoneNo", customer.PhoneNo),
                                                new SqlParameter("@MobileNo", customer.MobileNo),
                                                new SqlParameter("@Address", customer.Address),
                                                new SqlParameter("@Email", customer.Email),
                                                 new SqlParameter("@PanNo", customer.PanNo),
                                                new SqlParameter("@PostedBy", Global.UserId)
                                                );
                if (customer.Cid == 0)
                {
                  

                    returnMessage.Msg = "Customer Added Sucessfully";
                    
                }
                else
                {
                    returnMessage.Msg = "Customer Edited Sucessfully";
                }
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Msg = "Customer Not Saved";
                returnMessage.Success = false;
                return returnMessage;

                throw ex;
            }
        }

        public ReturnBaseMessageModel changeStatus(int? cId)
        {
            //var statusChange = uow.ExecWithStoreProcedure("update Customer set status=@status where Cid =@cId",
            //new SqlParameter("status", 0),
            //new SqlParameter("cId", cId));
            //    returnMessage.Msg = "Status Changed Successfully!";
            var customer = uow.Repository<Customer>().FindBy(x => x.Cid == cId).FirstOrDefault();
            customer.Status = 0;
            uow.Repository<Customer>().Edit(customer);
            uow.Commit();
            returnMessage.Msg = "Status Changed Successfully!";
            returnMessage.Success = true;
            returnMessage.BoolValue = true;
            return returnMessage;
        }

        public MainViewModel.CustomerViewModel getSingleCustomerList(int? cId)
        {
            var customerList = uow.Repository<Customer>().FindBy(x => x.Cid == cId);
            var customerSingleList = customerList.Select(x => new MainViewModel.CustomerViewModel
            {
                CustomerName=x.CustomerName,
                CustomerTypeId=x.CustomerTypeId,
                PhoneNo=x.PhoneNo,
                MobileNo=x.MobileNo,
                Address=x.Address,
                Email=x.Email,
                PanNo=x.PanNo
            }).SingleOrDefault();
            return customerSingleList;
        }

        public List<MainViewModel.CustomerViewModel> getCustomerList(int? customerno,string Name, string Address, string contact, int? cType, int status, int pageNo, int pageSize)
        {
            try
            {

                string query = "";
                if (status == 1)
                {
                    query = "select  COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetCustomerTB]()  where  CustomerName like'%" + Name.Trim() + "%'";

                }
                else
                {
                    query = "select  COUNT(*) OVER () AS TotalCount,* from [dbo].[fgetDisabledCustomerTB]()  where  CustomerName like'%" + Name.Trim() + "%'";

                }
                //if (Name != "")
                //{
                //    query += " and Name like'" + Name + "%'";
                //}
                if (customerno != null&& customerno!=0)
                {
                    query += " and CustNo ="+ customerno;
                }
                if (contact != "")
                {
                    query += " and MobileNo like'%" + contact.Trim() + "%'";
                }
                if (Address != "")
                {
                    query += " and Address like'%" + Address.Trim() + "%'";
                }

                if (cType != 0 && cType != null)
                {
                    query += " and CustTypeId =" + cType;
                }
                query += @" ORDER BY  CustNo desc
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var customerList = uow.Repository<MainViewModel.CustomerViewModel>().SqlQuery(query).ToList();
               
                return customerList;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }


        public List<MainViewModel.CustomerViewModel> CustomerInfoList( string searchParameter, string searchOption, string mode, string type, int pageNo, int pageSize)
        {
            string query = "";
            query = "select COUNT(*) OVER () AS TotalCount,* from[dbo].[fgetCustomerTB]() ";
            if (searchParameter != "")
            {
                if (searchOption == "Customer No")
                {
                    query += " where CustNo =" + searchParameter;
                }
                else if (searchOption == "Customer Name")
                {
                    query += " where CustomerName like'%" + searchParameter + "%'";
                }
                else if (searchOption == "Mobile No")
                {
                    query += " where MobileNo like'%" + searchParameter + "%'";
                }
                else if (searchOption == "Address")
                {
                    query += " where Address like'%" + searchParameter + "%'";
                }
                else if (searchOption == "Customer Type")
                {
                    query += " where CustTypeId =" + searchParameter  ;
                }
            }

            query += @" ORDER BY  CustNo
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var customerList = uow.Repository<MainViewModel.CustomerViewModel>().SqlQuery(query).ToList();

            return customerList;
        }



        public MainViewModel.CustomerViewModel GetSelectedCustomer(int customerID, string custType)
        {
            MainViewModel.CustomerViewModel customerInfoList = new MainViewModel.CustomerViewModel();
           
                customerInfoList = uow.Repository<MainViewModel.CustomerViewModel>().SqlQuery(@"SELECT  CustNo ,CustomerName ,CustomerTypeId,MobileNo
      ,Address,Email ,PanNo,PostedOn FROM Customer where Cid={0} ", customerID).SingleOrDefault();
           

            return customerInfoList;
        }


  
        public MainViewModel.CustomerViewModel GetSelectedMultipleCustomer(int listBox)
        {
            //string customer = "";
            //foreach (var item in listBox)
            //{
            //    if (customer.Length > 0)
            //    {
            //        customer += ", ";
            //    }
            //    customer += item;
            //}
            var customerInfoList = uow.Repository<MainViewModel.CustomerViewModel>().SqlQuery(@"SELECT 
           CustNo ,CustomerName ,CustomerTypeId,MobileNo,Address ,Email,PanNo ,PostedOn
          FROM Customer where Cid ="+ listBox).SingleOrDefault();
            return customerInfoList;
        }
        public List<MainViewModel.CustomerViewModel> getCustomer(string prefix)
        {
            var customerList = uow.Repository<MainViewModel.CustomerViewModel>().SqlQuery("select CustomerName from Customer").Where(x => x.CustomerName.ToLower().Contains(prefix.ToLower())).ToList();
            
            return customerList;
        }
        public List<MainViewModel.CustomerViewModel> getAddress(string prefix)
        {
            var addressList = uow.Repository<MainViewModel.CustomerViewModel>().SqlQuery("select Address from Customer").Where(x => x.Address.ToLower().Contains(prefix.ToLower())).ToList();

            return addressList;
        }

    }
}

