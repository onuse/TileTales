using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.GameContent
{
    internal interface ITTDrawble
    {
        public void Draw(GameTime gameTime, SpriteBatch sprite, float zoomLevel);
    }
}
