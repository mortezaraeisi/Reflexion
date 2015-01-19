using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB.Object.Tools.Buttons
{
    [Explanation("Start Point", "Page will started here", "REFLEXION_LIB.Object.Tools.Buttons.StartPoint")]
    [Programmable(true)]
    [Serializable]
    public class StartPoint : Button
    {
        public StartPoint(string nameId) : base(nameId, "S") { ;}
    };
}
