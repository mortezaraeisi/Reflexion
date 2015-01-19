using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    [Flags]
    public enum Borders : int
    {
        None = 0,

        Right = 1,
        Left = 2,
        Top = 4,
        Bottom = 8,

        RightLeft = 3,
        Horizontal = 3,
        TopBottom = 12,
        Vertical = 12,

        All = 15,
    }
}
