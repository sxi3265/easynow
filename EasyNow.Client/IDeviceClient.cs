using System.Threading.Tasks;
using EasyNow.Dto.Device;
using JetBrains.Annotations;

namespace EasyNow.Client
{
    public interface IDeviceClient
    {
        Task<DeviceDto> AddOrUpdateAsync(DeviceDto model);
        Task UpdateStatusAsync([NotNull]string socketId,Dto.DeviceStatus status);
    }
}