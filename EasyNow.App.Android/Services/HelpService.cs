using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Gestures;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Autofac;
using EasyNow.App.Droid.Extensions;
using EasyNow.App.Droid.Script;
using EasyNow.App.Droid.Script.Module;
using EasyNow.App.Droid.Service;
using EasyNow.App.Droid.Util;
using EasyNow.App.Services;
using EasyNow.App.Views;
using Jint;
using Org.Apache.Http.Impl.Client;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
using Uri = Android.Net.Uri;
using View = Android.Views.View;

namespace EasyNow.App.Droid.Services
{
    public class HelpService:IHelpService
    {
        private readonly ILifetimeScope _scope;
        private Context Context => _scope.Resolve<Context>();

        private bool initViewPlace;
        private float touchStartX;
        private float touchStartY;
        private float x;
        private float y;
        private WindowManagerLayoutParams wmParams;

        public HelpService(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public void RunApp()
        {
            var js = @"app.startAuto();
                app.launchApp('com.ophone.reader.ui');
                app.toast('等待首页搜索');
                var node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/btn_bookshelf_search'||node.id=='com.ophone.reader.ui:id/recom_btn_search';});
                node.click();
                app.toast('等待输入框');
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/etSearch';});
                node.inputText('天天爱阅读');
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/btn_search_txt';});
                node.click();
                app.toast('等待搜索结果');
                node=ui.wait(function(node){return node.text=='%E6%90%9C%E7%B4%A2%E5%8F%A3%E4%BB%A4%E5%9B%BE';});
                node.click();
                node=ui.wait(function(node){return node.text=='去阅读'||node.text=='已完成';});
                if(node.text=='已完成'){
                    if(ui.where(function(node){return node.text=='已完成';}).length==2){
                        app.toast('已完成打卡');
                        return;
                    }
                    app.toast('已完成阅读，准备开始签到');
                    node=ui.wait(function(node){return node.text=='签到'&&node.clickable;});
                    node.click();
                    return;
                }
                node.click();
                node=ui.wait(function(node){return node.text=='cover180240';});
                node.click();
                app.toast('进入书籍');
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/reader_content_view';});
                app.toast('开始翻页');
                var startTime = new Date().getTime();
                while(true){
                    var rnd = Math.random();
                    device.touch(node.boundsInScreen.left+node.boundsInScreen.width()*(0.80+0.5*rnd),node.boundsInScreen.top+node.boundsInScreen.height()*(0.80+0.5*rnd));
                    app.sleep(10000);
                    if(new Date().getTime()-startTime>1020000){
                        app.toast('已阅读17分钟');
                        return;
                    }
                }
                ";

            var context = _scope.Resolve<Context>();
            var intent = new Intent(context,typeof(ScriptService));
            intent.PutExtra("ScriptSource", js);
            context.StartService(intent);
            //if (!Settings.CanDrawOverlays(Context))
            //{
            //    SettingUtil.SettingOverlayPermission(Context);
            //    return;
            //}

            //var windowManager = Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            //var floatingView = new FloatingView();
            //var param = new WindowManagerLayoutParams(100,100,WindowManagerTypes.ApplicationOverlay,WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal,Format.Transparent);
            //param.Gravity = GravityFlags.Top | GravityFlags.Left;
            //param.X = 0;
            //param.Y = 100;
            //windowManager.AddView(floatingView.ConvertFormsToNative(Context,new Rectangle(0,0,38,38)),param);

            //var imageView = new ImageView(Context);
            //imageView.SetImageResource(Resource.Mipmap.icon);
            //wmParams = new WindowManagerLayoutParams(
            //    100,
            //    100,
            //    WindowManagerTypes.ApplicationOverlay,
            //    WindowManagerFlags.NotFocusable | WindowManagerFlags.NotTouchModal,
            //    Format.Transparent);
            //wmParams.Gravity = GravityFlags.Top | GravityFlags.Left;
            //wmParams.X = 0;
            //wmParams.Y = 100;
            //imageView.Click += ImageView_Click;
            //windowManager.AddView(imageView, wmParams);
            //imageView.Touch += ImageView_Touch;


            //var engine = new Engine(opts =>
            //{
            //});
            //var uiSelector = _scope.Resolve<UiSelectorModule>();
            //var app = _scope.Resolve<AppModule>();
            //engine.SetValue("ui", uiSelector);
            //engine.SetValue("app", app);
            //engine.Execute("ui.where(function(node){return node.text=='ADD';})[0].click();");



            //Thread.Sleep(5000);
            //var service = AccessibilityService.Instance;
            //if (service == null)
            //{
            //    return;
            //}
            //var nodeInfo=service.RootInActiveWindow;
            //if (nodeInfo?.PackageName == "com.ophone.reader.ui")
            //{
            //    var indexSearchBtn = service.FindViewById("com.ophone.reader.ui:id/btn_bookshelf_search");
            //    while (indexSearchBtn==null)
            //    {
            //        Thread.Sleep(500);
            //        indexSearchBtn = service.FindViewById("
            // ");
            //    }

            //    service.PerformViewClick(indexSearchBtn);
            //    var searchText = service.FindViewById("com.ophone.reader.ui:id/etSearch");
            //    while (searchText == null)
            //    {
            //        Thread.Sleep(500);
            //        searchText = service.FindViewById("com.ophone.reader.ui:id/etSearch");
            //    }
            //    service.InputText(searchText, "天天爱阅读");
            //    var searchBtn = service.FindViewById("com.ophone.reader.ui:id/btn_search_txt");
            //    service.PerformViewClick(searchBtn);

            //    Thread.Sleep(2000);
            //    var path = new Path();
            //    path.MoveTo(540,450);
            //    service.DispatchGesture(
            //        new GestureDescription.Builder().AddStroke(new GestureDescription.StrokeDescription(path, 50, 10))
            //            .Build(), null, null);

            //    //com.ophone.reader.ui:id/base_web_page_layout
            //    //var entry = mainView.TraversalAfter;
            //    //service.PerformViewClick(entry);
            //}
        }

        private void ImageView_Touch(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    if (!initViewPlace)
                    {
                        initViewPlace = true;
                        x=touchStartX = e.Event.RawX;
                        y=touchStartY = e.Event.RawY;
                    }
                    else
                    {
                        touchStartX += (e.Event.RawX - x);
                        touchStartY += (e.Event.RawY - y);
                    }
                    break;
                case MotionEventActions.Move:
                    x = e.Event.RawX;
                    y = e.Event.RawY;
                    var imageView = (ImageView) sender;
                    wmParams.X=((int)(x-touchStartX));
                    wmParams.Y=((int)(y-touchStartY));
                    Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>().UpdateViewLayout(imageView,wmParams);
                    break;
                case MotionEventActions.Up:
                    e.Handled = false;
                    break;
            }
        }

