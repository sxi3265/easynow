using System;
using System.Threading;
using Autofac;
using EasyNow.App.Droid.Script.Module;
using Jint;

namespace EasyNow.App.Droid.Script
{
    public class JsScriptEngine:IScriptEngine
    {
        private readonly ILifetimeScope _scope;

        public JsScriptEngine(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public IScriptRuntime CreateScriptRuntime(CancellationToken token=default)
        {
            var engine = new Engine(opts =>
            {
                opts.CancellationToken(token);
                opts.DebugMode();
            });
            engine.SetValue("app", _scope.Resolve<AppModule>());
            engine.SetValue("ui", _scope.Resolve<UiModule>());
            engine.SetValue("device", _scope.Resolve<DeviceModule>());
            engine.SetValue("window", _scope.Resolve<WindowModule>());
            return _scope.Resolve<IScriptRuntime>(new TypedParameter(engine.GetType(), engine));
        }
    }
}