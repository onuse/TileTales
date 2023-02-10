using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal static class ContentWriter
    {
        private static readonly String FOLDER_CONTENT = "";

        internal static void WriteFile(string filename, byte[] bytes)
        {
            FileStream fileStream = new FileStream(FOLDER_CONTENT + filename, FileMode.Create);
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        internal static void WriteFile(string filename, string contents)
        {
            // Write to disc
            using (var stream = File.OpenWrite(FOLDER_CONTENT + filename))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(contents);
                }
            }
        }

        /*internal static void WriteFile(string filename, Texture2D texture)
        {
            FileStream fs = new FileStream(FOLDER_CONTENT + filename, FileMode.Create);
            texture.SaveAsPng(fs, texture.Width, texture.Height);
            fs.Close();
        }*/

        internal static void WriteFile(string filename, SKBitmap texture)
        {
            FileStream fs = new FileStream(FOLDER_CONTENT + filename, FileMode.Create);
            texture.Encode(fs, SKEncodedImageFormat.Png, 100);
            fs.Close();
        }
    }
}
