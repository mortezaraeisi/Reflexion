using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REFLEXION_LIB.Programming
{
    [Serializable]
    public sealed class ProgramBlock
    {
        public static readonly string INFO = "MA.rv group[Rme] - Reflexion, Compiler 1.0v";

        private List<CodeLine> _lines;
        private List<string> _errors;

        private Page _ownerPage; // owner page
        private Game _ownerGame;
        private string _source;//pagename::objname.eventname
        private bool _haveTrueCode;

        internal ProgramBlock(string source, Page owner) : this(source, owner, null) { }
        internal ProgramBlock(string source, Page owner, params CodeLine[] lines)
        {
            _haveTrueCode = false;
            this.setSource(source);
            this.setOwnerPage(owner);
            _errors = new List<string>();
            _lines = new List<CodeLine>();
            if (lines != null)
                _lines.AddRange(lines);
        }
        internal void Add(string line)
        {
            CodeLine cl = new CodeLine(this, line);
            _lines.Add(cl);
            _haveTrueCode = false;
        }
        public void Compile()
        {
            _errors.Clear();
            foreach (var l in _lines)
                try
                {
                    l.Compile();
                }
                catch (Exception ex)
                {
                    _errors.Add(ex.Message + string.Format("< {0} => {1}::{2} > from 'Block Parser'", l, _ownerGame.GetNameId(), _ownerPage.GetNameId()));
                }

            if (_errors.Count == 0) _haveTrueCode = true;
            else _haveTrueCode = false;
        }

        internal void Execute()
        {
            if (!_haveTrueCode)
            {
                this.Compile();
                if (!_haveTrueCode) throw new Exception("Execute: There is error in your code.");
            }
            foreach (var l in _lines)
                try
                {
                    l.Run();
                }
                catch { throw new InvalidCastException("Cannot read value of: " + l.Value); }
        }

        public static ProgramBlock Create(string codes, string source, Page owner)
        {
            ProgramBlock prg = new ProgramBlock(source, owner);
            foreach (string s in codes.Split(';'))
            {
                if (string.IsNullOrWhiteSpace(s)) continue;
                CodeLine cl = new CodeLine(prg, s);
                prg._lines.Add(cl);
            }

            return prg;
        }

        public static void Execute(ProgramBlock blc) { blc.Execute(); }
        public IEnumerable<string> GetErrors()
        {
            if (!_haveTrueCode)
                this.Compile();
            foreach (var t in _errors) yield return t;
        }

        #region Properties & event

        public string GetSource() { return _source; }
        private void setSource(string src) { _source = src; }
        private void setOwnerPage(Page pg)
        {
            if (pg == null || pg.GetOwner() == null)
                throw new ArgumentNullException("Owner page/game cannot be null.");
            _ownerPage = pg;
            _ownerGame = pg.GetOwner();
        }
        public string GetCodes()
        {
            StringBuilder strBL = new StringBuilder();
            foreach (var l in _lines) strBL.AppendLine(l.PureCode + ";");
            return strBL.ToString();
        }
        public Page OwnerPage { get { return _ownerPage; } }
        public Game OwnerGame { get { return _ownerGame; } }
        public bool HaveTrueCode() { return _haveTrueCode; }

        internal void SetOwnerPage(Page pg) { _ownerPage = pg; _ownerGame = pg.GetOwner(); }

        #endregion

    };
}
