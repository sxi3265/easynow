using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Com.Xiaomi.Mipush.Sdk;
using EasyNow.App.Droid.Accessibility;
using EasyNow.App.Droid.Accessibility.Event;
using EasyNow.App.Droid.Frida;
using EasyNow.App.Droid.Native;
using EasyNow.App.Droid.Runtime;
using EasyNow.App.Droid.Runtime.Api;
using EasyNow.App.Droid.Script;
using EasyNow.App.Droid.Script.Module;
using EasyNow.App.Droid.Service;
using EasyNow.App.Droid.Services;
using EasyNow.App.Droid.Util;
using EasyNow.App.Models;
using EasyNow.App.Services;
using EasyNow.App.ViewModels;
using Java.IO;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using Xamarin.Forms;
using Activity = Android.App.Activity;
using Logger = EasyNow.App.Services.Logger;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using Process = System.Diagnostics.Process;
using String = Java.Lang.String;

namespace EasyNow.App.Droid
{
    [Activity(Label = "EasyNow", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private IContainer _container;

        private bool ShouldInitMiPush() {
            ActivityManager am = ((ActivityManager) GetSystemService(Context.ActivityService));
            string mainProcessName = this.ApplicationInfo.ProcessName;
            int myPid = Android.OS.Process.MyPid();
            foreach (var info in am.RunningAppProcesses) {
                if (info.Pid == myPid && mainProcessName==info.ProcessName) {
                    return true;
                }
            }
            return false;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            MessagingCenter.Subscribe<object,Item>(this,"ItemClick", (_,item) =>
            {
                if (string.IsNullOrEmpty(item.Source))
                {
                    Task.Run(_container.Resolve<FridaAgent>().Download).Wait();
                    Task.Run(_container.Resolve<FridaAgent>().Start).Wait();
                    return;
                }

                var intent = new Intent(this,typeof(ScriptService));
                intent.PutExtra("ScriptSource", item.Source);
                this.StartService(intent);
            });
            if (ShouldInitMiPush())
            {
                MiPushClient.RegisterPush(this, "2882303761518998214", "5331899889214");
            }

            MiPushClient.EnablePush(this);
            
            // 捕获全局未处理异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            var builder = new ContainerBuilder();
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                var config = new XmlLoggingConfiguration("assets/nlog.config");
                var logDir = this.GetExternalFilesDir("logs").Path;
                if (config.Variables.ContainsKey("logDirectory"))
                {
                    config.Variables["logDirectory"] = logDir;
                }
                else
                {
                    config.Variables.Add("logDirectory",logDir);
                }

                loggingBuilder.AddNLog(config);
            });
            builder.Populate(services);
            builder.RegisterType<MockDataStore>().AsImplementedInterfaces();
            builder.RegisterType<ItemsViewModel>();
            builder.RegisterType<Logger>().AsImplementedInterfaces();
            builder.RegisterType<App>();
            builder.RegisterType<HelpService>().AsImplementedInterfaces();
            builder.RegisterType<DefaultAccessibilityBridge>().As<AccessibilityBridge>().SingleInstance();
            builder.RegisterType<UiModule>().SingleInstance();
            builder.RegisterType<UiHandler>().SingleInstance();
            builder.RegisterType<AppModule>().SingleInstance();
            builder.RegisterType<DeviceModule>().SingleInstance();
            builder.RegisterInstance(this).Named<Activity>("Main");
            builder.RegisterType<JsScriptEngine>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<JsScriptRuntime>().AsImplementedInterfaces();
            builder.RegisterType<NotificationEvent>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ActivityInfoEvent>().AsSelf().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ProcessShell>().AsSelf().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<FridaAgent>().SingleInstance();
            builder.Register(c => c.ResolveNamed<Activity>("Main").ApplicationContext);
            _container = builder.Build();
            LoadApplication(_container.Resolve<App>());
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", e.Exception);
            LogUnhandledException(newExc);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", e.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal void LogUnhandledException(Exception exception)
        {
            try
            {
                _container?.Resolve<ILogger<App>>().LogError(exception,"未处理异常");
                var errorMessage = string.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                    DateTime.Now, exception.ToString());

                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch
            {
                // just suppress any error logging exceptions
            }
        } 

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}