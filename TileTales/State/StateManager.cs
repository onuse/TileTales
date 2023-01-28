using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Timers;
using TileTales.Utils;

namespace TileTales.State
{
    internal class StateManager
    {
        private static StateManager _instance;
        public static StateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StateManager();
                }
                return _instance;
            }
        }

        private readonly Stack<BaseState> _states = new Stack<BaseState>();

        private StateManager()
        {
        }

        public BaseState GetCurrentState()
        {
            return _states.Peek();
        }

        public void PushState(BaseState state)
        {
            _states.Push(state);
            state.Enter();
        }

        public void PopState()
        {
            _states.Pop();
        }

        public void ChangeState(BaseState state)
        {
            PopState();
            PushState(state);
        }
        public void Update(GameTime gameTime, KeyboardState keyboardState, MouseState mouseState)
        {
            if (_states.Count == 0)
            {
                return;
            }
            _states.Peek().Update(gameTime, keyboardState, mouseState);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_states.Count == 0)
            {
                return;
            }
            _states.Peek().Draw(gameTime, spriteBatch);
        }
    }
}
