using Autofac;
using EasyNow.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace EasyNow.Dal
{
    public static class DbInitializer
    {
        public static void Initialize(ILifetimeScope scope, EasyNowContext context)
        {
            context.Database.Migrate();

            if (!context.Script.Any())
            {
                context.Script.Add(new Script
                {
                    Name = "test",
                    Code = "test",
                    Content = new byte[0]
                });
            }

            if (!context.User.Any())
            {
                context.User.Add(new User
                {
                    Account = "sxi3265",
                    Password = "sbxaialhj"
                });
            }

            if (context.ChangeTracker.Entries().Any())
            {
                context.SaveChanges();
            }
        }
    }
}