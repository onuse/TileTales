using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;
using TileTales.Graphics;

namespace TileTales.Utils
{
    /**
     * WorldLocation/WorldCoords = if the world is seen as a grid of pixels, this is the index of a pixel in that grid
     * Most stuff actually has a worldlocation, even though every change is done with +/- 16 pixels (tilesize)
     * 
     * TileIndex = if the world is seen as a grid of tiles, this is the index of a tile in that grid
     * Every item and player has a TileIndex and moves between tiles
     * 
     * MapIndex = if the world is seen as a grid of maps, this is the index of a map in that grid
     * Every raw map has a MapIndex and are named after that index
     * */
    internal class CoordinateHelper
    {
        internal static void GetMapIndexForWorldLocation(float x, float y, ContentLibrary contentLibrary, out int mapX, out int mapY)
        {
            float chunkWidth = contentLibrary.ChunkWidth; // (mapWidth * tileWidth) = chunkWidth
            float chunkHeight = contentLibrary.ChunkHeight;
            mapX = (int)Math.Floor(x / chunkWidth);
            mapY = (int)Math.Floor(y / chunkHeight);
        }

        internal static Point3D GetMapIndexForWorldLocation(Point3D location, ContentLibrary contentLibrary)
        {
            GetMapIndexForWorldLocation(location.X, location.Y, contentLibrary, out int mapX, out int mapY);
            return new Point3D(mapX, mapY, location.Z);
        }

        internal static Point3D WorldCoordsToMapIndex(Point3D location, ContentLibrary contentLibrary)
        {
            GetMapIndexForWorldLocation(location.X, location.Y, contentLibrary, out int mapX, out int mapY);
            return new Point3D(mapX, mapY, location.Z);
        }

        internal static Point GetMapIndexForTileIndex(int x, int y, ContentLibrary contentLibrary)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            int mapX = x / (mapWidth * tileWidth);
            int mapY = y / (mapHeight * tileHeight);

            return new Point(mapX, mapY);
        }

        internal static void GetTileIndexForMapIndex(int x, int y, ContentLibrary contentLibrary, out int tileX, out int tileY)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            tileX = x * (mapWidth * tileWidth);
            tileY = y * (mapHeight * tileHeight);
        }

        internal static void GetTileIndexForTileIndex(int x, int y, ContentLibrary contentLibrary, out int tileX, out int tileY)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            tileX = x % (mapWidth * tileWidth);
            tileY = y % (mapHeight * tileHeight);
        }

        internal static void GetMapIndexForMapIndex(int x, int y, ContentLibrary contentLibrary, out int mapX, out int mapY)
        {
            int mapWidth = contentLibrary.MapWidth;
            int mapHeight = contentLibrary.MapHeight;
            int tileWidth = contentLibrary.TileWidth;
            int tileHeight = contentLibrary.TileHeight;

            mapX = x / (mapWidth * tileWidth);
            mapY = y / (mapHeight * tileHeight);
        }

        internal static void GetMapIndexForTileIndex(Point3D location, ContentLibrary contentLibrary, out int mapX, out int mapY)
        {
            GetMapIndexForWorldLocation(location.X, location.Y, contentLibrary, out mapX, out mapY);
        }

        internal static void GetTileIndexForMapIndex(Point3D location, ContentLibrary contentLibrary, out int tileX, out int tileY)
        {
            GetTileIndexForMapIndex(location.X, location.Y, contentLibrary, out tileX, out tileY);
        }

        internal static void ScreenToWorldCoords(int screenX, int screenY, ContentLibrary contentLibrary, Player player, out int worldX, out int worldY)
        {
            Settings gameSettings = contentLibrary.GameSettings;
            if (gameSettings == null)
            {
                worldX = 0;
                worldY = 0;
                return;
            }
            int pxPerTile = gameSettings.TileSize;
            float scale = gameSettings.Scale;

            int screenCenterPointX = gameSettings.WindowWidth / 2;
            int screenCenterPointY = gameSettings.WindowHeight / 2;

            int screenXOffset = screenX - screenCenterPointX;
            int screenYOffset = screenY - screenCenterPointY;

            int worldXOffset = (int)(screenXOffset / scale);
            int worldYOffset = (int)(screenYOffset / scale);

            // Snap to tile
            worldXOffset = (int)(Math.Round(worldXOffset / (float)pxPerTile) * pxPerTile);
            worldYOffset = (int)(Math.Round(worldYOffset / (float)pxPerTile) * pxPerTile);

            worldX = player.X + worldXOffset;
            worldY = player.Y + worldYOffset;
        }

        internal static float GetDistanceInMapsForWorldCoords(Point3D one, Point3D two, ContentLibrary contentLibrary)
        {
            Vector2 vOne = new(one.X, one.Y);
            Vector2 vTwo = new(two.X, two.Y);
            float distancePixels = Vector2.Distance(vOne, vTwo);
            float chunkSize = contentLibrary.MapWidth * contentLibrary.TileWidth;
            return distancePixels / chunkSize;
        }

        internal static float GetDistanceInTilesForWorldCoords(Point3D one, Point3D two, ContentLibrary contentLibrary)
        {
            Vector2 vOne = new(one.X, one.Y);
            Vector2 vTwo = new(two.X, two.Y);
            float distancePixels = Vector2.Distance(vOne, vTwo);
            float tileSize = contentLibrary.TileWidth;
            return distancePixels / tileSize;
        }

        internal static List<Point3D> GetPointsInRadius(Point3D center, int radius)
        {
            List<Point3D> points = new();
            // Create all points that exists within radius from center
            for (int x = center.X - radius; x <= center.X + radius; x++)
            {
                for (int y = center.Y - radius; y <= center.Y + radius; y++)
                {
                    points.Add(new Point3D(x, y, center.Z));
                }
            }
            return points;
        }
    }
}
