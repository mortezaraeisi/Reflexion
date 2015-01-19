using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    public class PaintedEventArgs : System.EventArgs
    {
        private readonly Page _page;
        private readonly System.Drawing.Bitmap _image;

        public PaintedEventArgs(Page pg, System.Drawing.Bitmap img)
        {
            _page = pg;
            _image = img;
        }
        public Page Page { get { return _page; } }
        public System.Drawing.Bitmap Image { get { return _image; } }


    };
}
