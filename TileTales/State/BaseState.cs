using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
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
    internal abstract class BaseState: ITTDrawble, IEquatable<BaseState>
    {
        protected StateManager stateManager;
        protected ServerConnector serverConnector;
        protected UI.AppUI ui;
        protected TileTalesGame game;
        protected EventBus eventBus;
        protected Player Player;
        protected ContentLibrary content;

        public BaseState()
        {
            this.game = TileTalesGame.Singleton;
            this.stateManager = game.StateManager;
            this.serverConnector = game.ServerConnector;
            this.ui = game.AppUI;
            this.Player = game.GameWorld.player;
            this.eventBus = EventBus.Singleton;
            this.content = game.ContentLibrary;
        }

        protected bool IsMouseInsideWindow()
        {
            MouseState ms = Mouse.GetState();
            return game.GraphicsDevice.Viewport.Bounds.Contains(new Point(ms.X, ms.Y));
        }

        protected bool IsMouseOverUI()
        {
            return ui.IsMouseOverGUI();
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

        public bool Equals(BaseState other)
        {
            return this == other;
        }
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tile)obj);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}