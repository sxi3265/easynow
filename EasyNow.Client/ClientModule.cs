using Autofac;
using Grpc.Core;
using Grpc.Net.Client;

namespace EasyNow.Client
{
    public class ClientModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(GrpcChannel.ForAddress("https://api.easynow.me")).As<ChannelBase>();
            builder.RegisterType<DeviceGrpcClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ScriptGrpcClient>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<Device.DeviceClient>().SingleInstance();
            builder.RegisterType<Script.ScriptClient>().SingleInstance();
        }
    }
}