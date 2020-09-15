using System;
using System.Threading.Tasks;

namespace EasyNow.Job
{
    public interface IJob
    {
        Task ExecuteAsync();
    }
}