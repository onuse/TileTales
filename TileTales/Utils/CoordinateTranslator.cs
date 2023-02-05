using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;

namespace TileTales.Utils
{
    /**
     * TileIndex = if the world is seen as a grid of tiles, this is the index of a tile in that grid
     * Every item and player has a TileIndex and moves between tiles
     * 
     * MapIndex = if the world is seen as a grid of maps, this is the index of a map in that grid
     * Every raw map has a MapIndex and are named after that index
     * */
    internal class CoordinateTranslator
    {
        public static void getMapIndexForTileIndex(int x, int y, ContentLibrary contentLibrary, out int mapX, out int mapY)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            mapX = x / (mapWidth * tileWidth);
            mapY = y / (mapHeight * tileHeight);
        }

        public static void getTileIndexForMapIndex(int x, int y, ContentLibrary contentLibrary, out int tileX, out int tileY)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            tileX = x * (mapWidth * tileWidth);
            tileY = y * (mapHeight * tileHeight);
        }

        public static void getTileIndexForTileIndex(int x, int y, ContentLibrary contentLibrary, out int tileX, out int tileY)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            tileX = x % (mapWidth * tileWidth);
            tileY = y % (mapHeight * tileHeight);
        }

        public static void getMapIndexForMapIndex(int x, int y, ContentLibrary contentLibrary, out int mapX, out int mapY)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            mapX = x / (mapWidth * tileWidth);
            mapY = y / (mapHeight * tileHeight);
        }

        public static void getMapIndexForTileIndex(Location location, ContentLibrary contentLibrary, out int mapX, out int mapY)
        {
            getMapIndexForTileIndex(location.X, location.Y, contentLibrary, out mapX, out mapY);
        }

        public static void getTileIndexForMapIndex(Location location, ContentLibrary contentLibrary, out int tileX, out int tileY)
        {
            getTileIndexForMapIndex(location.X, location.Y, contentLibrary, out tileX, out tileY);
        }
    }
}
