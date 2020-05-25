﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.DatabaseModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GarbageCollectionDBEntities : DbContext
    {
        public GarbageCollectionDBEntities()
            : base("name=GarbageCollectionDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AppCustomerClaim> AppCustomerClaims { get; set; }
        public virtual DbSet<CustomerLogin> CustomerLogins { get; set; }
        public virtual DbSet<CustomerRole> CustomerRoles { get; set; }
        public virtual DbSet<CustomerUserTable> CustomerUserTables { get; set; }
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<BSADCal> BSADCals { get; set; }
        public virtual DbSet<CollectionType> CollectionTypes { get; set; }
        public virtual DbSet<CollectionVerifyLog> CollectionVerifyLogs { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerRole1> CustomerRole1 { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<FiscalYear> FiscalYears { get; set; }
        public virtual DbSet<LocationMaster> LocationMasters { get; set; }
        public virtual DbSet<LocationVsCollector> LocationVsCollectors { get; set; }
        public virtual DbSet<Month> Months { get; set; }
        public virtual DbSet<NotificationQueue> NotificationQueues { get; set; }
        public virtual DbSet<NotificationSuccess> NotificationSuccesses { get; set; }
        public virtual DbSet<Status> Status { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<SubscriptionCollection> SubscriptionCollections { get; set; }
        public virtual DbSet<SubscriptionDue> SubscriptionDues { get; set; }
        public virtual DbSet<LicenseBranch> LicenseBranches { get; set; }
        public virtual DbSet<BloodGroup> BloodGroups { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CustomerUser> CustomerUsers { get; set; }
        public virtual DbSet<DataType> DataTypes { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FiscalYear1> FiscalYears1 { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<LoginLog> LoginLogs { get; set; }
        public virtual DbSet<MaritialStatu> MaritialStatus { get; set; }
        public virtual DbSet<MenuMain> MenuMains { get; set; }
        public virtual DbSet<MenuTemplate> MenuTemplates { get; set; }
        public virtual DbSet<MenuVsTemplate> MenuVsTemplates { get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Param> Params { get; set; }
        public virtual DbSet<ParamScript> ParamScripts { get; set; }
        public virtual DbSet<ParamValue> ParamValues { get; set; }
        public virtual DbSet<Religion> Religions { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Status1> Status1 { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserClaim> UserClaims { get; set; }
        public virtual DbSet<UserConnection> UserConnections { get; set; }
        public virtual DbSet<UserLogin> UserLogins { get; set; }
        public virtual DbSet<UserVSBranch> UserVSBranches { get; set; }
        public virtual DbSet<NDateD> NDateDs { get; set; }
    }
}
