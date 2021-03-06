using CHPOUTSRCMES.TASK.Forms;
using CHPOUTSRCMES.TASK.Models;
using CHPOUTSRCMES.TASK.Models.Service;
using CHPOUTSRCMES.TASK.Tasks;
using CHPOUTSRCMES.TASK.Tasks.Interfaces;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions.Configuration;

namespace CHPOUTSRCMES.TASK
{
    public class MainController : IDisposable
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        private static MainController instance;

        internal static MainController Instance => instance ?? (instance = new MainController());

        internal Configuration configuration { set; get; }

        private string erpConnStr;

        internal string ErpConnStr => erpConnStr ?? (erpConnStr =
#if BIOTECH
            System.Configuration.ConfigurationManager.ConnectionStrings["OracleBiotechContext"].ToString()
#elif TEST
            System.Configuration.ConfigurationManager.ConnectionStrings["OracleTestContext"].ToString()
#else
            System.Configuration.ConfigurationManager.ConnectionStrings["ErpContext"].ToString()
#endif
            );

        private string mesConnStr;

        internal string MesConnStr => mesConnStr ?? (mesConnStr =
#if BIOTECH
            System.Configuration.ConfigurationManager.ConnectionStrings["MesBiotechContext"].ToString()
#elif TEST
            System.Configuration.ConfigurationManager.ConnectionStrings["MesTestContext"].ToString()
#else
            System.Configuration.ConfigurationManager.ConnectionStrings["MesContext"].ToString()
#endif
            );

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

        internal MainForm MainForm => mainForm ?? (mainForm = new MainForm() { Controller = this });

        private ScheduleForm scheduleForm = null;

        internal ScheduleForm ScheduleForm => (scheduleForm == null || scheduleForm.IsDisposed) ? (scheduleForm = new ScheduleForm() { Controller = this }) : scheduleForm;

        internal Tasker masterTasker;

        internal Tasker ctrTasker;

        internal Tasker dlvTasker;

        internal Tasker ospTasker;

        internal Tasker trfTasker;

