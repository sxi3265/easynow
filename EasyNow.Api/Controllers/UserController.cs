using System.Threading.Tasks;
using Autofac;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyNow.Api.Controllers
{
    [ApiVersion("1")]
    public class UserController:ApiBaseController
    {
        private IUserBo UserBo => Scope.Resolve<IUserBo>();

        [HttpPost,AllowAnonymous]
        public Task<LoginResult> Login(LoginModel model)
        {
            return UserBo.Login(model);
        }
    }
}