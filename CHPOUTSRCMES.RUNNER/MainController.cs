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

        private List<ITasker> taskers { set; get; }

        private Dictionary<string, Task> taskList { set; get; }

        private LimitedConcurrencyLevelTaskScheduler taskScheduler { set; get; }
        
        private TaskFactory factory { set; get; }

        private MainForm _mainForm = null;


        internal MainForm MainForm => _mainForm ?? (_mainForm = new MainForm());


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

        internal void testConnection()
        {

            string cnstr = System.Configuration.ConfigurationManager.ConnectionStrings["oracleContext"].ToString();
            using (var cn = new Oracle.ManagedDataAccess.Client.OracleConnection(cnstr))
            {
                SqlMapper.AddTypeMap(typeof(DateTime), System.Data.DbType.Date);
                cn.Open();
                var result = cn.Query<XXCINV_SUBINVENTORY_V>(@"SELECT * FROM XXCINV_SUBINVENTORY_V").FirstOrDefault();
                LogInfo(result.ORGANIZATION_CODE);
            }
        }


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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
