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
        public readonly EventBus EventBus;
        public readonly ContentLibrary ContentLibrary;
        public readonly Canvas Canvas;
        public readonly UI.AppUI AppUI;
        public readonly ServerConnector ServerConnector;
        public readonly StateManager StateManager;
        public readonly GameWorld GameWorld;
        public readonly GameSettings GameSettings;

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
            GameSettings = new GameSettings(int.MaxValue, int.MaxValue, 16, 16);

            GraphicsDeviceManager graphicsManager = new GraphicsDeviceManager(this);
            graphicsManager.PreferredBackBufferWidth = _settingsReader.GetSettings().WindowWidth;
            graphicsManager.PreferredBackBufferHeight = _settingsReader.GetSettings().WindowHeight;
            graphicsManager.ApplyChanges();

            ServerConnector = new ServerConnector();

            ContentLibrary = new ContentLibrary(GraphicsDevice);
            GameWorld = new GameWorld(this);
            Canvas = new Canvas(this, graphicsManager);
            AppUI = new UI.AppUI(this, graphicsManager);

            Window.ClientSizeChanged += OnClientSizeChanged;
            Exiting += Shutdown;
            EventBus.Subscribe("quit", (data) => Exit());
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            xnbContentManager = new ContentManager(Services, "Content");
            ContentLibrary.LoadPrepackagedContent();
            Canvas.LoadContent();
            AppUI.LoadContent();
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
            StateManager.Draw(gameTime, null, 0);
            AppUI.Draw();
            base.Draw(gameTime);
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("OnClientSizeChanged w: " + Window.ClientBounds.Width + " h: " + Window.ClientBounds.Height);
            StateManager.OnClientSizeChanged(Window.ClientBounds.Width, Window.ClientBounds.Height);
            _settingsReader.GetSettings().WindowWidth = Window.ClientBounds.Width;
            _settingsReader.GetSettings().WindowHeight = Window.ClientBounds.Height;
            _settingsReader.SaveSettings();
        }
        public void Shutdown(object sender, EventArgs e)
        {
            ServerConnector.Shutdown();
            //Checking if Shutdown works, and it does.
            Console.WriteLine("Exiting Game");
}
    }
}