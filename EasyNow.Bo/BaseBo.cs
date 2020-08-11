using Autofac;
using EasyNow.Dal;

namespace EasyNow.Bo
{
    public abstract class BaseBo
    {
        public ILifetimeScope Scope { get; set; }
        protected virtual EasyNowContext Db => Scope.Resolve<EasyNowContext>();
    }
}