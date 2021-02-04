using Android.Content;

namespace EasyNow.App.Droid.Runtime.Api
{
    public abstract class BaseShell:IShell
    {
        protected Context Context;
        protected bool Root;

        

        protected BaseShell():this(false){}

        protected BaseShell(bool root) : this(null, root){}

        protected BaseShell(Context context, bool root)
        {
            Context = context;
            Root = root;
            Init(root ? Command.COMMAND_SU : Command.COMMAND_SH);
        }

        public abstract void Init(string initialCommand);
        public abstract void Exec(string command);
        public abstract void Exit();
        public abstract ShellResult GetShellResult();

        public virtual void Dispose()
        {
            this.Exit();
            Context?.Dispose();
        }
    }
}