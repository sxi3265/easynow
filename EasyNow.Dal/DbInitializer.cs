using System.Linq;
using Autofac;
using Microsoft.EntityFrameworkCore;

namespace EasyNow.Dal
{
    public static class DbInitializer
    {
        public static void Initialize(ILifetimeScope scope, EasyNowContext context)
        {
            context.Database.Migrate();

            if (!context.User.Any())
            {
                context.User.Add(new User
                {
                    Account = "sxi3265",
                    Password = "sbxaialhj"
                });
            }

            if (context.HasChanges())
            {
                context.SaveChanges();
            }
        }
    }
}