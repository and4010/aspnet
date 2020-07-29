using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
using CHPOUTSRCMES.Web.Util;
using NLog;

namespace CHPOUTSRCMES.Web.DataModel.UnitOfWorks
{
    public class StockUOW : UnitOfWork
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly IRepository<STOCK_T> stockTRepositiory;
        private readonly IRepository<STOCK_HT> stockHtRepositiory;


        public StockUOW(DbContext context) : base(context)
        {
            this.stockTRepositiory = new GenericRepository<STOCK_T>(this);
            this.stockHtRepositiory = new GenericRepository<STOCK_HT>(this);
        }

        public void generateTestData()
        {
            try
            {
                #region 第一筆測試資料 平版 令包

                stockTRepositiory.Create(new STOCK_T()
                {
                    OrganizationId = 265,
                    OrganizationCode = "FTY",
                    SubinventoryCode = "TB2",
                    LocatorId = null,
                    LocatorSegments = "",
                    Barcode = "A2007290001",
                    InventoryItemId = 504029,
                    ItemNumber = "4DM00A03500214K512K",
                    ItemDescription = "全塗灰銅卡",
                    ReamWeight = "274.27",
                    ItemCategory = "平版",
                    PaperType = "DM00",
                    BasicWeight = "03500",
                    Specification = "214K512K",
                    PackingType = "令包",
                    RollReamWt = 100,
                    ReasonCode = "",
                    ReasonDesc = "",
                    OspBatchNo = "P9B0288",
                    LotNumber = "",
                    StatusCode = "",
                    PrimaryTransactionQty = 1000,
                    PrimaryAvailableQty = 1000,
                    PrimaryUomCode = "KG",
                    SecondaryTransactionQty = 100,
                    SecondaryAvailableQty = 100,
                    SecondaryUomCode = "RE",
                    Note = "",
                    CreatedBy = "1",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateDate = DateTime.Now,
                }, true);

                #endregion

                #region 第二筆測試資料 平版 無令打件
                stockTRepositiory.Create(new STOCK_T()
                {
                    OrganizationId = 265,
                    OrganizationCode = "FTY",
                    SubinventoryCode = "TB2",
                    LocatorId = null,
                    LocatorSegments = "",
                    Barcode = "A2007290002",
                    InventoryItemId = 505675,
                    ItemNumber = "4DM00P0270008271130",
                    ItemDescription = "全塗灰銅卡",
                    ReamWeight = "278.13",
                    ItemCategory = "平版",
                    PaperType = "DM00",
                    BasicWeight = "02700",
                    Specification = "08271130",
                    PackingType = "無令打件",
                    RollReamWt = 100,
                    ReasonCode = "",
                    ReasonDesc = "",
                    OspBatchNo = "P2010087",
                    LotNumber = "",
                    StatusCode = "",
                    PrimaryTransactionQty = 1000,
                    PrimaryAvailableQty = 1000,
                    PrimaryUomCode = "KG",
                    SecondaryTransactionQty = 100,
                    SecondaryAvailableQty = 100,
                    SecondaryUomCode = "RE",
                    Note = "",
                    CreatedBy = "1",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateDate = DateTime.Now,
                }, true);

                #endregion

                #region 第三筆測試資料 平版 無令打件
                stockTRepositiory.Create(new STOCK_T()
                {
                    OrganizationId = 265,
                    OrganizationCode = "FTY",
                    SubinventoryCode = "SFG",
                    LocatorId = null,
                    LocatorSegments = "",
                    Barcode = "A2007290003",
                    InventoryItemId = 558705,
                    ItemNumber = "4AH00A00900362KRL00",
                    ItemDescription = "捲筒琉麗",
                    ReamWeight = "2.2",
                    ItemCategory = "捲筒",
                    PaperType = "AH00",
                    BasicWeight = "00900",
                    Specification = "362KRL00",
                    PackingType = "",
                    RollReamWt = 1,
                    ReasonCode = "",
                    ReasonDesc = "",
                    OspBatchNo = "",
                    LotNumber = "1234567890",
                    StatusCode = "",
                    PrimaryTransactionQty = 1000,
                    PrimaryAvailableQty = 1000,
                    PrimaryUomCode = "KG",
                    SecondaryTransactionQty = null,
                    SecondaryAvailableQty = null,
                    SecondaryUomCode = "",
                    Note = "",
                    CreatedBy = "1",
                    CreationDate = DateTime.Now,
                    LastUpdateBy = "1",
                    LastUpdateDate = DateTime.Now,
                }, true);

                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(LogUtilities.BuildExceptionMessage(ex));
            }


        }
    }
}