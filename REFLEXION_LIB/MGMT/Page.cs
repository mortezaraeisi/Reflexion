using System;
using System.Collections.Generic;

using System.Drawing;

namespace REFLEXION_LIB
{
    [Serializable]
    public sealed class Page
    {
        #region Data fields

        private string _nameId;
        private Size _cellSize;
        private Point _scrSize;
        private int _bgColorRGB;
        [NonSerialized]
        private Color _bgColor;

        private Object.Tools.Buttons.StartPoint _startPoint;
        private Object.Ball _ball;

        private List<Object.BaseObject> _items;
        private List<Object.Tools.GameTool> _itemsOfGameTools;
        private List<Object.Ball> _itemsOfBall;

        private bool _verticalGridLine, _horizontalGridLine;
        [NonSerialized]
        private Graphics _gr;
        [NonSerialized]
        private Bitmap _pgScreen;
        private Game _owner;

        private Programming.ProgramBlock _initBlock, _startBlock;
        [NonSerialized]
        private Action<Page> _paintDel;
        [NonSerialized]
        private System.WeakReference _weakRef;
        [NonSerialized]
        private List<Point> _snowList;
        
        private bool _isInitialized;
        private bool _isStarted;

        #endregion

        #region Constructors

        public Page(string nameId) : this(nameId, Policy.DEFAULT_SCREEN_SIZE, Policy.DEFAULT_CELL_SIZE, Policy.DEFAULT_BACKCOLOR) { ;}
        public Page(string nameId, Point screenSize, Size cellSize, int backColor)
        {
            _isStarted = _isInitialized = false;
            this.SetNameId(nameId);
            this.SetCellSize(cellSize);
            this.SetBackColor(backColor);
            this.SetScreenSizePrivately(screenSize);
            {
                Object.Tools.Buttons.StartPoint start = new Object.Tools.Buttons.StartPoint(Policy.DEFAULT_STARTPOINT_NAME);
                start.SetLoc(new Point(0, 0));
                this.Add(start);
                Object.Ball bal = new Object.Ball(Policy.DEFAULT_BALL_NAME);
                bal.SetLoc(new Point(0, 1));
                this.Add(bal);
            }
        }

        #endregion

        #region Control

        public void MouseClicked(Point loc)
        {
            var t = this.Find(loc) as Object.Tools.GameTool;
            if (t != null)
            {
                t.MouseClicked();
                this.Paint();
            }
        }

        internal void Pulse()
        {
            if (!_isStarted && _startBlock != null)
            {
                _startBlock.Execute();
                _isStarted = true;
            }

            foreach (var b in _itemsOfBall)
            {
                b.Move();
                var tmpObj = b.GetCurrentObj();
                if (tmpObj != null)
                {
                    tmpObj.BallHandling(b);
                }

            }//endeach b
            this.Paint();
        }
        public void Load()
        {
            this.loadBall().SetLoc(this.loadStartPoint().GetLoc());
            this.resetScreen();
            this.Paint();
        }
        internal void Initialize()
        {
            if (_owner.Status == GameStates.Initialized && !_isInitialized && _initBlock != null)
            {
                _initBlock.Execute();
                _isInitialized = true;
            }
        }
        internal void endPointEntered(Object.Tools.Buttons.EndPoint endPointObj)
        {
            _owner.TryForWin();
        }
        internal bool IsAnyFoodWasAteCheck()
        {
            foreach (var t in _itemsOfGameTools)
            {
                Object.Tools.Foods.Food fd = t as Object.Tools.Foods.Food;
                if (fd != null && !fd.Is_I_am_ate())
                    return false;
            }//endeach t
            return true;
        }
        internal void ate()
        {
            if (this.IsAnyFoodWasAteCheck())
            {
                //show exit way
            }
        }
        private Object.Tools.Buttons.StartPoint loadStartPoint()
        {
            Object.Tools.Buttons.StartPoint ok = null;
            foreach (var t in _items)
            {
                var tmp = t as Object.Tools.Buttons.StartPoint;
                if (tmp != null)
                {
                    if (ok == null)
                        ok = tmp;
                    else
                        throw new Exception("LoadStartPoint: Multiple start point are defined.");
                }
            }//endeach t
            if (ok == null)
                throw new Exception("LoadStartPoint: Not defined start point.");
            if (ok.GetNameId() != Policy.DEFAULT_STARTPOINT_NAME)
                throw new Exception("LoadStartPoint: Not found default start point objecy. '" + ok.GetNameId() + "' change to " + Policy.DEFAULT_STARTPOINT_NAME);
            return _startPoint = ok;
        }
        private Object.Ball loadBall()
        {
            if (_itemsOfBall.Count == 0)
                throw new Exception("LoadBall: Not defined ball.");

            if (_itemsOfBall[0].GetNameId() != Policy.DEFAULT_BALL_NAME)
                throw new Exception("LoadBall: Not found default ball object. '" + _itemsOfBall[0].GetNameId() + "' change to " + Policy.DEFAULT_BALL_NAME);

            return _ball = _itemsOfBall[0];
        }
        #endregion

