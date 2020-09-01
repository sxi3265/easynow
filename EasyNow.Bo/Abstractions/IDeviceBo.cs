using System.Threading.Tasks;
using EasyNow.Dto;
using EasyNow.Dto.Device;
using JetBrains.Annotations;

namespace EasyNow.Bo.Abstractions
{
    public interface IDeviceBo
    {
        Task<DeviceDto> AddAsync(DeviceDto model);
        Task<DeviceDto> AddOrUpdateAsync(DeviceDto model);

        Task UpdateStatusAsync([NotNull]string socketId, DeviceStatus status);
    }
}