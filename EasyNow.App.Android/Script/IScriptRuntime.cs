using System;

namespace EasyNow.App.Droid.Script
{
    public interface IScriptRuntime:IDisposable
    {
        public void Execute(string script);
    }
}