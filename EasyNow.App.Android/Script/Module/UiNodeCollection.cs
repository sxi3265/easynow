using System.Collections.Generic;
using System.Linq;
using Android.Views.Accessibility;

namespace EasyNow.App.Droid.Script.Module
{
    public class UiNodeCollection:List<UiNode>
    {
        public UiNodeCollection(){}

        public UiNodeCollection(List<UiNode> nodes):base(nodes)
        {
            
        }

        public bool PerformAction(Action action)
        {
            // 任意一个执行失败则为false
            return this.All(e => e != null && e.PerformAction(action));
        }
    }
}