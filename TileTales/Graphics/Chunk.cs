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
        public Chunk(Texture2D texture, SKBitmap[] tiles, SKBitmap map, int width, int height, SKColor[] pixelData)
        {
            Texture = texture;
            Tiles = tiles;
            Map = map;
            Width = width;
            Height = height;
            PixelData = pixelData;
        }

        public Texture2D Texture { get; }
        public SKBitmap[] Tiles { get; }
        public SKBitmap Map { get; }
        public int Width { get; }
        public int Height { get; }
        public SKColor[] PixelData { get; }
    }
}
