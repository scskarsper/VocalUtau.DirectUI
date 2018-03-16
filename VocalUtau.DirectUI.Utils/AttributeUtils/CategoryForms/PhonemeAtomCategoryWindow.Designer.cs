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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_pa_Start)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_OK
            // 
            this.btn_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_OK.Location = new System.Drawing.Point(23, 468);
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
            this.btn_Cancel.Location = new System.Drawing.Point(114, 468);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 1;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "音符总长：";
            // 
            // lbl_Length
            // 
            this.lbl_Length.AutoSize = true;
            this.lbl_Length.Location = new System.Drawing.Point(65, 141);
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
            this.lbl_NoteView.Location = new System.Drawing.Point(91, 43);
            this.lbl_NoteView.Name = "lbl_NoteView";
            this.lbl_NoteView.Size = new System.Drawing.Size(374, 31);
            this.lbl_NoteView.TabIndex = 3;
            this.lbl_NoteView.Text = "label2";
            this.lbl_NoteView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnl_Phoneme
            // 
            this.pnl_Phoneme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnl_Phoneme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pnl_Phoneme.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnl_Phoneme.Location = new System.Drawing.Point(91, 77);
            this.pnl_Phoneme.Name = "pnl_Phoneme";
            this.pnl_Phoneme.Size = new System.Drawing.Size(374, 30);
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
            this.ctl_pa_Start.Location = new System.Drawing.Point(84, 112);
            this.ctl_pa_Start.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.ctl_pa_Start.Maximum = 10000;
            this.ctl_pa_Start.Name = "ctl_pa_Start";
            this.ctl_pa_Start.Size = new System.Drawing.Size(388, 16);
            this.ctl_pa_Start.TabIndex = 6;
            this.ctl_pa_Start.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.ctl_pa_Start.Value = 13;
            this.ctl_pa_Start.Scroll += new System.EventHandler(this.ctl_pa_Start_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "音符歌词：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "发音部件图：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "发音起始点：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "发音部件长度类型：";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(288, 140);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(60, 16);
            this.checkBox1.TabIndex = 12;
            this.checkBox1.Text = "百分比";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // PhonemeAtomCategoryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 503);
            this.Controls.Add(this.checkBox1);
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
            this.Name = "PhonemeAtomCategoryWindow";
            this.Text = "PhonemeAtomCategoryWindow";
            this.Load += new System.EventHandler(this.PhonemeAtomCategoryWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ctl_pa_Start)).EndInit();
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
        private System.Windows.Forms.CheckBox checkBox1;
    }
}