using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.Graphics
{
    internal class Sprite
    {
        internal Sprite(Texture2D texture)
        {
            Texture = texture;
        }
        public Texture2D Texture { get; internal set; }

        internal void Draw(SpriteBatch tileBatch, Vector2 pos, object value, Color tint, float rotation, Vector2 origin, float scale, SpriteEffects spriteEffects, float layerDepth)
        {
            tileBatch.Draw(Texture, pos, null, tint, rotation, origin, scale, spriteEffects, layerDepth);
        }
    }
}
