using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Graphics
{
    internal class Tile : IEquatable<Tile>, IComparable<Tile>
    {
        public Tile(int id)
        {
            Id = id;
        }
        public Tile(int id, SKBitmap image)
        {
            Id = id;
            Image = image;
        }
        public int Id { get; }
        public SKBitmap Image { get; set; }

        public int CompareTo(Tile other)
        {
            return Id.CompareTo(other.Id);
        }

        public bool Equals(Tile other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tile)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
