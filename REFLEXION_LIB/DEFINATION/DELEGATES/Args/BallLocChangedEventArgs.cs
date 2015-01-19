using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace REFLEXION_LIB
{
    public sealed class BallLocChangedEventArgs : System.EventArgs
    {
        private readonly Object.Ball _ball;
        private readonly Point _oldLoc, _newLoc;
        private readonly Int64 _handlingId;

        internal BallLocChangedEventArgs(Object.Ball ball, Point oldLoc, Point newLoc, Int64 handlingId)
        {
            _ball = ball;
            _oldLoc = oldLoc;
            _newLoc = newLoc;
            _handlingId = handlingId;
        }
        public Object.Ball Ball { get { return _ball; } }
        public Point OldLoc { get { return _oldLoc; } }
        public Point NewLoc { get { return _newLoc; } }
        public Int64 HandlingId { get { return _handlingId; } }
    };
}
