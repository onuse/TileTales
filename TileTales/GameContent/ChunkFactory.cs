using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Color = Microsoft.Xna.Framework.Color;

namespace TileTales.GameContent
{
    internal class ChunkFactory
    {
        private readonly GraphicsDevice graphicsDevice;
        private readonly ContentLibrary contentLibrary;

        public ChunkFactory(GraphicsDevice graphicsDevice, ContentLibrary contentLibrary)
        {
            this.graphicsDevice = graphicsDevice;
            this.contentLibrary = contentLibrary;
        }

        public Chunk CreateChunkFromTexture(Texture2D map)
        {
            int width = map.Bounds.Width;
            int height = map.Bounds.Height;
            ContentLibrary clib = contentLibrary;
            Color[] pixelData = new Color[width * height];
            map.GetData<Color>(pixelData);
            Texture2D[] tiles = new Texture2D[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = x + y * width;
                    Color pixel = pixelData[index];
                    Texture2D tile = clib.GetTile(pixel.R, pixel.G, pixel.B);
                    tiles[index] = tile;
                }
            }
            int tileWidth = tiles[0].Bounds.Width;
            int tileHeight = tiles[0].Bounds.Height;
            return CreateChunk(tiles, width, height, tileWidth, tileHeight, pixelData);
        }

        private Chunk CreateChunk(Texture2D[] tiles, int width, int height, int tileWidth, int tileHeight, Color[] pixelData)
        {
            int totalWidth = width * tileWidth;
            int totalHeight = height * tileHeight;

            Texture2D finalTexture = new Texture2D(graphicsDevice, totalWidth, totalHeight);

            Color[] tileData = new Color[tileWidth * tileHeight];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Texture2D tile = tiles[y * width + x];
                    tile.GetData(tileData);
                    finalTexture.SetData(0, new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight), tileData, 0, tileData.Length);
                }
            }
            return new Chunk(finalTexture, tiles, width, height, pixelData);
        }
    }
}
