using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.GameContent
{
    internal abstract class BaseState
    {
        // The StateManager that manages this state
        protected StateManager stateManager;

        public BaseState(StateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        // Called when this state becomes the current state
        public virtual void Enter() { }

        // Called when this state is no longer the current state
        public virtual void Exit() { }

        // Called when the game is updated
        public virtual void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState) { }

        // Called when the game is drawn
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        // Only called from TileTalesGame.Activate()
        public virtual void Activate() { }
    }
}