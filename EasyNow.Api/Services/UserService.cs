using System.Threading.Tasks;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto.User;
using EasyNow.Utility.Extensions;
using Grpc.Core;

namespace EasyNow.Api.Services
{
    public class UserService:User.UserBase
    {
        private readonly IUserBo _userBo;

        public UserService(IUserBo userBo)
        {
            this._userBo = userBo;
        }

        public override async Task<UserLoginResp> Login(UserLoginReq request, ServerCallContext context)
        {
            return (await _userBo.Login(request.To<LoginModel>())).To<UserLoginResp>();
        }
    }
}