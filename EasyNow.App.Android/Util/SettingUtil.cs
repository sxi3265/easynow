using System;
using System.Linq;
using Android.AccessibilityServices;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Support.Annotation;
using Android.Views.Accessibility;
using Xamarin.Forms;
using AccessibilityService = EasyNow.App.Droid.Accessibility.AccessibilityService;
using Uri = Android.Net.Uri;

namespace EasyNow.App.Droid.Util
{
    public class SettingUtil
    {
        /// <summary>
        /// 出现在其他应用上 权限
        /// </summary>
        /// <param name="context"></param>
        [RequiresApi(Api = (int)BuildVersionCodes.M)]
        public static void SettingOverlayPermission(Context context)
        {
            var intent = new Intent(Settings.ActionManageOverlayPermission);
            intent.SetData(Uri.Parse($"package:{context.PackageName}"));
            context.StartActivity(intent.AddFlags(ActivityFlags.NewTask));
        }

        /// <summary>
        /// 打开无障碍设置界面
        /// </summary>
        /// <param name="context"></param>
        public static void SettingAccessibility(Context context)
        {
            var intent = new Intent(Settings.ActionAccessibilitySettings);
            intent.SetFlags(ActivityFlags.NewTask);
            context.StartActivity(intent);
        }

        /// <summary>
        /// 判断是否开启无障碍服务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsAccessibilityEnabled(Context context)
        {
            if (context == null)
            {
                return false;
            }

            var expectedComponentName = new ComponentName(context, Java.Lang.Class.FromType(typeof(AccessibilityService)));
            var enabledServicesSetting =
                Settings.Secure.GetString(context.ContentResolver, Settings.Secure.EnabledAccessibilityServices);
            if (string.IsNullOrEmpty(enabledServicesSetting))
            {
                return false;
            }
            return enabledServicesSetting.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries)
                .Any(e =>
                {
                    var enabledService = ComponentName.UnflattenFromString(e);
                    return enabledService.PackageName== expectedComponentName.PackageName&&enabledService.ClassName==expectedComponentName.ClassName;
                });
        }
    }
}