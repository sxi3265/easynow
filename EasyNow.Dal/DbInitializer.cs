using Autofac;
using Microsoft.EntityFrameworkCore;

namespace EasyNow.Dal
{
    public static class DbInitializer
    {
        public static void Initialize(ILifetimeScope scope, EasyNowContext context)
        {
            context.Database.Migrate();
        }
    }
}