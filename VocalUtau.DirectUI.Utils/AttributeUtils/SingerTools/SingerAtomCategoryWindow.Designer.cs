namespace VocalUtau.DirectUI.Utils.AttributeUtils.SingerTools
{
    partial class SingerAtomCategoryWindow
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
            this.list_Singer = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_Name = new System.Windows.Forms.TextBox();
            this.txt_Dir = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_Resampler = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_Flags = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_GUID = new System.Windows.Forms.TextBox();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_CreateNew = new System.Windows.Forms.Button();
            this.btn_DelSinger = new System.Windows.Forms.Button();
            this.btn_SaveAsSystem = new System.Windows.Forms.Button();
            this.btn_EditOto = new System.Windows.Forms.Button();
            this.btn_BrowseResampler = new System.Windows.Forms.Button();
            this.btn_BrowseVoiceDir = new System.Windows.Forms.Button();
            this.UtauPic = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.UtauPic)).BeginInit();
            this.SuspendLayout();
            // 
            // list_Singer
            // 
            this.list_Singer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.list_Singer.FormattingEnabled = true;
            this.list_Singer.ItemHeight = 15;
            this.list_Singer.Location = new System.Drawing.Point(26, 44);
            this.list_Singer.Name = "list_Singer";
            this.list_Singer.Size = new System.Drawing.Size(269, 259);
            this.list_Singer.TabIndex = 0;
            this.list_Singer.SelectedIndexChanged += new System.EventHandler(this.list_Singer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "当前歌手清单：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(337, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "歌手名(音色名)：";
            // 
            // txt_Name
            // 
            this.txt_Name.Location = new System.Drawing.Point(471, 16);
            this.txt_Name.Name = "txt_Name";
            this.txt_Name.Size = new System.Drawing.Size(358, 25);
            this.txt_Name.TabIndex = 3;
            // 
            // txt_Dir
            // 
            this.txt_Dir.Location = new System.Drawing.Point(471, 111);
            this.txt_Dir.Name = "txt_Dir";
            this.txt_Dir.Size = new System.Drawing.Size(273, 25);
            this.txt_Dir.TabIndex = 5;
            this.txt_Dir.TextChanged += new System.EventHandler(this.txt_Dir_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(337, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "VoiceDir：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(337, 166);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "默认重采样器：";
            // 
            // txt_Resampler
            // 
            this.txt_Resampler.Location = new System.Drawing.Point(471, 163);
            this.txt_Resampler.Name = "txt_Resampler";
            this.txt_Resampler.Size = new System.Drawing.Size(358, 25);
            this.txt_Resampler.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(337, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "默认Flags：";
            // 
            // txt_Flags
            // 
            this.txt_Flags.Location = new System.Drawing.Point(471, 215);
            this.txt_Flags.Name = "txt_Flags";
            this.txt_Flags.Size = new System.Drawing.Size(446, 25);
            this.txt_Flags.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(337, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "唯一标识符：";
            // 
            // txt_GUID
            // 
            this.txt_GUID.Location = new System.Drawing.Point(471, 65);
            this.txt_GUID.Name = "txt_GUID";
            this.txt_GUID.ReadOnly = true;
            this.txt_GUID.Size = new System.Drawing.Size(358, 25);
            this.txt_GUID.TabIndex = 5;
            // 
            // btn_Save
            // 
            this.btn_Save.Location = new System.Drawing.Point(496, 274);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(89, 29);
            this.btn_Save.TabIndex = 10;
            this.btn_Save.Text = "保存";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_CreateNew
            // 
            this.btn_CreateNew.Location = new System.Drawing.Point(356, 274);
            this.btn_CreateNew.Name = "btn_CreateNew";
            this.btn_CreateNew.Size = new System.Drawing.Size(93, 29);
            this.btn_CreateNew.TabIndex = 11;
            this.btn_CreateNew.Text = "新建";
            this.btn_CreateNew.UseVisualStyleBackColor = true;
            this.btn_CreateNew.Click += new System.EventHandler(this.btn_CreateNew_Click);
            // 
            // btn_DelSinger
            // 
            this.btn_DelSinger.Location = new System.Drawing.Point(636, 274);
            this.btn_DelSinger.Name = "btn_DelSinger";
            this.btn_DelSinger.Size = new System.Drawing.Size(89, 29);
            this.btn_DelSinger.TabIndex = 12;
            this.btn_DelSinger.Text = "删除";
            this.btn_DelSinger.UseVisualStyleBackColor = true;
            this.btn_DelSinger.Click += new System.EventHandler(this.btn_DelSinger_Click);
            // 
            // btn_SaveAsSystem
            // 
            this.btn_SaveAsSystem.Location = new System.Drawing.Point(767, 274);
            this.btn_SaveAsSystem.Name = "btn_SaveAsSystem";
            this.btn_SaveAsSystem.Size = new System.Drawing.Size(132, 29);
            this.btn_SaveAsSystem.TabIndex = 13;
            this.btn_SaveAsSystem.Text = "注册为系统歌姬";
            this.btn_SaveAsSystem.UseVisualStyleBackColor = true;
            // 
            // btn_EditOto
            // 
            this.btn_EditOto.Location = new System.Drawing.Point(838, 111);
            this.btn_EditOto.Name = "btn_EditOto";
            this.btn_EditOto.Size = new System.Drawing.Size(82, 29);
            this.btn_EditOto.TabIndex = 14;
            this.btn_EditOto.Text = "声库编辑";
            this.btn_EditOto.UseVisualStyleBackColor = true;
            // 
            // btn_BrowseResampler
            // 
            this.btn_BrowseResampler.Location = new System.Drawing.Point(835, 160);
            this.btn_BrowseResampler.Name = "btn_BrowseResampler";
            this.btn_BrowseResampler.Size = new System.Drawing.Size(82, 29);
            this.btn_BrowseResampler.TabIndex = 15;
            this.btn_BrowseResampler.Text = "浏览...";
            this.btn_BrowseResampler.UseVisualStyleBackColor = true;
            this.btn_BrowseResampler.Click += new System.EventHandler(this.btn_BrowseResampler_Click);
            // 
            // btn_BrowseVoiceDir
            // 
            this.btn_BrowseVoiceDir.Location = new System.Drawing.Point(750, 111);
            this.btn_BrowseVoiceDir.Name = "btn_BrowseVoiceDir";
            this.btn_BrowseVoiceDir.Size = new System.Drawing.Size(82, 29);
            this.btn_BrowseVoiceDir.TabIndex = 16;
            this.btn_BrowseVoiceDir.Text = "浏览...";
            this.btn_BrowseVoiceDir.UseVisualStyleBackColor = true;
            this.btn_BrowseVoiceDir.Click += new System.EventHandler(this.btn_BrowseVoiceDir_Click);
            // 
            // UtauPic
            // 
            this.UtauPic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UtauPic.Location = new System.Drawing.Point(838, 17);
            this.UtauPic.Margin = new System.Windows.Forms.Padding(4);
            this.UtauPic.Name = "UtauPic";
            this.UtauPic.Size = new System.Drawing.Size(79, 70);
            this.UtauPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.UtauPic.TabIndex = 17;
            this.UtauPic.TabStop = false;
            // 
            // SingerAtomCategoryWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 326);
            this.Controls.Add(this.UtauPic);
            this.Controls.Add(this.btn_BrowseVoiceDir);
            this.Controls.Add(this.btn_BrowseResampler);
            this.Controls.Add(this.btn_EditOto);
            this.Controls.Add(this.btn_SaveAsSystem);
            this.Controls.Add(this.btn_DelSinger);
            this.Controls.Add(this.btn_CreateNew);
            this.Controls.Add(this.btn_Save);
            this.Controls.Add(this.txt_Flags);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_Resampler);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_GUID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txt_Dir);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Name);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.list_Singer);
            this.Name = "SingerAtomCategoryWindow";
            this.Text = "SingerAtomCategoryWindow";
            this.Load += new System.EventHandler(this.SingerAtomCategoryWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UtauPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox list_Singer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_Name;
        private System.Windows.Forms.TextBox txt_Dir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_Resampler;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_Flags;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_GUID;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_CreateNew;
        private System.Windows.Forms.Button btn_DelSinger;
        private System.Windows.Forms.Button btn_SaveAsSystem;
        private System.Windows.Forms.Button btn_EditOto;
        private System.Windows.Forms.Button btn_BrowseResampler;
        private System.Windows.Forms.Button btn_BrowseVoiceDir;
        private System.Windows.Forms.PictureBox UtauPic;
    }
}