using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using REFLEXION_LIB;

namespace REFLEXION_DESIGNER
{
    public partial class frmPageSettings : Form
    {
        private Page _currentPage;
        public Page Result { get { return _currentPage; } }

        public frmPageSettings() : this(null) { ;}
        public frmPageSettings(Page pg)
        {
            InitializeComponent();
            this.nmWidth.Minimum = Policy.MIN_SCREEN_SIZE.X;
            this.nmWidth.Maximum = Policy.MAX_SCREEN_SIZE.X;
            this.nmHeaight.Minimum = Policy.MIN_SCREEN_SIZE.Y;
            this.nmHeaight.Maximum = Policy.MAX_SCREEN_SIZE.Y;
            this.nmCellWidth.Minimum = Policy.MIN_CELL_SIZE.Width;
            this.nmCellWidth.Maximum = Policy.MAX_CELL_SIZE.Width;
            this.nmCellHeight.Minimum = Policy.MIN_CELL_SIZE.Height;
            this.nmCellHeight.Maximum = Policy.MAX_CELL_SIZE.Height;

            this.lblScrWidth.Text = "Width:" + string.Format("{0}-{1}", Policy.MIN_SCREEN_SIZE.X, Policy.MAX_SCREEN_SIZE.X);
            this.lblScrHeight.Text = "Height:" + string.Format("{0}-{1}", Policy.MIN_SCREEN_SIZE.Y, Policy.MAX_SCREEN_SIZE.Y);
            this.lblCellWidth.Text = "Width:" + string.Format("{0}-{1}", Policy.MIN_CELL_SIZE.Width, Policy.MAX_CELL_SIZE.Width);
            this.lblCellHeight.Text = "Height:" + string.Format("{0}-{1}", Policy.MIN_CELL_SIZE.Height, Policy.MAX_CELL_SIZE.Height);

            _currentPage = pg;// ?? new Page("page", Policy.DEFAULT_SCREEN_SIZE, Policy.DEFAULT_CELL_SIZE, Policy.DEFAULT_BACKCOLOR);
            this.reloadInfo();
        }
        private void reloadInfo()
        {
            if (_currentPage == null)
            {
                this.Text = ":::. Page Setting, Add new page";
                this.txtBackColor.BackColor = Color.FromArgb(Policy.DEFAULT_BACKCOLOR);
                this.txtBackColor.Text = this.txtBackColor.BackColor.ToArgb().ToString();

                this.nmWidth.Value = Policy.DEFAULT_SCREEN_SIZE.X;
                this.nmHeaight.Value = Policy.DEFAULT_SCREEN_SIZE.Y;

                this.nmCellWidth.Value = Policy.DEFAULT_CELL_SIZE.Width;
                this.nmCellHeight.Value = Policy.DEFAULT_CELL_SIZE.Height;

                this.btnOk.Text = "&Add";
                this.txtNameId.Focus();
                return;
            }
            this.Text = ":::. Page Setting, " + _currentPage.GetNameId();
            this.txtNameId.Text = _currentPage.GetNameId();
            this.txtBackColor.BackColor = Color.FromArgb(_currentPage.GetBackColor());
            this.txtBackColor.Text = this.txtBackColor.BackColor.ToArgb().ToString();

            this.nmWidth.Value = _currentPage.GetScreenSize().X;
            this.nmHeaight.Value = _currentPage.GetScreenSize().Y;

            this.nmCellWidth.Value = _currentPage.GetCellSize().Width;
            this.nmCellHeight.Value = _currentPage.GetCellSize().Height;

            if (_currentPage.IsMainPage()) this.txtNameId.Enabled = false;
        }

        private void nmCellW_ValueChanged(object sender, EventArgs e)
        {
            this.nmCellHeight.Value = this.nmCellWidth.Value;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Point scrSize = new Point((int)this.nmWidth.Value, (int)this.nmHeaight.Value);
            Size cellSize = new Size((int)this.nmCellWidth.Value, (int)this.nmCellHeight.Value);

            this.txtNameId.Text = this.txtNameId.Text.Trim();
            try
            {
                if (_currentPage == null)//add
                {
                    _currentPage = new Page(this.txtNameId.Text, scrSize, cellSize, this.txtBackColor.BackColor.ToArgb());
                }
                else
                {
                    if (_currentPage.GetNameId() != this.txtNameId.Text) _currentPage.SetNameId(this.txtNameId.Text);
                    _currentPage.SetCellSize(cellSize);
                    _currentPage.SetScreenSize(scrSize);
                    _currentPage.SetBackColor(this.txtBackColor.BackColor.ToArgb());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.txtNameId.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.txtNameId.Focus();
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        private void txtBackColor_DoubleClick(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                dlg.Color = this.txtBackColor.BackColor;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.txtBackColor.Text = (this.txtBackColor.BackColor = dlg.Color).ToArgb().ToString();
                }
            }
        }
    };
}
