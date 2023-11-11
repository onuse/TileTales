using Microsoft.Xna.Framework.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;

namespace TileTales.Graphics {
    internal class Tile: IEquatable<Tile>, IComparable<Tile> {
        public Tile(int replacementColor) {
            Tags = new List<string>();
            ReplacementColor = replacementColor;
        }
        public Tile(int replacementColor, SKBitmap image) : this(replacementColor) {
            BackingImage = image;
        }
        public int ReplacementColor { get; }
        public List<String> Tags { get; set; }
        public SKBitmap BackingImage { get; set; }
        public Texture2D Image { get; set; }
        public int LegacyColor { get; set; }
        public string LegacyColorAsString {
            get {
                return LegacyColor.ToString("X6");
            }
            set {
                LegacyColor = int.Parse(value, System.Globalization.NumberStyles.HexNumber);
            }
        }

        public string Name { get; internal set; }
        public string Description { get; internal set; }

        public int CompareTo(Tile other) {
            return ReplacementColor.CompareTo(other.ReplacementColor);
        }

        public bool Equals(Tile other) {
            return ReplacementColor == other.ReplacementColor;
        }

        public override bool Equals(object obj) {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tile)obj);
        }

        public override int GetHashCode() {
            return ReplacementColor;
        }
    }
}
