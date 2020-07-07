using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using Android.Widget;
using Autofac;
using EasyNow.App.Droid.Util;
using Microsoft.Extensions.Logging;
using Uri = Android.Net.Uri;

namespace EasyNow.App.Droid.Script.Module
{
    public class AppModule
    {
        private readonly Context _context;
        private readonly ILifetimeScope _scope;
        private UiHandler UiHandler => _scope.Resolve<UiHandler>();
        public string FileProviderAuthority { get; }
        private ILogger<AppModule> Logger => _scope.Resolve<ILogger<AppModule>>();

        public AppModule(Context context, ILifetimeScope scope, string fileProviderAuthority=null)
        {
            _context = context;
            _scope = scope;
            this.FileProviderAuthority = fileProviderAuthority;
        }

        public void Toast(string msg)
        {
            UiHandler.Toast(msg);
        }

        public void Toast(int resId)
        {
            UiHandler.Toast(resId);
        }

        /// <summary>
        /// 开启自动化操作
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool StartAuto(double timeout=60_000)
        {
            try
            {
                var startTime = DateTime.UtcNow;
                if (!SettingUtil.IsAccessibilityEnabled(_context))
                {
                    UiHandler.SettingAccessibility();
                    do
                    {
                        if (timeout != 0 && (DateTime.UtcNow - startTime).TotalMilliseconds >= timeout)
                        {
                            return false;
                        }

                        Thread.Sleep(2000);
                    } while (!SettingUtil.IsAccessibilityEnabled(_context));
                }

                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e,"开启自动化操作失败");
                return false;
            }
            
        }

        /// <summary>
        /// 通过应用名称启动应用
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public bool LaunchAppByAppName(string appName)
        {
            return this.LaunchApp(GetPackageNameByAppName(appName));
        }

        /// <summary>
        /// 根据应用名称得到包名
        /// </summary>
        /// <param name="appName"></param>
        /// <returns></returns>
        public string GetPackageNameByAppName(string appName)
        {
            var packageManager = _context.PackageManager;
            return packageManager.GetInstalledApplications(PackageInfoFlags.MetaData)
                .Where(e => packageManager.GetApplicationLabel(e).Equals(appName)).Select(e => e.PackageName)
                .FirstOrDefault();
        }

        /// <summary>
        /// 根据包名获取应用名称
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public string GetAppName(string packageName)
        {
            var packageManager = _context.PackageManager;
            try
            {
                return packageManager.GetApplicationLabel(packageManager.GetApplicationInfo(packageName, 0));
            }
            catch(Exception e)
            {
                Logger.LogWarning(e,$"根据包名{packageName}获取应用名失败");
                return null;
            }
        }

        /// <summary>
        /// 通过包名启用应用
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public bool LaunchApp(string packageName)
        {
            try
            {
                var packageManager = _context.PackageManager;
                var intent = packageManager.GetLaunchIntentForPackage(packageName);
                if (intent == null)
                {
                    return false;
                }
                _context.StartActivity(intent.AddFlags(ActivityFlags.NewTask));
                return true;
            }
            catch (Exception e)
            {
                Logger.LogWarning(e,$"启动App{packageName}失败");
                return false;
            }
        }

        /// <summary>
        /// 打开应用设置
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public bool OpenAppSetting(string packageName)
        {
            return IntentUtil.OpenAppDetailSettings(_context,packageName);
        }

        /// <summary>
        /// 卸载应用
        /// </summary>
        /// <param name="packageName"></param>
        public void Uninstall(string packageName)
        {
            _context.StartActivity(
                new Intent(Intent.ActionDelete, Uri.Parse($"package:{packageName}")).AddFlags(ActivityFlags.NewTask));
        }

        /// <summary>
        /// 查看文件
        /// </summary>
        /// <param name="path"></param>
        public void ViewFile(string path)
        {
            IntentUtil.ViewFile(_context, path, FileProviderAuthority);
        }

        /// <summary>
        /// 睡眠
        /// </summary>
        /// <param name="milliseconds"></param>
        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }
    }
}