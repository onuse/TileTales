using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Graphics
{
    internal class Map : IEquatable<Map>, IComparable<Map>
    {
        public Map(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public SKBitmap Image { get; set; }

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
