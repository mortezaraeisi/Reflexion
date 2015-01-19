using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ExplanationAttribute : System.Attribute
    {
        private readonly string _name;
        private readonly string _explanation;
        private readonly string _typeStringId;

        public ExplanationAttribute(string name, string explanation) : this(name: name, explanation: explanation, typeIdString: null) { ;}
        public ExplanationAttribute(string name, string explanation, string typeIdString)
        {
            _name = name;
            _explanation = explanation;
            _typeStringId = string.IsNullOrWhiteSpace(typeIdString) ? this.GetType().ToString() : typeIdString;

        }
        public string Name { get { return _name; } }
        public string TypeStringId { get { return _typeStringId; } }
        public string Explanation { get { return _explanation; } }
    };

}
