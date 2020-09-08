using System.Threading.Tasks;
using EasyNow.Dto.User;

namespace EasyNow.Bo.Abstractions
{
    public interface IUserBo
    {
        Task<LoginResult> Login(LoginModel model);
    }
}