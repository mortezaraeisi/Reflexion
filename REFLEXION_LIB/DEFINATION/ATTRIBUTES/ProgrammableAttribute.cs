using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    sealed class ProgrammableAttribute : Attribute
    {
        private readonly bool _can;
       // private readonly Int32 _argsCount;
        public ProgrammableAttribute(bool can)
        {
            _can = can;
        }
        public ProgrammableAttribute()
        {
            
        }
        public bool Can { get { return _can; } }
    };
}