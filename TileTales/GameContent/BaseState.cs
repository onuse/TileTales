using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.Network;
using TileTales.Utils;

namespace TileTales.GameContent
{
    internal abstract class BaseState
    {
        protected StateManager stateManager;
        protected ServerConnector serverConnector;
        protected UI.AppUI ui;
        protected TileTalesGame game;
        protected EventBus eventBus;

        public BaseState(StateManager stateManager, ServerConnector serverConnector, UI.AppUI ui, TileTalesGame game)
        {
            this.stateManager = stateManager;
            this.serverConnector = serverConnector;
            this.ui = ui;
            this.game = game;
            this.eventBus = EventBus.Instance;
        }

        // Called when this state becomes the current state
        public virtual void Enter() { }

        // Called when this state is no longer the current state
        public virtual void Exit() { }

        // Called when the game is updated
        public virtual void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState) { }

        // Called when the game is drawn
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        // Only called from TileTalesGame.Initialize()
        public virtual void Activate() { }
    }
}