using System.Threading.Tasks;
using EasyNow.Bo.Abstractions;
using EasyNow.Dto.Device;
using EasyNow.Utility.Extensions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace EasyNow.Api.Services
{
    public class DeviceService:Device.DeviceBase
    {
        private readonly IDeviceBo _deviceBo;

        public DeviceService(IDeviceBo deviceBo)
        {
            _deviceBo = deviceBo;
        }

        public override async Task<DeviceInfo> AddOrUpdate(DeviceInfo request, ServerCallContext context)
        {
            return (await _deviceBo.AddOrUpdateAsync(request.To<DeviceDto>())).To<DeviceInfo>();
        }

        public override async Task<Empty> UpdateStatus(UpdateStatusReq request, ServerCallContext context)
        {
            await _deviceBo.UpdateStatusAsync(request.SocketId, request.Status.To<Dto.DeviceStatus>());
            return new Empty();
        }
    }
}