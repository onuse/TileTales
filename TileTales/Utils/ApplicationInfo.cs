using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal static class ApplicationInfo
    {
        internal static string Name = "Tile Tales";
        internal static string Version = "0.0.1";
        internal static string Author = "Jonas Karlsson";

        internal static bool isConnectedToServer = false;
        internal static bool isSignedIn = false;
        internal static LogEventLevel LogEventLevel = LogEventLevel.Information;
    }
}
