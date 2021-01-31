using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EasyNow.App.Services;
using EasyNow.App.Views;
using Xamarin.Forms.Internals;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
namespace EasyNow.App
{
    public partial class App : Application
    {
        private readonly ILifetimeScope _scope;

        public App(ILifetimeScope scope)
        {
            _scope = scope;
            InitializeComponent();
            DependencyResolver.ResolveUsing(type => _scope.IsRegistered(type) ? _scope.Resolve(type) : null);
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start("android=1e6d6c50-5eac-49ab-b9e7-c8337214b4a3;"
                            //+
                            //"uwp={Your UWP App secret here};" +
                            //"ios={Your iOS App secret here}"
                ,
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
