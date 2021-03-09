using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EasyNow.Utility.Extensions
{
    public static class InvokeExtensions
    {
        public static async Task InvokeAsync<TEvents>(this IEnumerable<TEvents> events, Func<TEvents, Task> dispatch, ILogger logger)
        {
            foreach (var sink in events)
            {
                try
                {
                    await dispatch(sink);
                }
                catch (Exception ex)
                {
                    HandleException(ex, logger, typeof(TEvents).Name, sink.GetType().FullName);
                }
            }
        }

        public static void HandleException(Exception ex, ILogger logger, string sourceType, string method)
        {
            if (IsLogged(ex))
            {
                logger.LogError(new EventId(), ex, "{0} thrown from {1} by {2}",
                    sourceType,
                    method,
                    ex.GetType().Name);
            }

            if (ex.IsFatal())
            {
                throw ex;
            }
        }

        private static bool IsLogged(Exception ex)
        {
            return !ex.IsFatal();
        }
    }
}
