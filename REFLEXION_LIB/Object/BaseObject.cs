using System;
using System.Drawing;

namespace REFLEXION_LIB.Object
{
    [Explanation("Base Object", "Base object", "REFLEXION_LIB.Object.BaseObject")]
    [Serializable]
    public abstract class BaseObject
    {
        #region Fields

        protected Page _owner;
        protected string _nameID;
        protected Point _loc;
        protected bool _enabled;
        protected bool _visibled;
        protected Borders _border;
        protected Programming.ProgramBlock _ballEnterBlock;
        #endregion

        #region Constructor

        public BaseObject(string nameId)
        {
            this.SetNameId(nameId);
            _enabled =
            _visibled = true;
            _border = Borders.None;
            _loc = new Point(0, 0);
        }
        #endregion

        #region abstracts

        public abstract void Drawn(Graphics gr, Point location, Size size);

        internal abstract void BallHandling(Ball ball);
        #endregion

        #region Controls

        internal virtual bool CheckLoc(Point loc)
        {
            return _loc.X == loc.X && _loc.Y == loc.Y;
        }
        #endregion

        #region Properties

        public void SetNameId(string value) { if (Policy.CheckObjectNameSyntax(value)) _nameID = value; }
        public string GetNameId() { return _nameID; }
        public virtual void SetLoc(Point value) { _loc = value; _dlLocChanged.RaiseEvent(this, new StateChangedArgs(this)); }
        public Point GetLoc() { return _loc; }
        public void SetEnabled(bool value) { _enabled = value; _dlEnabledChanged.RaiseEvent(this, new StateChangedArgs(this)); }
        public bool GetEnabled() { return _enabled; }
        public void SetVisibled(bool value) { _visibled = value; _dlVisibledChanged.RaiseEvent(this, new StateChangedArgs(this)); }
        public bool GetVisibled() { return _visibled; }

        public void SetBorder(Borders value) { _border = value; _dlBorderChanged.RaiseEvent(this, new StateChangedArgs(this)); }
        public Borders GetBorder() { return _border; }

        public virtual void SetOwner(Page pg) { _owner = pg; }
        public Page GetOwner() { return _owner; }

        public void SetBallEnterBlock(Programming.ProgramBlock block)
        {
            _ballEnterBlock = block;
            _ballEnterBlock.SetOwnerPage(_owner);
        }
        public Programming.ProgramBlock GetBallEnterBlock() { return _ballEnterBlock; }
        #endregion

        #region Event Handlers

        private StateChanged _dlLocChanged, _dlEnabledChanged, _dlVisibledChanged, _dlBorderChanged;
        public event StateChanged LocationChanged { add { _dlLocChanged += value; } remove { _dlLocChanged -= value; } }
        public event StateChanged EnabledChanged { add { _dlEnabledChanged += value; } remove { _dlEnabledChanged -= value; } }
        public event StateChanged VisibledChanged { add { _dlVisibledChanged += value; } remove { _dlVisibledChanged -= value; } }
        public event StateChanged BorderChanged { add { _dlBorderChanged += value; } remove { _dlBorderChanged -= value; } }

        #endregion

        #region Programmable

        [Programmable]
        public void loc(string value)
        {
            string[] split = value.Split('-');
            if (split.Length != 2)
                throw new InvalidCastException("Cannot convert to loc, invalid format. try this way 'x-y'");
            this.SetLoc(new Point(Int32.Parse(split[0]), Int32.Parse(split[1])));
        }
        [Programmable]
        public void enabled(string value) { this.SetEnabled(Convert.ToBoolean(value)); }
        [Programmable]
        public void visibled(string value) { this.SetVisibled(Convert.ToBoolean(value)); }
        [Programmable]
        public void border(string value) { this.SetBorder((Borders)Enum.Parse(typeof(Borders), value, true)); }

        #endregion
    };
}
