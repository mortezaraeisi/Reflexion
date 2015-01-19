using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    public class StateChangedArgs : System.EventArgs
    {
        private readonly Object.BaseObject _object;
        public StateChangedArgs(Object.BaseObject obj)
        {
            _object = obj;
        }
        public Object.BaseObject Object { get { return _object; } } 

    }
}
