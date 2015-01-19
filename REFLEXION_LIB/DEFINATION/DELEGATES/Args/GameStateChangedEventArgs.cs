using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    public sealed class GameStateChangedEventArgs : System.EventArgs
    {
        private readonly GameStates _oldState, _newState;

        internal GameStateChangedEventArgs(GameStates oldState, GameStates newState)
        {
            _oldState = oldState;
            _newState = newState;
        }
        public GameStates OldState { get { return _oldState; } }
        public GameStates NewState { get { return _newState; } }
    };
}
