using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended.Timers;

namespace TileTales.GameContent
{
    internal class StateManager
    {
        private readonly Stack<BaseState> _states = new Stack<BaseState>();

        public void PushState(BaseState state)
        {
            _states.Push(state);
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

        internal void Activate()
        {
            if (_states.Count == 0)
            {
                return;
            }
            _states.Peek().Activate();
        }
    }
}
