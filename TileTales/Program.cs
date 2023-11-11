
using Serilog.Events;
using TileTales.Utils;

#if DEBUG
Log.Init(LogEventLevel.Verbose, null);
#else

#endif

SettingsReader settingsReader = SettingsReader.Singleton;
UserSettings settings = settingsReader.GetSettings();
using var game = new TileTales.TileTalesGame();
game.Run();
