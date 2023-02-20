using Microsoft.Xna.Framework;
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
        public static void getMapIndexForWorldLocation(float x, float y, ContentLibrary contentLibrary, out int mapX, out int mapY)
        {
            float chunkWidth = contentLibrary.ChunkWidth;
            float chunkHeight = contentLibrary.ChunkHeight;
            mapX = (int)Math.Floor(x / chunkWidth);
            mapY = (int)Math.Floor(y / chunkHeight);
        }

        public static Point getMapIndexForTileIndex(int x, int y, ContentLibrary contentLibrary)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            int mapX = x / (mapWidth * tileWidth);
            int mapY = y / (mapHeight * tileHeight);

            return new Point(mapX, mapY);
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
            getMapIndexForWorldLocation(location.X, location.Y, contentLibrary, out mapX, out mapY);
        }

        public static void getTileIndexForMapIndex(Location location, ContentLibrary contentLibrary, out int tileX, out int tileY)
        {
            getTileIndexForMapIndex(location.X, location.Y, contentLibrary, out tileX, out tileY);
        }

        internal static void ScreenToWorldX(int screenX, int screenY, ContentLibrary contentLibrary, Player player, out int worldX, out int worldY)
        {
            Settings gameSettings = contentLibrary.GameSettings;
            int pxPerTile = gameSettings.TileSize;
            int pxPerChunk = gameSettings.MapSize * pxPerTile;
            int pxInWorld = pxPerChunk * gameSettings.WorldSize;
            float scale = gameSettings.Scale;

            float realScale =  1 / scale;

            int screenCenterPointX = gameSettings.WindowWidth / 2;
            int screenCenterPointY = gameSettings.WindowHeight / 2;
            int screenCenterWorldX = player.X;
            int screenCenterWorldY = player.Y;

            int screenXOffset = screenX - screenCenterPointX;
            int screenYOffset = screenY - screenCenterPointY;

            int worldXOffset = (int)(screenXOffset * realScale);
            int worldYOffset = (int)(screenYOffset * realScale);

            // Snap to tile
            worldXOffset = (int)(Math.Round(worldXOffset / (float)pxPerTile) * pxPerTile);
            worldYOffset = (int)(Math.Round(worldYOffset / (float)pxPerTile) * pxPerTile);

            worldX = screenCenterWorldX + worldXOffset;
            worldY = screenCenterWorldY + worldYOffset;



            //Todo: implement
            //worldX = 0;
            //worldY = 0;
        }
    }
}
