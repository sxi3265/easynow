using Jint.Native;

namespace EasyNow.App.Droid.Script.Module
{
    public class WindowModule
    {
        public JsValue SetTimeout(JsValue fn, double delay)
        {
            return fn.As<Jint.Native.Function.FunctionInstance>().Call(JsValue.Undefined, new JsValue[0]);
        }
    }
}