        private void ImageView_Click(object sender, System.EventArgs e)
        {
            var js = @"app.launchApp('com.ophone.reader.ui');
                var node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/btn_bookshelf_search'||node.id=='com.ophone.reader.ui:id/recom_btn_search';});
                node.click();
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/etSearch';});
                node.inputText('天天爱阅读');
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/btn_search_txt';});
                node.click();
                node=ui.wait(function(node){return node.text=='%E6%90%9C%E7%B4%A2%E5%8F%A3%E4%BB%A4%E5%9B%BE';});
                node.click();
                node=ui.wait(function(node){return node.text=='去阅读'||node.text=='已完成';});
                if(node.text=='已完成'){
                    if(ui.where(function(node){return node.text=='已完成';}).length==2){
                        app.toast('已完成打卡');
                        return;
                    }
                    app.toast('已完成阅读，准备开始签到');
                    node=ui.wait(function(node){return node.text=='签到'&&node.clickable;});
                    node.click();
                    return;
                }
                node.click();
                node=ui.wait(function(node){return node.text=='cover180240';});
                node.click();
                app.toast('进入书籍');
                node=ui.wait(function(node){return node.id=='com.ophone.reader.ui:id/reader_content_view';});
                app.toast('开始翻页');
                var startTime = new Date().getTime();
                while(true){
                    device.touch(node.boundsInScreen.left+node.boundsInScreen.width()*0.85,node.boundsInScreen.top+node.boundsInScreen.height()*0.85);
                    app.sleep(10000);
                    if(new Date().getTime()-startTime>1020000){
                        app.toast('已阅读17分钟');
                        return;
                    }
                }
                ";

            var context = _scope.Resolve<Context>();
            var intent = new Intent(context,typeof(ScriptService));
            intent.PutExtra("ScriptSource", js);
            context.StartService(intent);

            //Task.Run(() =>
            //{
            //    try
            //    {
            //        using var runtime = _scope.Resolve<IScriptEngine>().CreateScriptRuntime();
            //        runtime.Execute(js);
            //    }
            //    catch(Exception e)
            //    {

            //    }

            //});
        }
    }
}