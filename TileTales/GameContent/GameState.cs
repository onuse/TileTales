using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
using Net.Tiletales.Network.Protomsg.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.Network;
using TileTales.UI;
using TileTales.Utils;

namespace TileTales.GameContent
{
    internal class GameState : BaseState
    {
        private readonly GameUI _gameUI;
        public GameState(StateManager stateManager, ServerConnector serverConnector, UI.AppUI ui, TileTalesGame game) : base(stateManager, serverConnector, ui, game)
        {
            _gameUI = ui._gameUI;
            _gameUI.paddedCenteredButton.Click += (s, a) =>
            {
                if (serverConnector.isConnected())
                {
                    AccountLoginRequest loginRequest = new AccountLoginRequest
                    {
                        Username = "admin",
                        Password = "admin"
                    };
                    Any msg = Any.Pack(loginRequest);
                    serverConnector.SendMessage(msg);
                    /*
                     * Any anyMessage = new Any {Value = loginRequest.ToByteString()};
_                       socketClient.SendMessage(anyMessage);
                     * */
                    //serverConnector.SendMessageBytes(loginRequest.ToByteArray());
                }
            };
        }

        public override void Enter()
        {
            eventBus.Publish("connect", null);
        }
    }
}
