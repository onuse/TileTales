using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            if (map == null)
            {
                return null;
            }
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
            stopwatch1.Stop();
            System.Diagnostics.Debug.WriteLine($"CreateChunkFromMap Elapsed time: {stopwatch1.ElapsedMilliseconds} ms");
            return CreateChunk(tiles, map, width, height, tileWidth, tileHeight, pixelData);
        }

        private Chunk CreateChunk2(SKBitmap[] tiles, SKBitmap map, int width, int height, int tileWidth, int tileHeight, SKColor[] pixelData, bool createTexture)
        {
            Stopwatch stopwatch1 = new Stopwatch();
            //Stopwatch stopwatch1 = new Stopwatch();
            //Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
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
            //SurfaceFormat format = graphicsDevice.GetTextureFormat()
            System.Diagnostics.Debug.WriteLine($"CreateChunk canvasImage.ColorType: {canvasImage.ColorType}");
            
            //SKData skData = canvasImage.Encode(SKEncodedImageFormat.Png, 100);
            Texture2D finalTexture = null;
            if (createTexture)
            {
                /*SKImageInfo info = new SKImageInfo(totalWidth, totalHeight, SKColorType.Bgra8888);
                SKBitmap newBitmap = new SKBitmap(info);
                canvasImage.CopyTo(newBitmap, info.ColorType);
                //finalTexture = new Texture2D(graphicsDevice, totalWidth, totalHeight);
                finalTexture = new Texture2D(graphicsDevice, totalWidth, totalHeight, false, SurfaceFormat.Color);
                finalTexture.SetData(newBitmap.Pixels);*/

                /*SKImageInfo info = new SKImageInfo(totalWidth, totalHeight, SKColorType.Bgra8888);
                SKPixmap pixmap = new SKPixmap(info, canvasImage.GetPixels());
                finalTexture = new Texture2D(graphicsDevice, totalWidth, totalHeight, false, SurfaceFormat.Color);
                finalTexture.SetData<byte>(pixmap.GetPixelSpan().ToArray());*/

                finalTexture = CreateTexture(canvasImage, graphicsDevice);

                //Stream stream = new MemoryStream(map.Bytes);
                //finalTexture = Texture2D.FromStream(graphicsDevice, stream);
                //finalTexture = Texture2D.FromStream(graphicsDevice, skData.AsStream());
            }
            stopwatch1.Stop();
            System.Diagnostics.Debug.WriteLine($"CreateChunk Elapsed time: {stopwatch1.ElapsedMilliseconds} ms");

            return new Chunk(finalTexture, tiles, map, totalWidth, totalHeight, pixelData, null);
        }

        private Chunk CreateChunk(SKBitmap[] tiles, SKBitmap map, int width, int height, int tileWidth, int tileHeight, SKColor[] pixelData)
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            int totalWidth = width * tileWidth;
            int totalHeight = height * tileHeight;
            Texture2D finalTexture = CreateTextureFast(tiles, width, height, tileWidth, tileHeight, pixelData, graphicsDevice);
            stopwatch1.Stop();
            System.Diagnostics.Debug.WriteLine($"CreateChunk Elapsed time: {stopwatch1.ElapsedMilliseconds} ms");
            return new Chunk(finalTexture, tiles, map, totalWidth, totalHeight, pixelData, null);
        }

            private Texture2D CreateTexture(SKBitmap bitmap, GraphicsDevice graphicsDevice)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            byte[] pixelData = bitmap.Bytes;
            Color[] colors = new Color[width * height];

            for (int i = 0; i < pixelData.Length; i += 4)
            {
                // Convert BGRA to RGBA
                colors[i / 4] = new Color(pixelData[i + 2], pixelData[i + 1], pixelData[i], pixelData[i + 3]);
            }

            texture.SetData(colors);
            return texture;
        }

        private unsafe Texture2D CreateTextureFast(SKBitmap[] tiles, int width, int height, int tileWidth, int tileHeight, SKColor[] pixelData, GraphicsDevice graphicsDevice)
        {
            int totalWidth = width * tileWidth;
            int totalHeight = height * tileHeight;

            // Allocate memory for the texture data
            byte[] textureData = new byte[totalWidth * totalHeight * 4];

            // Obtain pointers to the texture data and the canvas image data
            fixed (byte* textureDataPtr = textureData)
            fixed (SKColor* pixelPtr = pixelData)
            {
                byte* dstPtr = textureDataPtr;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        SKBitmap tile = tiles[y * width + x];
                        SKColor* srcPtr = (SKColor*)tile.GetPixels();

                        for (int j = 0; j < tileHeight; j++)
                        {
                            for (int i = 0; i < tileWidth; i++)
                            {
                                SKColor srcPixel = srcPtr[i + j * tileWidth];
                                byte* dstPixel = dstPtr + ((y * tileHeight + j) * totalWidth + x * tileWidth + i) * 4;
                                dstPixel[0] = srcPixel.Red;
                                dstPixel[1] = srcPixel.Green;
                                dstPixel[2] = srcPixel.Blue;
                                dstPixel[3] = srcPixel.Alpha;
                            }
                        }
                    }
                }
            }

            // Create the texture from the pixel data
            Texture2D finalTexture = new Texture2D(graphicsDevice, totalWidth, totalHeight);
            finalTexture.SetData(textureData);

            return finalTexture;
        }



        public void CreateTextureForChunk(Chunk chunk)
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            chunk.Image = Texture2D.FromStream(graphicsDevice, chunk.Data.AsStream());
            stopwatch1.Stop();
            System.Diagnostics.Debug.WriteLine($"CreateTextureForChunk Elapsed time: {stopwatch1.ElapsedMilliseconds} ms");
        }
    }

}
