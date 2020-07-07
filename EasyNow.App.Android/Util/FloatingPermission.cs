using Android.Content;
using Android.Net;
using Android.OS;
using Android.Provider;
using Android.Support.Annotation;

namespace EasyNow.App.Droid.Util
{
    public class FloatingPermission
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
    }
}