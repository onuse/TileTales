using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;
using TileTales.Utils;

namespace TileTales.State
{
    internal class WorldGenState : BaseState
    {
        private readonly StateManager _stateManager;
        private readonly UI.AppUI _ui;
        public WorldGenState() : base()
        {
            _stateManager = stateManager;
            _ui = ui;
        }
        public override void Enter()
        {

        }

        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {

        }

        public override void Draw(GameTime gameTime)
        {
            
        }
    }
}
