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
    public static class SkiaSharpExtensions
    {
        public static Texture2D ToTexture2D(this SKBitmap bitmap, GraphicsDevice graphicsDevice)
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
    }
}
