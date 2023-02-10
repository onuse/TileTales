using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.GameContent
{
    internal class GameSettings
    {
        public int TILE_WIDTH { get; }
        public int TILE_HEIGHT { get; }

        public int WORLD_WIDTH { get; }
        public int WORLD_HEIGHT { get; }

        //public int PIXELS_PER_CHUNK { get; }

        public static int PIXELS_PER_CHUNK = 1600;

        public GameSettings(int worldWidth, int worldHeight, int tileWidth, int tileHeight)
        {
            // TODO Get this from Server
            WORLD_WIDTH = worldWidth;
            WORLD_HEIGHT = worldHeight;
            TILE_WIDTH = tileWidth;
            TILE_HEIGHT = tileHeight;

            PIXELS_PER_CHUNK = 100 * 16;
        }
    }
}
