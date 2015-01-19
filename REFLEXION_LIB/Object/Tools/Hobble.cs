using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;
using REFLEXION_LIB;

namespace REFLEXION_LIB.Object.Tools
{
    [Explanation("Hobble", "Change ball's direction", "REFLEXION_LIB.Object.Tools.Hobble")]
    [Programmable]
    [Serializable]
    public sealed class Hobble : GameTool
    {
        private Direction _direction;
        [NonSerialized]
        private Int64 _handledId;

        public Hobble(string nameId)
            : base(nameId)
        {
            _direction = Direction.Right;
            _handledId = 0;
        }

        internal override void BallHandling(Ball ball)
        {
            if (ball.GetHandlingId() == _handledId) return;

            if(base.ballEnterCheckForBorder(ball.GetDirection()))
            {
                ball.SetDirection(ball.GetDirection().rReverse());
                return;
            }

            var sz = _owner.GetCellSize(); sz = new Size(sz.Width / 2, sz.Height / 2);
            var meLocation = _loc.rToLocation(_owner); meLocation.X += sz.Width; meLocation.Y += sz.Height;
            var bLocation = ball.getHeadLocation();
            var blDir = ball.GetDirection();

            #region Horizontal ball direction handling

            if (blDir == Direction.Right)//  ->[\] ->[/]
            {
                if (bLocation.X >= meLocation.X)
                {
                    _handledId = ball.GetHandlingId();
                    if (this.isPrimary())// ->[/]
                        ball.SetDirection(Direction.Up);
                    else ball.SetDirection(Direction.Down);// ->[\]
                    base.executeBallEnterBlock();
                }
            }
            else if (blDir == Direction.Left)// [\]<-  [/]<-
            {
                if (bLocation.X <= meLocation.X)
                {
                    _handledId = ball.GetHandlingId();
                    if (this.isPrimary())// [/]<-
                        ball.SetDirection(Direction.Down);
                    else ball.SetDirection(Direction.Up);// [\]<-
                    base.executeBallEnterBlock();
                }

            }
            #endregion

            #region Vertical ball direction handling

            else if (blDir == Direction.Up)//  <=[\] [/]=>
            {
                if (bLocation.Y <= meLocation.Y)
                {
                    _handledId = ball.GetHandlingId();
                    //if (_direction == Direction.Right || _direction == Direction.Up)// [/]=>
                    if (this.isPrimary())// [/]=>
                        ball.SetDirection(Direction.Right);
                    else ball.SetDirection(Direction.Left);//  <=[\]
                    base.executeBallEnterBlock();
                }
            }
            else if (blDir == Direction.Down)// [\]<-  [/]<-
            {
                if (bLocation.Y >= meLocation.Y)
                {
                    _handledId = ball.GetHandlingId();
                    if (this.isPrimary())// <=[/]
                        ball.SetDirection(Direction.Left);
                    else ball.SetDirection(Direction.Right);// [\]=>
                    base.executeBallEnterBlock();
                }

            }
            #endregion
        }
        public override void Drawn(System.Drawing.Graphics gr, System.Drawing.Point location, System.Drawing.Size size)
        {
            if (!_visibled) return;

            LinearGradientBrush L;
            if (_enabled)
                L = new LinearGradientBrush(new Rectangle(location, size), _enabled ?
                    Color.Black : Color.Red, _enabled ? Color.Red : Color.Black, LinearGradientMode.Vertical);
            else
                L = new LinearGradientBrush(new Rectangle(location, size), _enabled ?
                    Color.Gray : Color.Red, _enabled ? Color.Yellow : Color.Orange, LinearGradientMode.Vertical);

            int wIDTH = size.Width / 4;
            int spacer = wIDTH / 3;
            Pen p = new Pen(L, wIDTH);
            if (this.isPrimary())// [/]
                gr.DrawLine(p, new Point(location.X + size.Width - spacer, location.Y + spacer), new Point(location.X + spacer, location.Y + size.Height - spacer));
            else
                gr.DrawLine(p, new Point(location.X + spacer, location.Y + spacer), new Point(location.X + size.Width - spacer, location.Y + size.Height - spacer));

            base.Drawn(gr, location, size);
        }
        private bool isPrimary() { return _direction == Direction.Right || _direction == Direction.Up; }
        public override void MouseClicked()
        {
            if (!_enabled || !_visibled) return;
            _direction = this.isPrimary() ? Direction.Left : Direction.Right;
        }

        #region Programmable

        [Programmable]
        public void change(string value)
        {
            _direction = (Direction)Enum.Parse(typeof(Direction), value, true);
        }
        #endregion
    };
}
