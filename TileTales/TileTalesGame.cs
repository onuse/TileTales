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
using TileTales.UI;
using System.Diagnostics;
using System.Text.RegularExpressions;

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
        public readonly GraphicsDeviceManager GraphicsManager;

        private readonly SettingsReader _settingsReader;

        public Settings GameSettings;
        private ContentManager xnbContentManager;

        public static TileTalesGame Singleton { get; private set; }

        public float FPS {
            get => renderer.FPS;
        }

        public float MPF
        {
            get => renderer.MPF;
        }
        public bool IsGameMode => StateManager.IsState(GameState.Singleton);

        public TileTalesGame()
        {
            Singleton = this;
            Log.Info("TileTalesGame");
            _settingsReader = SettingsReader.Singleton;
            Content.RootDirectory = "Content";
            StateManager = StateManager.Singleton;
            EventBus = EventBus.Singleton;

            GraphicsManager = new GraphicsDeviceManager(this);
            GraphicsManager.PreferredBackBufferWidth = _settingsReader.GetSettings().WindowWidth;
            GraphicsManager.PreferredBackBufferHeight = _settingsReader.GetSettings().WindowHeight;
            GraphicsManager.ApplyChanges();

            ServerConnector = new ServerConnector();

            ContentLibrary = new ContentLibrary(GraphicsDevice);
            GameWorld = new GameWorld(this);
            renderer = new GameRenderer(this, GraphicsManager);
            AppUI = new AppUI(this, GraphicsManager);
            AppUI.Width = _settingsReader.GetSettings().WindowWidth;
            AppUI.Height = _settingsReader.GetSettings().WindowHeight;

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
            StateManager.PushState(new StartupState());
        }

        protected override void Update(GameTime gameTime)
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();
            EventBus.Update();
            stopwatch1.Stop();
            if (stopwatch1.ElapsedMilliseconds > 100 && Log.IsAtLeastDebug)
                Log.Warning("EventBus.Update() took " + stopwatch1.ElapsedMilliseconds + "ms");
            StateManager.Update(gameTime, Keyboard.GetState(), Mouse.GetState());

            /*if (Keyboard.GetState().IsKeyDown(Keys.F11))
                GraphicsDevice.PresentationParameters.
            else
                GraphicsDevice.PresentationParameters.FullScreenRefreshRateInHz = 0;*/

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!this.IsActive) return;
            GraphicsDevice.Clear(Color.Black);
            StateManager.Draw(gameTime);
            AppUI.Draw();
            base.Draw(gameTime);
        }

        private void OnClientSizeChanged(object sender, EventArgs e)
        {
            Log.Verbose("OnClientSizeChanged w: " + Window.ClientBounds.Width + " h: " + Window.ClientBounds.Height);
            StateManager.OnClientSizeChanged(Window.ClientBounds.Width, Window.ClientBounds.Height);
            UserSettings userSettings = _settingsReader.GetSettings();
            userSettings.WindowWidth = Window.ClientBounds.Width;
            userSettings.WindowHeight = Window.ClientBounds.Height;
            GraphicsManager.PreferredBackBufferWidth = userSettings.WindowWidth;
            GraphicsManager.PreferredBackBufferHeight = userSettings.WindowHeight;
            GraphicsManager.ApplyChanges();
            AppUI.Width = userSettings.WindowWidth;
            AppUI.Height = userSettings.WindowHeight;
            _settingsReader.SaveSettings();
        }
        public void Shutdown(object sender, EventArgs e)
        {
            ServerConnector.Shutdown();
            //Checking if Shutdown works, and it does.
            Log.Info("Exiting Game");
}

        internal void InitGameSettings(RealmInfo realmData)
        {
            Log.Debug("InitGameSettings");
            GameSettings = new Settings(realmData.WorldSize, realmData.TileSize, realmData.MapSize, _settingsReader.GetSettings());
            ContentLibrary.GameSettings = GameSettings;
        }

        internal void ActivateArtistState()
        {
            StateManager.ClearStates();
            StateManager.PushState(RunningState.Singleton);
            StateManager.PushState(ArtistState.Singleton);
        }

        internal void ActivateGameState()
        {
            StateManager.ClearStates();
            StateManager.PushState(RunningState.Singleton);
            StateManager.PushState(GameState.Singleton);
        }
    }
}