using System.Threading.Tasks;
using Android.OS;
using Jint;

namespace EasyNow.App.Droid.Script
{
    public class JsScriptRuntime:IScriptRuntime
    {
        private Engine _engine;

        public JsScriptRuntime(Engine engine)
        {
            _engine = engine;
        }

        public void Execute(string script)
        {
            _engine.Execute(script);
        }

        public void Dispose()
        {
            _engine = null;
        }
    }
}