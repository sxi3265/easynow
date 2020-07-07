using System.Linq;
using Java.Lang;

namespace Com.Tencent.Bugly.Crashreport.Crash
{
    public partial class CrashDetailBean
    {
        public int CompareTo(Object o)
        {
            // todo 临时实现
            if (o is CrashDetailBean crashDetailBean)
            {
                foreach (var propertyInfo in this.GetType().GetProperties().OrderBy(e=>e.Name))
                {
                    var thisValue = propertyInfo.GetValue(this);
                    var objValue = propertyInfo.GetValue(crashDetailBean);
                    if (thisValue == null)
                    {
                        if (objValue == null)
                        {
                            return 0;
                        }

                        return -1;
                    }
                    if (!thisValue.Equals(objValue))
                    {
                        return 1;
                    }
                }

                return 0;
            }
            return 1;
        }
    }
}