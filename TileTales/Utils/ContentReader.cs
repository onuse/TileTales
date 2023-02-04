using Google.Protobuf;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Utils
{
    internal static class ContentReader
    {
        public static Dictionary<string, Texture2D> readTexturesInDirectory(GraphicsDevice graphicsDecvice, string relativeDirectory)
        {
            DirectoryInfo dir = new DirectoryInfo(relativeDirectory);
            System.Diagnostics.Debug.WriteLine("dir: " + dir);
            if (!dir.Exists)
                throw new DirectoryNotFoundException();
            Dictionary<String, Texture2D> result = new Dictionary<String, Texture2D>();

            FileInfo[] files = dir.GetFiles("*.png");
            foreach (FileInfo file in files)
            {
                System.Diagnostics.Debug.WriteLine("file.FullName: " + file.FullName);
                FileStream fileStream = new FileStream(file.FullName, FileMode.Open);
                result[file.Name] = Texture2D.FromStream(graphicsDecvice, fileStream);
                fileStream.Dispose();
            }
            return result;
        }

        public static Texture2D textureFromByteString(ByteString byteString, GraphicsDevice graphicsDecvice)
        {
            MemoryStream stream = new MemoryStream(byteString.ToByteArray());
            return Texture2D.FromStream(graphicsDecvice, stream);
        }
    }
}
