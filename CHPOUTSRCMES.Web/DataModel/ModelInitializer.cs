using CHPOUTSRCMES.Web.DataModel.UnitOfWorks;
using CHPOUTSRCMES.Web.Migrations;
using NLog;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Data.Entity;
using System.IO;

namespace CHPOUTSRCMES.Web.DataModel
{
    internal class ModelInitializer : MigrateDatabaseToLatestVersion<MesContext, Configuration>
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        //protected override void Seed(MesContext context)
        //{

        //    executeSqlScript(context);
        //    //readUsersXls(context);
        //    readFromXls(context);
        //    new PurchaseUOW(context).generateTestData();
        //    new DeliveryUOW(context).generateTestData();
        //    new MasterUOW(context).generateStockTestData();
        //    new ProcessUOW(context).generateTestData();
        //    base.Seed(context);

        //}

        internal static void executeSqlScript(MesContext context)
        {
            var baseDir = AppDomain.CurrentDomain
                   .BaseDirectory
                   .Replace("\\bin", string.Empty) + "Data\\SQLScript";
            var files = Directory.GetFiles(baseDir);
            foreach (var file in files)
            {
                try
                {
                    if (!string.IsNullOrEmpty(file) && File.Exists(file))
                    {
                        context.Database.ExecuteSqlCommand(File.ReadAllText(file));
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "執行SQL腳本出現錯誤");
                }
            }
        }


        internal static void readFromXls(MesContext context)
        {
            var baseDir = AppDomain.CurrentDomain
                               .BaseDirectory
                               .Replace("\\bin", string.Empty) + "Data\\Excel";

            string initialFile = baseDir + "\\MES_20200903.xlsx";

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

                    new IdentityUOW(context).Import(workbook);
                }
            }
        }


        internal static void readUsersXls(MesContext context)
        {
            var baseDir = AppDomain.CurrentDomain
                               .BaseDirectory
                               .Replace("\\bin", string.Empty) + "Data\\Excel";

            string initialFile = baseDir + "\\MES_USERS.xlsx";

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
                    IdentityUOW identity = new IdentityUOW(context);

                    identity.generateRoles();
                    identity.ImportUser(workbook);


                }
            }
        }
    }
}