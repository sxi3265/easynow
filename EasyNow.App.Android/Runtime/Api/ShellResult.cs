namespace EasyNow.App.Droid.Runtime.Api
{
    public class ShellResult
    {
        public int Code { get;set; }
        public string Error{ get;set; }
        public string Result{ get;set; }

        public override string ToString()
        {
            return $"ShellResult{{code={Code}, error='{Error}', result='{Result}'}}";
        }
    }
}