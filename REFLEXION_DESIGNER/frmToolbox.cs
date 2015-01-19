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
    public partial class frmToolbox : Form
    {
        private List<Type> _toolsTypeList;
        private static REFLEXION_LIB.Object.BaseObject _selectedTool;
        public static REFLEXION_LIB.Object.BaseObject SelectedTool { get { return _selectedTool; } }
        public static void DeselectAnyTool() { _selectedTool = null; }
        public static void ReSelectTool() { _selectedTool = Policy.CreateInstance(_selectedTool.GetType()); }

        public frmToolbox()
        {
            InitializeComponent();
            this.loadItems();
        }

        private void loadItems()
        {
            this.listView1.Items.Clear();
            this.listView1.Items.Add("(none)", -1);
            _selectedTool = null;
            _toolsTypeList = new List<Type>();
            string n, x;

            Rectangle rBig = new Rectangle(0, 0, 110, 110);
            ImageList imgListBig = new ImageList() { ImageSize = rBig.Size };

            Rectangle rSmall = new Rectangle(0, 0, 36, 36);
            ImageList imgListSmall = new ImageList() { ImageSize = rSmall.Size };
                        this.listView1.SmallImageList = imgListSmall;
            this.listView1.LargeImageList = imgListBig;
            Image cImg;
            Graphics gr;
            foreach (var t in REFLEXION_LIB.Policy.GetRegisteredProgrammableObject())
            {
                _toolsTypeList.Add(t);
                REFLEXION_LIB.Object.BaseObject obj = Policy.CreateInstance(t);
                Policy.GetExplanationAttribute(t, out n, out x);

                cImg = new Bitmap(rBig.Width, rBig.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                gr = Graphics.FromImage(cImg);
                obj.Drawn(gr, rBig.Location, rBig.Size);
                imgListBig.Images.Add(cImg);

                cImg = new Bitmap(rSmall.Width, rSmall.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                gr = Graphics.FromImage(cImg);
                obj.Drawn(gr, rSmall.Location, rSmall.Size);
                imgListSmall.Images.Add(cImg);

                this.listView1.Items.Add(new ListViewItem(new string[] { n, x }, imgListBig.Images.Count - 1));
            }
        }
        private void largeIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.LargeIcon;
        }
        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.Details;
        }
        private void smallIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.SmallIcon;
        }
        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.List;
        }
        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.View = View.Tile;
        }
        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listView1.Items[0].Selected = true;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count == 0 || this.listView1.SelectedItems[0].Index == 0)
            {
                _selectedTool = null;
                return;
            }
            int I = this.listView1.SelectedItems[0].ImageIndex;
            if (I < 0) _selectedTool = null;
            else _selectedTool = Policy.CreateInstance(_toolsTypeList[I]);

            //string strTpye = this.listView1.SelectedItems[0].Text;
            //Type tp = null;
            //foreach (var t in _toolsTypeList) if (t.Name == strTpye) { tp = t; break; }
            //if (tp != null) _selectedTool = Policy.CreateInstance(tp);
        }

        private void frmToolbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

    };
}
