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
        public static bool IsTurnedOn => logger != null;

        public static bool IsAtLeastVerbose => IsTurnedOn && Level <= LogEventLevel.Verbose;
        public static bool IsAtLeastDebug => IsTurnedOn && Level <= LogEventLevel.Debug;
        public static bool IsAtLeastInfo => IsTurnedOn && Level <= LogEventLevel.Information;
        public static bool IsAtLeastWarning => IsTurnedOn && Level <= LogEventLevel.Warning;
        public static bool IsAtLeastError => IsTurnedOn && Level <= LogEventLevel.Error;
        public static bool IsAtLeastFatal => IsTurnedOn && Level <= LogEventLevel.Fatal;

        static Log()
        {
            Level = LogEventLevel.Fatal;
        }

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
            if (IsTurnedOn) logger.Verbose(NicePath(path, name) + message);
        }

        public static void Debug(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsTurnedOn) logger.Debug(NicePath(path, name) + message);
        }

        public static void Info(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsTurnedOn) logger.Information(NicePath(path, name) + message);
        }

        public static void Warning(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsTurnedOn) logger.Warning(NicePath(path, name) + message);
        }

        public static void Error(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsTurnedOn) logger.Error(NicePath(path, name) + message);
        }

        public static void Fatal(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsTurnedOn) logger.Fatal(NicePath(path, name) + message);
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
