namespace VocalUtau.DirectUI.Utils.AttributeUtils.CategoryForms
{
    partial class PhonemeAtomCategoryWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PhonemeAtomCategoryWindow));
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_Length = new System.Windows.Forms.Label();
            this.lbl_NoteView = new System.Windows.Forms.Label();
            this.pnl_Phoneme = new System.Windows.Forms.Panel();
            this.ctl_pa_Start = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.chk_Bfb = new System.Windows.Forms.CheckBox();
            this.AtomPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.chk_ZyB = new System.Windows.Forms.CheckBox();
            this.dynPainterBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_pa_Start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dynPainterBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(23, 451);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 23);
            this.btn_OK.TabIndex = 0;
            this.btn_OK.Text = "确定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Cancel.Location = new System.Drawing.Point(114, 451);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "音符总长：";
            // 
            // lbl_Length
            // 
            this.lbl_Length.AutoSize = true;
            this.lbl_Length.Location = new System.Drawing.Point(65, 107);
            this.lbl_Length.Name = "lbl_Length";
            this.lbl_Length.Size = new System.Drawing.Size(41, 12);
            this.lbl_Length.TabIndex = 2;
            this.lbl_Length.Text = "Length";
            // 
            // lbl_NoteView
            // 
            this.lbl_NoteView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_NoteView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbl_NoteView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbl_NoteView.Location = new System.Drawing.Point(91, 10);
            this.lbl_NoteView.Name = "lbl_NoteView";
            this.lbl_NoteView.Size = new System.Drawing.Size(309, 31);
            this.lbl_NoteView.TabIndex = 3;
            this.lbl_NoteView.Text = "label2";
            this.lbl_NoteView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_Phoneme
            // 
            this.pnl_Phoneme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_Phoneme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pnl_Phoneme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Phoneme.Location = new System.Drawing.Point(91, 43);
            this.pnl_Phoneme.Name = "pnl_Phoneme";
            this.pnl_Phoneme.Size = new System.Drawing.Size(309, 30);
            this.pnl_Phoneme.TabIndex = 4;
            this.pnl_Phoneme.Resize += new System.EventHandler(this.pnl_Phoneme_Resize);
            // 
            // ctl_pa_Start
            // 
            this.ctl_pa_Start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_pa_Start.AutoSize = false;
            this.ctl_pa_Start.BackColor = System.Drawing.SystemColors.Control;
            this.ctl_pa_Start.LargeChange = 1;
            this.ctl_pa_Start.Location = new System.Drawing.Point(84, 78);
            this.ctl_pa_Start.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ctl_pa_Start.Maximum = 10000;
            this.ctl_pa_Start.Name = "ctl_pa_Start";
            this.ctl_pa_Start.Size = new System.Drawing.Size(322, 16);
            this.ctl_pa_Start.TabIndex = 6;
            this.ctl_pa_Start.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.ctl_pa_Start.Value = 13;
            this.ctl_pa_Start.Scroll += new System.EventHandler(this.ctl_pa_Start_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "音符歌词：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "发音部件图：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "发音起始点：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(154, 107);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "发音部件类型：";
            // 
            // chk_Bfb
            // 
            this.chk_Bfb.AutoSize = true;
            this.chk_Bfb.Location = new System.Drawing.Point(244, 106);
            this.chk_Bfb.Name = "chk_Bfb";
            this.chk_Bfb.Size = new System.Drawing.Size(60, 16);
            this.chk_Bfb.TabIndex = 12;
            this.chk_Bfb.Text = "百分比";
            this.chk_Bfb.UseVisualStyleBackColor = true;
            this.chk_Bfb.CheckedChanged += new System.EventHandler(this.chk_Bfb_CheckedChanged);
            // 
            // AtomPropertyGrid
            // 
            this.AtomPropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AtomPropertyGrid.LineColor = System.Drawing.SystemColors.ControlDark;
            this.AtomPropertyGrid.Location = new System.Drawing.Point(409, 10);
            this.AtomPropertyGrid.Margin = new System.Windows.Forms.Padding(2);
            this.AtomPropertyGrid.Name = "AtomPropertyGrid";
            this.AtomPropertyGrid.Size = new System.Drawing.Size(238, 425);
            this.AtomPropertyGrid.TabIndex = 13;
            // 
            // chk_ZyB
            // 
            this.chk_ZyB.AutoSize = true;
            this.chk_ZyB.Location = new System.Drawing.Point(306, 106);
            this.chk_ZyB.Name = "chk_ZyB";
            this.chk_ZyB.Size = new System.Drawing.Size(60, 16);
            this.chk_ZyB.TabIndex = 14;
            this.chk_ZyB.Text = "自由部";
            this.chk_ZyB.UseVisualStyleBackColor = true;
            this.chk_ZyB.CheckedChanged += new System.EventHandler(this.chk_ZyB_CheckedChanged);
            // 
            // dynPainterBox
            // 
            this.dynPainterBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dynPainterBox.Image = ((System.Drawing.Image)(resources.GetObject("dynPainterBox.Image")));
            this.dynPainterBox.Location = new System.Drawing.Point(6, 156);
            this.dynPainterBox.Name = "dynPainterBox";
            this.dynPainterBox.Size = new System.Drawing.Size(398, 176);
            this.dynPainterBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.dynPainterBox.TabIndex = 17;
            this.dynPainterBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "图例说明：";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Location = new System.Drawing.Point(4, 335);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(396, 100);
            this.label7.TabIndex = 19;
            this.label7.Text = "参数说明：\r\n    第一个发音元件的交叠参数和先行发音参数等价于整个发音的交叠参数和先行发音参数。\r\n    第一个和最后一个发音元件的淡入淡出参数分别用于设置" +
    "该发音整体的前置过度和后置过度\r\n    中间的发音元件包络淡入淡出过度与交叠（Overlap）等长。\r\n    自定义属性值为\"空\"时代表使用默认值，默认值来" +
    "自音源的oto设定。\r\n";
            // 
            // PhonemeAtomCategoryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 487);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dynPainterBox);
            this.Controls.Add(this.chk_ZyB);
            this.Controls.Add(this.AtomPropertyGrid);
            this.Controls.Add(this.chk_Bfb);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pnl_Phoneme);
            this.Controls.Add(this.lbl_NoteView);
            this.Controls.Add(this.lbl_Length);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.ctl_pa_Start);
            this.Controls.Add(this.label5);
            this.MinimumSize = new System.Drawing.Size(671, 526);
            this.Name = "PhonemeAtomCategoryWindow";
            this.Text = "PhonemeAtomCategoryWindow";
            this.Load += new System.EventHandler(this.PhonemeAtomCategoryWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ctl_pa_Start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dynPainterBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_Length;
        private System.Windows.Forms.Label lbl_NoteView;
        private System.Windows.Forms.Panel pnl_Phoneme;
        private System.Windows.Forms.TrackBar ctl_pa_Start;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chk_Bfb;
        private System.Windows.Forms.PropertyGrid AtomPropertyGrid;
        private System.Windows.Forms.CheckBox chk_ZyB;
        private System.Windows.Forms.PictureBox dynPainterBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
    }
}