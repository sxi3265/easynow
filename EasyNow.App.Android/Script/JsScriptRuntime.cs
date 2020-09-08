using System;
using System.Threading.Tasks;
using Android.OS;
using Jint;
using Microsoft.Extensions.Logging;

namespace EasyNow.App.Droid.Script
{
    public class JsScriptRuntime:IScriptRuntime
    {
        private Engine _engine;
        private readonly ILogger _logger;

        public JsScriptRuntime(Engine engine, ILogger<JsScriptRuntime> logger)
        {
            _engine = engine;
            _logger = logger;
        }

        public void Execute(string script)
        {
            try
            {
                _engine.Execute(script);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"执行脚本失败");
            }
        }

        public void Dispose()
        {
            _engine = null;
        }
    }
}