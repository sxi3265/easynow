using Android.Content;

namespace EasyNow.App.Droid.Runtime.Api
{
    public abstract class BaseShell
    {
        protected Context _context;
        private bool _root;

        protected BaseShell(Context context, bool root)
        {
            _context = context;
            _root = root;
        }
        public abstract void Exec(string command);
        public abstract void Exit();
    }
}