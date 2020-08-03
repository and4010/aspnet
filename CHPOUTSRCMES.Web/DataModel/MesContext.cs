﻿using CHPOUTSRCMES.DataAnnotation;
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
using CHPOUTSRCMES.Web.DataModel.Entiy.Delivery;
using System.Web.Configuration;
using System.Data;

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

        #region 主檔 (來自ERP)
        /// <summary>
        ///  料號
        /// </summary>
        public DbSet<ITEMS_T> ItemsTs { set; get; }
        /// <summary>
        /// 組織料號
        /// </summary>
        public DbSet<ORG_ITEMS_T> OrgItemsTs { set; get; }
        /// <summary>
        /// 倉庫
        /// </summary>
        public DbSet<SUBINVENTORY_T> SubinventoryTs { set; get; }
        /// <summary>
        /// 組織
        /// </summary>
        public DbSet<ORGANIZATION_T> OrganizationTs { set; get; }
        /// <summary>
        /// 儲位
        /// </summary>
        public DbSet<LOCATOR_T> LocatorTs { set; get; }
        /// <summary>
        /// 機台紙別
        /// </summary>
        public DbSet<MACHINE_PAPER_TYPE_T> MachinePaperTypeTs { set; get; }
        /// <summary>
        /// 餘切規格
        /// </summary>
        public DbSet<RELATED_T> RelatedTs { set; get; }

        /// <summary>
        /// 令重包數
        /// </summary>
        public DbSet<YSZMPCKQ_T> YszmpckqTs { set; get; }
        /// <summary>
        /// 庫存交易類別
        /// </summary>
        public DbSet<TRANSACTION_TYPE_T> TransactionTypeTs { set; get; }
        #endregion 主檔 (來自ERP)

        /// <summary>
        /// 貨故原因
        /// </summary>
        public DbSet<STK_REASON_T> StockReasonTs { set; get; }
        /// <summary>
        /// NLOG
        /// </summary>
        public DbSet<LOG_ENTRY_T> LogEntries { set; get; }

        #region 庫存
        public DbSet<STOCK_T> STOCK_Ts { set; get; }

        public DbSet<STOCK_HT> STOCK_HTs { set; get; }

        public DbSet<STK_TXN_T> STK_TXN_Ts { set; get; }

        #endregion

        #region 條碼檔

        /// <summary>
        /// 條碼設定檔
        /// </summary>
        public DbSet<BCD_MISC_T> BcdMiscTs { set; get; }
        /// <summary>
        /// 條碼序號檔
        /// </summary>
        public DbSet<BCD_SERIAL_T> BcdSerialTs { set; get; }
        /// <summary>
        /// 條碼檔
        /// </summary>
        public DbSet<BCD_UNIQUE_T> BcdUniqueTs { set; get; }

        #endregion

        #region 入庫

        /// <summary>
        /// 入庫歷史明細
        /// </summary>
        public DbSet<CTR_DETAIL_HT> CTR_DETAIL_HTs { set; get; }
        /// <summary>
        /// 入庫明細
        /// </summary>
        public DbSet<CTR_DETAIL_T> CTR_DETAIL_Ts { set; get; }
        /// <summary>
        /// 入庫照片資訊
        /// </summary>
        public DbSet<CTR_FILEINFO_T> CTR_FILEINFO_Ts { set; get; }
        /// <summary>
        /// 入庫照片實體
        /// </summary>
        public DbSet<CTR_FILES_T> CTR_FILES_Ts { set; get; }
        /// <summary>
        /// 入庫檔頭
        /// </summary>
        public DbSet<CTR_HEADER_T> CTR_HEADER_Ts { set; get; }
        /// <summary>
        /// 入庫主檔
        /// </summary>
        public DbSet<CTR_ORG_T> CTR_ORG_Ts { set; get; }
        /// <summary>
        /// 入庫歷史揀貨
        /// </summary>
        public DbSet<CTR_PICKED_HT> CTR_PICKED_HTs { set; get; }
        /// <summary>
        /// 入庫揀貨
        /// </summary>
        public DbSet<CTR_PICKED_T> CTR_PICKED_Ts { set; get; }

        #endregion 入庫

        #region 出貨
        /// <summary>
        /// 出貨主檔
        /// </summary>
        public DbSet<DLV_ORG_T> DLV_ORG_Ts { set; get; }
        /// <summary>
        /// 出貨檔頭
        /// </summary>
        public DbSet<DLV_HEADER_T> DLV_HEADER_Ts { set; get; }
        /// <summary>
        /// 出貨明細
        /// </summary>
        public DbSet<DLV_DETAIL_T> DLV_DETAIL_Ts { set; get; }
        /// <summary>
        /// 出貨歷史明細
        /// </summary>
        public DbSet<DLV_DETAIL_HT> DLV_DETAIL_HTs { set; get; }
        /// <summary>
        /// 出貨揀貨
        /// </summary>
        public DbSet<DLV_PICKED_T> DLV_PICKED_Ts { set; get; }
        /// <summary>
        /// 出貨歷史揀貨
        /// </summary>
        public DbSet<DLV_PICKED_HT> DLV_PICKED_HTs { set; get; }

        #endregion 出貨


    }
}