using AutoMapper;
using EasyNow.Dto.Device;

namespace EasyNow.Client.Profiles
{
    public class DeviceProfile:Profile
    {
        public DeviceProfile()
        {
            this.CreateMap<DeviceInfo, DeviceDto>()
                .ForAllMembers(opts =>
                {
                    opts.MapFrom(s=>s.BaseInfo);
                    opts.MapFrom(s=>s);
                });
        }
    }
}