using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace REFLEXION_LIB.Object.Tools
{
    [Explanation("Game Tool [abs]", "Father all of game tools", "No need")]
    [Serializable]
    public abstract class GameTool : BaseObject
    {
        public GameTool(string nameId)
            : base(nameId)
        { }

        public override void Drawn(System.Drawing.Graphics gr, System.Drawing.Point location, System.Drawing.Size size)
        {
            if (!_visibled) return;

            Pen pp = new Pen(Color.Gray, 1.50f);
            if ((_border & Borders.Top) == Borders.Top)
                gr.DrawLine(pp, location, new Point(location.X + size.Width, location.Y));
            if ((_border & Borders.Bottom) == Borders.Bottom)
                gr.DrawLine(pp, new Point(location.X, location.Y + size.Height), new Point(location.X + size.Width, location.Y + size.Height));

            if ((_border & Borders.Left) == Borders.Left)
                gr.DrawLine(pp, location, new Point(location.X, location.Y + size.Height));
            if ((_border & Borders.Right) == Borders.Right)
                gr.DrawLine(pp, new Point(location.X + size.Width, location.Y), new Point(location.X + size.Width, location.Y + size.Height));
        }

        public virtual void MouseClicked() { ; }// there is no way for 'abstract' definition, I'm sorry!

        protected void executeBallEnterBlock()
        {
            if (_ballEnterBlock != null)
                _ballEnterBlock.Execute();//I love you
        }
        protected bool ballEnterCheckForBorder(Direction ballDirection)
        {
            Borders blAsBorder = ballDirection.rReverse().rToBorder();

            if ((blAsBorder & _border) == Borders.None)
                return false;
            return true;
        }
    };
}
