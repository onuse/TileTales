using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Graphics
{
    /**
     * A chunk is a map of tiles that is loaded into memory.
     * It is created from maps where each color is replaced  with the corresponding tile.
     * So chunk size is naturally widthOfMap * widthOfTile;
     * */
    internal class Chunk
    {
        public Chunk(Texture2D texture, Tile[] tiles, Texture2D[] t2Tiles, Map map, int width, int height, SKColor[] pixelData, SKData skData)
        {
            Image = texture;
            Tiles = tiles;
            //T2Tiles = t2Tiles;
            Map = map;
            
            Width = width;
            Height = height;
            PixelData = pixelData;
            Data = skData;
        }

        public Texture2D Image { get; set;  }
        public Tile[] Tiles { get; }
        //public Texture2D[] T2Tiles { get; }
        public Map Map { get; }
        public int Width { get; }
        public int Height { get; }
        public SKColor[] PixelData { get; }
        public SKData Data { get; }

        public Boolean DrawCacheImage { get; set; } = false;

        internal void Draw(SpriteBatch tileBatch, Vector2 pos, object value, Color tint, float rotation, Vector2 origin, float scale, SpriteEffects none, float layerDepth)
        {
            if (DrawCacheImage)
            {
                tileBatch.Draw(Image, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);
                return;
            }
            else
            {
                int mapWidth = Map.Image.Width;
                int mapHeight = Map.Image.Height;
                // Draw Tiles on spritebatch, using Width and Height
                for (int y = 0; y < mapHeight; y++)
                {
                    for (int x = 0; x < mapWidth; x++)
                    {
                        Texture2D tile = Tiles[x + y * mapWidth].Image;
                        Vector2 tilePos = new Vector2(x * tile.Width * scale, y * tile.Height * scale); // Apply the scale to the tile position
                        tileBatch.Draw(tile, pos + tilePos, null, tint, rotation, origin, scale, none, layerDepth);
                    }
                }
            }
        }

        internal void DrawBackingImage(SpriteBatch tileBatch, Vector2 pos, object value, Color tint, float rotation, Vector2 origin, float scale, SpriteEffects none, float layerDepth)
        {
            tileBatch.Draw(Image, pos, null, tint, rotation, origin, scale * 8, SpriteEffects.None, layerDepth);
        }

        internal void DrawMap(SpriteBatch tileBatch, Vector2 pos, object value, Color tint, float rotation, Vector2 origin, float scale, SpriteEffects none, float layerDepth)
        {
            tileBatch.Draw(Map.Texture, pos, null, tint, rotation, origin, scale * 16, SpriteEffects.None, layerDepth);
        }

        internal void DrawTiles(SpriteBatch tileBatch, Vector2 pos, object value, Color tint, float rotation, Vector2 origin, float scale, SpriteEffects none, float layerDepth)
        {
            int mapWidth = Map.Image.Width;
            int mapHeight = Map.Image.Height;
            // Draw Tiles on spritebatch, using Width and Height
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    Texture2D tile = Tiles[x + y * mapWidth].Image;
                    Vector2 tilePos = new Vector2(x * tile.Width * scale, y * tile.Height * scale); // Apply the scale to the tile position
                    tileBatch.Draw(tile, pos + tilePos, null, tint, rotation, origin, scale, none, layerDepth);
                }
            }
        }
    }
}
