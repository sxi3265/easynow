using Android.Content;
using EasyNow.App.Droid.Runtime.Api;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EasyNow.App.Droid.Runtime
{
    public class Shell : BaseShell
    {
        private bool _shouldReadOutput;

        public Shell() : this(false)
        {

        }

        public Shell(Context context) : this(context, false)
        {

        }

        public Shell(Context context, bool root) : this(context, root, true)
        {

        }

        public Shell(Context context, bool root, bool shouldReadOutput) : base(context, root)
        {
            _shouldReadOutput = shouldReadOutput;
        }

        public Shell(bool root) : this(DependencyService.Resolve<Context>(), root)
        {

        }

        public delegate void NewLineHandler(object sender, string line);

        public event NewLineHandler OnNewLine;

        public override void Exec(string command)
        {
            throw new System.NotImplementedException();
        }

        public override void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}