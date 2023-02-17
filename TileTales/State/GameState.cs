using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Net.Tiletales.Network.Proto.App;
using Net.Tiletales.Network.Proto.Game;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TileTales.GameContent;
using TileTales.Graphics;
using TileTales.Network;
using TileTales.UI;
using TileTales.Utils;

namespace TileTales.State
{
    internal class GameState : BaseState
    {
        private readonly GameUI _gameUI;
        bool _hasLoadedWorld = false;
        private int windowWidth;
        private int windowHeight;
        private int lastScrollWheelValue = 0;
        private int zoomLevel = 1;

        private static List<float> SCALE_VALUES = new float[] {
            8,
            4,
            2,
            1,
            1f / 2f,
            1f / 4f,
            1f / 8f,
            1f / 16f,
            1f / 32f,
            1f / 64f,
            1f / 128,
            1f / 256,
            1f / 512,
            1f / 1024,
            1f / 2048
        }.ToList();

        private static List<float> SCALES_INVERSE = SCALE_VALUES.ToArray().ToList();
        private object moveRequestThrottle;

        public GameState(TileTalesGame game) : base(game)
        {
            SCALES_INVERSE.Reverse();
            _gameUI = ui._gameUI;
            _gameUI.paddedCenteredButton.Click += (s, a) =>
            {
                windowWidth = SettingsReader.Instance.GetSettings().WindowWidth;
                windowHeight = SettingsReader.Instance.GetSettings().WindowHeight;
                Login();
            };

            /*eventBus.Subscribe(AccountLoginResponse.Descriptor.Name, (o) => {
                AccountLoginResponse response = AccountLoginResponse.Parser.ParseFrom((o as Any).Value);
                if (response.Success)
                {
                    AllTilesRequest allTilesRequest = rf.CreateAllTilesRequest();
                    serverConnector.SendMessage(allTilesRequest);
                }
            });*/

            eventBus.Subscribe(PlayerObjectInfo.Descriptor.Name, (o) => {
                PlayerObjectInfo response = PlayerObjectInfo.Parser.ParseFrom((o as Any).Value);
                game.GameWorld.createPlayerObject(response);
                AllTilesRequest allTilesRequest = rf.CreateAllTilesRequest();
                System.Diagnostics.Debug.WriteLine("eventBus.Subscribe(PlayerObjectInfo");
                serverConnector.SendMessage(allTilesRequest);
            });

            eventBus.Subscribe(AllTilesResponse.Descriptor.Name, (o) => {
                System.Diagnostics.Debug.WriteLine("eventBus.Subscribe(AllTilesResponse");
                AllTilesResponse response = AllTilesResponse.Parser.ParseFrom((o as Any).Value);
                response.Tiles.ToList().ForEach(tile => AddTile(tile));
                content.CreateWaterChunk();
                Player p = game.GameWorld.GetPlayer();
                MapsRequest zoneMapsRequest = rf.CreateZoneMapsRequest(p.X, p.Y, p.Z, 0);
                serverConnector.SendMessage(zoneMapsRequest);
            });

            eventBus.Subscribe(ObjectLocationUpdate.Descriptor.Name, (o) => {
                System.Diagnostics.Debug.WriteLine("eventBus.Subscribe(ObjectLocationUpdate");
                ObjectLocationUpdate response = ObjectLocationUpdate.Parser.ParseFrom((o as Any).Value);
                if (response.ObjectId == Player.ObjectId)
                {
                    Player.X = response.X;
                    Player.Y = response.Y;
                    Player.Z = response.Z;
                }
            });

            eventBus.Subscribe(MapResponse.Descriptor.Name, (o) => {
                MapResponse response = MapResponse.Parser.ParseFrom((o as Any).Value);
                Task.Run(() => LoadMap(response));
            });

            eventBus.Subscribe(MapsResponse.Descriptor.Name, (o) => {
                System.Diagnostics.Debug.WriteLine("eventBus.Subscribe(MapsResponse");
                MapsResponse response = MapsResponse.Parser.ParseFrom((o as Any).Value);
                // Start new thread to load maps
                Task.Run(() => LoadMaps(response));
                _hasLoadedWorld = true;
            });
        }
        
        private void AddTile(TileData tileData)
        {
            Tile tile = new Tile(tileData.ReplacementColor);
            tile.LegacyColor = tileData.LegacyColor;
            tile.Image = SKBitmap.Decode(tileData.Image.ToByteArray());
            content.AddTile(tile);
        }

        private void LoadMaps(MapsResponse response)
        {
            response.Maps.ToList().ForEach(response => LoadMap(response));
        }

        private void LoadMap(MapResponse response)
        {
            ByteString mapBytes = response.Map;
            String mapName = ContentLibrary.CreateMapName(response.X, response.Y, response.Z, response.ZoomLevel);
            game.ContentLibrary.AddMap(mapName, mapBytes, true, true);
        }


        private void Login()
        {
            if (serverConnector.isConnected())
            {
                UserSettings settings = SettingsReader.Instance.GetSettings();
                AccountLoginRequest loginRequest = new AccountLoginRequest
                {
                    Username = settings.AccountUsername,
                    Password = settings.AccountPassword
                };
                serverConnector.SendMessage(loginRequest);
            }
        }

        public override void Enter()
        {
            eventBus.Publish("connect", null);
        }

        internal override void OnClientSizeChanged(int newWindowWidth, int newWindowHeight) 
        {
            windowWidth = newWindowWidth;
            windowHeight = newWindowHeight;
        }
        
        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            int deltaX = 0;
            int deltaY = 0;
            int deltaZ = 0;
            if (ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W))
                deltaY = -16;
            if (ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S))
                deltaY = 16;
            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.A))
                deltaX = -16;
            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D))
                deltaX = +16;

            if (deltaX != 0 || deltaY != 0 || deltaZ != 0)
            {
                sendMoveRequestThrottled(deltaX, deltaY, deltaZ);
            }

            int dir = ms.ScrollWheelValue - lastScrollWheelValue;
            if (dir != 0)
            {
                if (dir > 0)
                {
                    ZoomIn();
                }
                else
                {
                    ZoomOut();
                }
            }
            lastScrollWheelValue = ms.ScrollWheelValue;
        }

        private void sendMoveRequestThrottled(int deltaX, int deltaY, int deltaZ)
        {
            if (moveRequestThrottle == null)
            {
                moveRequestThrottle = new Timer((o) =>
                {
                    MoveRequest moveRequest = rf.createMoveRequest(deltaX, deltaY, deltaZ);
                    serverConnector.SendMessage(moveRequest);
                    moveRequestThrottle = null;
                }, null, 150, Timeout.Infinite);
            }
        }

        private void ZoomIn()
        {
            if (--zoomLevel <= -1)
                zoomLevel = 0;
        }
        private void ZoomOut()
        {
            if (++zoomLevel >= SCALE_VALUES.Count)
                zoomLevel = SCALE_VALUES.Count - 1;
        }

        public override void Draw(GameTime gameTime)
        {
            if (_hasLoadedWorld)
            {
                game.renderer.Draw(game.GameWorld, gameTime, windowWidth, windowHeight, SCALE_VALUES[zoomLevel]);
            }
        }
    }
}
