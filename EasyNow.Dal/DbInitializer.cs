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
                    Password = "sbxaialhj",
                    Name = "sxi3265"
                });
            }
            else
            {
                var user=context.User.First();
                user.Name = "test";
            }

            if (context.HasChanges())
            {
                context.SaveChanges();
            }
        }
    }
}