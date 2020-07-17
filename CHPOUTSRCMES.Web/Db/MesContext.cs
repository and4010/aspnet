using CHPOUTSRCMES.DataAnnotation;
using CHPOUTSRCMES.Web.Db;
using CHPOUTSRCMES.Web.Db.Entiy;
using CHPOUTSRCMES.Web.Db.Entiy.Purchase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;


namespace CHPOUTSRCMES.Web.Data
{
    public class MesContext : DbContext
    {
        public MesContext() : base("MesContext")
        {
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
        }
        public DbSet<LogEntry> LogEntries { set; get; }

        public DbSet<ITEMS_T> ItemsTs { set; get; }

        public DbSet<SUBINVENTORY_T> SubinventoryTs { set; get; }

        public DbSet<ORGANIZATION_T> OrganizationTs { set; get; }

        public DbSet<LOCATOR_T> LocatorTs { set; get; }

        public DbSet<MACHINE_PAPER_TYPE_T> MachinePaperTypeTs { set; get; }

        public DbSet<RELATED_T> ReLatedTs { set; get; }

        public DbSet<STOCK_REASON_T> StockReasonTs { set; get; }

        public DbSet<YSZMPCKQ_T> YszmpckqTs { set; get; }

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