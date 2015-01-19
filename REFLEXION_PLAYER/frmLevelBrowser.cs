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
    public partial class frmLevelBrowser : Form
    {
        private string _result;
        public string Result { get { return _result; } }

        public frmLevelBrowser()
        {
            InitializeComponent();
            this.button3_Click(null, null);
        }

        private void loadLevels(string spacePath)
        {
            if (string.IsNullOrWhiteSpace(spacePath)) return;
            if (!System.IO.Directory.Exists(spacePath))
            {
                if (MessageBox.Show("'LevelSpace' directory not exist!\nTry to browse another directory?", "Load", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                    != System.Windows.Forms.DialogResult.OK)
                    this.button1_Click(null, null);
                return;
            }
           
            this.textBox1.Text = Settings.Option.SpacePath = spacePath;
            this.listView1.Items.Clear();
            foreach (var f in System.IO.Directory.GetFiles
                (Settings.Option.SpacePath, "*" + REFLEXION_LIB.Policy.REFLEXION_GAME_FILE_EXTENSION))
            {
                this.listView1.Items.Add(f);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.loadLevels(dlg.SelectedPath);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.loadLevels(Settings.Option.SpacePath);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(this.listView1.Items.Count>0 && this.listView1.SelectedItems.Count>0)
            {
                _result = this.listView1.SelectedItems[0].Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    };
}
