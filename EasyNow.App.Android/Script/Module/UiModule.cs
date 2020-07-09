using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Views.Accessibility;
using Autofac;
using EasyNow.App.Droid.Accessibility;
using EasyNow.App.Droid.Accessibility.Event;
using Microsoft.Extensions.Logging;

namespace EasyNow.App.Droid.Script.Module
{
    public class UiModule
    {
        private AccessibilityBridge AccessibilityBridge => _scope.Resolve<AccessibilityBridge>();
        private ActivityInfoEvent ActivityInfoEvent => _scope.Resolve<ActivityInfoEvent>();
        private readonly ILifetimeScope _scope;
        private ILogger Logger => _scope.Resolve<ILogger<UiModule>>();

        public UiModule(ILifetimeScope scope)
        {
            _scope = scope;
        }

        /// <summary>
        /// 当前包名
        /// </summary>
        public string CurrentPackageName => ActivityInfoEvent.LatestPackage;

        /// <summary>
        /// 当前Activity
        /// </summary>
        public string CurrentActivity => ActivityInfoEvent.LatestActivity;

        public UiNodeCollection Where(Func<UiNode,bool> func=null)
        {
            var roots=AccessibilityBridge.WindowRoots;
            return new UiNodeCollection(roots.Where(e => e != null).Select(e => new UiNode(e, 0, -1)).SelectMany(e=>e.All(func)).ToList());
        }

        /// <summary>
        /// 等待某个元素出现
        /// </summary>
        /// <param name="func"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public UiNode Wait(Func<UiNode,bool> func,double timeout=0f)
        {
            try
            {
                var startTime = DateTime.UtcNow;
                var node = this.Where(func).FirstOrDefault();
                while (node == null)
                {
                    if (timeout!=0&&(DateTime.UtcNow - startTime).TotalMilliseconds >= timeout)
                    {
                        return null;
                    }
                    Thread.Sleep(500);
                    node = this.Where(func).FirstOrDefault();
                }

                return node;
            }
            catch(Exception e)
            {
                Logger.LogError(e,"查找元素时发生错误");
                return null;
            }
        }
    }
}