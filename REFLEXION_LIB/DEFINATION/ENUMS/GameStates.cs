using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    public enum GameStates : int
    {
        Initialized = 0,
        Playing = 1,
        Paused = 2,
        Terminated = 3,
        Won=4
    };
}
