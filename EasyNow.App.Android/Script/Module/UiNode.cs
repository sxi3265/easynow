using System;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views.Accessibility;
using Xamarin.Forms;
using Action = Android.Views.Accessibility.Action;

namespace EasyNow.App.Droid.Script.Module
{
    public class UiNode
    {
        public int Depth { get; }
        private readonly AccessibilityNodeInfo _nodeInfo;
        public int IndexInParent { get; }

        public string Id
        {
            get
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr2)
                {
                    return _nodeInfo.ViewIdResourceName;
                }

                return null;
            }
        }

        public string Text => _nodeInfo.Text;

        public string Desc => _nodeInfo.ContentDescription;
        public string ClassName => _nodeInfo.ClassName;
        public string PackageName => _nodeInfo.PackageName;

        public Rect BoundsInScreen
        {
            get
            {
                var rect=new Rect();
                _nodeInfo.GetBoundsInScreen(rect);
                return rect;
            }
        }

        public bool Clickable => _nodeInfo.Clickable;

        public UiNode(AccessibilityNodeInfo nodeInfo,int depth, int indexInParent)
        {
            _nodeInfo = nodeInfo;
            Depth = depth;
            IndexInParent = indexInParent;
        }

        public UiNode Parent()
        {
            try
            {
                var parent = _nodeInfo.Parent;
                if (parent == null)
                {
                    return null;
                }
                return new UiNode(parent,Depth-1,-1);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public UiNode Child(int index)
        {
            try
            {
                var child = _nodeInfo.GetChild(index);
                if (child == null)
                {
                    return null;
                }

                return new UiNode(child, Depth + 1, index);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public UiNodeCollection All(Func<UiNode, bool> func=null)
        {
            func ??= _ => true;
            var result = new UiNodeCollection();
            if (func(this))
            {
                result.Add(this);
            }
            result.AddRange(this.Children().Where(e=>e!=null).SelectMany(e=>e.All(func)).ToArray());
            return result;
        }

        public UiNodeCollection Children()
        {
            return new UiNodeCollection(Enumerable.Range(0, _nodeInfo.ChildCount).Select(Child).ToList());
        }

        public int ChildCount => _nodeInfo.ChildCount;

        public bool PerformAction(Action action)
        {
            return _nodeInfo.PerformAction(action);
        }

        public bool Click()
        {
            var node = _nodeInfo;
            while (node!=null)
            {
                if (node.Clickable)
                {
                    node.PerformAction(Action.Click);
                    return true;
                }

                node = node.Parent;
            }

            return false;
        }

        public void InputText(string text)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                var arguments=new Bundle();
                arguments.PutCharSequence(AccessibilityNodeInfo.ActionArgumentSetTextCharsequence,text);
                _nodeInfo.PerformAction(Action.SetText, arguments);
            }
            else if (Build.VERSION.SdkInt >= BuildVersionCodes.JellyBeanMr2)
            {
                var clipboard = (ClipboardManager)DependencyService.Resolve<Context>().GetSystemService(Context.ClipboardService);
                var clip = ClipData.NewPlainText("label", text);
                clipboard.PrimaryClip = clip;
                _nodeInfo.PerformAction(Action.Focus);
                _nodeInfo.PerformAction(Action.Paste);
            }
        }
    }
}