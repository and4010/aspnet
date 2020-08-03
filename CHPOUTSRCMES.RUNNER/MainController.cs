using CHPOUTSRCMES.TASK.DataModel.Views;
using CHPOUTSRCMES.TASK.Tasks;
using CHPOUTSRCMES.TASK.Tasks.Interfaces;
using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK
{
    internal class MainController :IDisposable
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
        /// 
        /// </summary>
        private List<ITasker> taskers { set; get; }

        private Dictionary<string, Task> taskList { set; get; }

        private LimitedConcurrencyLevelTaskScheduler taskScheduler { set; get; }
        
        private TaskFactory factory { set; get; }

        private MainForm _mainForm = null;


        internal MainForm MainForm => _mainForm ?? (_mainForm = new MainForm());

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

        private void LogInfo(string message)
        {
            logger.Info(message);
            Console.WriteLine(message);
        }

        internal void GenerateTestTasker()
        {
            MainController.Instance.AddTasker(new Tasker("測試一", 1, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");

                int count = 5;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));

            MainController.Instance.AddTasker(new Tasker("測試二", 2, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");
                int count = 5;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));

            MainController.Instance.AddTasker(new Tasker("測試三", 3, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");
                int count = 5;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));

            MainController.Instance.AddTasker(new Tasker("測試四", 1, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");
                int count = 100;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));

            MainController.Instance.AddTasker(new Tasker("測試五", 1, (tasker, token) =>
            {
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-開始");
                int count = 180;
                while (count-- > 0)
                {
                    Thread.Sleep(1000);
                }
                LogInfo($"{DateTime.Now.ToString("HH:mm:ss")}-{tasker.Name}-{tasker.Unit}-結束");
            }));
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
                    MainForm.Dispose();
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
