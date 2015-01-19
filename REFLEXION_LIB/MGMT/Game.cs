using System;
using System.Collections.Generic;

using System.Drawing;

namespace REFLEXION_LIB
{
    [Serializable]
    public sealed class Game
    {
        private string _nameId;
        private string _author;
        private string _email;
        private string _exp;
        private System.Drawing.Imaging.PixelFormat _pixelFormat;

        private List<Page> _pages;
        private Page _currentPage;
        [NonSerialized]
        private REFLEXION_LIB.PaintedEvent _screenPaintDEL;
        [NonSerialized]
        private REFLEXION_LIB.PageChangedEvent _pageChangedDel;

        [NonSerialized]
        private REFLEXION_LIB.GameStateChanged _gameLoaded, _gamePlayed, _gamePaused, _gameTerminated, _gameWon;
        [NonSerialized]
        private REFLEXION_LIB.Messaging _messagingDel;

        [NonSerialized]
        private System.Windows.Forms.Timer _TIMER;
        [NonSerialized]
        private GameStates _state;
        private Int64 _authentication;
        private readonly Int64 _AUTHENTICATION_KEY = 83568923689100125;

        public Game(string nameId) : this(nameId, string.Empty, string.Empty, string.Empty, System.Drawing.Imaging.PixelFormat.Format32bppPArgb) { ;}
        public Game(string nameId, string author, string email, string explanation, System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            _pages = new List<Page>();
            _state = GameStates.Terminated;
            this.initTimer();

            this.SetNameId(nameId);
            this.Author = author;
            this.Email = email;
            this.Explanation = explanation;
            this.PixelFormat = pixelFormat;
        }

        private void initTimer()
        {
            _TIMER = new System.Windows.Forms.Timer();
            _TIMER.Interval = 50;
            _TIMER.Tick += (s, e) => __clock_pulse();
            _TIMER.Enabled = true;

        }
        private void __clock_pulse()
        {
            if (_state == GameStates.Paused) _currentPage.PaintPaused();
            if (_state != GameStates.Playing) return;
            _currentPage.Pulse();
        }

        #region Publishing and load

        public void Publish(System.IO.Stream stream)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            _authentication = _AUTHENTICATION_KEY;
            formatter.Serialize(stream, this);
            _authentication = 0;
        }
        public static Game Load(System.IO.Stream stream)
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter =
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            Game g = (Game)formatter.Deserialize(stream);
            if (g._authentication != g._AUTHENTICATION_KEY)
                throw new FormatException("Authentication failed.");

