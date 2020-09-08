using System.Threading;

namespace EasyNow.App.Droid.Script
{
    public interface IScriptEngine
    {
        IScriptRuntime CreateScriptRuntime(CancellationToken token=default);
    }
}