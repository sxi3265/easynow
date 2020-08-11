using Android.Content;
using Android.Net;
using Android.Provider;
using Java.IO;
using Xamarin.Essentials;

namespace EasyNow.App.Droid.Util
{
    public class IntentUtil
    {
        public static bool OpenAppDetailSettings(Context context, string packageName)
        {
            try
            {
                var intent=new Intent(Settings.ActionApplicationDetailsSettings);
                intent.AddCategory(Intent.CategoryDefault);
                intent.AddFlags(ActivityFlags.NewTask);
                intent.SetData(Uri.Parse($"package:{packageName}"));
                context.StartActivity(intent);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool ViewFile(Context context, string path,string fileProviderAuthority)
        {
            var mimeType = MimeTypeUtil.FromFile(path);
            return ViewFile(context,path,mimeType,fileProviderAuthority);
        }

        public static bool ViewFile(Context context, string path,string mimeType,string fileProviderAuthority)
        {
            try
            {
                context.StartActivity(new Intent(Intent.ActionView)
                    .SetDataAndType(GetUriOfFile(context, path, fileProviderAuthority), mimeType)
                    .AddFlags(ActivityFlags.NewTask)
                    .AddFlags(ActivityFlags.GrantReadUriPermission)
                    .AddFlags(ActivityFlags.GrantWriteUriPermission));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static Uri GetUriOfFile(Context context, string path, string fileProviderAuthority)
        {
            if (string.IsNullOrEmpty(fileProviderAuthority))
            {
                return Uri.Parse($"file://{path}");
            }

            return FileProvider.GetUriForFile(context, fileProviderAuthority, new File(path));
        }
    }
}