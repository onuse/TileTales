using Google.Protobuf;
using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace TileTales.Graphics
{
    internal class Map : IEquatable<Map>, IComparable<Map>
    {
        internal Map(int x, int y, int z, ByteString byteString)
        {
            X = x;
            Y = y;
            Z = z;
            ByteString = byteString;
            Location = new Point3D(x, y, z);
        }

        internal Map(Point3D location) : this(location.X, location.Y, location.Z, null)
        {
        }

        internal Point3D Location { get; }
        internal int X { get; }
        internal int Y { get; }
        internal int Z { get; }
        internal int ZoomLevel { get; }
        internal SKBitmap Image { get; set; }
        internal Texture2D Texture { get; set; }
        public ByteString ByteString { get; }

        internal static Point3D CreateLocationFromMapName(string name)
        {
            // Texture name is in format: x_y_z.png
            int _idx = name.IndexOf("_");
            int _lidx = name.LastIndexOf("_");
            int x = int.Parse(name.Substring(0, _idx));
            int y = int.Parse(name.Substring(_idx + 1, _lidx - _idx - 1));
            int z = int.Parse(name.Substring(_lidx + 1, name.LastIndexOf(".") - _lidx - 1));
            return new Point3D(x, y, z);
        }

        public bool Equals(Map other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tile)obj);
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
