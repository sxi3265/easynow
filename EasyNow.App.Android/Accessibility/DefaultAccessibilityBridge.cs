namespace EasyNow.App.Droid.Accessibility
{
    public class DefaultAccessibilityBridge:AccessibilityBridge
    {
        public override AccessibilityService Service => AccessibilityService.Instance;
    }
}