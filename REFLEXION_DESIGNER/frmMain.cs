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
using System.Diagnostics;

namespace REFLEXION_DESIGNER
{
    public partial class frmMain : Form
    {
        private static frmMain _frmMain;
        private frmToolbox _frmToolBox;
        private Action _stateSaved;
        private bool _stateNotSaved;

        public frmMain()
        {
            InitializeComponent();
            _frmMain = this;
            projectToolStripMenuItem.Visible = false;
            this.Text = ":::. MA.rv Group[Rme] Reflexion level designer";
            _frmToolBox = new frmToolbox();
            _frmToolBox.MdiParent = this;
            _frmToolBox.Location = new Point();
            _frmToolBox.Show();
        }

        public static void RemovePage(Page pg)
        {
            foreach(var f in _frmMain.MdiChildren)
            {
                frmPage fpg = f as frmPage;
                if (fpg != null && fpg.ExitPage(pg)) 
                    {
                        _frmMain.windowsToolStripMenuItem.DropDownItems.RemoveByKey(pg.GetNameId());
                        break;
                    }
            }
        }
        public void StateChanged() { _stateNotSaved = true; }
        public void ShowGameSettings()
        {
            new frmGameSettings(Project.Option.Game).ShowDialog();
            this.updatePageForms();
            this.StateChanged();
        }
        public void Play()
        {
            this.saveToolStripMenuItem_Click(null, null);
            if (_stateNotSaved) return;
            string path = Project.Option.Path.Replace(".refprj", string.Empty) + REFLEXION_LIB.Policy.REFLEXION_GAME_FILE_EXTENSION;
            try
            {
                Project.Publish(path, Project.Option.Game);
                new global::REFLEXION_PLAYER.frmMain(path).ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Run failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void save(string path)
        {
            try
            {
                Project.Option.SaveToFile(path);
                _stateNotSaved = false;
                _stateSaved();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot save to:\n" + Project.Option.Path + "\n\nError:" + ex.Message);
            }
        }
        private void open(string path)
        {
            try
            {
                Project.LoadFromFile(path);
                this.loadGameInfo();
                _stateNotSaved = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Project not loaded!:\n" + ex.Message);
            }
        }
        private void loadGameInfo()
        {
            this.projectToolStripMenuItem.Visible = true;
            this.Text = string.Format(":::. {0} - MA.rv Group[Rme] Reflexion level designer", Project.Option.Game.NameId);
            foreach (var f in this.MdiChildren)
            {
                var frm = f as frmPage;
                if (frm != null) frm.ExitPage();
            }
            this.updatePageForms();
        }
        private void updatePageForms()
        {
            foreach (var p in Project.Option.Game.Pages)
            {
                bool res = false;
                foreach (var f in this.MdiChildren)
                {
                    frmPage frm = f as frmPage;
                    if (frm == null) continue;

                    if (frm.Result.GetNameId() == p.GetNameId()) { res = true; break; }
                }//endeach f
                if (res) continue;
                this.initPageForm(p);
            }//endeachp
        }
        private void initPageForm(Page pg)
        {

            ToolStripMenuItem mnu = new ToolStripMenuItem(pg.GetNameId());
            mnu.Name = pg.GetNameId();
            this.windowsToolStripMenuItem.DropDownItems.Add(mnu);
            frmPage frm = new frmPage(pg);
            frm.Name = pg.GetNameId();
            frm.MdiParent = this;
            frm.StartPosition = FormStartPosition.WindowsDefaultLocation;
            frm.Activated += (s, e) => mnu.Checked = true;
            frm.Deactivate += (s, e) => mnu.Checked = false;
            frm.FormClosed += (s, e) => this.windowsToolStripMenuItem.DropDownItems.Remove(mnu);
            mnu.Click += (s, e) => frm.Show();
            _stateSaved += frm.StateSave;
            frm.Show();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_stateNotSaved)
            {
                DialogResult res = MessageBox.Show("Project not saved! Do you wanna to save first?", "New Project", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (res == System.Windows.Forms.DialogResult.Cancel) return;
                else if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    this.saveToolStripMenuItem_Click(null, null);
                    if (_stateNotSaved) return;
                }
            }
            Project.CreateNewProject();
            this.loadGameInfo();
            this.propertiesToolStripMenuItem_Click(null, null);
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_stateNotSaved)
            {
                DialogResult res = MessageBox.Show("Opening...\nDo you wanna save current project?", "Reflexion Designer: Open", 
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    this.saveToolStripMenuItem_Click(null, null);
                    if (_stateNotSaved) return;
                }
            }

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "Reflexion project|*.refprj";
                dlg.Title = "Open Reflexion project";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.open(dlg.FileName);
                }
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_stateNotSaved) return;
            string path = Project.Option.Path;

            if (string.IsNullOrEmpty(path))
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Filter = "Reflexion project|*.refprj";
                    dlg.Title = "Save Reflexion project";
                    dlg.FileName = Project.Option.Game.NameId;
                    if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        this.save(dlg.FileName);
                    return;
                }
            this.save(path);
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Reflexion project|*.refprj";
                dlg.Title = "Save Reflexion project";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    this.save(dlg.FileName);
                
            }          
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }
        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }
        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }
        private void arrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_stateNotSaved)
            {
                DialogResult res = MessageBox.Show("Closing...\nDo you wanna save project?", "Reflexion Designer:", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == System.Windows.Forms.DialogResult.Cancel)
                { e.Cancel = true; return; }
                else if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    this.saveToolStripMenuItem_Click(null, null);
                    if (e.Cancel = _stateNotSaved) return;
                }//else do closing normally!
            }
            Environment.Exit(0);
        }

        private void toolboxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmToolBox.Show();
        }
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Play();
        }
        private void publishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Project.Option.Game != null)
                new frmPublish(Project.Option.Game).ShowDialog();
        }
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmGameSettings(Project.Option.Game) { StartPosition = FormStartPosition.CenterScreen }.ShowDialog();
            this.loadGameInfo();
            _stateNotSaved = true;
        }      
    };
}
