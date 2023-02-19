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
using TileTales.Graphics;
using Net.Tiletales.Network.Proto.App;

namespace TileTales
{
    internal class TileTalesGame : Game
    {
        public readonly EventBus EventBus;
        public readonly ContentLibrary ContentLibrary;
        public readonly GameRenderer renderer;
        public readonly UI.AppUI AppUI;
        public readonly ServerConnector ServerConnector;
        public readonly StateManager StateManager;
        public readonly GameWorld GameWorld;
        public Settings GameSettings;
        public readonly GraphicsDeviceManager GraphicsManager;

        private readonly SettingsReader _settingsReader;
        private ContentManager xnbContentManager;

        public TileTalesGame()
        {
            System.Diagnostics.Debug.WriteLine("TileTalesGame");
            System.Console.WriteLine("Hello TileTalesGame World!");
            _settingsReader = SettingsReader.Instance;
            Content.RootDirectory = "Content";
            StateManager = StateManager.Instance;
            EventBus = EventBus.Instance;

            GraphicsManager = new GraphicsDeviceManager(this);
            GraphicsManager.PreferredBackBufferWidth = _settingsReader.GetSettings().WindowWidth;
            GraphicsManager.PreferredBackBufferHeight = _settingsReader.GetSettings().WindowHeight;
            GraphicsManager.ApplyChanges();

            ServerConnector = new ServerConnector();

            ContentLibrary = new ContentLibrary(GraphicsDevice);
            GameWorld = new GameWorld(this);
            renderer = new GameRenderer(this, GraphicsManager);
            AppUI = new UI.AppUI(this, GraphicsManager);

            Window.ClientSizeChanged += OnClientSizeChanged;
            Exiting += Shutdown;
            EventBus.Subscribe(EventType.Quit, (data) => Exit());
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            xnbContentManager = new ContentManager(Services, "Content");
            ContentLibrary.LoadPrepackagedContent();
            renderer.LoadContent();
            AppUI.LoadMainMenu();
        }

        protected override void Initialize()
        {
            base.Initialize();
            StateManager.PushState(new StartupState(this));
        }

        protected override void Update(GameTime gameTime)
        {
            StateManager.Update(gameTime, Keyboard.GetState(), Mouse.GetState());
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            /*if (Keyboard.GetState().IsKeyDown(Keys.F11))
                GraphicsDevice.PresentationParameters.
            else
                GraphicsDevice.PresentationParameters.FullScreenRefreshRateInHz = 0;*/

            EventBus.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            StateManager.Draw(gameTime);
            AppUI.Draw();
            base.Draw(gameTime);
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnClientSizeChanged w: " + Window.ClientBounds.Width + " h: " + Window.ClientBounds.Height);
            StateManager.OnClientSizeChanged(Window.ClientBounds.Width, Window.ClientBounds.Height);
            UserSettings userSettings = _settingsReader.GetSettings();
            userSettings.WindowWidth = Window.ClientBounds.Width;
            userSettings.WindowHeight = Window.ClientBounds.Height;
            GraphicsManager.PreferredBackBufferWidth = userSettings.WindowWidth;
            GraphicsManager.PreferredBackBufferHeight = userSettings.WindowHeight;
            GraphicsManager.ApplyChanges();
            _settingsReader.SaveSettings();
            if (GameSettings != null)
            {
                GameSettings.WindowWidth = userSettings.WindowWidth;
                GameSettings.WindowHeight = userSettings.WindowHeight;
            }
        }
        public void Shutdown(object sender, EventArgs e)
        {
            ServerConnector.Shutdown();
            //Checking if Shutdown works, and it does.
            Console.WriteLine("Exiting Game");
}

        internal void InitGameSettings(RealmInfo data)
        {
            System.Diagnostics.Debug.WriteLine("InitGameSettings");
            GameSettings = new Settings(data.MapSize, data.MapSize, data.TileSize, data.TileSize);
            UserSettings userSettings = _settingsReader.GetSettings();
            GameSettings.WindowWidth = userSettings.WindowWidth;
            GameSettings.WindowHeight = userSettings.WindowHeight;
            ContentLibrary.SetGameSettings(GameSettings);
        }
    }
}