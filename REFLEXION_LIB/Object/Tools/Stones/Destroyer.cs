using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace REFLEXION_LIB.Object.Tools.Stones
{
    [Explanation("Destroyer Stone", "This is the hell!", "REFLEXION_LIB.Object.Tools.Stones.Destroyer")]
    [Programmable]
    [Serializable]
    public sealed class Destroyer : Stone
    {
        public Destroyer(string nameId) : base(nameId) { _border = Borders.All; }

        internal override void BallHandling(Ball ball)
        {
            //Direction blDirection = ball.GetDirection();
            //Borders blAsBorder = blDirection.rReverse().rToBorder();

            //if ((blAsBorder & _border) == Borders.None)
            //{
            //    base.BallHandling(ball);
            //    return;
            //}
            if (base.ballEnterCheckForBorder(ball.GetDirection()))
                _owner.GetOwner().Terminate();
            else
                base.BallHandling(ball);
        }

        public override void Drawn(Graphics gr, Point location, Size size)
        {
            if (!_visibled) return;

            float space1 = size.Width / 5;
            float space2 = size.Height / 5;
            {
                RectangleF r = new RectangleF(location.X + space1, location.Y + space2, size.Width - (space1 * 2f), size.Height - (space2 * 2f));
                LinearGradientBrush l = new LinearGradientBrush(r, Color.Red, Color.Yellow, 45f);
                gr.FillRectangle(l, r);
            }
            if ((_border & Borders.Top) == Borders.Top)
            {
                float last = location.X;
                for (float i = location.X + space1; i <= location.X + size.Width; i += space1)
                {
                    gr.FillPolygon(Brushes.Red, new PointF[] {
                        new PointF(last+space1/2,location.Y),
                        new PointF(last,location.Y+space1),
                        new PointF(i,location.Y+space1)
                    });
                    last = i;
                }//next i
            }
            if ((_border & Borders.Bottom) == Borders.Bottom)
            {
                float last = location.X;
                for (float i = location.X + space1; i <= location.X + size.Width; i += space1)
                {
                    gr.FillPolygon(Brushes.Red, new PointF[] {
                        new PointF(last+space1/2,location.Y+size.Height),
                        new PointF(last,location.Y+size.Height-space1),
                        new PointF(i,location.Y+size.Height-space1)
                    });
                    last = i;
                }//next i
            }
            if ((_border & Borders.Left) == Borders.Left)
            {
                float last = location.Y;
                for (float i = location.Y + space2; i <= location.Y + size.Height; i += space2)
                {
                    gr.FillPolygon(Brushes.Red, new PointF[] {
                        new PointF(location.X+space2, last),
                        new PointF(location.X,i-space2/2),
                        new PointF(location.X+space2,i)
                    });
                    last = i;
                }//next i
            }
            if ((_border & Borders.Right) == Borders.Right)
            {
                float last = location.Y;
                for (float i = location.Y + space2; i <= location.Y + size.Height; i += space2)
                {
                    gr.FillPolygon(Brushes.Red, new PointF[] {
                        new PointF(location.X+size.Width-space2, last),
                        new PointF(location.X+size.Width,i-space2/2),
                        new PointF(location.X+size.Width-space2,i)
                    });
                    last = i;
                }//next i
            }
        }
    };
}
