﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DatabaseModel;
using BussinessLogic.Repository;
using BuisnessObject.ViewModel;
using System.Security.Cryptography;

namespace BussinessLogic.Service
{
    public class CustomerUserService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnBaseMessageModel = null;
        public CustomerUserService()
        {
            uow = new GenericUnitOfWork();
            returnBaseMessageModel = new ReturnBaseMessageModel();

        }
        public List<Customer> GetCustomerAutoCompleteBranchGroupTree(string term)
        {

            try
            {
                var treelist = uow.Repository<Customer>().FindBy(x => x.CustomerName.ToLower().Contains(term.ToLower())).ToList();
                List<Customer> list = treelist;
                return list;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public ReturnBaseMessageModel SaveCustomerUser(CustomerUserViewModel user)
        {
            try
            {
                var singleUser = uow.Repository<CustomerUser>().FindBy(x => x.CustomerId == user.CustomerId).SingleOrDefault();
                if (singleUser == null)
                {
                    singleUser = new CustomerUser();
                    singleUser.PasswordHash = HashPassword(user.PasswordHash);
                    singleUser.CustomerId = user.CustomerId;
                    singleUser.EffDate = user.EffDate;
                    singleUser.TillDate = user.TillDate;
                    singleUser.UserName = user.UserName;
                    singleUser.MTId = user.MTId;
                    singleUser.Email = user.Email;
                    singleUser.IsActive = user.IsActive;
                    singleUser.IsUnlimited = user.IsUnlimited;
                    uow.Repository<CustomerUser>().Add(singleUser);
                 

                }

                else
                {
                   
                    singleUser.CustomerId = user.CustomerId;
                    singleUser.EffDate = user.EffDate;
                    singleUser.TillDate = user.TillDate;
                    singleUser.UserName = user.UserName;
                    singleUser.MTId = user.MTId;
                    singleUser.Email = user.Email;
                    singleUser.IsActive = user.IsActive;
                    singleUser.IsUnlimited = user.IsUnlimited;
                  
                    uow.Repository<CustomerUser>().Edit(singleUser);
                   
                   


                }
                uow.Commit();
                returnBaseMessageModel.Success = true;
                returnBaseMessageModel.Msg = "Customer  Created Successfully !";
                return returnBaseMessageModel;

            }
            catch(Exception ex)
            {
                returnBaseMessageModel.Success = false;
                returnBaseMessageModel.Msg = "Error";
                return returnBaseMessageModel;
            }

        }

        public string getCustomerName(int? customerId)
        {
            var userName = uow.Repository<Customer>().FindBy(x => x.Cid == customerId).Select(x => x.CustomerName).SingleOrDefault();
            return userName;
        }



        // Hashing:

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
//        Verifying:

public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        public static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }
        public List<CustomerUserViewModel> getUserCustomerList(string Name ,int pageNo, int pageSize)
        {
            try
            {

                string query = "";
               
                    query = "select  COUNT(*) OVER () AS TotalCount,* from [LG].[CustomerUser]  where  UserName like'%" + Name.Trim() + "%'";

               
            
                query += @" ORDER BY  CustomerUserId 
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
                var customerList = uow.Repository<CustomerUserViewModel>().SqlQuery(query).ToList();

                return customerList;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public CustomerUserViewModel getSingleCustomerList(int? userID)
        {
            var customerList = uow.Repository<CustomerUser>().FindBy(x => x.CustomerUserId == userID);
            var customerSingleList = customerList.Select(x => new CustomerUserViewModel
            {
                CustomerId = x.CustomerId,
                UserName=x.UserName,
                IsUnlimited=x.IsUnlimited,
                EffDate=x.EffDate,
                TillDate=x.TillDate,
                IsActive=x.IsActive,
                PasswordHash=x.PasswordHash,
                Email=x.Email
                
            }).SingleOrDefault();
            return customerSingleList;
        }
    }
}