using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.GameContent
{
    /**
     * A chunk is a map of tiles that is loaded into memory.
     * It is created from maps where each color is replaced  with the corresponding tile.
     * So chunk size is naturally widthOfMap * widthOfTile;
     * */
    internal class Chunk
    {
        public Chunk(Texture2D texture, Texture2D[] tiles, int width, int height, Color[] pixelData)
        {
            Texture = texture;
            Tiles = tiles;
            Width = width;
            Height = height;
            PixelData = pixelData;
        }
        
        public Texture2D Texture { get; }
        public Texture2D[] Tiles { get; }
        public int Width { get; }
        public int Height { get; }
        public Color[] PixelData { get; }
    }
}
