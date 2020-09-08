using AutoMapper;
using EasyNow.Dto.Script;

namespace EasyNow.Api.Profiles
{
    public class ScriptProfile: Profile
    {
        public ScriptProfile()
        {
            this.CreateMap<ScriptQueryReq, ScriptQueryModel>();
        }
    }
}