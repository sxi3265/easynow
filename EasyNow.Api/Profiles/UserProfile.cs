using AutoMapper;
using EasyNow.Dto.User;

namespace EasyNow.Api.Profiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            this.CreateMap<UserLoginReq, LoginModel>();
            this.CreateMap<LoginResult, UserLoginResp>();
        }
    }
}