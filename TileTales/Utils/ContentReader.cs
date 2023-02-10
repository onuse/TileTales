using Google.Protobuf;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal static class ContentReader
    {
        public static Dictionary<string, SKBitmap> readTexturesInDirectory(GraphicsDevice graphicsDecvice, string relativeDirectory)
        {
            DirectoryInfo dir = new DirectoryInfo(relativeDirectory);
            System.Diagnostics.Debug.WriteLine("dir: " + dir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, SKBitmap> result = new Dictionary<String, SKBitmap>();

            FileInfo[] files = dir.GetFiles("*.png");
            foreach (FileInfo file in files)
            {
                System.Diagnostics.Debug.WriteLine("file.FullName: " + file.FullName);
                FileStream fileStream = new FileStream(file.FullName, FileMode.Open);

                //result[file.Name] = Texture2D.FromStream(graphicsDecvice, fileStream);
                result[file.Name] =  SKBitmap.Decode(fileStream);
                //SkiaSharp.SKBitmap bitmap = SkiaSharp.SKBitmap.Decode(fileStream);
                fileStream.Dispose();
            }
            return result;
        }

        public static Dictionary<string, SKBitmap> readTilesInDirectory(GraphicsDevice graphicsDecvice, string relativeDirectory)
        {
            DirectoryInfo dir = new DirectoryInfo(relativeDirectory);
            System.Diagnostics.Debug.WriteLine("Tiles dir: " + dir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, SKBitmap> result = new Dictionary<String, SKBitmap>();

            FileInfo[] files = dir.GetFiles("*.png");
            foreach (FileInfo file in files)
            {
                FileStream fileStream = new FileStream(file.FullName, FileMode.Open);
                String tileName = file.Name.Substring(2, file.Name.Length - 6);
                System.Diagnostics.Debug.WriteLine("Add Tile: " + tileName);
                //result[tileName] = Texture2D.FromStream(graphicsDecvice, fileStream);
                result[tileName] = SKBitmap.Decode(fileStream);
                fileStream.Dispose();
            }
            return result;
        }

        public static SKBitmap textureFromByteString(ByteString byteString, GraphicsDevice graphicsDecvice)
        {
            MemoryStream stream = new MemoryStream(byteString.ToByteArray());
            //return Texture2D.FromStream(graphicsDecvice, stream);
            return SKBitmap.Decode(stream);
        }
    }
}
