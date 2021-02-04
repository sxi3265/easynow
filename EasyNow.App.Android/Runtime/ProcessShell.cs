using Android.Content;
using EasyNow.App.Droid.Runtime.Api;
using Java.IO;
using Java.Lang;

namespace EasyNow.App.Droid.Runtime
{
    public class ProcessShell : BaseShell
    {
        public Process Process { get; private set; }
        private DataOutputStream _commandOutputStream;
        public BufferedReader SucceedReader{ get; private set; }
        public BufferedReader ErrorReader{ get; private set; }

        public StringBuilder SucceedOutput { get; } = new StringBuilder();
        public StringBuilder ErrorOutput { get; }= new StringBuilder();

        public ProcessShell(Context context, bool root) : base(context, root)
        {
        }

        public override void Init(string initialCommand)
        {
            try
            {
                Process = Java.Lang.Runtime.GetRuntime().Exec(initialCommand);
                _commandOutputStream = new DataOutputStream(Process.OutputStream);
                SucceedReader = new BufferedReader(new InputStreamReader(Process.InputStream));
                ErrorReader = new BufferedReader(new InputStreamReader(Process.ErrorStream));
            } catch (IOException e) {
                throw new UncheckedIOException(e);
            }
        }

        public override void Exec(string command)
        {
            try {
                _commandOutputStream.WriteBytes(command);
                if (!command.EndsWith(Command.COMMAND_LINE_END)) {
                    _commandOutputStream.WriteBytes(Command.COMMAND_LINE_END);
                }
                _commandOutputStream.Flush();
            } catch (IOException e) {
                throw new RuntimeException(e);
            }
        }

        public override void Exit()
        {
            if (Process != null) {
                //Log.d(TAG, "exit: pid = " + ProcessUtils.getProcessPid(mProcess));
                Process.Destroy();
                Process = null;
            }

            if (SucceedReader != null)
            {
                try
                {
                    SucceedReader.Close();
                }
                catch (IOException ignored)
                {

                }

                SucceedReader = null;
            }

            if (ErrorReader != null)
            {
                try
                {
                    ErrorReader.Close();
                }
                catch (IOException ignored)
                {

                }

                ErrorReader = null;
            }
        }

        public override ShellResult GetShellResult()
        {
            var result = new ShellResult
            {
                Code = this.Process.WaitFor()
            };
            ReadAll();
            result.Error = this.ErrorOutput.ToString();
            result.Result = this.SucceedOutput.ToString();
            return result;
        }

        public ProcessShell ReadAll()
        {
            return ReadSucceedOutput().ReadErrorOutput();
        }

        public ProcessShell ReadSucceedOutput() {
            Read(SucceedReader, SucceedOutput);
            return this;
        }

        private void Read(BufferedReader reader, StringBuilder sb) {
            try
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    sb.Append(line).Append("\n");
                }
            }
            catch (IOException e)
            {
                throw new UncheckedIOException(e);
            }
        }

        public ProcessShell ReadErrorOutput() {
            Read(ErrorReader, ErrorOutput);
            return this;
        }
    }
}