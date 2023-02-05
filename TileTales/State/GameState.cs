using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Net.Tiletales.Network.Proto.App;
using Net.Tiletales.Network.Proto.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TileTales.GameContent;
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

        public GameState(TileTalesGame game) : base(game)
        {
            _gameUI = ui._gameUI;
            _gameUI.paddedCenteredButton.Click += (s, a) =>
            {
                windowWidth = SettingsReader.Instance.GetSettings().WindowWidth;
                windowHeight = SettingsReader.Instance.GetSettings().WindowHeight;
                Login();
            };

            eventBus.Subscribe(PlayerDetailResponse.Descriptor.Name, (o) => {
                PlayerDetailResponse response = PlayerDetailResponse.Parser.ParseFrom((o as Any).Value);
                ZoneMapRequest zoneMapRequest = createZoneMapRequest(response.X, response.Y, response.Z, 0);
                serverConnector.SendMessage(createZoneMapsRequest(response.X, response.Y, response.Z, 0));
            });

            eventBus.Subscribe(ZoneMapResponse.Descriptor.Name, (o) => {
                ZoneMapResponse response = ZoneMapResponse.Parser.ParseFrom((o as Any).Value);
                ByteString mapBytes = response.Map;
                String mapName = ContentLibrary.CreateMapName(response.X, response.Y, response.Z, response.ZoomLevel);
                game.ContentLibrary.AddMap(mapName, mapBytes, true, true);
            });

            eventBus.Subscribe(ZoneMapsResponse.Descriptor.Name, (o) => {
                ZoneMapsResponse response = ZoneMapsResponse.Parser.ParseFrom((o as Any).Value);
                response.Maps.ToList().ForEach((map) =>
                {
                    ByteString mapBytes = map.Map;
                    String mapName = ContentLibrary.CreateMapName(map.X, map.Y, map.Z, map.ZoomLevel);
                    game.ContentLibrary.AddMap(mapName, mapBytes, true, true);
                    _hasLoadedWorld = true;
                });
            });
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

        private ZoneMapsRequest createZoneMapsRequest(int x, int y, int z, int zoomLevel)
        {
            ZoneMapsRequest zoneMapRequest = new ZoneMapsRequest
            {
                CenterX = x,
                CenterY = y,
                Z = z,
                Width = 3,
                Height = 3,
                ZoomLevel = zoomLevel
            };
            return zoneMapRequest;
        }

        private ZoneMapRequest createZoneMapRequest(int x, int y, int z, int zoomLevel)
        {
            ZoneMapRequest zoneMapRequest = new ZoneMapRequest
            {
                X = x,
                Y = y,
                Z = z,
                ZoomLevel = zoomLevel,
                MyVersion = 0
            };
            return zoneMapRequest;
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
            if (ks.IsKeyDown(Keys.Up))
                game.GameWorld.MovePlayer(0, -1);
            if (ks.IsKeyDown(Keys.Down))
                game.GameWorld.MovePlayer(0, 1);
            if (ks.IsKeyDown(Keys.Left))
                game.GameWorld.MovePlayer(-1, 0);
            if (ks.IsKeyDown(Keys.Right))
                game.GameWorld.MovePlayer(+1, 0);

            game.Canvas.scrollWheelValue(ms.ScrollWheelValue);
        }

        public override void Draw(GameTime gameTime, SpriteBatch sprite, float zoomLevel)
        {
            if (_hasLoadedWorld)
            {
                game.Canvas.Draw(game.GameWorld, gameTime, windowWidth, windowHeight);
            }
        }
    }
}
