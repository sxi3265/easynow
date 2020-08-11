using AutoMapper;
using EasyNow.Dal.Entities;
using EasyNow.Dto.Device;
using EasyNow.Dto.Script;

namespace EasyNow.Bo.Profiles
{
    public class BasicProfile:Profile
    {
        public BasicProfile()
        {
            this.CreateMap<Device, DeviceDto>().ReverseMap();
            this.CreateMap<Script, ScriptInfo>().ReverseMap();
        }
    }
}