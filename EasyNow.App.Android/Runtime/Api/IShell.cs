using System;
using Autofac;
using Xamarin.Forms;

namespace EasyNow.App.Droid.Runtime.Api
{
    public interface IShell:IDisposable
    {
        void Exec(string command);
        void Exit();
        ShellResult GetShellResult();

        public static ShellResult Exec(string command, bool isRoot=false) {
            var commands = command.Split("\n",StringSplitOptions.RemoveEmptyEntries);
            return Exec(commands, isRoot);
        }

        public static ShellResult Exec(string[] commands,bool isRoot=false)
        {
            using var shell = DependencyService.Resolve<ILifetimeScope>()
                .Resolve<IShell>(new TypedParameter(typeof(bool), isRoot));
            foreach (var command in commands)
            {
                shell.Exec(command);
            }
            shell.Exec(Command.COMMAND_EXIT);
            var result = shell.GetShellResult();
            shell.Exit();
            return result;
        }
    }
}