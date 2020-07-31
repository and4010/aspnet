using CHPOUTSRCMES.TASK.Tasks.Interfaces;
using NLog;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public class Tasker : ITasker
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// 任務名稱
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// 是否啟用
        /// </summary>
        public bool Enabled { set; get; }

        /// <summary>
        /// 單位量
        /// </summary>
        public int Unit { set; get; }

        /// <summary>
        /// 倒數單位
        /// </summary>
        private int countDown = 0;

        /// <summary>
        /// 執行的程序
        /// </summary>
        private Action<Tasker, CancellationToken> action;

        /// <summary>
        /// 程序為可執行狀態
        /// </summary>
        public bool ShallRun
        {
            get { return Enabled && --countDown <= 0; }
        }

        /// <summary>
        /// 重置倒數
        /// </summary>
        private void resetCountDown()
        {
            logger.Info($"resetCountDown (Name:{Name}, Unit:{Unit})");
            countDown = Unit;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="name">任務名稱</param>
        /// <param name="unit">單位量</param>
        /// <param name="action">程序</param>
        public Tasker(string name, int unit, Action<Tasker, CancellationToken> action)
        {
            logger.Info($"Name:{name}, Unit:{unit}, Enabled:{Enabled}");
            Name = name;
            Unit = unit;
            countDown = 0;
            this.action = action;
            Enabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="token"></param>
        Task ITasker.Run(TaskFactory factory, CancellationToken token, Action<string, Task> completed)
        {
            if (!ShallRun) return null;

            var task = factory.StartNew(() =>
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                action.Invoke(this, token);
            });

            task.ContinueWith(t => {
                //任務結束，恢復倒數 通知Timer
                resetCountDown();
                completed?.Invoke(this.Name, task);
                
            });

            return task;
        }
    }
}
