using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB.Object.Tools.Buttons
{
    [Explanation("End Point", "Page will ended here", "REFLEXION_LIB.Object.Tools.Buttons.EndPoint")]
    [Programmable(true)]
    [Serializable]
    public class EndPoint:Button
    {
        [NonSerialized]
        private Int64 _handledId;

        public EndPoint(string nameId) : base(nameId, "(E)") { ;}

        internal override void BallHandling(Ball ball)
        {
            if (!_enabled || !_visibled || _handledId == ball.GetHandlingId()) return;
            _handledId = ball.GetHandlingId();

            _owner.endPointEntered(this);
        }
    };
}
