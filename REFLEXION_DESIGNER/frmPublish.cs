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
    public partial class frmPublish : Form
    {
        private Game _game;
        public frmPublish(Game game)
        {
            InitializeComponent();
            _game = game;
            loadGameInfo();
        }

        private void loadGameInfo()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine(REFLEXION_LIB.Policy.REFLEXION_GAME_NAME);
            str.AppendLine("@Reference Assembly: " + System.Reflection.Assembly.GetAssembly(typeof(REFLEXION_LIB.Game)).FullName);
            str.AppendLine("Author: " + REFLEXION_LIB.Policy.REFLEXION_GAME_AUTHOR);
            str.AppendLine("mailto:" + REFLEXION_LIB.Policy.REFLEXION_GAME_AUTHOR_EMAIL);

            str.AppendLine("\r\n***Product info:");
            str.AppendFormat("\tName: {0}\r\n",_game.NameId);
            str.AppendFormat("\tAuthor: {0}\r\n", _game.Author);
            str.AppendFormat("\tEmail: mailto:{0}\r\n", _game.Email);
            str.AppendFormat("\tDescription: {0}\r\n", _game.Explanation);

            this.txtGameInfo.Text = str.ToString();

            str = new StringBuilder();
            foreach (var p in _game.Pages)
            {
                str.AppendLine(p.GetNameId() + "::init");
                if (p.GetInitBlock() != null)
                    str.AppendLine("\t" + p.GetInitBlock().GetCodes().Replace("\n", "\n\t"));
                else str.AppendLine("\t[no code]");

                str.AppendLine(p.GetNameId() + "::start");
                if (p.GetStartBlock() != null)
                    str.AppendLine("\t" + p.GetStartBlock().GetCodes().Replace("\n", "\n\t"));
                else str.AppendLine("\t[no code]");

                foreach (var o in p.GetEnumerator())
                {
                    str.AppendLine();
                    var b = o.GetBallEnterBlock();
                    str.AppendFormat("\t\t{0}.{1}::enter\r\n\t\t\t", p.GetNameId(), o.GetNameId());
                    if (o.GetBallEnterBlock() != null)
                        str.AppendLine(o.GetBallEnterBlock().GetCodes().Replace("\n", "\n\t\t\t"));
                    else str.AppendLine("\t[no code]");
                }//endeach o
            }//endeach p
            this.txtPrograms.Text = str.ToString();
        }

        private void btnEditInfo_Click(object sender, EventArgs e)
        {
            new frmGameSettings(_game).ShowDialog();
            this.loadGameInfo();
        }
        private void btnNextInfo_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtPath.Text))
                this.button3_Click(null, null);
            if (string.IsNullOrWhiteSpace(this.txtPath.Text)) return;

            try
            {
                Project.Publish(this.txtPath.Text, _game);
                MessageBox.Show("Published at:\n" + this.txtPath.Text, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                if (MessageBox.Show("Error!\n" + ex.Message + "\n\nDo you wanna to change destination?", 
                    "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.Yes)
                    this.button3_Click(null, null);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = Policy.REFLEXION_GAME_NAME + "|*" + Policy.REFLEXION_GAME_FILE_EXTENSION;
                dlg.FileName = _game.NameId;
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.txtPath.Text = dlg.FileName;
            }
        }

    };
}
