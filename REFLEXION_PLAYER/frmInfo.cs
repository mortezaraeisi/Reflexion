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

namespace REFLEXION_PLAYER
{
    public partial class frmInfo : Form
    {
        public frmInfo(Game game)
        {
            InitializeComponent();

            this.Text = "::. Reflexion game Info";
            this.lblGameNameId.Text = game.NameId;
            this.lblAuthor.Text = game.Author;
            this.lblEmail.Text = game.Email;
            this.lblExp.Text = game.Explanation;
        }

        private void lblEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.lblEmail.Text))
                try
                {
                    System.Diagnostics.Process.Start("mailto:" + this.lblEmail.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on email", MessageBoxButtons.OK, MessageBoxIcon.Error); ;
                }
        }
    };
}
