using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Autofac;
using EasyNow.App.Droid.Script;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;

namespace EasyNow.App.Droid.Service
{
    [Service]
    public class ScriptService:Android.App.Service
    {
        private ILifetimeScope _scope;
        private ConcurrentDictionary<Guid, CancellationTokenSource> _ctsList;
        private ILogger Logger=>_scope.Resolve<ILogger<ScriptService>>();

        public override void OnCreate()
        {
            base.OnCreate();
            _scope = DependencyService.Resolve<ILifetimeScope>();
            _ctsList=new ConcurrentDictionary<Guid, CancellationTokenSource>();
            var manager = (NotificationManager)GetSystemService(NotificationService);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notificationChannel=new NotificationChannel("Script","Script",NotificationImportance.High);
                manager.CreateNotificationChannel(notificationChannel);
            }

            var notification = BuildForegroundNotification();
            // todo 点击打开应用
            StartForeground(1, notification);
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var source = intent.GetStringExtra("ScriptSource");
            var cts=  new CancellationTokenSource();
            var jobId = Guid.NewGuid();
            _ctsList.TryAdd(jobId, cts);
            Task.Run(() =>
            {
                try
                {
                    using var scriptRuntime = _scope.Resolve<IScriptEngine>().CreateScriptRuntime(cts.Token);
                    scriptRuntime.Execute(source);
                    _ctsList.TryRemove(jobId, out _);
                }
                catch (Exception e)
                {
                    Logger.LogError(e,"执行脚本报错");
                }
            }, cts.Token);
            return base.OnStartCommand(intent, flags, startId);
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override bool StopService(Intent name)
        {
            foreach (var item in _ctsList)
            {
                if (!item.Value.IsCancellationRequested)
                {
                    item.Value.Cancel();
                }
            }

            return base.StopService(name);
        }

        private Android.App.Notification BuildForegroundNotification() {
            
            var builder = new Android.App.Notification.Builder(this,"Script");

            builder.SetOngoing(true);
            builder.SetContentTitle(GetString(Resource.String.notification_title))
                .SetContentText(GetString(Resource.String.notification_content))
                .SetSmallIcon(Resource.Mipmap.icon)
                .SetTicker(GetString(Resource.String.notification_ticker));
            return builder.Build();
        }
    }
}