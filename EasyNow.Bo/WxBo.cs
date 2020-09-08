using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EasyNow.ApiClient.WxPusher;
using EasyNow.Bo.Abstractions;
using EasyNow.Dal;
using EasyNow.Dto.WxPusher;
using EasyNow.Utility.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EasyNow.Bo
{
    public class WxBo:BaseBo,IWxBo
    {
        private IWxPusher WxPusher => Scope.Resolve<IWxPusher>();

        public async Task WxPusherUserSubscribeAsync(string appKey, UserDto user)
        {
            if (!await Db.WxPusherAppUser.AnyAsync(e => e.WxPusherUser.Uid == user.Uid && e.WxPusherApp.Key == appKey))
            {
                var app = await Db.WxPusherApp.FirstOrDefaultAsync(e => e.Key == appKey);
                if (app == null)
                {
                    return;
                }
                var wxPusherUser = await Db.WxPusherUser.FirstOrDefaultAsync(e => e.Uid == user.Uid);
                if (wxPusherUser == null)
                {
                    wxPusherUser = user.To<WxPusherUser>();
                    await Db.WxPusherUser.AddAsync(wxPusherUser);
                }

                await Db.WxPusherAppUser.AddAsync(new WxPusherAppUser
                {
                    WxPusherApp = app,
                    WxPusherUser = wxPusherUser
                });
                await Db.SaveChangesAsync();
            }
        }
    }
}