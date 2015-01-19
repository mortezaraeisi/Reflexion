namespace REFLEXION_DESIGNER
{
    partial class frmPageSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCellHeight = new System.Windows.Forms.Label();
            this.lblCellWidth = new System.Windows.Forms.Label();
            this.nmCellHeight = new System.Windows.Forms.NumericUpDown();
            this.nmCellWidth = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblScrHeight = new System.Windows.Forms.Label();
            this.lblScrWidth = new System.Windows.Forms.Label();
            this.nmHeaight = new System.Windows.Forms.NumericUpDown();
            this.nmWidth = new System.Windows.Forms.NumericUpDown();
            this.txtBackColor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNameId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmCellHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmCellWidth)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmHeaight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(335, 45);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(38, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.txtBackColor_DoubleClick);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(155, 273);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(106, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(267, 273);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(106, 28);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "&Save";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCellHeight);
            this.groupBox2.Controls.Add(this.lblCellWidth);
            this.groupBox2.Controls.Add(this.nmCellHeight);
            this.groupBox2.Controls.Add(this.nmCellWidth);
            this.groupBox2.Location = new System.Drawing.Point(71, 180);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(302, 77);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = ":. Cell size";
            // 
            // lblCellHeight
            // 
            this.lblCellHeight.AutoSize = true;
            this.lblCellHeight.Location = new System.Drawing.Point(139, 28);
            this.lblCellHeight.Name = "lblCellHeight";
            this.lblCellHeight.Size = new System.Drawing.Size(39, 13);
            this.lblCellHeight.TabIndex = 1;
            this.lblCellHeight.Text = "height:";
            // 
            // lblCellWidth
            // 
            this.lblCellWidth.AutoSize = true;
            this.lblCellWidth.Location = new System.Drawing.Point(32, 28);
            this.lblCellWidth.Name = "lblCellWidth";
            this.lblCellWidth.Size = new System.Drawing.Size(35, 13);
            this.lblCellWidth.TabIndex = 1;
            this.lblCellWidth.Text = "width:";
            // 
            // nmCellHeight
            // 
            this.nmCellHeight.Location = new System.Drawing.Point(142, 44);
            this.nmCellHeight.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nmCellHeight.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nmCellHeight.Name = "nmCellHeight";
            this.nmCellHeight.ReadOnly = true;
            this.nmCellHeight.Size = new System.Drawing.Size(79, 20);
            this.nmCellHeight.TabIndex = 1;
            this.nmCellHeight.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // nmCellWidth
            // 
            this.nmCellWidth.Location = new System.Drawing.Point(35, 44);
            this.nmCellWidth.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nmCellWidth.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nmCellWidth.Name = "nmCellWidth";
            this.nmCellWidth.Size = new System.Drawing.Size(79, 20);
            this.nmCellWidth.TabIndex = 0;
            this.nmCellWidth.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nmCellWidth.ValueChanged += new System.EventHandler(this.nmCellW_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblScrHeight);
            this.groupBox1.Controls.Add(this.lblScrWidth);
            this.groupBox1.Controls.Add(this.nmHeaight);
            this.groupBox1.Controls.Add(this.nmWidth);
            this.groupBox1.Location = new System.Drawing.Point(71, 85);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(302, 77);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = ":. Screen size";
            // 
            // lblScrHeight
            // 
            this.lblScrHeight.AutoSize = true;
            this.lblScrHeight.Location = new System.Drawing.Point(139, 28);
            this.lblScrHeight.Name = "lblScrHeight";
            this.lblScrHeight.Size = new System.Drawing.Size(39, 13);
            this.lblScrHeight.TabIndex = 1;
            this.lblScrHeight.Text = "height:";
            // 
            // lblScrWidth
            // 
            this.lblScrWidth.AutoSize = true;
            this.lblScrWidth.Location = new System.Drawing.Point(32, 28);
            this.lblScrWidth.Name = "lblScrWidth";
            this.lblScrWidth.Size = new System.Drawing.Size(35, 13);
            this.lblScrWidth.TabIndex = 1;
            this.lblScrWidth.Text = "width:";
            // 
            // nmHeaight
            // 
            this.nmHeaight.Location = new System.Drawing.Point(142, 44);
            this.nmHeaight.Maximum = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.nmHeaight.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmHeaight.Name = "nmHeaight";
            this.nmHeaight.Size = new System.Drawing.Size(79, 20);
            this.nmHeaight.TabIndex = 1;
            this.nmHeaight.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // nmWidth
            // 
            this.nmWidth.Location = new System.Drawing.Point(35, 44);
            this.nmWidth.Maximum = new decimal(new int[] {
            45,
            0,
            0,
            0});
            this.nmWidth.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmWidth.Name = "nmWidth";
            this.nmWidth.Size = new System.Drawing.Size(79, 20);
            this.nmWidth.TabIndex = 0;
            this.nmWidth.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // txtBackColor
            // 
            this.txtBackColor.Location = new System.Drawing.Point(71, 48);
            this.txtBackColor.Name = "txtBackColor";
            this.txtBackColor.ReadOnly = true;
            this.txtBackColor.Size = new System.Drawing.Size(258, 20);
            this.txtBackColor.TabIndex = 1;
            this.txtBackColor.DoubleClick += new System.EventHandler(this.txtBackColor_DoubleClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Back Color:";
            // 
            // txtNameId
            // 
            this.txtNameId.Location = new System.Drawing.Point(71, 12);
            this.txtNameId.Name = "txtNameId";
            this.txtNameId.Size = new System.Drawing.Size(302, 20);
            this.txtNameId.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Name Id:";
            // 
            // frmPageSettings
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(397, 308);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtBackColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNameId);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPageSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "::. Page Settings";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmCellHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmCellWidth)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmHeaight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblCellHeight;
        private System.Windows.Forms.Label lblCellWidth;
        private System.Windows.Forms.NumericUpDown nmCellHeight;
        private System.Windows.Forms.NumericUpDown nmCellWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblScrHeight;
        private System.Windows.Forms.Label lblScrWidth;
        private System.Windows.Forms.NumericUpDown nmHeaight;
        private System.Windows.Forms.NumericUpDown nmWidth;
        private System.Windows.Forms.TextBox txtBackColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNameId;
        private System.Windows.Forms.Label label1;
    }
}