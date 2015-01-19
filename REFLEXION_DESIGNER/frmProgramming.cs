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
    public partial class frmProgramming : Form
    {
        private Page _page;
        private string _defaulObjName;
        private class RECORD
        {
            public string Text;
            public REFLEXION_LIB.Object.BaseObject Object;
            public int Index;
            public override string ToString()
            {
                return Text;
            }
        }
        public frmProgramming(Page pg, string objName)
        {
            InitializeComponent();
            _page = pg;
            _defaulObjName = objName;

            this.Text = "::. Programming, " + pg.GetNameId();
            this.loadBlocks();
        }

        private void loadBlocks()
        {
            this.cmbBlock.Items.Clear();
            string pgName = _page.GetNameId();
            this.cmbBlock.Items.Add(new RECORD() { Text = "::init", Index = 0 });
            this.cmbBlock.Items.Add(new RECORD() { Text = "::start", Index = 1 });
            this.cmbBlock.SelectedIndex = 0;
            foreach (var o in _page.GetEnumerator())
            {
                if (!(o is REFLEXION_LIB.Object.Ball)) this.cmbBlock.Items.Add(
                    new RECORD()
                    {
                        Text = "." + o.GetNameId() + "::ballenter",
                        Object = o,
                        Index = this.cmbBlock.Items.Count
                    });

                if (o.GetNameId() == _defaulObjName) this.cmbBlock.SelectedIndex = this.cmbBlock.Items.Count - 1;
            }
        }
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.richTextBox1.ZoomFactor = this.trackBar1.Value;
        }

        private void btnSyntaxCheck_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.richTextBox1.Text)) return;

            this.listBox1.Items.Add("-----------------------------------------------");
            this.listBox1.Items.Add(REFLEXION_LIB.Programming.ProgramBlock.INFO);
            this.listBox1.Items.Add("Compile start at: " + DateTime.Now.ToString());
            var blc = REFLEXION_LIB.Programming.ProgramBlock.Create(this.richTextBox1.Text, _page.GetNameId() + this.cmbBlock.Text, _page);
            this.listBox1.Items.Add(blc.GetSource());
            int errorCount = 0;
            foreach (var r in blc.GetErrors())
            {
                this.listBox1.Items.Add((++errorCount).ToString("000_ ") + r);
            }
            this.listBox1.Items.Add("*****************end. error counts: " + errorCount.ToString());
            this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;

            if (this.splitContainer1.Panel2Collapsed) this.lblMORE_Click(null, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.listBox1.Items.Clear();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "Text format|*.txt";
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.IO.StreamWriter writer = null;
                    try
                    {
                        writer = new System.IO.StreamWriter(dlg.FileName);
                        foreach (var o in this.listBox1.Items) writer.WriteLine(o.ToString());
                        writer.Flush();
                        writer.Close();
                        MessageBox.Show("Error log saved to:\n" + dlg.FileName, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:\n" + ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (writer != null) writer.Close();
                    }
                }
            }
        }

        private void lblMORE_Click(object sender, EventArgs e)
        {
            this.lblMORE.Text = (this.splitContainer1.Panel2Collapsed = !this.splitContainer1.Panel2Collapsed) ? "<<" : ">>";
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6) this.btnSyntaxCheck_Click(null, null);
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            var blc = REFLEXION_LIB.Programming.ProgramBlock.Create(this.richTextBox1.Text, _page.GetNameId() + this.cmbBlock.Text, _page);
            RECORD r = (RECORD)this.cmbBlock.SelectedItem;
            if (r.Index == 0)//first_page::init
            { _page.SetInitBlock(blc); _page.Load(); }
            else if (r.Index == 1)//firstpage::start
                _page.SetStartBlock(blc);
            else
            {
                r.Object.SetBallEnterBlock(blc);
            }
        }

        private void cmbBlock_SelectedIndexChanged(object sender, EventArgs e)
        {
            REFLEXION_LIB.Programming.ProgramBlock blc;
            if (this.cmbBlock.SelectedIndex == 0) blc = _page.GetInitBlock();
            else if (this.cmbBlock.SelectedIndex == 1) blc = _page.GetStartBlock();
            else
            {
                RECORD r = (RECORD)this.cmbBlock.SelectedItem;
                blc = r.Object.GetBallEnterBlock();
            }
            if (blc == null)
                this.richTextBox1.Text = string.Empty;
            else
                this.richTextBox1.Text = blc.GetCodes();
        }
    };
}
