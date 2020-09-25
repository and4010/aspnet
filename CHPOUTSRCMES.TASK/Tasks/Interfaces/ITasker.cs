using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CHPOUTSRCMES.TASK.Tasks.Interfaces
{
    public interface ITasker
    {
        /// <summary>
        /// 名稱
        /// </summary>
        string Name { set; get; }

        /// <summary>
        /// 修改執行間隔
        /// </summary>
        /// <param name="interval"></param>
        void ChangeInterval(int interval);

        /// <summary>
        /// 執行
        /// </summary>
        /// <param name="factory">TaskFactory元件</param>
        /// <param name="token">取消權杖</param>
        /// <param name="completed">完成回呼</param>
        Task Run(TaskFactory factory, CancellationToken token, Action<string, Task> completed);
    }
}
