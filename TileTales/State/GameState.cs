using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
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
        public GameState(StateManager stateManager, ServerConnector serverConnector, AppUI ui, TileTalesGame game) : base(stateManager, serverConnector, ui, game)
        {
            _gameUI = ui._gameUI;
            _gameUI.paddedCenteredButton.Click += (s, a) =>
            {
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
                game.ContentLibrary.AddTexture(mapName, mapBytes, true);
            });

            eventBus.Subscribe(ZoneMapsResponse.Descriptor.Name, (o) => {
                ZoneMapsResponse response = ZoneMapsResponse.Parser.ParseFrom((o as Any).Value);
                response.Maps.ToList().ForEach((map) =>
                {
                    ByteString mapBytes = map.Map;
                    String mapName = ContentLibrary.CreateMapName(map.X, map.Y, map.Z, map.ZoomLevel);
                    game.ContentLibrary.AddTexture(mapName, mapBytes, true);
                });
            });
        }
        
        private void Login()
        {
            if (serverConnector.isConnected())
            {
                Settings settings = SettingsReader.Instance.GetSettings();
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
    }
}
