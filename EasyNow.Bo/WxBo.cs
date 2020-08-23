using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EasyNow.ApiClient.WxPusher;
using EasyNow.Bo.Abstractions;
using EasyNow.Dal;
using EasyNow.Utility.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EasyNow.Bo
{
    public class WxBo:BaseBo,IWxBo
    {
        private IWxPusher WxPusher => Scope.Resolve<IWxPusher>();

        public async Task WxPusherUserSubscribeAsync(string appKey, string uid)
        {
            if (!await Db.WxPusherAppUser.AnyAsync(e => e.WxPusherUser.Uid == uid && e.WxPusherApp.Key == appKey))
            {
                var app = await Db.WxPusherApp.FirstOrDefaultAsync(e => e.Key == appKey);
                if (app == null)
                {
                    return;
                }
                var user = await Db.WxPusherUser.FirstOrDefaultAsync(e => e.Uid == uid);
                if (user == null)
                {
                    return;
                }

                await Db.WxPusherAppUser.AddAsync(new WxPusherAppUser
                {
                    WxPusherApp = app,
                    WxPusherUser = user
                });
                await Db.SaveChangesAsync();
            }
        }
    }
}