using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using EasyNow.App.Services;
using EasyNow.App.Views;
using Xamarin.Forms.Internals;

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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
