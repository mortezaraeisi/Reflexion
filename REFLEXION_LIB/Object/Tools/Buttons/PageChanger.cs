using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB.Object.Tools.Buttons
{
    [Explanation("Page Changer", "Let's go to next page", "REFLEXION_LIB.Object.Tools.Buttons.PageChanger")]
    [Programmable(true)]
    [Serializable]
    public class PageChanger : Button
    {
        private string _targetPageNameId;
        [NonSerialized]
        private Int64 _handledId;
        public PageChanger(string nameId) : base(nameId, "{N}") { ; }

        internal override void BallHandling(Ball ball)
        {
            if (!_enabled || !_visibled || _handledId == ball.GetHandlingId()) return;
            _handledId = ball.GetHandlingId();
            if (!string.IsNullOrWhiteSpace(_targetPageNameId))
                _owner.GetOwner().ChangePage(_targetPageNameId);
            base.executeBallEnterBlock();
        }
        public void SetTarget(string id) { _targetPageNameId = id; }
        public string GetTarget() { return _targetPageNameId; }

        #region Programmable

        [Programmable]
        public void target(string id) { this.SetTarget(id); }

        #endregion
    };
}
