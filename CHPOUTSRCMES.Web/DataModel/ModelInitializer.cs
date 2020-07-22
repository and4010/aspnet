using CHPOUTSRCMES.Web.DataModel;
using CHPOUTSRCMES.Web.DataModel.Entiy;
using CHPOUTSRCMES.Web.DataModel.Entiy.Information;
using CHPOUTSRCMES.Web.DataModel.Entiy.Interfaces;
using CHPOUTSRCMES.Web.DataModel.Entiy.Purchase;
using CHPOUTSRCMES.Web.DataModel.Entiy.Repositorys;
using CHPOUTSRCMES.Web.Domain;
using CHPOUTSRCMES.Web.Models.Purchase;
using CHPOUTSRCMES.Web.Util;
using Microsoft.Extensions.Logging;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;

namespace CHPOUTSRCMES.Web.DataModel
{
    public class ModelInitializer : DropCreateDatabaseIfModelChanges<MesContext>
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        protected override void Seed(MesContext context)
        {
            readFromXls(context);
            base.Seed(context);

        }


        internal static void readFromXls(MesContext context)
        {
            var baseDir = AppDomain.CurrentDomain
                               .BaseDirectory
                               .Replace("\\bin", string.Empty) + "Data\\Excel";

            string initialFile = baseDir + "\\ERP0193164.xlsx";

            if (!string.IsNullOrEmpty(initialFile) && File.Exists(initialFile))
            {
                using (FileStream fs = new FileStream(initialFile, FileMode.Open))
                {
                    IWorkbook workbook = null;
                    if (fs.Length > 0 && initialFile.Substring(initialFile.LastIndexOf(".")).Equals(".xls"))
                    {
                        //把xls文件中的数据写入wk中
                        workbook = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        //把xlsx文件中的数据写入wk中
                        workbook = new XSSFWorkbook(fs);
                    }
                    Org(context);

                    new MasterUOW(context).Import(workbook);
                }
            }
        }

        private static void Org(MesContext context)
        {
            try
            {
                IRepository<CTR_ORG_T> CtrOrgRepositiory = new GenericRepository<CTR_ORG_T>(context);
                CTR_ORG_T ctrorg = new CTR_ORG_T();
                ctrorg.CtrOrgId = 1;
                ctrorg.ProcessCode = "XXIFP217";
                ctrorg.ServerCode = "123";
                ctrorg.BatchId = "20200721141600100000";
                ctrorg.BatchLineId = 1;
                ctrorg.HeaderId = 1;
                ctrorg.OrgId = 1;
                ctrorg.OrgName = "入庫";
                ctrorg.LineId = 1;
                ctrorg.ContainerNo = "WHAU5231488";
                ctrorg.MvContainerDate = DateTime.Now;
                ctrorg.OrganizationId = 265;
                ctrorg.OrganizationCode = "FTY";
                ctrorg.Subinventory = "SFG";
                ctrorg.LocatorId = 22016;
                ctrorg.LocatorCode = "SFG";
                ctrorg.DetailId = 1;
                ctrorg.InventoryItemId = 503117;
                ctrorg.ShipItemNumber = "4DM00E02700310K502K";
                ctrorg.PaperType = "DM00";
                ctrorg.BasicWeight = "02700";
                ctrorg.ReamWeight = "299.11";
                ctrorg.RollReamQty = 1;
                ctrorg.RollReamWt = 1;
                ctrorg.TtlRollReam = 1;
                ctrorg.Specification = "310K502K";
                ctrorg.PackingType = "令包";
                ctrorg.ShipMtQty = 1;
                ctrorg.TransactionQuantity = 3;
                ctrorg.TransactionUom = "MT";
                ctrorg.PrimaryQuantity = 3000;
                ctrorg.PrimaryUom = "KG";
                ctrorg.SecondaryQuantity = 3000;
                ctrorg.SecondaryUom = "RE";
                ctrorg.LotNumber = "";
                ctrorg.TheoryWeight = "";
                ctrorg.CreatedBy = 1;
                ctrorg.CreationDate = DateTime.Now;
                ctrorg.LastUpdateBy = 1;
                ctrorg.LastUpdateDate = DateTime.Now;
                CtrOrgRepositiory.Create(ctrorg, true);
            }
            catch(Exception e)
            {
                logger.Error(e.Message.ToString());
            }
            DataProcess process = new DataProcess();
            process.OrgToHeader(context);

                }
            
        

    }
}