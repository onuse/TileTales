﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;
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

        public BaseState(TileTalesGame game)
        {
            this.game = game;
            this.stateManager = game.StateManager;
            this.serverConnector = game.ServerConnector;
            this.ui = game.AppUI;
            eventBus = EventBus.Instance;
        }

        // Called when this state becomes the current state
        public virtual void Enter() { }

        // Called when this state is no longer the current state
        public virtual void Exit() { }

        // Called when the game is updated
        public virtual void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState) { }
        
        // Only called from TileTalesGame.Initialize()
        public virtual void Activate() { }

        public virtual void Draw(GameTime gameTime, SpriteBatch sprite, float zoomLevel) { }

        internal virtual void OnClientSizeChanged(int newWindowWidth, int newWindowHeight) { }
    }
}