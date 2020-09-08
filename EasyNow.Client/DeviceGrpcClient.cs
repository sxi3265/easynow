using System.Threading.Tasks;
using EasyNow.Dto.Device;
using EasyNow.Utility.Extensions;

namespace EasyNow.Client
{
    public class DeviceGrpcClient : IDeviceClient
    {
        private readonly Device.DeviceClient _client;

        public DeviceGrpcClient(Device.DeviceClient client)
        {
            _client = client;
        }

        public async Task<DeviceDto> AddOrUpdateAsync(DeviceDto model)
        {
            return (await _client.AddOrUpdateAsync(model.To<DeviceInfo>())).To<DeviceDto>();
        }

        public async Task UpdateStatusAsync(string socketId, Dto.DeviceStatus status)
        {
            await _client.UpdateStatusAsync(new UpdateStatusReq
            {
                SocketId = socketId,
                Status = status.To<DeviceStatus>()
            });
        }
    }
}