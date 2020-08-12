using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using EasyNow.Bo.Abstractions;
using EasyNow.Dal;
using EasyNow.Dto.Device;
using EasyNow.Utility.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EasyNow.Bo
{
    public class DeviceBo:BaseBo,IDeviceBo
    {
        public async Task<DeviceDto> AddAsync(DeviceDto model)
        {
            var entity = model.To<Device>();
            await this.Db.Device.AddAsync(entity);
            await this.Db.SaveChangesAsync();
            return entity.To<DeviceDto>();
        }

        public async Task<DeviceDto> AddOrUpdateAsync(DeviceDto model)
        {
            var device=await this.Db.Device.FirstOrDefaultAsync(e => e.Uuid == model.Uuid);
            if (device == null)
            {
                device = model.To<Device>();
                await this.Db.Device.AddAsync(device);
            }

            model.CopyTo(device);
            await this.Db.SaveChangesAsync();
            return device.To<DeviceDto>();
        }
    }
}