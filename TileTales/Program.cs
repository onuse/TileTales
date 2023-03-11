
using System;
using TileTales.Utils;

#if DEBUG
Log.Init(ApplicationInfo.LogEventLevel);
#else

#endif

SettingsReader settingsReader = SettingsReader.Singleton;
UserSettings settings = settingsReader.GetSettings();
using var game = new TileTales.TileTalesGame();
game.Run();
