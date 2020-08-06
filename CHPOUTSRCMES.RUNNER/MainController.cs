using CHPOUTSRCMES.TASK.Models.Views;
using CHPOUTSRCMES.TASK.Forms;
using CHPOUTSRCMES.TASK.Tasks;
using CHPOUTSRCMES.TASK.Tasks.Interfaces;
using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BatchPrint.Model.UnitOfWork;
using Oracle.ManagedDataAccess.Client;

namespace CHPOUTSRCMES.TASK
{
    public class MainController :IDisposable
    {

        private static MainController _instance;

        internal static MainController Instance => _instance ?? (_instance = new MainController());

        private Logger logger = LogManager.GetCurrentClassLogger();
        
        private bool disposed = false;
        /// <summary>
        /// 取消權杖來源
        /// </summary>
        private CancellationTokenSource cancelTokenSource { set; get; }
        /// <summary>
        /// 取消權杖
        /// </summary>
        private CancellationToken cancelToken { set; get; }
        /// <summary>
        /// 任務清單
        /// </summary>
        private List<ITasker> taskers { set; get; }

        private Dictionary<string, Task> taskList { set; get; }

        private LimitedConcurrencyLevelTaskScheduler taskScheduler { set; get; }
        
        private TaskFactory factory { set; get; }

        private MainForm mainForm = null;

        internal MainForm MainForm => mainForm ?? (mainForm = new MainForm() { Controller = this});
        
        private UomConverterForm uomConverterForm = null;

        internal UomConverterForm UomConverterForm => (uomConverterForm == null || uomConverterForm.IsDisposed) ? (uomConverterForm = new UomConverterForm() { Controller = this }) : uomConverterForm;

        private MasterViewForm masterViewForm = null;

        internal MasterViewForm MasterViewForm => (masterViewForm == null || masterViewForm.IsDisposed) ? (masterViewForm = new MasterViewForm() { Controller = this }) : masterViewForm;
        
        /// <summary>
        /// 
        /// </summary>
        public MainController()
        {
            taskScheduler = new LimitedConcurrencyLevelTaskScheduler(20);
            factory = new TaskFactory(taskScheduler);
            taskers = new List<ITasker>();
            taskList = new Dictionary<string, Task>();
        }

        /// <summary>
        /// 開始計時器
        /// </summary>
        internal void StartTimer()
        {
            cancelTokenSource = new CancellationTokenSource();
            cancelToken = cancelTokenSource.Token;
            var date = DateTime.Now.AddMinutes(1); //開始時間加一分鐘為啟動時間
            //Timer 一分鐘時間啟動一次
            Tasks.TaskTimer.Instance.Schedule(date.Hour, date.Minute, 1,
                () =>
            {
                

                foreach (ITasker profile in taskers)
                {
                    if (taskList.ContainsKey(profile.Name))
                        continue;

                    if (cancelToken.IsCancellationRequested)
                    {
                        cancelToken.ThrowIfCancellationRequested();
                    }

                    try
                    {
                        var task = profile.Run(factory, cancelToken, (n, t) =>
                        {
                            if (taskList.ContainsKey(n))
                            {
                                taskList.Remove(n);
                            }

                        });

                        if (task != null)
                        {
                            taskList.Add(profile.Name, task);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex);
                    }
                }
            });
        }

        /// <summary>
        /// 停止計時器
        /// </summary>
        internal async void StopTimer()
        {
            if (!cancelTokenSource.IsCancellationRequested)
            {
                cancelTokenSource.Cancel();
            }

            try
            {
                await Task.WhenAll(taskList.Values.ToArray());
            }
            catch (OperationCanceledException)
            {
                LogInfo("使用者取消 OperationCanceledException");
            }
            finally
            {
                cancelTokenSource.Dispose();
            }
        }

        /// <summary>
        /// 加入 TASKER
        /// </summary>
        /// <param name="tasker"></param>
        internal void AddTasker(ITasker tasker)
        {
            taskers.Add(tasker);
        }

        /// <summary>
        /// LOG - 一般
        /// </summary>
        /// <param name="message"></param>
        private void LogInfo(string message)
        {
            logger.Info(message);
            Console.WriteLine(message);
        }

        /// <summary>
        /// LOG - 錯誤
        /// </summary>
        /// <param name="message"></param>
        private void LogError(string message)
        {
            logger.Error(message);
            Console.WriteLine(message);
        }

