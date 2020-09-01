using Autofac;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

namespace EasyNow.Client
{
    public class ClientModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => GrpcChannel.ForAddress(c.Resolve<IOptions<ClientOptions>>().Value.ApiUri)).As<ChannelBase>();
            builder.RegisterType<DeviceGrpcClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ScriptGrpcClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<Device.DeviceClient>().SingleInstance();
            builder.RegisterType<Script.ScriptClient>().SingleInstance();
        }
    }
}