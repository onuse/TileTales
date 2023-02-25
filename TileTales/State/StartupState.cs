using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.Network;
using TileTales.Utils;

namespace TileTales.State
{
    internal class StartupState : BaseState
    {
        private readonly StateManager _stateManager;
        private ServerConnector _serverConnector;
        private readonly UI.AppUI _ui;

        public StartupState() : base()
        {
            _stateManager = stateManager;
            _serverConnector = serverConnector;
            _ui = ui;
        }

        public override void Enter()
        {
            UI.MainMenu menu = _ui.ShowStartMenu();

            menu.menuStartNewGame.Selected += (s, a) =>
            {
                stateManager.ChangeState(new ConnectState());
                eventBus.Publish(EventType.LoadGameUI, null);
            };


            menu.menuQuit.Selected += (s, a) =>
            {
                eventBus.Publish(EventType.Quit, null);
            };
        }
    }
}
