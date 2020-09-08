using System;
using System.Threading.Tasks;
using EasyNow.Dto;
using EasyNow.Dto.Script;
using EasyNow.Dto.User;
using EasyNow.Utility.Collection;
using Refit;

namespace EasyNow.ApiClient
{
    public interface IEasyNowApi
    {
        [Get("/api/v1.0/Script/Query")]
        Task<Result<PagedList<ScriptInfo>>> QueryScript(ScriptQueryModel model);

        [Post("/api/v1.0/User/Login")]
        Task<Result<LoginResult>> Login([Body]LoginModel model);
    }
}
