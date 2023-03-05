
using TileTales.Utils;

SettingsReader settingsReader = SettingsReader.Singleton;
UserSettings settings = settingsReader.GetSettings();
using var game = new TileTales.TileTalesGame();
game.Run();
