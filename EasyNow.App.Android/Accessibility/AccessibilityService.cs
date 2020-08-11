using System.Collections.Generic;
using Android;
using Android.App;
using Android.Views;
using Android.Views.Accessibility;
using Autofac;
using EasyNow.App.Droid.Accessibility.Event;
using EasyNow.App.Droid.Util;
using Xamarin.Forms;

namespace EasyNow.App.Droid.Accessibility
{
    [Service(Label = "EasyNow自动服务", Permission = Manifest.Permission.BindAccessibilityService,Name = "me.easynow.app.AccessibilityService")]
    [IntentFilter(new[] { "android.accessibilityservice.AccessibilityService" })]
    [MetaData("android.accessibilityservice", Resource = "@xml/auto_accessibility_service_config")]
    public class AccessibilityService:Android.AccessibilityServices.AccessibilityService
    {
        public static AccessibilityService Instance { get; private set; }

        public AccessibilityNodeInfo FastRootInActiveWindow { get; private set; }
        private readonly Dictionary<int,IAccessibilityEvent> _accessibilityEvents=new Dictionary<int, IAccessibilityEvent>();

        public AccessibilityService()
        {
            AddEvent(100,DependencyService.Resolve<ActivityInfoEvent>());
            AddEvent(200,DependencyService.Resolve<NotificationEvent>());
        }

        protected override void OnServiceConnected()
        {
            Instance = this;
            base.OnServiceConnected();
        }

        protected override bool OnKeyEvent(KeyEvent e)
        {
            return base.OnKeyEvent(e);
        }

        public override void OnAccessibilityEvent(AccessibilityEvent e)
        {
            Instance = this;
            if (e == null || string.IsNullOrEmpty(e.PackageName) || string.IsNullOrEmpty(e.ClassName))
            {
                return;
            }

            var eventType = e.EventType;
            if (eventType == EventTypes.WindowStateChanged || eventType == EventTypes.ViewFocused)
            {
                if (RootInActiveWindow != null)
                {
                    FastRootInActiveWindow = RootInActiveWindow;
                }
            }

            foreach (var item in _accessibilityEvents)
            {
                if (item.Value.OnAccessibilityEvent(this, e))
                {
                    break;
                }
            }
        }

        public void AddEvent(int uniquePriority, IAccessibilityEvent @event)
        {
            if (this._accessibilityEvents.ContainsKey(uniquePriority))
            {
                this._accessibilityEvents[uniquePriority] = @event;
            }
            else
            {
                this._accessibilityEvents.Add(uniquePriority,@event);
            }
        }

        public override void OnInterrupt()
        {
            
        }

        public override void OnDestroy()
        {
            Instance = null;
            base.OnDestroy();
        }
    }
}