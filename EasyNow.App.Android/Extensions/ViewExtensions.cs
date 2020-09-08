using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

namespace EasyNow.App.Droid.Extensions
{
    public static class ViewExtensions
    {
        public static View ConvertFormsToNative(this Xamarin.Forms.View view,Context context, Rectangle size)
        {
            var vRenderer = Platform.CreateRendererWithContext(view,context);
            var viewGroup = vRenderer.View;
            vRenderer.Tracker.UpdateLayout ();
            var layoutParams = new ViewGroup.LayoutParams ((int)size.Width, (int)size.Height);
            viewGroup.LayoutParameters = layoutParams;
            view.Layout (size);
            viewGroup.Layout(0, 0, (int)view.WidthRequest, (int)view.HeightRequest);
            return viewGroup; 
        }
    }
}