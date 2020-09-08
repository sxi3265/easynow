using System.Threading.Tasks;

namespace EasyNow.Tasks
{
    public interface ITask
    {
        Task ExecuteAsync();
    }
}