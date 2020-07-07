using Android.Content;
using Android.OS;
using Android.Widget;

namespace EasyNow.App.Droid.Util
{
    public class UiHandler:Handler
    {
        public Context Context { get; }

        public UiHandler(Context context):base(Looper.MainLooper)
        {
            Context = context;
        }

        public void Toast(string msg)
        {
            Post(() =>
            {
                Android.Widget.Toast.MakeText(Context,msg,ToastLength.Short).Show();
            });
        }

        public void Toast(int resId)
        {
            Post(() =>
            {
                Android.Widget.Toast.MakeText(Context,resId,ToastLength.Short).Show();
            });
        }
    }
}