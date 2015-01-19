using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REFLEXION_PLAYER
{
    public partial class frmMenu : Form
    {
        public frmMenu(bool thereIsGame)
        {
            InitializeComponent();
            if (!thereIsGame)
            {
                this.lnkResume.Enabled =
                    this.lnkRestart.Enabled =
                    this.lnkLevelInfo.Enabled = false;
            }
        }

        private void lnkResume_Leave(object sender, EventArgs e)
        {
            ((LinkLabel)sender).BackColor = Color.Black;
        }

        private void lnkResume_MouseHover(object sender, EventArgs e)
        {
            ((LinkLabel)sender).BackColor = Color.FromArgb(64, 64, 64);
        }

        private void lnkResume_MouseDown(object sender, MouseEventArgs e)
        {
            ((LinkLabel)sender).BackColor = Color.FromArgb(32, 32, 32);
        }

        private void lnkResume_MouseUp(object sender, MouseEventArgs e)
        {
            ((LinkLabel)sender).BackColor = Color.FromArgb(50, 50, 50);
        }
        private void lnkResume_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        private void lnkRestart_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Retry;
        }

        private void lnkLoad_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
        }

        private void lnkLevelInfo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void lnkSetting_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void lnkClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
        }

        private void frmMenu_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
