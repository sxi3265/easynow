using System.Threading.Tasks;

namespace EasyNow.Coin.Bo
{
    public interface IRule
    {
        Task RunAsync();
    }
}