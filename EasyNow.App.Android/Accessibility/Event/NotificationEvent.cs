using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views.Accessibility;
using Java.Lang;
using AccessibilityService = EasyNow.App.Droid.Accessibility.AccessibilityService;
using Exception = System.Exception;

namespace EasyNow.App.Droid.Accessibility.Event
{
    public class NotificationEvent:IAccessibilityEvent,INotificationListener
    {
        private readonly Context _context;

        private List<INotificationListener> _notificationListeners=new List<INotificationListener>();
        private List<IToastListener> toastListeners=new List<IToastListener>();

        public NotificationEvent(Context context)
        {
            _context = context;
        }

        public bool OnAccessibilityEvent(AccessibilityService accessibilityService, AccessibilityEvent e)
        {
            if (e.ParcelableData is Notification.Notification notification)
            {
                OnNotification(notification);
            }
            else
            {
                var list = e.Text;
                if (e.PackageName == this._context.PackageName)
                {
                    return false;
                }

                if (list != null)
                {
                    OnToast(e,new Toast(e.PackageName,list));
                }
            }

            return false;
        }

        public void OnNotification(Notification.Notification notification)
        {
            foreach (var listener in _notificationListeners)
            {
                try
                {
                    listener.OnNotification(notification);
                }
                catch(Exception e)
                {
                    // todo 打印日志
                }
            }
        }

        private void OnToast(AccessibilityEvent @event,Toast toast)
        {
            foreach (var listener in toastListeners)
            {
                try
                {
                    listener.OnToast(toast);
                }
                catch (Exception e)
                {
                    // todo 打印日志
                }
            }
        }

        public class Toast
        {
            private readonly List<string> _texts;
            private readonly string _packageName;

            public string Text => _texts.FirstOrDefault();

            public Toast(string packageName, IList<ICharSequence> texts)
            {
                _packageName = packageName;
                _texts = texts.Select(e => e.ToString()).ToList();
            }

            public override string ToString()
            {
                return $"Toast{{texts={_texts}, packageName='{_packageName}'}}";
            }
        }

        public interface IToastListener
        {
            void OnToast(Toast toast);
        }
    }
}