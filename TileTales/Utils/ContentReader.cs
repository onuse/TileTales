using Google.Protobuf;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal static class ContentReader
    {
        public static Dictionary<string, SKBitmap> readTexturesInDirectory(GraphicsDevice graphicsDecvice, string relativeDirectory)
        {
            DirectoryInfo dir = new DirectoryInfo(relativeDirectory);
            Log.Debug("dir: " + dir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, SKBitmap> result = new Dictionary<String, SKBitmap>();

            FileInfo[] files = dir.GetFiles("*.png");
            foreach (FileInfo file in files)
            {
                Log.Debug("file.FullName: " + file.FullName);
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
            Log.Debug("Tiles dir: " + dir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, SKBitmap> result = new Dictionary<String, SKBitmap>();

            FileInfo[] files = dir.GetFiles("*.png");
            foreach (FileInfo file in files)
            {
                FileStream fileStream = new FileStream(file.FullName, FileMode.Open);
                String tileName = file.Name.Substring(2, file.Name.Length - 6);
                Log.Debug("Add Tile: " + tileName);
                //result[tileName] = Texture2D.FromStream(graphicsDecvice, fileStream);
                result[tileName] = SKBitmap.Decode(fileStream);
                fileStream.Dispose();
            }
            return result;
        }

        public static SKBitmap bitmapFromByteString(ByteString byteString)
        {
            byte[] bytes = byteString.ToByteArray();
            //MemoryStream stream = new MemoryStream(bytes);
            //return SKBitmap.Decode(stream);
            return DecodeBitmap(bytes);
        }

        public static Texture2D textureFromByteString(GraphicsDevice graphicsDevice, ByteString byteString)
        {
            byte[] bytes = byteString.ToByteArray();
            MemoryStream stream = new MemoryStream(bytes);
            return Texture2D.FromStream(graphicsDevice, stream);
        }

        private unsafe static SKBitmap DecodeBitmap(byte[] bytes)
        {
            fixed (byte* ptr = bytes)
            {
                using (var stream = new UnmanagedMemoryStream(ptr, bytes.Length))
                {
                    return SKBitmap.Decode(stream);
                }
            }
        }

        internal static Texture2D readTexture(GraphicsDevice graphicsDevice, string textureFilePath)
        {
            FileStream fileStream = new FileStream(textureFilePath, FileMode.Open);
            return Texture2D.FromStream(graphicsDevice, fileStream);
        }
    }
}
