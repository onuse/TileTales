using System.Collections.Generic;
using System.Linq;
using TileTales.Utils;

namespace TileTales.GameContent {
    internal class Settings {
        public int WorldSize { get; }
        public int MapSize { get; }
        public int TileSize { get; }
        public int ChunkSize { get; }

        public bool HasLoadedWorld { get; set; }

        public int WindowWidth => UserSettings.WindowWidth;
        public int WindowHeight => UserSettings.WindowHeight;

        public int LastScrollWheelValue { get; set; }
        public int ZoomLevel { get; set; }
        public float Scale => SCALE_VALUES[ZoomLevel];
        public float WorldScale => WORLDMAP_SCALE_VALUES[ZoomLevel];

        public UserSettings UserSettings { get; }
        public float Fps { get; internal set; }

        public static List<float> SCALE_VALUES = new float[] {
            8,
            4,
            2,
            1,
            1f / 2f,
            1f / 4f,
            1f / 8f,
            1f / 16f,
            1f / 32f,
            1f / 64f,
            1f / 128,
            1f / 256,
            1f / 512,
            1f / 1024,
            1f / 2048,
            1f / 4096,
            1f / 8192
        }.ToList();

        public static List<float> WORLDMAP_SCALE_VALUES = new float[] {
            16384,
            8192,
            4096,
            2048,
            1024,
            512,
            256,
            128,
            64,
            32,
            16,
            8,
            4,
            2,
            1,
            1f / 2f,
            1f / 4f
        }.ToList();

        public Settings(int worldSize, int tileSize, int mapSize, UserSettings userSettings) {
            // TODO Get this from Server
            WorldSize = worldSize;
            MapSize = mapSize;
            TileSize = tileSize;
            UserSettings = userSettings;

            ChunkSize = MapSize * TileSize;

            ZoomLevel = 1;
        }
    }
}
