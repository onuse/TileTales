using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.GameContent
{
    internal class Settings
    {

        public int WORLD_SIZE { get; }
        public int MAP_SIZE { get; }
        public int TILE_SIZE { get; }
        public int CHUNK_SIZE { get; }

        public bool HasLoadedWorld { get; set; }

        public int WindowWidth { get; set; }
        public int WindowHeight { get; set;  }

        public int LastScrollWheelValue { get; set; }
        public int ZoomLevel { get; set; }

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

        public Settings(int worldSize, int tileSize, int mapSize, int tileHeight)
        {
            // TODO Get this from Server
            WORLD_SIZE = worldSize;
            MAP_SIZE = tileSize;
            TILE_SIZE = mapSize;

            CHUNK_SIZE = MAP_SIZE * TILE_SIZE;

            ZoomLevel = 1;
        }
    }
}
