using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using REFLEXION_LIB;

namespace REFLEXION_DESIGNER
{
    public partial class frmPage : Form
    {
        private REFLEXION_LIB.Object.BaseObject _selectedTool;
        private Point _mouseDownLoc;

        private bool _isControlKeyDown, _isAltKeyDown;
        private bool _forcelyClose;

        private List<Point> _dragLoc;
        private Page _page;
        public Page Result { get { return _page; } }

        public frmPage(Page pg)
        {
            InitializeComponent();
            _dragLoc = new List<Point>();
            this.txtNameId.MaxLength = Policy.MAX_NAME_LENGTH;
            this.panel1.Location = this.pnlNameId.Location;

            _page = pg;
            _page.GridLinesVisiblity(true);
            _page.Load();
            _page.Painted += _page_PaintEvent;
            this.loadPageInfo();
        }

        internal bool ExitPage()
        {
            _forcelyClose = true;
            _page = null;
            this.Close();
            return true;
        }
        internal bool ExitPage(Page pg)
        {
            if (_page.GetNameId() == pg.GetNameId())
            {
                return this.ExitPage();
            }
            return false;
        }

        private void stateChanged()
        {
            this.Text = "::. " + _page.GetNameId() + "*";
            ((frmMain)this.MdiParent).StateChanged();
        }
        public void StateSave()
        {
            this.Text = this.Text.Replace("*", string.Empty);
        }

