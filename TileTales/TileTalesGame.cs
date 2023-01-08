using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Net;
using TileTales.Network;

namespace TileTales
{
    public class TileTalesGame : Game
    {
        private static int WINDOW_HEIGHT_START = 900;
        private static int WINDOW_WIDTH_START = 1400;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SocketClient _socketClient;
        private UI.MyraUI _ui;

        public TileTalesGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH_START;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT_START;
            _graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
            _ui = new UI.MyraUI(this, _graphics);

            // print to console
            System.Console.WriteLine("Hello TileTalesGame World!");
        }
        
        protected override void Initialize()
        {
            // Initialize the SocketClient
            _socketClient = new SocketClient();
            try
            {
                // Connect to the server
                _socketClient.Connect(IPAddress.Parse("127.0.0.1"), 8080);
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _ui.LoadContent();

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _ui.Draw();

            base.Draw(gameTime);
        }
    }
}