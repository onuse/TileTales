using Google.Protobuf;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;

namespace TileTales.Graphics
{
    internal class Map : IEquatable<Map>, IComparable<Map>
    {
        public Map(int x, int y, int z, int zoomLevel)
        {
            X = x;
            Y = y;
            Z = z;
            ZoomLevel = zoomLevel;
            Name = ContentLibrary.CreateMapName(x, y, z, 0);
        }

        public Map(Location location) : this(location.X, location.Y, location.Z, 0)
        {
        }

        public String Name { get; }
        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public int ZoomLevel { get; }
        public SKBitmap Image { get; set; }
        public Texture2D Texture { get; set; }
        public ByteString ByteString { get; internal set; }

        public bool Equals(Map other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public int CompareTo(Map other)
        {
            int compareX = X.CompareTo(other.X);
            int compareY = Y.CompareTo(other.Y);
            int compareZ = Z.CompareTo(other.Z);

            if (compareX != 0)
            {
                return compareX;
            }
            else if (compareY != 0)
            {
                return compareY;
            }
            else
            {
                return compareZ;
            }
        }

        public override int GetHashCode()
        {
            return X ^ Y ^ Z;
        }
    }
}