        private void addPointPath(Point loc)
        {
            loc = Policy.ConvertShortCircuiteLocToLong(loc, _page);
            loc.X += _page.GetCellSize().Width / 2;
            loc.Y += _page.GetCellSize().Height / 2;
            _dragLoc.Add(loc);
        }
        private void selectTool(REFLEXION_LIB.Object.BaseObject tool)
        {
            if (_selectedTool == tool) return;
            _dragLoc.Clear();
            _selectedTool = tool;

            if (tool == null)
            {
                _dragLoc.Clear();
                this.pictureBox1.Cursor = Cursors.Default;
                this.pnlNameId.Visible = false;
                return;
            }
            pnlNameId.Visible = true;
            this.txtNameId.Text = _selectedTool.GetNameId();
            if (string.IsNullOrWhiteSpace(this.txtRun.Text)) this.txtRun.Text = this.txtNameId.Text;
            this.txtNameId.Focus();
            this.txtNameId.SelectAll();
            addPointPath(tool.GetLoc());
        }
        private void _page_PaintEvent(Page obj)
        {
            var i = (Bitmap)obj.ScreenImage.Clone();
            if (_selectedTool != null)
            {
                Graphics gr = Graphics.FromImage(i);
                Point lc = Policy.ConvertShortCircuiteLocToLong(_selectedTool.GetLoc(), _page);
                lc.X -= 3;
                lc.Y -= 3;
                Size sz = _page.GetCellSize();
                sz.Width += 6;
                sz.Height += 6;
                Pen pen = new Pen(Brushes.Gray, 1.5f) { DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot };
                gr.DrawRectangle(pen, new Rectangle(lc, sz));
                pen.Color = Color.Red;
                if (_dragLoc.Count > 1) gr.DrawCurve(pen, _dragLoc.ToArray<Point>());
            }
            this.pictureBox1.Image = i;
        }
        private void loadPageInfo()
        {
            _page.Load();
            this.Text = "::. " + _page.GetNameId();
            {
                Point p = _page.GetScreenSize();
                p.X *= _page.GetCellSize().Width;
                p.Y *= _page.GetCellSize().Height;
                Size sz = new Size(p.X, p.Y);
                this.pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
                this.pictureBox1.Location = new Point(10, 10);
                this.pictureBox1.Size = sz;
                sz.Width += 40;
                sz.Height += 80;
                this.Size = this.MinimumSize = sz;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _mouseDownLoc = Policy.ConvertLocationToShortCircuite(e.Location, _page);
            this.selectTool(_page.Find(_mouseDownLoc));

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (frmToolbox.SelectedTool != null)
                {
                    frmToolbox.DeselectAnyTool();
                    return;
                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (frmToolbox.SelectedTool != null)
                {
                    var obj = frmToolbox.SelectedTool;
                    if (_selectedTool != null) return;//there is a tool already, cancel adding tool
                    this.stateChanged();/*try to save later*/
                    obj.SetLoc(_mouseDownLoc);
                    obj.SetNameId(Policy.SuggestObjectNameId(obj.GetType(), _page));
                    _page.Add(obj);
                    this.selectTool(obj);
                    if (_isControlKeyDown) frmToolbox.ReSelectTool(); else frmToolbox.DeselectAnyTool();
                    _page.GetOwner().Paint(_page);
                }
                else
                {
                    if (_isAltKeyDown && _selectedTool != null) { _page.MouseClicked(_mouseDownLoc); this.stateChanged();/*try to save later*/ }
                }
            }
            Project.Option.Game.Paint(_page);
        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Point loc = Policy.ConvertLocationToShortCircuite(e.Location, _page);
            if (_page.IsLocationBusy(loc))
                this.pictureBox1.Cursor = Cursors.Hand;
            else
                this.pictureBox1.Cursor = (frmToolbox.SelectedTool != null) ? Cursors.Cross : Cursors.Default;

            this.lblStatus.Text = string.Format("loc:{0} - location: {1}", loc, e.Location);
            if (e.Button == System.Windows.Forms.MouseButtons.Left && _selectedTool != null)//draging obj
            {
                this.pictureBox1.Cursor = Cursors.SizeAll;
                if (!_page.IsLocationBusy(loc) && _page.IsLocationValid(loc))
                {
                    this.stateChanged();/*try to save later*/
                    this.addPointPath(loc);
                    _selectedTool.SetLoc(loc);
                    Project.Option.Game.Paint(_page);
                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

        }
        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            this.programmingToolStripMenuItem_Click(null, null);
        }

        private void txtNameId_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = e.KeyChar == ' ';
            if (_selectedTool == null) return;
            if (e.KeyChar == 13)//enter
            {
                try
                {
                    _selectedTool.SetNameId(this.txtNameId.Text);
                    this.stateChanged();/*try to save later*/
                    this.txtNameId.SelectAll();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Tool id not valid!\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void txtRun_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)//enter
            {
                REFLEXION_LIB.Programming.ProgramBlock blc = REFLEXION_LIB.Programming.ProgramBlock.Create(this.txtRun.Text, _page.GetNameId() + "::init", _page);
                blc.Compile();
                if (!blc.HaveTrueCode())
                {
                    MessageBox.Show("Command syntax error:\n" + blc.GetErrors().ToArray<string>()[0], "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.txtRun.SelectAll();
                    this.txtRun.Focus();
                    return;
                }
                try
                {
                    REFLEXION_LIB.Programming.ProgramBlock.Execute(blc);
                    this.stateChanged();/*try to save later*/
                    this.loadPageInfo();

                    int idx = this.txtRun.Text.LastIndexOf(',');
                    if (idx >= 0)
                    {
                        idx++;
                        this.txtRun.SelectionStart = idx;
                        this.txtRun.SelectionLength = this.txtRun.TextLength - idx;
                    }
                    else this.txtRun.SelectAll();
                    this.txtRun.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Command runtime error:!\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void frmPage_KeyDown(object sender, KeyEventArgs e)
        {
            _isControlKeyDown = e.Control;
            _isAltKeyDown = e.Alt;
        }
        private void frmPage_KeyUp(object sender, KeyEventArgs e)
        {
            _isControlKeyDown = e.Control;
            _isAltKeyDown = e.Alt;
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (e.Control)
                        this.removeToolStripMenuItem_Click(null, null);
                    break;
                case Keys.F7:
                    this.programmingToolStripMenuItem_Click(null, null);
                    break;
                case Keys.F5:
                    this.runToolStripMenuItem_Click(null, null);
                    break;
                case Keys.G:
                    if (e.Control)
                        this.gameSettingsToolStripMenuItem_Click(null, null);
                    break;
                case Keys.P:
                    if (e.Control)
                        this.pageSettingsToolStripMenuItem_Click(null, null);
                    break;
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_selectedTool != null)
            {
                _page.Remove(_selectedTool);
                this.stateChanged();/*try to save later*/
                this.selectTool(null);
                //_page.Paint();
                Project.Option.Game.Paint(_page);
            }
        }
        private void programmingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = null;
            if (_selectedTool != null) name = _selectedTool.GetNameId();
            new frmProgramming(_page, name).ShowDialog();
            this.stateChanged();/*try to save later*/
        }
        private void gameSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((frmMain)this.MdiParent).ShowGameSettings();
        }
        private void pageSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (new frmPageSettings(_page).ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.loadPageInfo();
                this.stateChanged();/*try to save later*/
            }
        }
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((frmMain)this.MdiParent).Play();
        }

        private void frmPage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_forcelyClose) return;
            e.Cancel = true;
            this.Hide();
        }
    };
}
