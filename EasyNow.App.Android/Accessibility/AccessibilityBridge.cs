using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Views.Accessibility;

namespace EasyNow.App.Droid.Accessibility
{
    public abstract class AccessibilityBridge
    {
        private Func<AccessibilityWindowInfo, bool> _windowFilterFunc;

        public abstract AccessibilityService Service { get; }

        public IEnumerable<AccessibilityNodeInfo> WindowRoots
        {
            get
            {
                var service = Service;
                if (service == null)
                {
                    return Enumerable.Empty<AccessibilityNodeInfo>();
                }

                if (_windowFilterFunc != null && Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    return service.Windows.Where(_windowFilterFunc).Select(e => e.Root).Where(e => e != null).ToArray();
                }
                // todo 快速模式

                return new[] {service.RootInActiveWindow};
            }
        }
    }
}