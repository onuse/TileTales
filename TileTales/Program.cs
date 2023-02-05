
using TileTales.Utils;

SettingsReader settingsReader = SettingsReader.Instance;
UserSettings settings = settingsReader.GetSettings();
using var game = new TileTales.TileTalesGame();
game.Run();
