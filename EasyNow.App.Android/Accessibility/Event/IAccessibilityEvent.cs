using Android.Views.Accessibility;
using AccessibilityService = EasyNow.App.Droid.Accessibility.AccessibilityService;

namespace EasyNow.App.Droid.Accessibility.Event
{
    public interface IAccessibilityEvent
    {
        bool OnAccessibilityEvent(AccessibilityService accessibilityService, AccessibilityEvent e);
    }
}