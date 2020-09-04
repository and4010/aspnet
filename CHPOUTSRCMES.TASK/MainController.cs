using CHPOUTSRCMES.TASK.Forms;
using CHPOUTSRCMES.TASK.Models.Service;
using CHPOUTSRCMES.TASK.Tasks;
using CHPOUTSRCMES.TASK.Tasks.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK
{
    public class MainController : IDisposable
    {

        private static MainController instance;

        internal static MainController Instance => instance ?? (instance = new MainController());

        private Logger logger = LogManager.GetCurrentClassLogger();

        private string erpConnStr;

        internal string ErpConnStr => erpConnStr ?? (erpConnStr =
#if DEBUG
            System.Configuration.ConfigurationManager.ConnectionStrings["OracleTestContext"].ToString()
#else
            System.Configuration.ConfigurationManager.ConnectionStrings["ErpContext"].ToString()
#endif
            );

        private string mesConnStr;

        internal string MesConnStr => mesConnStr ?? (mesConnStr =
#if DEBUG
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

        #endregion 任務計數器


        internal void AddMasterTasker()
        {
            AddTasker(new Tasker("主檔轉檔程序", 25, (tasker, token) => {
                MasterService service = new MasterService(MesConnStr, ErpConnStr);
                //組織倉庫儲位
                var taskSubinventory = service.importSubinventory(tasker, token);
                //庫存交易類別
                var taskTransactionType = service.importTransactionType(tasker, token);
                //料號
                var taskItem = taskSubinventory.ContinueWith(task => service.importItem(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //餘切規格
                var taskOspRelatedItem = taskItem.ContinueWith(subTask => service.importOspRelatedItem(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //紙別機台
                var taskMachinePaperType = taskItem.ContinueWith(subTask => service.importMachinePaperType(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                //令重張數
                var tasYszmpckq = taskItem.ContinueWith(subTask => service.importYszmpckq(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(taskSubinventory, taskTransactionType, taskItem, taskOspRelatedItem, taskMachinePaperType, tasYszmpckq);

                AddCtrStTasker();
                AddDlvStTasker();
                AddOspStTasker();
            }));
        }

        /// <summary>
        /// 進櫃SOA排程
        /// </summary>
        internal void AddCtrStTasker()
        {
            AddTasker(new Tasker("進櫃轉檔程序", 5, (tasker, token) => {
                CtrStService service = new CtrStService(MesConnStr, ErpConnStr);
                var task = service.ImportCtrSt(tasker, token);
                var rvTask = task.ContinueWith(subTask => service.ExportCtrStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(task, rvTask);
            }));
        }

        /// <summary>
        /// 出貨SOA排程
        /// </summary>
        internal void AddDlvStTasker()
        {
            AddTasker(new Tasker("出貨轉檔程序", 5, (tasker, token) => {
                DlvStService service = new DlvStService(MesConnStr, ErpConnStr);
                var task = service.ImportDlvSt(tasker, token);
                var rvTask = task.ContinueWith(subTask => service.ExportDlvStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);

                Task.WaitAll(task, rvTask);
            }));
        }

        /// <summary>
        /// 加工SOA排程
        /// </summary>
        internal void AddOspStTasker()
        {
            AddTasker(new Tasker("加工轉檔程序", 5, (tasker, token) => {
                OspStService service = new OspStService(MesConnStr, ErpConnStr);
                var task = service.ImportOspSt(tasker, token);
                var rvStage1Task = task.ContinueWith(subTask => service.ExportOspStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                var rvStage2Task = task.ContinueWith(subTask => service.ExportOspStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                var rvStage3Task = task.ContinueWith(subTask => service.ExportOspStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                var rvStage4Task = task.ContinueWith(subTask => service.ExportOspStRv(tasker, token), TaskContinuationOptions.OnlyOnRanToCompletion);
                Task.WaitAll(task, rvStage1Task, rvStage2Task, rvStage3Task, rvStage4Task);
            }));
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
