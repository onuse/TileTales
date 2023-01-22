using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.Network;

namespace TileTales.GameContent
{
    internal class StartupState : BaseState
    {
        private readonly StateManager _stateManager;
        private ServerConnector _serverConnector;
        private readonly UI.AppUI _ui;

        public StartupState(StateManager stateManager, ServerConnector serverConnector, UI.AppUI ui, TileTalesGame game) : base(stateManager, serverConnector, ui, game)
        {
            this._stateManager = stateManager;
            this._serverConnector = serverConnector;
            this._ui = ui;
        }

        public override void Enter()
        {
            UI.MainMenu menu = _ui.ShowStartMenu();

            menu.menuStartNewGame.Selected += (s, a) =>
            {
                eventBus.Publish("loadgame", null);
                stateManager.ChangeState(new GameState(stateManager, serverConnector, _ui, game));
            };


            menu.menuQuit.Selected += (s, a) =>
            {
                eventBus.Publish("quit", null);
            };
        }
    }
}
