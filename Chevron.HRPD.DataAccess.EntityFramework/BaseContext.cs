using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Chevron.HRPD.BusinessEntities;

namespace Chevron.HRPD.DataAccess
{
    public class BaseContext : DbContext
    {
        public BaseContext()
            : base("HRPDConnectionString") 
        {
            Configuration.LazyLoadingEnabled = false;
        }





        
        //public DbSet<OverTimeApply> OverTimeApply { get; set; }
        public DbSet<UserLogInLog> UserLogInLog_Context { get; set; }
        public DbSet<UserActivityLog> UserActivityLog_Context { get; set; }
        public DbSet<Employee_PD_Info> Employee_PD_Info_Context { get; set; }

        public DbSet<User_Role> User_Role_Context { get; set; }

        //public DbSet<tblEmployeeInformation> tblEmployeeInformation { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           // modelBuilder.Entity<test>().ToTable("test");
           //modelBuilder.Entity<OverTimeApply>().ToTable("OverTimeApply");
            //modelBuilder.Entity<tblEmployeeInformation>().ToTable("tblEmployeeInformation");

            //modelBuilder.Entity<RateUpload>().ToTable("RateUpload");

            //modelBuilder.Entity<ManagerEntry>().ToTable("OverTimeManagerEntry");
            //modelBuilder.Entity<tblHolidaySchedule>().ToTable("tblHolidaySchedule");

            // modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail");
            //modelBuilder.Entity<Orders>().ToTable("Orders");

            modelBuilder.Entity<UserActivityLog>().ToTable("UserActivityLog");
            modelBuilder.Entity<UserLogInLog>().ToTable("UserLogInLog");
            modelBuilder.Entity<Employee_PD_Info>().ToTable("Employee_PD_Info");
            modelBuilder.Entity<User_Role>().ToTable("User_Role");
       
        }

        //public DbSet<test> tests { get; set; }

        //public DbSet<tblHolidaySchedule> tblHolidaySchedules { get; set; }

        //public DbSet<OrderDetail> OrderDetail { get; set; }
        //public DbSet<Orders> Orders { get; set; }
         
        

    }
}
