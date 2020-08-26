using System;
using AutoMapper;
using EasyNow.Dto.Device;
using EasyNow.Utility.Extensions;
using Google.Protobuf.WellKnownTypes;

namespace EasyNow.Api.Profiles
{
    public class DeviceProfile : Profile
    {
        public DeviceProfile()
        {
            this.CreateMap<Timestamp,DateTime>().ConvertUsing(s=>s.ToDateTime());
            this.CreateMap<DateTime,Timestamp>().ConvertUsing(s=>Timestamp.FromDateTime(DateTime.SpecifyKind(s,DateTimeKind.Utc)));

            this.CreateMap<DeviceDto, BaseInfo>(MemberList.None);
            this.CreateMap<BaseInfo, DeviceDto>(MemberList.None);
            this.CreateMap<DeviceDto, DeviceInfo>()
                .AfterMap((s, d) =>
                {
                    d.BaseInfo = s.To<BaseInfo>();
                });
            this.CreateMap<DeviceInfo, DeviceDto>().AfterMap((s, d) =>
                {
                    s.BaseInfo.CopyTo(d);
                });
        }
    }
}