        /// <summary>
        /// 
        /// </summary>
        public MainController()
        {
            initLogger(MesConnStr);
            taskScheduler = new LimitedConcurrencyLevelTaskScheduler(20);
            factory = new TaskFactory(taskScheduler);
            taskers = new List<ITasker>();
            taskList = new Dictionary<string, Task>();

            readConfig();

            masterTasker = new Tasker("主檔轉檔程序", configuration.MasterTaskInterval, configuration.MasterTaskEnabled, (tasker, token) =>
            {

                TaskerMessageUpdate(tasker, DateTime.Now.ToString("dd日 HH:mm:ss.fff"), "");

                MasterService service = new MasterService(MesConnStr, ErpConnStr);
                //組織倉庫儲位
                var taskSubinventory = Task.Run(() => service.importSubinventory(tasker, token));
                //庫存交易類別
                var taskTransactionType = Task.Run(() => service.importTransactionType(tasker, token));
                //料號
                var taskItem = taskSubinventory.ContinueWith(task => service.importItem(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //餘切規格
                var taskOspRelatedItem = taskItem.ContinueWith(subTask => service.importOspRelatedItem(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //紙別機台
                var taskMachinePaperType = taskItem.ContinueWith(subTask => service.importMachinePaperType(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //令重張數
                var tasYszmpckq = taskItem.ContinueWith(subTask => service.importYszmpckq(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(taskSubinventory, taskTransactionType, taskItem, taskOspRelatedItem, taskMachinePaperType, tasYszmpckq);

                AddOtherTasker(configuration);
            });

            ctrTasker = new Tasker("進櫃轉檔程序", configuration.CtrTaskInterval, configuration.CtrTaskEnabled, (tasker, token) =>
            {

                TaskerMessageUpdate(tasker, DateTime.Now.ToString("dd日 HH:mm:ss.fff"), "");

                CtrStService service = new CtrStService(MesConnStr, ErpConnStr);
                var task = Task.Run(() => service.ImportCtrSt(tasker, token));
                var statusTask = task.ContinueWith(t => service.UpdateStatusCtrSt(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                var rvTask = statusTask.ContinueWith(subTask => service.ExportCtrStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(task, rvTask, statusTask);
            });

            dlvTasker = new Tasker("出貨轉檔程序", configuration.DlvTaskInterval, configuration.DlvTaskEnabled, (tasker, token) =>
            {

                TaskerMessageUpdate(tasker, DateTime.Now.ToString("dd日 HH:mm:ss.fff"), "");

                DlvStService service = new DlvStService(MesConnStr, ErpConnStr);
                var task = Task.Run(() => service.ImportDlvSt(tasker, token));
                //var task = service.ImportDlvSt(tasker, token);
                var rvTask = task.ContinueWith(subTask => service.ExportDlvStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(task, rvTask);
            });

            ospTasker = new Tasker("加工轉檔程序", configuration.OspTaskInterval, configuration.OspTaskEnabled, (tasker, token) =>
            {

                TaskerMessageUpdate(tasker, DateTime.Now.ToString("dd日 HH:mm:ss.fff"), "");

                OspStService service = new OspStService(MesConnStr, ErpConnStr);
                var task = service.ImportOspSt(tasker, token);

                var rvStage1StatusCodeTask = task.ContinueWith(subTask => service.UpdateStatusOspStRvStage1(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                var rvStage1Task = rvStage1StatusCodeTask.ContinueWith(subTask => service.ExportOspStRvStage1(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                var rvStage2StatusCodeTask = rvStage1Task.ContinueWith(subTask => service.UpdateStatusOspStRvStage2(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                var rvStage2Task = rvStage2StatusCodeTask.ContinueWith(subTask => service.ExportOspStRvStage2(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                var rvStage3StatusCodeTask = rvStage2Task.ContinueWith(subTask => service.UpdateStatusOspStRvStage3(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                var rvStage3Task = rvStage3StatusCodeTask.ContinueWith(subTask => service.ExportOspStRvStage3(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(task, rvStage1Task, rvStage1StatusCodeTask, rvStage2Task, rvStage2StatusCodeTask, rvStage3Task, rvStage3StatusCodeTask);

                //Task.WaitAll(task);
            });

            trfTasker = new Tasker("庫存異動轉檔程序", configuration.TrfTaskInterval, configuration.TrfTaskEnabled, (tasker, token) =>
            {

                TaskerMessageUpdate(tasker, DateTime.Now.ToString("dd日 HH:mm:ss.fff"), "");

                TrfStService service = new TrfStService(MesConnStr, ErpConnStr);

                var trfTask1 = Task.Run(() => service.ExportTrfStRv(tasker, token));
                var trfTask2 = Task.Run(() => service.ExportMiscStRv(tasker, token));
                var trfTask3 = Task.Run(() => service.ExportRsnStRv(tasker, token));
                var trfTask4 = Task.Run(() => service.ExportObsStRv(tasker, token));
                var trfTask5 = Task.Run(() => service.ExportInvStRv(tasker, token));

                Task.WaitAll(trfTask1, trfTask2, trfTask3, trfTask4, trfTask5);
            });

        }

        public void initLogger(string strConnectionString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.ConnectionString = strConnectionString;

            DatabaseTarget targetDB = new DatabaseTarget();

            targetDB.Name = "MesLog";
            targetDB.ConnectionString = strConnectionString;

            targetDB.CommandText = string.Format(
@"INSERT INTO [dbo].[LOG_ENTRY_TASK_T] ([CallSite], [Date], [Exception], [Level], [Logger], [MachineName], [Message], [StackTrace], [Thread], [Username]) VALUES (@CallSite, @Date, @Exception, @Level, @Logger, @MachineName, @Message, @StackTrace, @Thread, @Username);");
            targetDB.Parameters.Add(new DatabaseParameterInfo("@CallSite", new NLog.Layouts.SimpleLayout("${callsite:filename=true}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@Date", new NLog.Layouts.SimpleLayout("${longdate}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@Exception", new NLog.Layouts.SimpleLayout("${exception}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@Level", new NLog.Layouts.SimpleLayout("${level}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@Logger", new NLog.Layouts.SimpleLayout("${logger}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@MachineName", new NLog.Layouts.SimpleLayout("${machinename}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@Message", new NLog.Layouts.SimpleLayout("${message}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@StackTrace", new NLog.Layouts.SimpleLayout("${stacktrace}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@Thread", new NLog.Layouts.SimpleLayout("${threadid}")));
            targetDB.Parameters.Add(new DatabaseParameterInfo("@Username", new NLog.Layouts.SimpleLayout("${windows-identity:domain=true}")));

            // Keep original configuration
            LoggingConfiguration config = LogManager.Configuration;
            if (config == null)
                config = new LoggingConfiguration();

            config.AddTarget(targetDB.Name, targetDB);

            LoggingRule rule = new LoggingRule("*", LogLevel.Info, targetDB);
            config.LoggingRules.Add(rule);
            LogManager.ThrowExceptions = false;
            LogManager.Configuration = config;
            
        }

        /// <summary>
        /// 讀取AppConfig設定
        /// </summary>
        internal void readConfig()
        {
            if (configuration == null) configuration = new Configuration();

            configuration.MasterTaskInterval = Helpers.AppConfigHelper.GetInt("MasterTaskInterval");
            configuration.CtrTaskInterval = Helpers.AppConfigHelper.GetInt("CtrTaskInterval");
            configuration.DlvTaskInterval = Helpers.AppConfigHelper.GetInt("DlvTaskInterval");
            configuration.OspTaskInterval = Helpers.AppConfigHelper.GetInt("OspTaskInterval");
            configuration.TrfTaskInterval = Helpers.AppConfigHelper.GetInt("TrfTaskInterval");

            configuration.MasterTaskEnabled = Helpers.AppConfigHelper.GetInt("MasterTaskEnabled") == 1;
            configuration.CtrTaskEnabled = Helpers.AppConfigHelper.GetInt("CtrTaskEnabled") == 1;
            configuration.DlvTaskEnabled = Helpers.AppConfigHelper.GetInt("DlvTaskEnabled") == 1;
            configuration.OspTaskEnabled = Helpers.AppConfigHelper.GetInt("OspTaskEnabled") == 1;
            configuration.TrfTaskEnabled = Helpers.AppConfigHelper.GetInt("TrfTaskEnabled") == 1;
        }


        #region 任務計數器

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
                                lock (TaskTimer.Instance)
                                {
                                    if (taskList.ContainsKey(n))
                                    {
                                        taskList.Remove(n);
                                    }
                                }
                            });

                            if (task != null)
                            {
                                lock (TaskTimer.Instance)
                                {
                                    taskList.Add(profile.Name, task);
                                }
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
        internal void StopTimer()
        {
            if (!cancelTokenSource.IsCancellationRequested)
            {
                cancelTokenSource.Cancel();
            }

            try
            {
                Task.WaitAll(taskList.Values.ToArray());
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
            if(!TaskerExists(tasker.Name))
            {
                taskers.Add(tasker);
            }
        }


        internal void RemoveTasker(string taskname)
        {
            for (int i = 0; i < taskers.Count; i++)
            {
                string name = taskers?[i]?.Name;
                if(!string.IsNullOrEmpty(name) && taskname.CompareTo(name) == 0 )
                {
                    taskers.RemoveAt(i);
                    break;
                }
                
            }
        }

        /// <summary>
        /// 檢查Tasker 是否存在
        /// </summary>
        /// <param name="taskname"></param>
        /// <returns></returns>
        internal bool TaskerExists(string taskname)
        {
            bool found = false;
            for (int i = 0; i < taskers.Count; i++)
            {
                string name = taskers?[i]?.Name;
                found = !string.IsNullOrEmpty(name) && taskname.CompareTo(name) == 0;
                if (found) break;
            }
            return found;
        }

        /// <summary>
        /// 變更TASK執行間隔
        /// </summary>
        /// <param name="taskname"></param>
        /// <param name="interval"></param>
        internal void TaskerChangeInterval(string taskname, int interval)
        {
            
            for (int i = 0; i < taskers.Count; i++)
            {
                string name = taskers?[i]?.Name;
                bool found = !string.IsNullOrEmpty(name) && taskname.CompareTo(name) == 0;
                if (found)
                {
                    taskers?[i].ChangeInterval(interval);
                }
            }
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tasker"></param>
        internal void TaskerMessageUpdate(ITasker tasker, string dateTime, string message)
        {
            try
            {
                if (scheduleForm != null)
                {
                    Enums.MessageType messageType = tasker.Enabled ? Enums.MessageType.Important : Enums.MessageType.Error;

                    scheduleForm.MessageUpdate(tasker.Name, tasker.Unit.ToString() + " 分鐘", tasker.Status, dateTime, messageType);
                }
            } 
            catch
            {

            }
            
        }

        #endregion 任務計數器

        /// <summary>
        /// 主檔轉檔排程
        /// </summary>
        /// <param name="configuration"></param>
        internal void AddMasterTasker(Configuration configuration)
        {
            if (!configuration.MasterTaskEnabled)
            {
                AddOtherTasker(configuration);
                return;
            }

            AddTasker(masterTasker);
        }

        /// <summary>
        /// SOA拋轉排程
        /// </summary>
        /// <param name="configuration"></param>
        internal void AddOtherTasker(Configuration configuration)
        {
            AddCtrStTasker(configuration);
            AddDlvStTasker(configuration);
            AddOspStTasker(configuration);
            AddTrfStTasker(configuration);
        }

        /// <summary>
        /// 進櫃SOA排程
        /// </summary>
        internal void AddCtrStTasker(Configuration configuration)
        {
            if(!configuration.CtrTaskEnabled)
            {
                return;
            }

            AddTasker(ctrTasker);
        }

        /// <summary>
        /// 出貨SOA排程
        /// </summary>
        internal void AddDlvStTasker(Configuration configuration)
        {
            if(!configuration.DlvTaskEnabled)
            {
                return;
            }

            AddTasker(dlvTasker);
        }

        /// <summary>
        /// 加工SOA排程
        /// </summary>
        internal void AddOspStTasker(Configuration configuration)
        {
            if(!configuration.OspTaskEnabled)
            {
                return;
            }

            AddTasker(ospTasker);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void AddTrfStTasker(Configuration configuration)
        {
            if(!configuration.TrfTaskEnabled)
            {
                return;
            }

            AddTasker(trfTasker);
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
                    if (scheduleForm != null && !scheduleForm.IsDisposed)
                        scheduleForm.Dispose();
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
