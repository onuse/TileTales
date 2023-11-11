using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System.Diagnostics;
using TileTales.GameContent;
using TileTales.Utils;
using Color = Microsoft.Xna.Framework.Color;

namespace TileTales.Graphics {
    internal class ChunkFactory {
        private readonly GraphicsDevice graphicsDevice;
        private readonly ContentLibrary contentLibrary;

        public ChunkFactory(GraphicsDevice graphicsDevice, ContentLibrary contentLibrary) {
            this.graphicsDevice = graphicsDevice;
            this.contentLibrary = contentLibrary;
        }

        public Chunk CreateChunkFromMap(Map map, float scaleFactor) {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            if (map == null) {
                return null;
            }
            int width = map.Image.Width;
            int height = map.Image.Height;
            ContentLibrary clib = contentLibrary;
            Tile[] tiles = new Tile[width * height];
            SKColor[] pixelData = new SKColor[width * height];
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    int index = x + y * width;
                    SKColor pixel = map.Image.GetPixel(x, y);
                    pixelData[index] = pixel;
                    Tile tile = clib.GetTile(pixel.Red, pixel.Green, pixel.Blue);
                    tiles[index] = tile;
                }
            }
            int tileWidth = tiles[0].BackingImage.Width;
            int tileHeight = tiles[0].BackingImage.Height;
            stopwatch1.Stop();
            Log.Verbose($"Elapsed time: {stopwatch1.ElapsedMilliseconds} ms");
            if (scaleFactor != 0f) {
                return CreateBackingTexture(tiles, map, width, height, tileWidth, tileHeight, pixelData, scaleFactor);
            } else {
                return new Chunk(null, tiles, null, map, width, height, pixelData, null);
            }
        }

        /*private Chunk CreateChunk2(SKBitmap[] tiles, SKBitmap map, int width, int height, int tileWidth, int tileHeight, SKColor[] pixelData, bool createTexture)
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

        /*finalTexture = CreateTexture(canvasImage, graphicsDevice);

        //Stream stream = new MemoryStream(map.Bytes);
        //finalTexture = Texture2D.FromStream(graphicsDevice, stream);
        //finalTexture = Texture2D.FromStream(graphicsDevice, skData.AsStream());
    }
    stopwatch1.Stop();
    System.Diagnostics.Debug.WriteLine($"CreateChunk Elapsed time: {stopwatch1.ElapsedMilliseconds} ms");

    return new Chunk(finalTexture, tiles, map, totalWidth, totalHeight, pixelData, null);
}*/

        internal Chunk CreateBackingTexture(Tile[] tiles, Map map, int width, int height, int tileWidth, int tileHeight, SKColor[] pixelData, float scaleFactor) {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            int totalWidth = width * tileWidth;
            int totalHeight = height * tileHeight;
            Texture2D finalTexture = CreateTextureFast(tiles, width, height, tileWidth, tileHeight, pixelData, graphicsDevice, scaleFactor);
            stopwatch1.Stop();
            Log.Verbose($"Elapsed time: {stopwatch1.ElapsedMilliseconds} ms");
            return new Chunk(finalTexture, tiles, null, map, totalWidth, totalHeight, pixelData, null);
        }

        private Texture2D CreateTexture(SKBitmap bitmap, GraphicsDevice graphicsDevice) {
            int width = bitmap.Width;
            int height = bitmap.Height;
            Texture2D texture = new Texture2D(graphicsDevice, width, height);

            byte[] pixelData = bitmap.Bytes;
            Color[] colors = new Color[width * height];

            for (int i = 0; i < pixelData.Length; i += 4) {
                // Convert BGRA to RGBA
                colors[i / 4] = new Color(pixelData[i + 2], pixelData[i + 1], pixelData[i], pixelData[i + 3]);
            }

            texture.SetData(colors);
            return texture;
        }

        internal Texture2D ChunkDataToTexture(Chunk chunk, float scaleFactor) {
            return CreateTextureFast(chunk.Tiles, contentLibrary.MapWidth, contentLibrary.MapHeight, contentLibrary.TileWidth, contentLibrary.TileHeight, chunk.PixelData, graphicsDevice, scaleFactor);
        }

        private unsafe Texture2D CreateTextureFast(Tile[] tiles, int width, int height, int tileWidth, int tileHeight, SKColor[] pixelData, GraphicsDevice graphicsDevice, float scaleFactor) {
            int totalWidth = (int)(width * tileWidth * scaleFactor); // Apply the scale factor to the width
            int totalHeight = (int)(height * tileHeight * scaleFactor); // Apply the scale factor to the height

            // Allocate memory for the texture data
            byte[] textureData = new byte[totalWidth * totalHeight * 4];

            // Obtain pointers to the texture data and the canvas image data
            fixed (byte* textureDataPtr = textureData)
            fixed (SKColor* pixelPtr = pixelData) {
                byte* dstPtr = textureDataPtr;
                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        SKBitmap tile = tiles[y * width + x].BackingImage;
                        SKColor* srcPtr = (SKColor*)tile.GetPixels();

                        for (int j = 0; j < tileHeight; j++) {
                            for (int i = 0; i < tileWidth; i++) {
                                SKColor srcPixel = srcPtr[i + j * tileWidth];
                                byte* dstPixel = dstPtr + ((int)(y * tileHeight * scaleFactor + j * scaleFactor) * totalWidth + (int)(x * tileWidth * scaleFactor + i * scaleFactor)) * 4; // Apply the scale factor to the pixel position
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


        private Texture2D[] ConvertToTexture2DArray2(SKBitmap[] tiles, GraphicsDevice graphicsDevice) {
            Texture2D[] textures = new Texture2D[tiles.Length];
            for (int i = 0; i < tiles.Length; i++) {
                textures[i] = CreateTexture(tiles[i], graphicsDevice);
            }
            return textures;
        }

        private Texture2D[] ConvertToTexture2DArray(SKBitmap[] tiles, GraphicsDevice graphicsDevice) {
            Texture2D[] textures = new Texture2D[tiles.Length];

            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, tiles[0].Width, tiles[0].Height);
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);

            for (int i = 0; i < tiles.Length; i++) {
                graphicsDevice.SetRenderTarget(renderTarget);
                graphicsDevice.Clear(Color.Transparent);

                spriteBatch.Begin();
                spriteBatch.Draw(SkiaSharpExtensions.ToTexture2D(tiles[i], graphicsDevice), Vector2.Zero, Color.White);
                spriteBatch.End();

                textures[i] = renderTarget;
            }

            graphicsDevice.SetRenderTarget(null);
            return textures;
        }

        /*private Texture2DArray ConvertToTexture2DArrayBest(SKBitmap[] tiles, GraphicsDevice graphicsDevice)
        {
            int width = tiles[0].Width;
            int height = tiles[0].Height;
            int depth = tiles.Length;

            Texture2DArray textureArray = new Texture2DArray(graphicsDevice, width, height, depth, false, SurfaceFormat.Color);

            Color[] colors = new Color[width * height];

            for (int i = 0; i < tiles.Length; i++)
            {
                SKBitmap tile = tiles[i];
                byte[] pixelData = tile.Bytes;

                for (int j = 0; j < pixelData.Length; j += 4)
                {
                    // Convert BGRA to RGBA
                    colors[j / 4] = new Color(pixelData[j + 2], pixelData[j + 1], pixelData[j], pixelData[j + 3]);
                }

                textureArray.SetData(i, null, colors, 0, width * height);
            }

            return textureArray;
        }*/




        public void CreateTextureForChunk(Chunk chunk) {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            chunk.QuarterResolution = Texture2D.FromStream(graphicsDevice, chunk.Data.AsStream());
            stopwatch1.Stop();
            Log.Verbose($"Elapsed time: {stopwatch1.ElapsedMilliseconds} ms");
        }
    }

}
