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
    public partial class frmGameSettings : Form
    {
        private Game _game;
        public Game Result { get { return _game; } }
        public frmGameSettings(Game game)
        {
            InitializeComponent();
            this.load(game);
        }
        private void load(Game game)
        {
            _game = game;
            this.btnReset_Click(null, null);
            this.loadPages();
        }
        private void loadPages()
        {
            this.listView1.Items.Clear();
            foreach (var t in _game.Pages)
            {
                ListViewItem lvi = new ListViewItem(t.GetNameId());
                lvi.SubItems.Add(t.GetScreenSize().ToString());
                lvi.SubItems.Add(t.GetCellSize().ToString());
                this.listView1.Items.Add(lvi);
            }
            this.listView1.Items[0].Selected = true;
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.txtNameId.Text = _game.NameId;
            this.txtAuthor.Text = _game.Author;
            this.txtEmail.Text = _game.Email;
            this.txtExplanation.Text = _game.Explanation;
            this.txtNameId.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _game.NameId = this.txtNameId.Text;
            _game.Author = this.txtAuthor.Text;
            _game.Email = this.txtEmail.Text;
            _game.Explanation = this.txtExplanation.Text;
           // this.btnReset_Click(null, null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmPageSettings frm = new frmPageSettings();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _game.Add(frm.Result);
                this.loadPages();
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0) return;
            int index = this.listView1.SelectedIndices[0];
            Page pg = _game.Find(this.listView1.Items[index].Text);
            frmPageSettings frm = new frmPageSettings(pg);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.loadPages();
            this.listView1.Items[index].Selected = true;
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0) return;
            int index = this.listView1.SelectedIndices[0];
            Page pg = _game.Find(this.listView1.Items[index].Text);
            if (pg.IsMainPage())
            {
                MessageBox.Show("Main page, cannot removed!", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Sure to delete page of '" + pg.GetNameId() + "'\nAll page info will be removed. there is no undo!\nSure?", "Remove",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    _game.Remove(pg);
                    frmMain.RemovePage(pg);
                    this.loadPages();
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.btnSave_Click(null, null);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    };
}
