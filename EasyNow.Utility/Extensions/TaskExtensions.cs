using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyNow.Utility.Extensions
{
    public static class TaskExtensions
    {
        /// <summary>
        /// 按顺序执行任务列表
        /// </summary>
        /// <param name="tasks">任务列表</param>
        /// <param name="index">开始位置</param>
        public static void OrderRunTask(this IList<Task> tasks, int index = 0)
        {
            if (tasks.Count > index)
            {
                var task = tasks[index];
                task.Start();
                if (tasks.Count > index + 1)
                {
                    task.ContinueWith(t => tasks.OrderRunTask(index + 1));
                }
            }
        }
    }
}