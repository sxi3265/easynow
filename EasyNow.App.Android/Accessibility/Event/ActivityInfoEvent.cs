using System;
using System.Linq;
using Android.App.Usage;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Util;
using Android.Views.Accessibility;
using EasyNow.App.Droid.Runtime;
using EasyNow.Utility.Extensions;
using AccessibilityService = EasyNow.App.Droid.Accessibility.AccessibilityService;

namespace EasyNow.App.Droid.Accessibility.Event
{
    public class ActivityInfoEvent:IAccessibilityEvent
    {
        private ComponentName _latestComponentFromShell;
        private string DUMP_WINDOW_COMMAND = "";

        private Shell _shell;
        private bool _useShell;

        public bool UseShell
        {
            get => this._useShell;
            set
            {
                if (value)
                {
                    _shell ??= CreateShell(200);
                }
                else
                {
                    this._shell?.Exit();
                    this._shell = null;
                }

                this._useShell = value;
            }
        }

        private string _latestPackage;
        private bool _useUsageStats;

        public string LatestPackage
        {
            get
            {
                var compFromShell = _latestComponentFromShell;
                if (_useShell && compFromShell != null)
                {
                    return compFromShell.PackageName;
                }

                if (_useUsageStats && Build.VERSION.SdkInt >= BuildVersionCodes.LollipopMr1)
                {
                    this._latestPackage = GetLatestPackageByUsageStats();
                }

                return this._latestPackage;
            }
        }
        private string _latestActivity;

        public string LatestActivity
        {
            get
            {
                var compFromShell = _latestComponentFromShell;
                if (_useShell && compFromShell != null)
                {
                    return compFromShell.ClassName;
                }

                return this._latestActivity;
            }
        }

        private readonly PackageManager _packageManager;
        private readonly Context _context;

        public ActivityInfoEvent(Context context)
        {
            _context = context;
            _packageManager = context.PackageManager;
        }

        public bool OnAccessibilityEvent(AccessibilityService accessibilityService, AccessibilityEvent @event)
        {
            if (@event.EventType == EventTypes.WindowStateChanged)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                {
                    var window = accessibilityService.Windows.FirstOrDefault(e => e.Id == @event.WindowId);
                    if (window?.IsFocused != false)
                    {
                        SetLatestComponent(@event.PackageName,@event.ClassName);
                        return false;
                    }
                }
            }

            return false;
        }

        private void SetLatestComponent(string latestPackage, string latestClass)
        {
            if (string.IsNullOrEmpty(latestPackage))
            {
                return;
            }

            if (IsPackageExists(latestPackage))
            {
                try
                {
                    this._latestPackage = latestPackage;
                    this._latestActivity =
                        this._packageManager.GetActivityInfo(new ComponentName(latestPackage, latestClass), 0)?.Name ??
                        string.Empty;
                }
                catch(Exception e)
                {
                    //Log.Error(this.GetType().Name, e.ToString());
                }
            }
        }

        private string GetLatestPackageByUsageStats()
        {
            var usageStatsManager = (UsageStatsManager) this._context.GetSystemService(Context.UsageStatsService);
            var utcNow = DateTime.UtcNow;
            var usageStats = usageStatsManager.QueryUsageStats(UsageStatsInterval.Best, utcNow.AddHours(-1).TimeStamp(),
                utcNow.TimeStamp());
            return usageStats.Any()
                ? usageStats.OrderByDescending(e => e.LastTimeStamp).Select(e => e.PackageName).FirstOrDefault()
                : _latestPackage;
        }

        private bool IsPackageExists(string packageName)
        {
            try
            {
                this._packageManager.GetPackageInfo(packageName, 0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Shell CreateShell(int dumpInterval)
        {
            var shell = new Shell(true);
            shell.OnNewLine += Shell_OnNewLine;
            shell.Exec(string.Format(DUMP_WINDOW_COMMAND,dumpInterval));
            return shell;
        }

        private void Shell_OnNewLine(object sender, string line)
        {
            SetLatestComponentFromShellOutput(line);
        }

        private void SetLatestComponentFromShellOutput(string output)
        {
            throw new NotImplementedException();
        }
    }
}