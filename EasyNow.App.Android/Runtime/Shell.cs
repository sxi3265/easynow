using Android.Content;
using EasyNow.App.Droid.Runtime.Api;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Android.OS;

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

        public override void Init(string initialCommand)
        {
            var uiHandler = new Handler(Context.MainLooper);
            //uiHandler.Post(() -> {
            //    var settings = new TermSettings(mContext.getResources(), PreferenceManager.getDefaultSharedPreferences(mContext));
            //    try {
            //        mTermSession = new MyShellTermSession(settings, initialCommand);
            //        mTermSession.initializeEmulator(1024, 40);
            //    } catch (IOException e) {
            //        mInitException = new UncheckedIOException(e);
            //    }
            //});
        }

        public override void Exec(string command)
        {
            Java.Lang.Runtime.GetRuntime().Exec(command);
        }

        public override void Exit()
        {
        }

        public override ShellResult GetShellResult()
        {
            throw new System.NotImplementedException();
        }
    }
}