        #region Screen and painting

        private void resetScreen()
        {
            _pgScreen = new Bitmap(_cellSize.Width * _scrSize.X, _cellSize.Height * _scrSize.Y, _owner.PixelFormat);
            _gr = Graphics.FromImage(_pgScreen);
        }
        internal void Paint()
        {
            _gr.Clear(_bgColor);
            if (_horizontalGridLine)
                for (int i = 1; i <= _scrSize.Y; ++i)
                    _gr.DrawLine(Pens.Gray, new Point(0, i * _cellSize.Height), new Point(_scrSize.X * _cellSize.Height, i * _cellSize.Height));
            if (_verticalGridLine)
                for (int i = 1; i < _scrSize.X; i++)
                    _gr.DrawLine(Pens.Gray, new Point(i * _cellSize.Width, 0), new Point(i * _cellSize.Width, _scrSize.Y * _cellSize.Height));
            
            foreach (var b in _itemsOfGameTools) b.Drawn(_gr, b.GetLoc().rToLocation(this), _cellSize);
            foreach (var b in _itemsOfBall) b.Drawn(_gr, b.GetLoc().rToLocation(this), _cellSize);

            if (_paintDel != null) _paintDel(this);
            _owner.PagePaint(this);
            if (_snowList != null)
            {
                _weakRef = new WeakReference(_snowList);
                _snowList = null;
            }
        }

        internal void PaintPaused()
        {
            int w = _scrSize.X * _cellSize.Width;
            int h = _scrSize.Y * _cellSize.Height;
            Random rnd = new Random();
            if (_snowList == null)// (_weakRef == null || !_weakRef.IsAlive))
            {
                if (_weakRef == null || !_weakRef.IsAlive)
                {
                    _snowList = new List<Point>(_scrSize.X * _scrSize.Y);
                    for (int i = 0; i < _snowList.Capacity; ++i)
                    {
                        Point p = new Point(rnd.Next(w), rnd.Next(h));
                        _snowList.Add(p);
                    }//next i
                }
                else
                    _snowList = (List<Point>)_weakRef.Target;
            }
            else
            {
                _gr.Clear(Color.Black);
                for (int i = 0; i < _snowList.Count; i++)
                {
                    Point p = _snowList[i];
                    _gr.FillEllipse(Brushes.White, new Rectangle(p, new Size(rnd.Next(4) + 2, rnd.Next(4) + 2)));
                    //p.X += 1;
                    p.Y += 5;
                    if (p.X >= w || p.Y >= h) p = new Point(rnd.Next(w), 0);
                    _snowList[i] = p;
                }
                _owner.PagePaint(this);
            }
        }
        #endregion

