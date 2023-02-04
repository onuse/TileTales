using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net;
using TileTales.State;
using TileTales.Network;
using TileTales.Utils;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using TileTales.GameContent;
using Google.Protobuf.Collections;
using Myra.Graphics2D.UI;

namespace TileTales
{
    internal class TileTalesGame : Game
    {
        private readonly SettingsReader _settingsReader;
        private readonly EventBus _eventBus;
        private readonly StateManager _stateManager;
        private readonly GraphicsDeviceManager _graphics;
        private readonly UI.AppUI _ui;
        private readonly ServerConnector _serverConnector;
        private SpriteBatch _spriteBatch;
        private ContentManager xnbContentManager;
        public readonly ContentLibrary ContentLibrary;

        Texture2D chestTexture;

        public TileTalesGame()
        {
            System.Diagnostics.Debug.WriteLine("TileTalesGame");
            System.Console.WriteLine("Hello TileTalesGame World!");
            _settingsReader = SettingsReader.Instance;
            Content.RootDirectory = "Content";
            _stateManager = StateManager.Instance;
            _eventBus = EventBus.Instance;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = _settingsReader.GetSettings().WindowWidth;
            _graphics.PreferredBackBufferHeight = _settingsReader.GetSettings().WindowHeight;
            _graphics.ApplyChanges();

            ContentLibrary = new ContentLibrary(GraphicsDevice);

            _serverConnector = new ServerConnector();
            
            Window.ClientSizeChanged += OnClientSizeChanged;

            Exiting += Shutdown;

            _ui = new UI.AppUI(this, _graphics);

            _eventBus.Subscribe("quit", (data) => Exit());
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            xnbContentManager = new ContentManager(Services, "Content");
            ContentLibrary.LoadPrepackagedContent();

            chestTexture = ContentLibrary.GetSprite("chest.png");

            _ui.LoadContent();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _stateManager.PushState(new StartupState(_stateManager, _serverConnector, _ui, this));
        }

        protected override void Update(GameTime gameTime)
        {
            _stateManager.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _eventBus.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _stateManager.Draw(gameTime, _spriteBatch);

            _ui.Draw();

            // draw chestTexture
            _spriteBatch.Begin();
            _spriteBatch.Draw(chestTexture, new Vector2(100, 100), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            _settingsReader.GetSettings().WindowWidth = Window.ClientBounds.Width;
            _settingsReader.GetSettings().WindowWidth = Window.ClientBounds.Height;
            _settingsReader.SaveSettings();
        }
        public void Shutdown(object sender, EventArgs e)
        {
            _serverConnector.Shutdown();
            //Checking if Shutdown works, and it does.
            Console.WriteLine("Exiting Game");
}
    }
}