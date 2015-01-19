using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace REFLEXION_LIB.Object.Tools.Stones
{
    [Explanation("Moveable Stone", "There is no way! I maybe moved in next time.", "REFLEXION_LIB.Object.Tools.Stones.Moveable")]
    [Programmable(true)]
    [Serializable]
    public class Moveable : Stone
    {
        [NonSerialized]
        private Int64 _hndl;
        public Moveable(string nameId) : base(nameId) { ;}

        internal override void BallHandling(Ball ball)
        {
            if (_hndl == ball.GetHandlingId()) return;
            _hndl = ball.GetHandlingId();

            Direction blDirection = ball.GetDirection();
            //Borders blAsBorder = blDirection.rReverse().rToBorder();
            //if ((blAsBorder & _border) == Borders.None)
            //{
            //    base.BallHandling(ball);
            //    return;
            //}
            if (base.ballEnterCheckForBorder(blDirection))
                this.Move(blDirection);
            base.BallHandling(ball);
        }
     
        public override void Drawn(System.Drawing.Graphics gr, System.Drawing.Point location, System.Drawing.Size size)
        {
            if (!_visibled) return;
            float hor, ver;
            hor = size.Width / 4 - 1;
            ver = size.Height / 4 - 1;
            Point[] _LOCATIONS = new Point[4];
            _LOCATIONS[0] = location;
            _LOCATIONS[1] = new Point(location.X + size.Width, location.Y);
            _LOCATIONS[2] = new Point(_LOCATIONS[1].X, location.Y + size.Height);
            _LOCATIONS[3] = new Point(location.X, _LOCATIONS[2].Y);

            Rectangle r = new Rectangle((int)location.X + 1, (int)location.Y + 1, (int)size.Width - 2, (int)size.Height - 2);
            gr.DrawRectangle(Pens.Gray, r);

            Brush b = new LinearGradientBrush(new RectangleF(location, size)
                , Color.Gray, Color.Goldenrod, LinearGradientMode.BackwardDiagonal);
            gr.FillRectangle(b, r);

            b = new LinearGradientBrush(new RectangleF(location, size)
               , Color.Black, Color.Red, LinearGradientMode.BackwardDiagonal);
            PointF[] pnts = new PointF[3];
            if ((_border & Borders.Bottom) == Borders.Bottom)
            {
                pnts[0] = new PointF((_LOCATIONS[0].X + _LOCATIONS[1].X) / 2, _LOCATIONS[3].Y - 2);
                pnts[1] = new PointF(((_LOCATIONS[0].X + _LOCATIONS[1].X) / 2) + hor, _LOCATIONS[3].Y - ver);
                pnts[2] = new PointF(((_LOCATIONS[0].X + _LOCATIONS[1].X) / 2) - hor, _LOCATIONS[3].Y - ver);
                gr.FillPolygon(b, pnts);
            }
            if ((_border & Borders.Top) == Borders.Top)
            {
                pnts[0] = new PointF((_LOCATIONS[0].X + _LOCATIONS[1].X) / 2, _LOCATIONS[0].Y + 2);
                pnts[1] = new PointF(((_LOCATIONS[0].X + _LOCATIONS[1].X) / 2) + hor, _LOCATIONS[0].Y + ver);
                pnts[2] = new PointF(((_LOCATIONS[0].X + _LOCATIONS[1].X) / 2) - hor, _LOCATIONS[0].Y + ver);
                gr.FillPolygon(b, pnts);
            }
            if ((_border & Borders.Left) == Borders.Left)
            {
                pnts[0] = new PointF(_LOCATIONS[0].X + 2, (_LOCATIONS[0].Y + _LOCATIONS[3].Y) / 2);
                pnts[1] = new PointF(_LOCATIONS[0].X + hor, ((_LOCATIONS[0].Y + _LOCATIONS[3].Y) / 2) + ver);
                pnts[2] = new PointF(_LOCATIONS[0].X + hor, ((_LOCATIONS[0].Y + _LOCATIONS[3].Y) / 2) - ver);
                gr.FillPolygon(b, pnts);
            }
            if ((_border & Borders.Right) == Borders.Right)
            {
                pnts[0] = new PointF(_LOCATIONS[1].X - 2, (_LOCATIONS[0].Y + _LOCATIONS[3].Y) / 2);
                pnts[1] = new PointF(_LOCATIONS[1].X - hor, ((_LOCATIONS[0].Y + _LOCATIONS[3].Y) / 2) + ver);
                pnts[2] = new PointF(_LOCATIONS[1].X - hor, ((_LOCATIONS[0].Y + _LOCATIONS[3].Y) / 2) - ver);
                gr.FillPolygon(b, pnts);
            }
            // base.Drawn(gr, location, size);
        }
        public void Move(Direction dir)
        {
            Point locMovedOn = _loc;

            if (dir == Direction.Right)
                locMovedOn.X++;
            else if (dir == Direction.Left)
                locMovedOn.X--;
            else if (dir == Direction.Up)
                locMovedOn.Y--;
            else //down
                locMovedOn.Y++;

            if (_owner.IsLocationValid(locMovedOn) && !_owner.IsLocationBusy(locMovedOn))
            {
                this.SetLoc(locMovedOn);
            }
        }

        [Programmable]
        public void move(string value) { this.Move((Direction)Enum.Parse(typeof(Direction), value, true)); }

    };
}
