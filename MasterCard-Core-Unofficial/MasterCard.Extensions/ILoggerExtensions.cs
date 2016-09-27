using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MasterCard_Core_Unofficial.MasterCard.Extensions {
    internal static class ILoggerExtensions {

        internal static void Debug(this ILogger log, string msg) {
            log.LogDebug(msg, new object[0]);
        }

        internal static void Debug<T>(this ILogger log, IEnumerable<T> msgs) {

            string msg = "";
            int c = 0;
            foreach(var p in msgs) {
                msg += typeof(T).GetTypeInfo().Name + "[" + c.ToString() + "] = " + p.ToString() + Environment.NewLine;
                c++;
            }

            log.LogDebug(msg, new object[0]);
        }

        internal static void Error(this ILogger log, string msg) {
            log.LogError(msg, new object[0]);
        }

        internal static void Error(this ILogger log, string msg, Exception ex) {
#if  NET461
            log.LogError(msg, ex, new object[0]);
#elif NETSTANDARD1_6
            log.LogError(msg, ex, new object[0]);
#endif
        }
    }
}
