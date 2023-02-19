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

        public StartupState(TileTalesGame game) : base(game)
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
                eventBus.Publish(EventType.LoadGameUI, null);
                stateManager.ChangeState(new ConnectState(game));
            };


            menu.menuQuit.Selected += (s, a) =>
            {
                eventBus.Publish(EventType.Quit, null);
            };
        }
    }
}
