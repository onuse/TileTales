using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;
using TileTales.Graphics;
using TileTales.Network;
using TileTales.Utils;

namespace TileTales.State
{
    internal abstract class BaseState: ITTDrawble
    {
        protected StateManager stateManager;
        protected ServerConnector serverConnector;
        protected UI.AppUI ui;
        protected TileTalesGame game;
        protected EventBus eventBus;
        protected RequestFactory rf;
        protected Player Player;
        protected ContentLibrary content;

        public BaseState()
        {
            this.game = TileTalesGame.Instance;
            this.stateManager = game.StateManager;
            this.serverConnector = game.ServerConnector;
            this.ui = game.AppUI;
            this.Player = game.GameWorld.player;
            this.eventBus = EventBus.Instance;
            this.rf = RequestFactory.Instance;
            this.content = game.ContentLibrary;
        }

        // Called when this state becomes the current state
        public virtual void Enter() { }

        // Called when this state is no longer the current state
        public virtual void Exit() { }

        // Called when the game is updated
        public virtual void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState) { }
        
        // Only called from TileTalesGame.Initialize()
        public virtual void Activate() { }

        public virtual void Draw(GameTime gameTime) { }

        internal virtual void OnClientSizeChanged(int newWindowWidth, int newWindowHeight) { }
    }
}