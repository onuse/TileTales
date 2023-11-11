using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework.Graphics;
using Net.Tiletales.Network.Proto.App;
using Net.Tiletales.Network.Proto.Game;
using SkiaSharp;
using System.Linq;
using System.Threading;
using TileTales.GameContent;
using TileTales.Graphics;
using TileTales.Network;
using TileTales.UI;
using TileTales.Utils;

namespace TileTales.State {
    internal class ConnectState: BaseState {
        private readonly GameUI _gameUI;
        internal ConnectState() : base() {
            eventBus.Subscribe(EventType.Connected, (o) => {
                /*_gameUI = ui._gameUI;
                _gameUI.paddedCenteredButton.Click += (s, a) =>
                {*/
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

            eventBus.Subscribe(RealmInfo.Descriptor, (o) => {
                Log.Debug("(RealmInfo)");
                RealmInfo data = RealmInfo.Parser.ParseFrom((o as Any).Value);
                game.InitGameSettings(data);
            });

            eventBus.Subscribe(PlayerObjectInfo.Descriptor, (o) => {
                Log.Debug("(PlayerObjectInfo)");
                PlayerObjectInfo data = PlayerObjectInfo.Parser.ParseFrom((o as Any).Value);
                game.GameWorld.createPlayerObject(data);
                AllTilesRequest allTilesRequest = RequestFactory.CreateAllTilesRequest();
                // System.Diagnostics.Debug.WriteLine("eventBus.Subscribe(PlayerObjectInfo");
                serverConnector.SendMessage(allTilesRequest);
            });

            eventBus.Subscribe(AllTilesData.Descriptor, (o) => {
                Log.Debug("(AllTilesData)");
                AllTilesData data = AllTilesData.Parser.ParseFrom((o as Any).Value);
                data.Tiles.ToList().ForEach(tile => AddTile(tile));
                //ContentLibrary.CreateWaterChunk();
                eventBus.Publish(EventType.AllTilesSaved, null);
                Player p = game.GameWorld.Player;
                Log.Debug("(AllTilesData) player: " + p);
                stateManager.ChangeState(RunningState.Singleton);
                CenterMapsRequest zoneMapsRequest = RequestFactory.CreateZoneMapsRequest(p.X, p.Y, p.Z, 0, 2);
                serverConnector.SendMessage(zoneMapsRequest);
                new Thread(RunningState.Singleton.SendDelayedMapsRequest).Start();
            });
        }

        private void AddTile(TileData tileData) {
            Tile tile = new(tileData.ReplacementColor) {
                Name = tileData.Name,
                Description = tileData.Description,
                LegacyColor = tileData.LegacyColor,
                Tags = tileData.Tags.ToList()
            };
            byte[] byteArray = tileData.Image.ToByteArray();
            tile.BackingImage = SKBitmap.Decode(byteArray);
            tile.Image = Texture2D.FromStream(game.GraphicsDevice, tile.BackingImage.Encode(SKEncodedImageFormat.Png, 100).AsStream());
            content.AddTile(tile);
        }


        private void Login() {
            if (serverConnector.IsConnected()) {
                UserSettings settings = SettingsReader.Singleton.GetSettings();
                AccountLoginRequest loginRequest = new AccountLoginRequest {
                    Username = settings.AccountUsername,
                    Password = settings.AccountPassword
                };
                serverConnector.SendMessage(loginRequest);
            }
        }

        public override void Enter() {
            eventBus.Publish(EventType.Connect, null);
        }
    }
}
