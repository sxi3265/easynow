using System.Runtime.InteropServices;

namespace EasyNow.App.Droid.Native
{
    public class NativeShell
    {
        [DllImport("NativeLibrary",EntryPoint = "NativeShell_Execute")]
        public extern static int Execute(string cmd);
    }
}