using AutoMapper;
using EasyNow.Dal.Entities;
using EasyNow.Dto.Device;

namespace EasyNow.Bo.Profiles
{
    public class BasicProfile:Profile
    {
        public BasicProfile()
        {
            this.CreateMap<Device, DeviceDto>().ReverseMap();
        }
    }
}