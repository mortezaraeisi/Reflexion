using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace REFLEXION_LIB.Object.Tools.Buttons
{
    [Explanation("Button", "There is an action, Take over me", "REFLEXION_LIB.Object.Tools.Buttons.Button")]
    [Programmable(true)]
    [Serializable]
    public class Button : GameTool
    {
        protected string _text;
        [NonSerialized]
        private Int64 _handledId;

        public Button(string nameId) : this(nameId, "Push") { _border = Borders.None; }
        protected Button(string nameId, string text) : base(nameId) { _text = text; }

        internal override void BallHandling(Ball ball)
        {
            if (!_enabled || !_visibled || _handledId == ball.GetHandlingId()) return;
            _handledId = ball.GetHandlingId();
            base.executeBallEnterBlock();
        }
        public override void Drawn(System.Drawing.Graphics gr, System.Drawing.Point location, System.Drawing.Size size)
        {
            if (!_visibled) return;
            //string txt = _text;
            Font f = new Font("Courier New", 14f, FontStyle.Bold);
            SizeF s = gr.MeasureString(_text, f);
            while (s.Width > size.Width)
            {
                f = new Font(f.Name, f.Size - 0.5f, FontStyle.Bold);
                s = gr.MeasureString(_text, f);
            }

            PointF p = new PointF(location.X, location.Y + size.Height / 2);
            // p.X -= s.Width / 2;
            p.Y -= s.Height / 2;
            RectangleF r = new RectangleF(p, s);

            gr.FillEllipse(Brushes.Yellow, r);
            gr.DrawString(_text, f, Brushes.Red, p);
            base.Drawn(gr, location, size);
        }


        #region Programmable

        [Programmable]
        public void text(string value)
        {
            _text = value;
        }

        #endregion

    };
}
