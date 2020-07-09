using Autofac;
using EasyNow.Dal;

namespace EasyNow.Bo
{
    public abstract class BaseBo
    {
        protected ILifetimeScope Scope;
        protected virtual EasyNowContext Db => Scope.Resolve<EasyNowContext>();
    }
}