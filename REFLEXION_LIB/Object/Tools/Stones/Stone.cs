using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace REFLEXION_LIB.Object.Tools.Stones
{
    [Explanation("Stone", "There is no way! Get back.", "REFLEXION_LIB.Object.Tools.Stones.Stone")]
    [Programmable(true)]
    [Serializable]
    public class Stone : GameTool
    {
        [NonSerialized]
        private Int64 _hndl;
        public Stone(string nameId) : base(nameId) { _hndl = 0; _border = Borders.All; }
       

        internal override void BallHandling(Ball ball)
        {
            if (_hndl == ball.GetHandlingId()) return;
            _hndl = ball.GetHandlingId();
            ball.SetDirection(ball.GetDirection().rReverse());
        }
        public override void Drawn(Graphics gr, Point location, Size size)
        {
            if (!_visibled) return;

            int XXX = size.Width / 5;
            int YYY = size.Height / 5;
            gr.FillRectangle(Brushes.Gray, new Rectangle(location, size));//back ground

            Color c1 = Color.Black, c2 = Color.Gray;
            Rectangle R = new Rectangle(location, size);
            R.Height = YYY;
            LinearGradientBrush L = new LinearGradientBrush(R, c1, c2, LinearGradientMode.Vertical);
            gr.FillRectangle(L, R);

            R.Y += size.Height - YYY;
            L.LinearColors = new Color[] { c2, c1 };
            gr.FillRectangle(L, R);

            R = new Rectangle(location, size);
            R.Width = XXX;
            R.Height -= XXX;
            L = new LinearGradientBrush(R, c1, c2, LinearGradientMode.Horizontal);
            gr.FillRectangle(L, R);

            R.X += size.Width - XXX;
            R.Y += YYY;
            L.LinearColors = new Color[] { c2, c1 };
            gr.FillRectangle(L, R);

            //base.Drawn(gr, location, size);
        }
    };
}