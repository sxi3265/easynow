using System.IO;
using Android.Webkit;

namespace EasyNow.App.Droid.Util
{
    public class MimeTypeUtil
    {
        public static string FromFile(string path, string defaultType="*/*")
        {
            var fileInfo = new FileInfo(path);
            var ext = fileInfo.Extension;
            return string.IsNullOrEmpty(ext) ? defaultType : MimeTypeMap.Singleton.GetMimeTypeFromExtension(ext);
        }
    }
}