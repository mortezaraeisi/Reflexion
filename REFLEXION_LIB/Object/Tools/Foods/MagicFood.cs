using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace REFLEXION_LIB.Object.Tools.Foods
{
    [Explanation("Magic Food", "Take ball over me and take more score", "REFLEXION_LIB.Object.Tools.Foods.MagicFood")]
    [Programmable]
    [Serializable]
    public class MagicFood : Food
    {
        public MagicFood(string nameId)
            : base(nameId)
        {
            _color = Color.Black;
        }

        public override void Drawn(System.Drawing.Graphics gr, System.Drawing.Point location, System.Drawing.Size size)
        {
            if (!_visibled) return;
            base.Drawn(gr, location, size);
            _angle += 30;
        }
    };
}
