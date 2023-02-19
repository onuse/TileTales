using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
using Net.Tiletales.Network.Proto.App;
using Net.Tiletales.Network.Proto.Game;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;
using TileTales.Graphics;
using TileTales.UI;
using TileTales.Utils;

namespace TileTales.State
{
    internal class ConnectState : BaseState
    {
        private GameUI _gameUI;
        private int windowWidth;
        private int windowHeight;
        public ConnectState(TileTalesGame game) : base(game)
        {
            eventBus.Subscribe(EventType.GameUILoaded, (o) =>
            {
                _gameUI = ui._gameUI;
                _gameUI.paddedCenteredButton.Click += (s, a) =>
                {
                    windowWidth = SettingsReader.Instance.GetSettings().WindowWidth;
                    windowHeight = SettingsReader.Instance.GetSettings().WindowHeight;
                    Login();
                };
            });

            /* eventBus.Subscribe(AccountLoginResponse.Descriptor.Name, (o) => {
                 AccountLoginResponse response = AccountLoginResponse.Parser.ParseFrom((o as Any).Value);
                 if (response.Success)
                 {
                     AllTilesRequest allTilesRequest = rf.CreateAllTilesRequest();
                     serverConnector.SendMessage(allTilesRequest);
                 }
             });*/

            eventBus.Subscribe(RealmInfo.Descriptor, (o) =>
            {
                System.Diagnostics.Debug.WriteLine("ConnectState(RealmInfo)");
                RealmInfo data = RealmInfo.Parser.ParseFrom((o as Any).Value);
                game.InitGameSettings(data);
            });

            eventBus.Subscribe(PlayerObjectInfo.Descriptor, (o) => {
                System.Diagnostics.Debug.WriteLine("ConnectState(PlayerObjectInfo)");
                PlayerObjectInfo data = PlayerObjectInfo.Parser.ParseFrom((o as Any).Value);
                game.GameWorld.createPlayerObject(data);
                AllTilesRequest allTilesRequest = rf.CreateAllTilesRequest();
                // System.Diagnostics.Debug.WriteLine("eventBus.Subscribe(PlayerObjectInfo");
                serverConnector.SendMessage(allTilesRequest);
            });

            eventBus.Subscribe(AllTilesData.Descriptor, (o) => {
                System.Diagnostics.Debug.WriteLine("ConnectState(AllTilesData)");
                AllTilesData data = AllTilesData.Parser.ParseFrom((o as Any).Value);
                data.Tiles.ToList().ForEach(tile => AddTile(tile));
                content.CreateWaterChunk();
                Player p = game.GameWorld.GetPlayer();
                System.Diagnostics.Debug.WriteLine("ConnectState(AllTilesData) player: " + p);
                stateManager.ChangeState(new GameState(game));
                CenterMapsRequest zoneMapsRequest = rf.CreateZoneMapsRequest(p.X, p.Y, p.Z, 0);
                serverConnector.SendMessage(zoneMapsRequest);
            });
        }

        private void AddTile(TileData tileData)
        {
            Tile tile = new Tile(tileData.ReplacementColor);
            tile.LegacyColor = tileData.LegacyColor;
            tile.Tags = tileData.Tags.ToList();
            tile.Image = SKBitmap.Decode(tileData.Image.ToByteArray());
            content.AddTile(tile);
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
            eventBus.Publish(EventType.Connect, null);
        }
    }
}
