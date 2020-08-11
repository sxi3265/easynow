using System.Threading.Tasks;
using EasyNow.Dto.Device;

namespace EasyNow.Bo.Abstractions
{
    public interface IDeviceBo
    {
        Task<DeviceDto> AddAsync(DeviceDto model);
        Task<DeviceDto> AddOrUpdateAsync(DeviceDto model);
    }
}