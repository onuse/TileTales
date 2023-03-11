using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    static class Log
    {
        private static Logger logger;
        public static LogEventLevel Level { get; private set; }

        public static bool IsAtLeastVerbose => Level <= LogEventLevel.Verbose;
        public static bool IsAtLeastDebug => Level <= LogEventLevel.Debug;
        public static bool IsAtLeastInfo => Level <= LogEventLevel.Information;
        public static bool IsAtLeastWarning => Level <= LogEventLevel.Warning;
        public static bool IsAtLeastError => Level <= LogEventLevel.Error;
        public static bool IsAtLeastFatal => Level <= LogEventLevel.Fatal;
        public static bool TurnedOn => logger != null;

        public static void Init(LogEventLevel level)
        {
            Level = level;
            logger = new LoggerConfiguration()
            .MinimumLevel.Is(level)
            .WriteTo.Console()
            .WriteTo.Debug()
            .CreateLogger();
        }

        public static void Verbose(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (TurnedOn)
            {
                logger.Verbose(NicePath(path, name) + message);
            }
        }

        public static void Debug(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (TurnedOn)
            {
                logger.Debug(NicePath(path, name) + message);
            }
        }

        public static void Info(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (TurnedOn)
            {
                logger.Information(NicePath(path, name) + message);
            }
        }

        public static void Warning(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (TurnedOn)
            {
                logger.Warning(NicePath(path, name) + message);
            }
        }

        public static void Error(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (TurnedOn)
            {
                logger.Error(NicePath(path, name) + message);
            }
        }

        public static void Fatal(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (TurnedOn)
            {
                logger.Fatal(NicePath(path, name) + message);
            }
        }


        private static String NicePath(String callerFilePath, string callerName)
        {
            var segments = callerFilePath.Split('\\', '/');
            var className = Path.GetFileNameWithoutExtension(segments.Last());
            var parentPackage = segments[segments.Length-2];
            return $"{parentPackage}/{className}.{callerName}(): ";
        }
    }
}
