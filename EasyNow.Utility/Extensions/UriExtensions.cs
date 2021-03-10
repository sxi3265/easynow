using System;
using System.Linq;

namespace EasyNow.Utility.Extensions
{
    public static class UriExtensions
    {
        public static Uri Combine(this Uri uri, params string[] paths)
        {
            return new Uri(paths.Aggregate(uri.AbsoluteUri, (current, path) =>
                $"{current.TrimEnd('/')}/{path.TrimStart('/')}"));
        }
    }
}