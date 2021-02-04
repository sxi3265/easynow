using System.Text.RegularExpressions;
using Android.AccessibilityServices;
using Android.Graphics;
using Autofac;
using EasyNow.App.Droid.Accessibility;
using EasyNow.App.Droid.Runtime.Api;

namespace EasyNow.App.Droid.Script.Module
{
    public class DeviceModule
    {
        private readonly ILifetimeScope _scope;
        private AccessibilityBridge AccessibilityBridge => _scope.Resolve<AccessibilityBridge>();

        private static Regex NewLineRegex = new Regex("\r|\n");

        public DeviceModule(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void WaitAuto()
        {

        }

        public void Touch(double x, double y, double duration=10)
        {
            var path = new Path();
            path.MoveTo((int)x, (int)y);
            AccessibilityBridge.Service.DispatchGesture(
                new GestureDescription.Builder().AddStroke(new GestureDescription.StrokeDescription(path, 0, (int)duration))
                    .Build(), null, null);
        }

        /// <summary>
        /// 获取AndroidId
        /// </summary>
        /// <returns></returns>
        public string GetAndroidId()
        {
            return Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }

        /// <summary>
        /// 获取CPU架构
        /// </summary>
        /// <returns></returns>
        public string GetCpuAbi()
        {
            return NewLineRegex.Replace(IShell.Exec("getprop ro.product.cpu.abi").Result,string.Empty);
        }
    }
}