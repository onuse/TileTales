using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
using Net.Tiletales.Network.Proto.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

            eventBus.Subscribe(AccountLoginResponse.Descriptor.Name, (o) => {
                AccountLoginResponse response = AccountLoginResponse.Parser.ParseFrom((o as Any).Value);
                //ui.ShowPopupMessage("Login response", "Login successful: " + response.Success);
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
                Any msg = Any.Pack(loginRequest);
                serverConnector.SendMessage(msg);
            }
        }

        public override void Enter()
        {
            eventBus.Publish("connect", null);
        }
    }
}
