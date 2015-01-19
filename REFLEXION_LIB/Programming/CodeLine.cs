using System;
using System.Linq;

using System.Reflection;

namespace REFLEXION_LIB.Programming
{
    [Serializable]
    internal sealed class CodeLine
    {
        #region Field Member
        
        private ProgramBlock _owner;
        private string _pureCode;

        [NonSerialized]// cpu usage better than memory usage!
        private string _object_page_name;
        [NonSerialized]
        private bool _objectAcross;
        [NonSerialized]
        private Page _objectPage;
        [NonSerialized]
        private Object.BaseObject _object;
        [NonSerialized]
        private string _methodName;
        [NonSerialized]
        private MethodInfo _method;
        [NonSerialized]
        private string _value;
        [NonSerialized]
        private bool _compiled_FLAG;

        [NonSerialized]
        private readonly static string[] _speicalMethods = { "message" };
        #endregion

        internal CodeLine(ProgramBlock owner, string pureLine)
        {
            _owner = owner;
            _pureCode = pureLine;
            _compiled_FLAG = false;
        }

        internal void Run()
        {
            if (!_compiled_FLAG)
            {
                this.Compile();
                if (!_compiled_FLAG) throw new Exception("CodeLine.Run: This line have error!");
            }
            if (this.IsSpecial())
                _method.Invoke(_owner.OwnerGame, new object[] { _methodName + ";" + _value });
            else
                _method.Invoke(_object, new object[] { _value });
        }
        internal void Compile()
        {
            _compiled_FLAG = false;
            string[] split = _pureCode.Split(',');
            if (split.Length != 3)
                throw new Exception("Syntax error! Line pattern missmatch {obj, action, value}");

            _value = split[2].Trim();
            _methodName = split[1].Trim();
            _object_page_name = split[0].Trim();

            loadObjectInfo();
            loadMethodInfo();
            _compiled_FLAG = true;
        }
        private void loadObjectInfo()
        {
            if (this.IsSpecial()) return;

            string //  {[pagename.]objectname}
                strObjName = _object_page_name,
                strPageName = _owner.OwnerPage.GetNameId();
            if (strObjName.Contains('.'))//pagename.objname
            {
                string[] splite = _object_page_name.Split('.');
                if (splite.Length != 2)
                    throw new Exception("Syntax error! Object definition.{[pagename.]objectname} " + _object_page_name);
                if (splite[0] != "page")//if being page 'strPageName' already is current page name id
                    strPageName = splite[0];
                strObjName = splite[1];
            }

            if (strPageName == _owner.OwnerPage.GetNameId())//current page
            {
                _objectPage = _owner.OwnerPage;
                _objectAcross = false;
            }
            else//objec across, another page
            {
                _objectPage = _owner.OwnerGame.Find(strPageName);
                _objectAcross = true;
                if (_objectPage == null)
                    throw new Exception("Not found page: " + strPageName);

                //edtebar sanji!!!!!!! badan minevisamesh
            }

            _object = _objectPage.Find(strObjName);
            if (_object == null)
                throw new Exception("Not found object: " + _object_page_name);
        }
        private void loadMethodInfo()
        {
            Type tp;
            MethodInfo t;

            if (this.IsSpecial())
            {
                tp = typeof(Game);
                t = tp.GetMethod(_object_page_name);
            }
            else
            {
                tp = _object.GetType();
                t = tp.GetMethod(_methodName);
            }
            if (t != null)
            {
                ProgrammableAttribute atr = Policy.IsProgrammableMethod(t);
                if (atr != null)
                {
                    _method = t;
                    return;
                }
            }
            throw new Exception("Not found method: " + _object_page_name + ", " + _methodName);
        }
    
        internal bool IsSpecial()
        {
            return IsSpecial(_object_page_name);
        }
        public static bool IsSpecial(string methodName)
        {
            foreach (var s in _speicalMethods) if (s == methodName) return true;
            return false;
        }
        public override string ToString() { return _pureCode; }

        #region Properties
        public string PureCode { get { return _pureCode; } }
        public string Object_page_name { get { return _object_page_name; } }
        public bool ObjectAcross { get { return _objectAcross; } }
        public Page ObjectPage { get { return _objectPage; } }
        public Object.BaseObject Object { get { return _object; } }

        public string MethodName { get { return _methodName; } }
        public MethodInfo Method { get { return _method; } }
        public string Value { get { return _value; } }

        #endregion

        #region Serializable

        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext contex)
        {
            _compiled_FLAG = false;
        }
        #endregion
    };
}
