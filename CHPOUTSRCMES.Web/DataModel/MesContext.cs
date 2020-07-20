using CHPOUTSRCMES.DataAnnotation;
using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using CHPOUTSRCMES.Web.DataModel.Entiy.Purchase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using CHPOUTSRCMES.Web.DataModel.Entiy.Information;

namespace CHPOUTSRCMES.Web.DataModel
{
    public class MesContext : IdentityDbContext<AppUser>
    {
        public MesContext() : base("MesContext")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Database.SetInitializer<MesContext>(new ModelInitializer());

            if (!Database.Exists())
            {
                Database.Initialize(true);
            }

        }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //設定decimal預設值
            modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            modelBuilder.Conventions.Add(new DecimalPropertyConvention(30, 10));
            Precision.ConfigureModelBuilder(modelBuilder);

            //修改使用者登入表格無ID及重新命名表格
            modelBuilder.Entity<AppUser>().HasKey<string>(l => l.Id).ToTable("USER_T");
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId).ToTable("USER_LOGIN_T");
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("ROLE_T");
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId }).ToTable("USER_ROLE_T");
            modelBuilder.Entity<IdentityUserClaim>().HasKey(r =>  r.Id).ToTable("USER_CLAIM_T");
        }

        public static MesContext Create()
        {
            return new MesContext();
        }

        //public DbSet<AppIdentityUser> User_T { set; get; }

        //public DbSet<IdentityUserLogin> UserLogin_T { set; get; }

        //public DbSet<IdentityRole> Role_T { set; get; }

        //public DbSet<IdentityUserRole> UserRole_T { set; get; }

        //public DbSet<IdentityUserClaim> UserClaim_T { set; get; }

        public DbSet<LOG_ENTRY_T> LogEntries { set; get; }

        public DbSet<ITEMS_T> ItemsTs { set; get; }

        public DbSet<ORG_ITEMS_T> OrgItemsTs { set; get; }

        public DbSet<SUBINVENTORY_T> SubinventoryTs { set; get; }

        public DbSet<ORGANIZATION_T> OrganizationTs { set; get; }

        public DbSet<LOCATOR_T> LocatorTs { set; get; }

        public DbSet<MACHINE_PAPER_TYPE_T> MachinePaperTypeTs { set; get; }

        public DbSet<RELATED_T> ReLatedTs { set; get; }

        public DbSet<STK_REASON_T> StockReasonTs { set; get; }

        public DbSet<YSZMPCKQ_T> YszmpckqTs { set; get; }

        public DbSet<TRANSACTION_TYPE_T> TransactionTypeTs { set; get; }

        public DbSet<CTR_DETAIL_HT> CTR_DETAIL_HTs { set; get; }

        public DbSet<CTR_DETAIL_T> CTR_DETAIL_Ts { set; get; }

        public DbSet<CTR_FILEINFO_T> CTR_FILEINFO_Ts { set; get; }

        public DbSet<CTR_FILES_T> CTR_FILES_Ts { set; get; }

        public DbSet<CTR_HEADER_T> CTR_HEADER_Ts { set; get; }

        public DbSet<CTR_ORG_T> CTR_ORG_Ts { set; get; }

        public DbSet<CTR_PICKED_HT> CTR_PICKED_HTs { set; get; }

        public DbSet<CTR_PICKED_T> CTR_PICKED_Ts { set; get; }
    }
}