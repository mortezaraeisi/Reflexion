using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace REFLEXION_LIB.Object.Tools.Foods
{
    [Explanation("Food", "Take ball over me and take score", "REFLEXION_LIB.Object.Tools.Foods.Food")]
    [Programmable(true)]
    [Serializable]
    public class Food : GameTool
    {
        protected Color _color;

        [NonSerialized]
        protected bool _is_I_am_ate;
        [NonSerialized]
        private Int64 _hndl;
        [NonSerialized]
        protected float _angle;

        public Food(string nameId)
            : base(nameId)
        {
            _is_I_am_ate = false;
            _color = Color.Red;
            _angle = 0f;
        }

        internal override void BallHandling(Ball ball)
        {
            if (_hndl == ball.GetHandlingId()) return;
            _hndl = ball.GetHandlingId();

            if (base.ballEnterCheckForBorder(ball.GetDirection()))
            {
                ball.SetDirection(ball.GetDirection().rReverse());
                return;
            }
            _is_I_am_ate = true;
            _owner.ate();
        }
        public override void Drawn(System.Drawing.Graphics gr, System.Drawing.Point location, System.Drawing.Size size)
        {
            if (!_visibled || _is_I_am_ate) return;

            //if (_is_I_am_ate)
            //    gr.FillEllipse(Brushes.YellowGreen, new Rectangle(location, size));
            //else
            Rectangle rect = new Rectangle(location, size);
            rect.Width -= rect.Width / 4;
            rect.Height -= rect.Height / 4;
            rect.X += size.Width / 8;
            rect.Y += size.Height / 8;

            LinearGradientBrush br = new LinearGradientBrush(rect, _color, Color.Yellow, _angle);
            gr.FillEllipse(br, rect);
            base.Drawn(gr, location, size);
            _angle += 10;
            if (_angle > 360f) _angle = 0;
        }

        internal bool Is_I_am_ate() { return _is_I_am_ate; }

        public Color GetColor() { return _color; }
        public void SetColor(Color value) { _color = value; }

        #region Programmable
        [Programmable]
        public void color(string value)
        {
            var c = (KnownColor)Enum.Parse(typeof(KnownColor), value, true);
            this.SetColor(Color.FromKnownColor(c));
        }
        #endregion
    };
}
