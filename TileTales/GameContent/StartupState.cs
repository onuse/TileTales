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

        public StartupState(StateManager stateManager) : base(stateManager)
        {
            this._stateManager = stateManager;
        }

        public StartupState(StateManager stateManager, ServerConnector serverConnector, UI.AppUI ui) : this(stateManager)
        {
            this._serverConnector = serverConnector;
            this._ui = ui;
        }

        public override void Activate()
        {
            Exception errorMessage = _serverConnector.connectToServer();
            if (errorMessage != null)
            {
                _ui.popConnectErrorUI(errorMessage.Message);
            }
        }
    }
}
