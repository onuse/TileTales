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
        
        private static String _filter;

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

        public static void Init(LogEventLevel level, String filter)
        {
            Init(level);
            _filter = filter;
        }

        public static void Verbose(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsAtLeastVerbose) FilteredLogging(Format(path, name, message), LogEventLevel.Verbose);
        }

        public static void Debug(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsAtLeastDebug) FilteredLogging(Format(path, name, message), LogEventLevel.Debug);
        }

        public static void Info(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsAtLeastInfo) FilteredLogging(Format(path, name, message), LogEventLevel.Information);
        }

        public static void Warning(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsAtLeastWarning) FilteredLogging(Format(path, name, message), LogEventLevel.Warning);
        }

        public static void Error(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsAtLeastError) FilteredLogging(Format(path, name, message), LogEventLevel.Error);
        }

        public static void Fatal(string message, [CallerFilePath] string path = "", [CallerMemberName] string name = "")
        {
            if (IsAtLeastFatal) FilteredLogging(Format(path, name, message), LogEventLevel.Fatal);
        }

        private static void FilteredLogging(String msg, LogEventLevel level)
        {
            if (Filter(_filter, msg))
            {
                if (level == LogEventLevel.Verbose) logger.Verbose(msg);
                else if (level == LogEventLevel.Debug) logger.Debug(msg);
                else if (level == LogEventLevel.Information) logger.Information(msg);
                else if (level == LogEventLevel.Warning) logger.Warning(msg);
                else if (level == LogEventLevel.Error) logger.Error(msg);
                else if (level == LogEventLevel.Fatal) logger.Fatal(msg);
            }
        }

        private static bool Filter(String filter, String contents)
        {
            if (filter == null) return true;
            return contents.Contains(filter);
        }

        private static String Format(String callerFilePath, string callerName, string message)
        {
            var segments = callerFilePath.Split('\\', '/');
            var className = Path.GetFileNameWithoutExtension(segments.Last());
            var parentPackage = segments[segments.Length-2];
            return $"{parentPackage}/{className}.{callerName}(): {message}";
        }
    }
}
