
using TileTales.Utils;

SettingsReader settingsReader = SettingsReader.Instance;
Settings settings = settingsReader.GetSettings();
using var game = new TileTales.TileTalesGame();
game.Run();
