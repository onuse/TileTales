using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using TileTales.GameContent;
using Color = Microsoft.Xna.Framework.Color;

namespace TileTales.Graphics
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

        public Chunk CreateChunkFromMap(SKBitmap map)
        {
            int width = map.Width;
            int height = map.Height;
            ContentLibrary clib = contentLibrary;
            SKBitmap[] tiles = new SKBitmap[width * height];
            SKColor[] pixelData = new SKColor[width * height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = x + y * width;
                    SKColor pixel = map.GetPixel(x, y);
                    pixelData[index] = pixel;
                    SKBitmap tile = clib.GetTile(pixel.Red, pixel.Green, pixel.Blue).BackingImage;
                    tiles[index] = tile;
                }
            }
            int tileWidth = tiles[0].Width;
            int tileHeight = tiles[0].Height;
            return CreateChunk(tiles, map, width, height, tileWidth, tileHeight, pixelData);
        }

        private Chunk CreateChunk(SKBitmap[] tiles, SKBitmap map, int width, int height, int tileWidth, int tileHeight, SKColor[] pixelData)
        {
            int totalWidth = width * tileWidth;
            int totalHeight = height * tileHeight;

            SKBitmap canvasImage = new SKBitmap(totalWidth, totalHeight);

            using (SKCanvas canvas = new SKCanvas(canvasImage))
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        SKBitmap tile = tiles[y * width + x];
                        canvas.DrawBitmap(tile, x * tileWidth, y * tileHeight);
                    }
                }
            }
            Texture2D finalTexture = Texture2D.FromStream(graphicsDevice, canvasImage.Encode(SKEncodedImageFormat.Png, 100).AsStream());
            return new Chunk(finalTexture, tiles, map, totalWidth, totalHeight, pixelData);
        }
    }
}
