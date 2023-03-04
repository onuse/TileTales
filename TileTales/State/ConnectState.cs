using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public ConnectState() : base()
        {
            eventBus.Subscribe(EventType.Connected, (o) =>
            {
                /*_gameUI = ui._gameUI;
                _gameUI.paddedCenteredButton.Click += (s, a) =>
                {*/
                    windowWidth = SettingsReader.Instance.GetSettings().WindowWidth;
                    windowHeight = SettingsReader.Instance.GetSettings().WindowHeight;
                    Login();
                //};
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
                eventBus.Publish(EventType.AllTilesSaved, null);
                Player p = game.GameWorld.GetPlayer();
                System.Diagnostics.Debug.WriteLine("ConnectState(AllTilesData) player: " + p);
                stateManager.ChangeState(RunningState.Instance);
                CenterMapsRequest zoneMapsRequest = rf.CreateZoneMapsRequest(p.X, p.Y, p.Z, 0, 4);
                serverConnector.SendMessage(zoneMapsRequest);
                zoneMapsRequest = rf.CreateZoneMapsRequest(p.X, p.Y, p.Z, 0, 32);
                serverConnector.SendMessage(zoneMapsRequest);
            });
        }

        private void AddTile(TileData tileData)
        {
            Tile tile = new Tile(tileData.ReplacementColor);
            tile.Name = tileData.Name;
            tile.Description = tileData.Description;
            tile.LegacyColor = tileData.LegacyColor;
            tile.Tags = tileData.Tags.ToList();
            byte[] byteArray = tileData.Image.ToByteArray();
            tile.BackingImage = SKBitmap.Decode(byteArray);
            tile.Image =  Texture2D.FromStream(game.GraphicsDevice, tile.BackingImage.Encode(SKEncodedImageFormat.Png, 100).AsStream());
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
