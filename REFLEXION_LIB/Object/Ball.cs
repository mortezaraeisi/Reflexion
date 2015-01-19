using System;
using System.Drawing;

namespace REFLEXION_LIB.Object
{
    [Explanation("Ball", "Move me to eat food", "REFLEXION_LIB.Object.Ball")]
    [Programmable]
    [Serializable]
    public class Ball : BaseObject
    {
        private Direction _direction;
        private int _stepOver;
        private Point _location;
        [NonSerialized]
        private BallLocChanged _locChanged;

        [NonSerialized]
        private Int64 _handlingId;
        [NonSerialized]
        private Object.BaseObject _currentObj;

        public Ball(string nameId)
            : base(nameId)
        {
            _direction = Direction.Right;
            _stepOver = 4;
        }

        internal Point Move()
        {
            if (_direction == Direction.Right)
            {
                _location.X += _stepOver;
            }
            else if (_direction == Direction.Left)
            {
                _location.X -= _stepOver;
            }
            else if (_direction == Direction.Up)
            {
                _location.Y -= _stepOver;
            }
            else if (_direction == Direction.Down)
            {
                _location.Y += _stepOver;
            }
            //else throw new Exception("Ball::Move, Unkown direction");//never happened

            this.changeLocTo(_location.rToLoc(_owner));
            return _location;
        }
        private void changeLocTo(Point loc)
        {
            if (this.CheckLoc(loc)) return;//not changed
            Point scrSize = _owner.GetScreenSize();

            if (loc.X >= scrSize.X) loc.X = 0;//boundary check
            if (loc.X < 0) loc.X = scrSize.X;
            if (loc.Y >= scrSize.Y) loc.Y = 0;
            if (loc.Y < 0) loc.Y = scrSize.Y;

            var oldPoint = _loc;// refresh ball real location! It's have to refresh every LocChange
            var sz = _owner.GetCellSize();
            _loc = loc;
            _location = _loc.rToLocation(_owner);
            {
                if (_direction == Direction.Left) _location.X += sz.Width - _stepOver;// this code fucked me
                if (_direction == Direction.Up) _location.Y += sz.Height - _stepOver;
            }

            if (_direction == Direction.Up || _direction == Direction.Down) _location.X += sz.Width / 2;
            else if (_direction == Direction.Right || _direction == Direction.Left) _location.Y += sz.Height / 2;

            var oldObj = _currentObj;
            _currentObj = _owner.Find(_loc, this);
            if (object.ReferenceEquals(_currentObj, this)) _currentObj = null;
            if (_currentObj != null) ++_handlingId;
            if (_currentObj == null && oldObj != null) ++_handlingId;//ball leave and add h

            if (_locChanged != null) _locChanged(_owner, new BallLocChangedEventArgs(this, oldPoint, _loc, _handlingId));
        }
        public override void Drawn(System.Drawing.Graphics gr, System.Drawing.Point location, System.Drawing.Size size)
        {
            Point p = _location;
            Size sz = new Size(size.Width / 2, size.Height / 2);
            p.X -= sz.Width / 2;
            p.Y -= sz.Height / 2;
            gr.FillEllipse(Brushes.Red, new Rectangle(p, sz));
        }
        public override void SetOwner(Page pg)
        {
            _owner = pg;
            _location = _loc.rToLocation(_owner);
        }
        internal override void BallHandling(Ball ball)
        {
            //another ball get here,, What's I most to do?
            // let's!!! I'm thinking... ;-)
        }

        #region Properties

        internal Point getHeadLocation() { return _location; }
        internal void SetDirection(Direction dir) { _direction = dir; }
        public Direction GetDirection() { return _direction; }
        internal void SetStepOver(int step)
        {
            if (step < 1 || step > 8)
                throw new ArgumentOutOfRangeException("Ball.Step: Out of rang. [1-8]");
            _stepOver = step;
        }
        public int GetStepOver() { return _stepOver; }
        internal Int64 GetHandlingId() { return _handlingId; }
        internal Object.BaseObject GetCurrentObj() { return _currentObj; }
        public override void SetLoc(Point value)
        {
            if (_owner == null)
                _loc = value;
            else
            {
                this.changeLocTo(value);
            }
        }

        public event BallLocChanged LocChangedEvent { add { _locChanged += value; } remove { _locChanged -= value; } }
        #endregion

        #region Programming

        [Programmable]
        public void direction(string value)
        {
            _direction = (Direction)Enum.Parse(typeof(Direction), value, true);
        }

        [Programmable]
        public void step(string value) { this.SetStepOver(int.Parse(value)); }

        #endregion
    };
}
