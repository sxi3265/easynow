using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Com.Xiaomi.Mipush.Sdk;
using EasyNow.App.Droid.Script.Module;
using Xamarin.Forms;

namespace EasyNow.App.Droid.Receiver
{
    /// <summary>
    /// mipush消息接收器
    /// </summary>
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "com.xiaomi.mipush.RECEIVE_MESSAGE","com.xiaomi.mipush.MESSAGE_ARRIVED","com.xiaomi.mipush.ERROR" })]
    public class MiPushMessageReceiver:PushMessageReceiver
    {
        public override void OnReceivePassThroughMessage(Context p0, MiPushMessage p1)
        {
            base.OnReceivePassThroughMessage(p0, p1);
        }

        public override void OnNotificationMessageClicked(Context p0, MiPushMessage p1)
        {
            DependencyService.Resolve<AppModule>().Toast($"点击消息{p1.Title}");
            base.OnNotificationMessageClicked(p0, p1);
        }

        public override void OnNotificationMessageArrived(Context p0, MiPushMessage p1)
        {
            DependencyService.Resolve<AppModule>().Toast($"接收到消息{p1.Title}");
            base.OnNotificationMessageArrived(p0, p1);
        }

        public override void OnCommandResult(Context p0, MiPushCommandMessage p1)
        {
            var command = p1.Command;
            base.OnCommandResult(p0, p1);
        }

        public override void OnReceiveRegisterResult(Context p0, MiPushCommandMessage p1)
        {
            if (p1.Command == MiPushClient.CommandRegister)
            {
                var args = p1.CommandArguments.ToArray();
                // todo 测试代码
                MiPushClient.SetAlias(p0,"sxi3265",null);
            }
            base.OnReceiveRegisterResult(p0, p1);
        }

        public override void OnReceiveMessage(Context p0, MiPushMessage p1)
        {
            base.OnReceiveMessage(p0, p1);
        }
    }
}