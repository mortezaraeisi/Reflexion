using System;
using System.Drawing;
using System.Windows.Forms;

using REFLEXION_LIB;
using System.IO;

namespace REFLEXION_PLAYER
{
    public partial class frmMain : Form
    {
        private Game _game;
        private string _gamePath;

        public frmMain(string path):this()
        {
            this.loadFromFile(path);
        }
        public frmMain()
        {
            InitializeComponent();
            this.BackColor = Color.Black;
            this.WindowState = FormWindowState.Maximized;
            this.Shown += (s, e) => this.pictureBox1_SizeChanged(null, null);

            this.lnkPlayPause.Visible = false;

            string path = Application.StartupPath; if (!path.EndsWith("\\")) path += "\\";
            Settings.MakeNewSetting(path + "config.bin");

            this.Deactivate += (s, e) => { if (_game != null)_game.Pause(); };
            this.SizeChanged += (s, e) => { if (this.WindowState != FormWindowState.Maximized)this.WindowState = FormWindowState.Maximized; };
            this.Shown += (s, e) => { if (_game == null) this.selectLevel(); };
        }

        private void loadFromFile(string path)
        {
            System.IO.FileStream stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
            Game g = Game.Load(stream);
            stream.Close();
            _gamePath = path;
            this.setGame(g);
        }
        private void setGame(Game g)
        {
            if (_game != null)
            {
                _game.Pause();
                _game.Painted -= _game_ScreenPaint;
                _game.Messaging -= _game_Messaging;
                _game.PageChanged -= _game_PageChanged;
                _game.Won -= _game_Won;
                _game.Terminated -= _game_Terminated;
                _game = null;
            }
            _game = g;

            this.lnkPlayPause.Visible = true;

            _game.Painted += _game_ScreenPaint;
            _game.Messaging += _game_Messaging;
            _game.PageChanged += _game_PageChanged;
            _game.Won += _game_Won;
            _game.Terminated += _game_Terminated;
            _game.Load();
            _game.Paint();
            this.Text = "::. Reflexion Player - " + _game.NameId;
        }

        void _game_Terminated(Game game, GameStateChangedEventArgs e)
        {
            if (MessageBox.Show("Game Over!!!!\n Sorry! you lose, the game was terminated.\n\nTry again?", "Game over",
                MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Retry)
            {
                this.restart();
            }
            else
            {
                this.showMenu();
            }
        }
        void _game_Won(Game game, GameStateChangedEventArgs e)
        {
            MessageBox.Show("Congratulation!!! You won the game!", "Winner", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.showMenu();
        }
        void _game_PageChanged(Game game, PageChangedEventArgs e)
        {
        }
        void _game_Messaging(Game game, MessagingEventArgs e)
        {
            MessageBox.Show(e.Message, e.Title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void _game_ScreenPaint(Game game, PaintedEventArgs e)
        {
            this.pictureBox1.Image = game.CurrentPage.ScreenImage;
        }
        private void showMenu()
        {
            if (_game != null && _game.Status == GameStates.Playing) _game.Pause();
            frmMenu frm = new frmMenu(_game != null);
            switch (frm.ShowDialog())
            {
                case System.Windows.Forms.DialogResult.Cancel://resume
                    break;
                case System.Windows.Forms.DialogResult.Retry://reload
                    this.restart();
                    break;
                case System.Windows.Forms.DialogResult.No://info
                    this.info();
                    break;

                case System.Windows.Forms.DialogResult.Ignore://load another level
                    this.selectLevel();
                    break;
                case System.Windows.Forms.DialogResult.OK://settings

                    break;
                case System.Windows.Forms.DialogResult.Abort://close
                    this.Close(); break;

            }//endith
        }
        private void selectLevel()
        {
            frmLevelBrowser frm = new frmLevelBrowser();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.loadFromFile(frm.Result);
            }
        }
        private void restart()
        {
            if (!string.IsNullOrWhiteSpace(_gamePath))
            {
                this.loadFromFile(_gamePath);
            }
        }
        private void info() {
            if (_game != null)
            {
                new frmInfo(_game).ShowDialog();
            }
        }
        private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void lnkPlayPause_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_game == null) return;
            if (_game.Status == GameStates.Terminated) return ;//reset game!;
            if (_game.Status == GameStates.Playing)
                _game.Pause();
            else
                _game.Play();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (_game == null) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                this.showMenu();
                return;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (_game.Status == GameStates.Playing || _game.Status == GameStates.Initialized)
                    _game.MouseClicked(e.Location);
                else if (_game.Status == GameStates.Paused)
                    _game.Play();
            }
        }
        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            this.pictureBox1.Left = ((this.Width - this.pictureBox1.Width) + 1) / 2;
            this.pictureBox1.Top = (((this.Height - this.pictureBox1.Height - 50) + 1) / 2) + 50;
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    this.lnkPlayPause_LinkClicked(null, null);
                    break;
                case Keys.Escape:
                    this.showMenu();
                    break;
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.showMenu();
        }
    };
}
