using AutoMapper;
using EasyNow.Dal;
using EasyNow.Dto.Device;
using EasyNow.Dto.Script;
using EasyNow.Dto.WxPusher;

namespace EasyNow.Bo.Profiles
{
    public class BasicProfile:Profile
    {
        public BasicProfile()
        {
            this.CreateMap<Device, DeviceDto>().ReverseMap();
            //this.CreateMap<Script, ScriptInfo>().ReverseMap();
            this.CreateMap<ReqData, Dto.WxPusher.UserDto>()
                .ForMember(e => e.NickName, opts =>
                {
                    opts.MapFrom(s => s.UserName);
                })
                .ForMember(e=>e.HeadImg, opts =>
                {
                    opts.MapFrom(s=>s.UserHeadImg);
                })
                .ForMember(e=>e.Enable, opts =>
                {
                    opts.MapFrom(s=>true);
                })
                .ForMember(e=>e.SubTime, opts =>
                {
                    opts.MapFrom(s=>s.Time);
                });
            this.CreateMap<UserDto, WxPusherUser>();

            this.CreateMap<Dto.DeviceStatus, Dal.DeviceStatus>();
        }
    }
}