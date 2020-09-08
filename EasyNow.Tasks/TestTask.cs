using System;
using System.Threading.Tasks;

namespace EasyNow.Tasks
{
    public class TestTask: ITask
    {
        public Task ExecuteAsync()
        {
            Console.WriteLine("测试任务");
            return Task.CompletedTask;
        }
    }
}
