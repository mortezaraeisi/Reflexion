using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    public class PageChangedEventArgs : System.EventArgs
    {
        private readonly Page _previous, _current;

        public PageChangedEventArgs(Page prev, Page curr)
        {
            _previous = prev;
            _current = curr;
        }
        public Page Previous { get { return _previous; } }
        public Page Current { get { return _current; } }
    }
}
