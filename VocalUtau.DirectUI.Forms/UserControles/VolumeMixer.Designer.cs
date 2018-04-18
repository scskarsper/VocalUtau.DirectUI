namespace VocalUtau.DirectUI.Forms.UserControles
{
    partial class VolumeMixer
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VolumeMixer));
            this.btnMuteUnmute = new System.Windows.Forms.Button();
            this.ilMuteUnmute = new System.Windows.Forms.ImageList(this.components);
            this.tbVolume = new System.Windows.Forms.TrackBar();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.num_Magnification = new System.Windows.Forms.NumericUpDown();
            this.txt_tName = new System.Windows.Forms.TextBox();
            this.num_Volume = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Magnification)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Volume)).BeginInit();
            this.SuspendLayout();
            // 
            // btnMuteUnmute
            // 
            this.btnMuteUnmute.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMuteUnmute.ImageKey = "Unmute.png";
            this.btnMuteUnmute.ImageList = this.ilMuteUnmute;
            this.btnMuteUnmute.Location = new System.Drawing.Point(47, 239);
            this.btnMuteUnmute.Name = "btnMuteUnmute";
            this.btnMuteUnmute.Size = new System.Drawing.Size(28, 26);
            this.btnMuteUnmute.TabIndex = 10;
            this.btnMuteUnmute.UseVisualStyleBackColor = true;
            this.btnMuteUnmute.Click += new System.EventHandler(this.btnMuteUnmute_Click);
            // 
            // ilMuteUnmute
            // 
            this.ilMuteUnmute.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilMuteUnmute.ImageStream")));
            this.ilMuteUnmute.TransparentColor = System.Drawing.Color.Transparent;
            this.ilMuteUnmute.Images.SetKeyName(0, "Mute.png");
            this.ilMuteUnmute.Images.SetKeyName(1, "Unmute.png");
            // 
            // tbVolume
            // 
            this.tbVolume.Location = new System.Drawing.Point(51, 88);
            this.tbVolume.Maximum = 100;
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbVolume.Size = new System.Drawing.Size(45, 145);
            this.tbVolume.TabIndex = 9;
            this.tbVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbVolume.Scroll += new System.EventHandler(this.tbVolume_Scroll);
            // 
            // num_Magnification
            // 
            this.num_Magnification.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.num_Magnification.Location = new System.Drawing.Point(13, 61);
            this.num_Magnification.Maximum = new decimal(new int[] {
            900,
            0,
            0,
            0});
            this.num_Magnification.Name = "num_Magnification";
            this.num_Magnification.Size = new System.Drawing.Size(38, 21);
            this.num_Magnification.TabIndex = 11;
            this.num_Magnification.ValueChanged += new System.EventHandler(this.num_Magnification_ValueChanged);
            // 
            // txt_tName
            // 
            this.txt_tName.BackColor = System.Drawing.SystemColors.Control;
            this.txt_tName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_tName.Location = new System.Drawing.Point(3, 32);
            this.txt_tName.Name = "txt_tName";
            this.txt_tName.Size = new System.Drawing.Size(123, 14);
            this.txt_tName.TabIndex = 12;
            this.txt_tName.Text = "trackName";
            this.txt_tName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // num_Volume
            // 
            this.num_Volume.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.num_Volume.Location = new System.Drawing.Point(71, 61);
            this.num_Volume.Name = "num_Volume";
            this.num_Volume.Size = new System.Drawing.Size(38, 21);
            this.num_Volume.TabIndex = 13;
            this.num_Volume.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.num_Volume.ValueChanged += new System.EventHandler(this.num_Volume_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "+";
            // 
            // VolumeMixer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.num_Volume);
            this.Controls.Add(this.txt_tName);
            this.Controls.Add(this.num_Magnification);
            this.Controls.Add(this.btnMuteUnmute);
            this.Controls.Add(this.tbVolume);
            this.Name = "VolumeMixer";
            this.Size = new System.Drawing.Size(129, 293);
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Magnification)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_Volume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnMuteUnmute;
        private System.Windows.Forms.ImageList ilMuteUnmute;
        private System.Windows.Forms.TrackBar tbVolume;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.NumericUpDown num_Magnification;
        private System.Windows.Forms.TextBox txt_tName;
        private System.Windows.Forms.NumericUpDown num_Volume;
        private System.Windows.Forms.Label label1;
    }
}
