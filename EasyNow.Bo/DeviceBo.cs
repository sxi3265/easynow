using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using EasyNow.Bo.Abstractions;
using EasyNow.Dal;
using EasyNow.Dto.Device;
using EasyNow.Utility.Extensions;
using Microsoft.EntityFrameworkCore;
using DeviceStatus = EasyNow.Dto.DeviceStatus;

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
            else
            {
                model.Id = device.Id;
                model.CopyTo(device);
            }
            
            await this.Db.SaveChangesAsync();
            return device.To<DeviceDto>();
        }

        public async Task UpdateStatusAsync(string socketId, DeviceStatus status)
        {
            var device =await this.Db.Device.FirstOrDefaultAsync(e => e.SocketId == socketId);
            if (device == null)
            {
                return;
            }

            device.Status = status.To<Dal.DeviceStatus>();
            await this.Db.SaveChangesAsync();
        }
    }
}