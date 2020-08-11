using Autofac;

namespace EasyNow.Bo
{
    public class BoModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly).AsImplementedInterfaces().PropertiesAutowired()
                .InstancePerLifetimeScope();
        }
    }
}