            g._currentPage = g.Find(Policy.DEFAULT_PAGE_NAME);
            foreach (var p in g.Pages) p.GridLinesVisiblity(false);
            return g;
        }

        #endregion

        #region Control
        public void Load()
        {
            if (_pages.Count == 0)
                throw new Exception("Game is not valid... [empty page]");
            _currentPage = this.Find(Policy.DEFAULT_PAGE_NAME);
            if (_currentPage == null)
                throw new ArgumentNullException("Default page not defined");

            foreach (var t in _pages) t.Load();
            _state = GameStates.Initialized;
            _gameLoaded.RaiseEvent(this, new GameStateChangedEventArgs(GameStates.Initialized, _state));
            this.ChangePage(_currentPage);
        }
        public void Play()
        {
            if (_state == GameStates.Playing) return;
            if (_state == GameStates.Initialized || _state == GameStates.Paused)
            {
                var os = _state;
                _state = GameStates.Playing;
                _gamePlayed.RaiseEvent(this, new GameStateChangedEventArgs(os, _state));
            }
        }
        public void Pause()
        {
            if (_state == GameStates.Paused) return;
            if (_state == GameStates.Playing)
            {
                var os = _state;
                _state = GameStates.Paused;
                _gamePaused.RaiseEvent(this, new GameStateChangedEventArgs(os, _state));
            }
        }
        internal void Terminate()
        {
            _TIMER.Enabled = false;
            var os = _state;
            _state = GameStates.Terminated;
            _gameTerminated.RaiseEvent(this, new GameStateChangedEventArgs(os, _state));
        }
        private void WonTheGame()
        {
            _TIMER.Enabled = false;
            var os = _state;
            _state = GameStates.Won;
            _gameWon.RaiseEvent(this, new GameStateChangedEventArgs(os, _state));
        }
        internal void TryForWin()
        {
            foreach (var p in _pages) if (!p.IsAnyFoodWasAteCheck()) return;
            this.WonTheGame();
        }

        internal void ChangePage(Page pg)
        {
            if (pg == null)
                throw new ArgumentNullException("PageChange: Page is empty");
            Page old = _currentPage;
            _currentPage = pg;
            _currentPage.Paint();
            _state = GameStates.Initialized;
            _pageChangedDel.RaiseEvent(this, new PageChangedEventArgs(old, _currentPage));
            _currentPage.Initialize();
       }
        public void ChangePage(string nameId)
        {
            this.ChangePage(this.Find(nameId));
        }
        public void MouseClicked(Point location)
        {
            if (_state == GameStates.Playing || _state == GameStates.Initialized)
                _currentPage.MouseClicked(location.rToLoc(_currentPage));
        }
        public void Paint() { this.Paint(_currentPage); }
        public void Paint(Page pg)
        {
            pg.Paint();
        }

        public void ShowMessage(string title, string msgText)
        {
            bool isPlaying = _state == GameStates.Playing;
            if (isPlaying) this.Pause();
            _messagingDel.RaiseEvent(this, new MessagingEventArgs(title, msgText));
            if (isPlaying) this.Play();
        }

        #endregion

        #region Collection mgmt

        public void Add(Page pg)
        {
            if (this.IsNameIdExist(pg.GetNameId()))
                throw new Exception("Duplicated name");
            pg.SetOwner(this);//forcely set owner
            // pg.SetPaintEventForcely(this.pagePaint);//handle painted page forcely and clear other registed methods
            _pages.Add(pg);
            _currentPage = _currentPage ?? pg;
        }
        public void Remove(Page pg)
        {
            if (this.IsNameIdExist(pg.GetNameId()))
                _pages.Remove(pg);
        }

        public Page Find(string nameId)
        {
            foreach (var t in _pages)
                if (t.GetNameId() == nameId) return t;
            return null;
        }
        public bool IsNameIdExist(string nameId)
        {
            return this.Find(nameId) != null;
            //return (_pages.Select(n => n.GetNameId() == nameId).ToArray().Length != 0);
        }

        internal void PagePaint(Page pg)
        {
            _screenPaintDEL.RaiseEvent(this, new PaintedEventArgs(pg, pg.ScreenImage));
        }

        public Game Clone()
        {
            return (Game)this.MemberwiseClone();
        }

        #endregion

        #region Properties & Events
        public IEnumerable<Page> Pages { get { foreach (var t in _pages) yield return t; } }
        internal void SetNameId(string nameId) { _nameId = nameId; }
        internal string GetNameId() { return _nameId; }
        public Page CurrentPage { get { return _currentPage; } }

        public System.Drawing.Imaging.PixelFormat PixelFormat { get { return _pixelFormat; } set { _pixelFormat = value; } }
        public GameStates Status { get { return _state; } }
        public string NameId { get { return _nameId; } set { _nameId = value; } }
        public string Author { get { return _author; } set { _author = value; } }
        public string Email { get { return _email; } set { _email = value; } }
        public string Explanation { get { return _exp; } set { _exp = value; } }

        public event REFLEXION_LIB.PaintedEvent Painted { add { _screenPaintDEL += value; } remove { _screenPaintDEL -= value; } }
        public event REFLEXION_LIB.PageChangedEvent PageChanged { add { _pageChangedDel += value; } remove { _pageChangedDel -= value; } }
        public event REFLEXION_LIB.Messaging Messaging { add { _messagingDel += value; } remove { _messagingDel -= value; } }

        public event REFLEXION_LIB.GameStateChanged Loaded { add { _gameLoaded += value; } remove { _gameLoaded -= value; } }
        public event REFLEXION_LIB.GameStateChanged Played { add { _gamePlayed += value; } remove { _gamePlayed -= value; } }
        public event REFLEXION_LIB.GameStateChanged Paused { add { _gamePaused += value; } remove { _gamePaused -= value; } }
        public event REFLEXION_LIB.GameStateChanged Won { add { _gameWon += value; } remove { _gameWon -= value; } }
        public event REFLEXION_LIB.GameStateChanged Terminated { add { _gameTerminated += value; } remove { _gameTerminated -= value; } }

        #endregion

        #region Serializable

        [System.Runtime.Serialization.OnDeserialized]
        private void OnDeserialized(System.Runtime.Serialization.StreamingContext contex)
        {
            this.initTimer();
            this.Load();
        }
        #endregion

        #region Programmable
        [Programmable]
        public void message(string value)
        {
            string[] splite = value.Split(';');
            this.ShowMessage(splite[0], splite[1]);
        }
        #endregion
    };

}
