
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB.Object.Tools.Buttons
{
    [Explanation("Help Sign", "There is a help", "REFLEXION_LIB.Object.Tools.Buttons.HelpSign")]
    [Programmable(true)]
    [Serializable]
    public class HelpSign : Button
    {
        private string _message;
        [NonSerialized]
        private Int64 _handledId;

        public HelpSign(string nameId) : base(nameId, "sOs") { ; }

        internal override void BallHandling(Ball ball)
        {
            if (!_enabled || !_visibled || _handledId == ball.GetHandlingId()) return;
            _handledId = ball.GetHandlingId();
            base.BallHandling(ball);
            _owner.GetOwner().ShowMessage("Help", _message);
        }
        public void SetMessage(string msg) { _message = msg; }
        public string GetMessage() { return _message; }

        #region Programmable

        [Programmable]
        public void message(string value) { this.SetMessage(value); }

        #endregion
    };
}