        #region Item collection mgmt
        public void Add(Object.BaseObject obj)
        {
            if (this.IsNameIdExist(obj.GetNameId()))
                throw new Exception("Duplicated name");
            if (!this.IsLocationValid(obj.GetLoc()) || this.IsLocationBusy(obj.GetLoc()))
                throw new Exception("Location not valid!");

            obj.SetOwner(this);
            _items.Add(obj);

            Object.Tools.GameTool g = obj as Object.Tools.GameTool;
            if (g != null)
                _itemsOfGameTools.Add(g);
            else
            {
                Object.Ball b = obj as Object.Ball;
                if (b != null)
                    _itemsOfBall.Add(b);
            }
        }
        public void Remove(Object.BaseObject obj)
        {
            if (this.IsNameIdExist(obj.GetNameId()))
            {
                _items.Remove(obj);

                Object.Tools.GameTool g = obj as Object.Tools.GameTool;
                if (g != null)
                    _itemsOfGameTools.Remove(g);
                else
                {
                    Object.Ball b = obj as Object.Ball;
                    if (b != null)
                        _itemsOfBall.Remove(b);
                }
            }
        }
        public Object.BaseObject Find(string nameId)
        {
            foreach (var t in _items) if (t.GetNameId() == nameId) return t;
            return null;
        }
        public Object.BaseObject Find(Point loc)
        {
            foreach (var t in _items) if (t.CheckLoc(loc)) return t;
            return null;
        }
        internal Object.BaseObject Find(Point loc, Object.BaseObject forcelyNot)
        {
            foreach (var t in _items) if (t.CheckLoc(loc) && !object.ReferenceEquals(forcelyNot, t)) return t;
            return null;
        }
        public bool IsNameIdExist(string nameId)
        {
            return this.Find(nameId) != null;
        }
        public bool IsLocationBusy(Point loc)
        {
            foreach (var t in _items)
                if (t.CheckLoc(loc)) return true;
            // if (t.GetLoc().X == loc.X && t.GetLoc().Y == loc.Y) return true;
            return false;
        }
        public bool IsLocationValid(Point loc)
        {
            return this.IsLocationValid(loc, _scrSize);
            //if (loc.X < 0 || loc.X >= _scrSize.X) return false;
            //if (loc.Y < 0 || loc.Y >= _scrSize.Y) return false;
            //return true;
        }
        public bool IsLocationValid(Point loc, Point screenSize)
        {
            if (loc.X < 0 || loc.X >= screenSize.X) return false;
            if (loc.Y < 0 || loc.Y >= screenSize.Y) return false;
            return true;
        }
        public Point GetFreeLocation()
        {
            for (int i = 0; i < _scrSize.X; ++i)
                for (int j = 0; j < _scrSize.Y; ++j)
                {
                    Point p = new Point(i, j);
                    if (!this.IsLocationBusy(p)) return p;
                }
            throw new Exception("GetFreeLocation: There is no free space");
        }
        public Object.BaseObject this[int index] { get { return _items[index]; } }
        public Object.BaseObject this[string obj] { get { return this.Find(obj); } }
        public IEnumerable<Object.BaseObject> GetEnumerator() { foreach (var t in _items) yield return t; }

        #endregion

        #region Properties
        public void SetNameId(string nameId)
        {
            Policy.CheckObjectNameSyntax(nameId);
            if (_owner != null)
            {
                Page p = _owner.Find(nameId);
                if (p != null)
                    throw new Exception("Page: SetNameId, Dupplicated");
            }
            _nameId = nameId;
        }
        public string GetNameId() { return _nameId; }
        public void SetCellSize(Size sz)
        {
            Policy.CheckCellSizeRange(sz);
            _cellSize = sz;
        }
        public Size GetCellSize() { return _cellSize; }
        private void SetScreenSizePrivately(Point value)
        {
            Policy.CheckScreenRange(value);
            _scrSize = value;
            _items = new List<Object.BaseObject>();// (value.X * value.Y);
            _itemsOfBall = new List<Object.Ball>();// (_items.Capacity);
            _itemsOfGameTools = new List<Object.Tools.GameTool>();// (_items.Capacity);
        }
        public void SetScreenSize(Point value)
        {
            Policy.CheckScreenRange(value);
            foreach (var o in _items)
                if (!this.IsLocationValid(o.GetLoc(), value))
                    throw new ArgumentOutOfRangeException
                        ("Page.SetScreenSize: There is an object that is not valid with new screen size. try to relocate object first. " + o.GetNameId());
            _scrSize = value;
            this.resetScreen();
        }
        public Point GetScreenSize() { return _scrSize; }
        public void SetBackColor(int RGB) { _bgColor = Color.FromArgb(RGB); _bgColorRGB = RGB; }
        public int GetBackColor() { return _bgColorRGB; }
        internal void SetOwner(Game owner) { _owner = owner; }
        public Game GetOwner() { return _owner; }

        public event Action<Page> Painted { add { _paintDel += value; } remove { _paintDel -= value; } }
        public System.Drawing.Bitmap ScreenImage { get { return _pgScreen; } }
        public void GridLinesVisiblity(bool value) { _horizontalGridLine = value; _verticalGridLine = value; }
        public bool IsMainPage() { return _nameId == Policy.DEFAULT_PAGE_NAME; }

        public void SetInitBlock(Programming.ProgramBlock block)
        {
            _initBlock = block;
            _initBlock.SetOwnerPage(this);
        }
        public void SetStartBlock(Programming.ProgramBlock block)
        {
            _startBlock = block;
            _startBlock.SetOwnerPage(this);
        }
        public Programming.ProgramBlock GetInitBlock() { return _initBlock; }
        public Programming.ProgramBlock GetStartBlock() { return _startBlock; }

        #endregion

        #region Serializable

        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext contex)
        {
            _bgColor = Color.FromArgb(_bgColorRGB);
            _isStarted = _isInitialized = false;
            this.resetScreen();
            this.Paint();
        }
        #endregion
    };
}
