using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CHPOUTSRCMES.TASK.Tasks
{
    /// <summary>
    /// 任務計時器(分鐘)
    /// </summary>
    public class TaskTimer
    {
        private static TaskTimer _instance;
        private List<Timer> timers = new List<Timer>();

        private TaskTimer() { }

        public static TaskTimer Instance => _instance ?? (_instance = new TaskTimer());

        public void Schedule(int hour, int min, double intervalMinutes, Action task)
        {
            DateTime now = DateTime.Now;
            #region 計算首次執行時間
            DateTime firstRun = new DateTime(now.Year, now.Month, now.Day, hour, min, 0, 0);
            if (now > firstRun)
            {
                firstRun = firstRun.AddDays(1);
            }

            TimeSpan timeToGo = firstRun - now;
            if (timeToGo <= TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }
            #endregion

            var timer = new Timer(x =>
            {
                task.Invoke();
            }, null, timeToGo, TimeSpan.FromMinutes(intervalMinutes));

            timers.Add(timer);
        }
    }
}