        internal void GenerateTestTasker()
        {
            AddTasker(new Tasker("測試一", 1, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");

                int count = 5;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));

            AddTasker(new Tasker("測試二", 2, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");
                int count = 5;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));

            AddTasker(new Tasker("測試三", 3, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");
                int count = 5;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));

            AddTasker(new Tasker("測試四", 1, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");
                int count = 100;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));

            AddTasker(new Tasker("測試五", 1, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now:HH:mm:ss}-{tasker.Name}-{tasker.Unit}-開始");
                int count = 180;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now:HH:mm:ss}-{tasker.Name}-{tasker.Unit}-結束");
            }));
        }


        internal void AddMasterTasker()
        {
            AddTasker(new Tasker("主檔轉檔程序", 1, ImportMaster));
        }

        internal async void ImportMaster(Tasker tasker, CancellationToken token)
        {
            //組織倉庫儲位
            var taskSubinventory = importSubinventory(tasker, token);
            //庫存交易類別
            var taskTransactionType = importTransactionType(tasker, token);
            //料號
            var taskItem = taskSubinventory.ContinueWith(task => importItem(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
            //餘切規格
            var taskOspRelatedItem = taskItem.ContinueWith(subTask => importOspRelatedItem(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
            //紙別機台
            var taskMachinePaperType = taskItem.ContinueWith(subTask => importMachinePaperType(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
            //令重張數
            var tasYszmpckq = taskItem.ContinueWith(subTask => importYszmpckq(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

            await Task.WhenAll(taskSubinventory, taskTransactionType, taskItem, taskOspRelatedItem, taskMachinePaperType, tasYszmpckq);
        }

        private async Task importSubinventory(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_SUBINVENTORY_V> subinventoryList = new List<XXCINV_SUBINVENTORY_V>();

                string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();

                using (var conn = new OracleConnection(cnstr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        subinventoryList = (await masterUOW.GetSubinventoryListAsync()).ToList();

                        if (subinventoryList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-無法從 {typeof(XXCINV_SUBINVENTORY_V)}VIEW取得資料");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importSubinventory-結束");
        }

        private async Task importItem(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXIFV050_ITEMS_FTY_V> itemList = new List<XXIFV050_ITEMS_FTY_V>();

                string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();

                using (var conn = new OracleConnection(cnstr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        itemList = (await masterUOW.GetItemListAsync()).ToList();

                        if (itemList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-無法從{typeof(XXIFV050_ITEMS_FTY_V)} VIEW取得資料");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importItem-結束");
        }

        private async Task importOspRelatedItem(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_OSP_RELATED_ITEM_V> itemList = new List<XXCINV_OSP_RELATED_ITEM_V>();

                string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();

                using (var conn = new OracleConnection(cnstr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        itemList = (await masterUOW.GetOspRelatedItemListAsync()).ToList();

                        if (itemList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-無法從{typeof(XXCINV_OSP_RELATED_ITEM_V)} VIEW取得資料");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importOspRelatedItem-結束");
        }

        private async Task importMachinePaperType(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCPO_MACHINE_PAPER_TYPE_V> machinePaperTypeList = new List<XXCPO_MACHINE_PAPER_TYPE_V>();

                string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();

                using (var conn = new OracleConnection(cnstr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        machinePaperTypeList = (await masterUOW.GetMachinePaperTypeListAsync()).ToList();

                        if (machinePaperTypeList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-無法從{typeof(XXCPO_MACHINE_PAPER_TYPE_V)} VIEW取得資料");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importMachinePaperType-結束");
        }

        private async Task importYszmpckq(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCOM_YSZMPCKQ_V> yszmpckqList = new List<XXCOM_YSZMPCKQ_V>();

                string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();

                using (var conn = new OracleConnection(cnstr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        yszmpckqList = (await masterUOW.GetYszmpckqListAsync()).ToList();

                        if (yszmpckqList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-無法從{typeof(XXCOM_YSZMPCKQ_V)} VIEW取得資料");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importYszmpckq-結束");
        }

        private async Task importTransactionType(Tasker tasker, CancellationToken token)
        {
            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-開始");

            try
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                List<XXCINV_TRANSACTION_TYPE_V> transactionTypeList = new List<XXCINV_TRANSACTION_TYPE_V>();

                string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();
                using (var conn = new OracleConnection(cnstr))
                {
                    using (MasterUOW masterUOW = new MasterUOW(conn))
                    {
                        transactionTypeList = (await masterUOW.GetTransactionTypeListAsync()).ToList();

                        if (transactionTypeList.Count() == 0)
                        {
                            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-無法從{typeof(XXCINV_TRANSACTION_TYPE_V)}VIEW取得資料");
                            return;
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-使用者取消");
            }
            catch (Exception ex)
            {
                LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-錯誤-{ex.Message}-{ex.StackTrace}");
            }

            LogInfo($"[{tasker.Name}]-{tasker.Unit}-importTransactionType-結束");

        }


        /// <summary>
        /// 
        /// </summary>
        internal void testConnection()
        {

            string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["OracleContext"].ToString();
            using (var cn = new Oracle.ManagedDataAccess.Client.OracleConnection(cnstr))
            {
                SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.Date);
                cn.Open();
                var count1 = cn.Query<XXCINV_SUBINVENTORY_V>(@"SELECT * FROM XXCINV_SUBINVENTORY_V").Count();
                var count2 = cn.Query<XXCINV_OSP_RELATED_ITEM_V>(@"SELECT * FROM XXCINV_OSP_RELATED_ITEM_V").Count();
                var count3 = cn.Query<XXCOM_YSZMPCKQ_V>(@"SELECT * FROM XXCOM_YSZMPCKQ_V").Count();
                var count4 = cn.Query<XXIFV050_ITEMS_FTY_V>(@"SELECT * FROM XXCPO_MACHINE_PAPER_TYPE_V").Count();
                var count5 = cn.Query<XXIFV050_ITEMS_FTY_V>(@"SELECT * FROM XXIFV050_ITEMS_FTY_V").Count();
                var quantity = cn.Query<decimal>(@"SELECT ROUND(TPMC_ADMIN.UOM_CONVERSION(1, 1, 'KG', 'RE'), 5) QUANTITY FROM dual").SingleOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    if (mainForm != null && !mainForm.IsDisposed)
                        mainForm.Dispose();
                    if (uomConverterForm != null && !uomConverterForm.IsDisposed)
                        uomConverterForm.Dispose();
                    if (masterViewForm != null && !masterViewForm.IsDisposed)
                        masterViewForm.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
