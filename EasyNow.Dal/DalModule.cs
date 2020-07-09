using Autofac;
using EasyNow.Dal.Mapping;

namespace EasyNow.Dal
{
    public class DalModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(this.GetType().Assembly).AssignableTo<IMap>()
                .Where(e => !e.IsAbstract).AsImplementedInterfaces().SingleInstance();
        }
    }
}