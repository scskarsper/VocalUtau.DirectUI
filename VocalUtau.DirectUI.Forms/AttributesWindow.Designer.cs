namespace VocalUtau.DirectUI.Forms
{
    partial class AttributesWindow
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
            this.components = new System.ComponentModel.Container();
            this.PropertyViewer = new System.Windows.Forms.PropertyGrid();
            this.MemoryCleaner = new System.Windows.Forms.Timer(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbl_RenderStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbl_RenderTotal = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_RenderCurrent = new System.Windows.Forms.Label();
            this.lbl_PosCurrent = new System.Windows.Forms.Label();
            this.btnMuteUnmute = new System.Windows.Forms.Button();
            this.tbVolume = new System.Windows.Forms.TrackBar();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // PropertyViewer
            // 
            this.PropertyViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyViewer.LineColor = System.Drawing.SystemColors.ControlDark;
            this.PropertyViewer.Location = new System.Drawing.Point(3, 2);
            this.PropertyViewer.Name = "PropertyViewer";
            this.PropertyViewer.Size = new System.Drawing.Size(199, 219);
            this.PropertyViewer.TabIndex = 0;
            this.PropertyViewer.UseCompatibleTextRendering = true;
            this.PropertyViewer.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.PropertyViewer_PropertyValueChanged);
            this.PropertyViewer.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.PropertyViewer_SelectedGridItemChanged);
            // 
            // MemoryCleaner
            // 
            this.MemoryCleaner.Enabled = true;
            this.MemoryCleaner.Interval = 10000;
            this.MemoryCleaner.Tick += new System.EventHandler(this.MemoryCleaner_Tick);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.lbl_RenderStatus);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lbl_RenderTotal);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.lbl_RenderCurrent);
            this.groupBox2.Controls.Add(this.lbl_PosCurrent);
            this.groupBox2.Controls.Add(this.btnMuteUnmute);
            this.groupBox2.Controls.Add(this.tbVolume);
            this.groupBox2.Location = new System.Drawing.Point(3, 227);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(199, 131);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "渲染信息";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(67, 105);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 5;
            this.label7.Text = "渲染状态:";
            // 
            // lbl_RenderStatus
            // 
            this.lbl_RenderStatus.AutoSize = true;
            this.lbl_RenderStatus.Location = new System.Drawing.Point(132, 105);
            this.lbl_RenderStatus.Name = "lbl_RenderStatus";
            this.lbl_RenderStatus.Size = new System.Drawing.Size(53, 12);
            this.lbl_RenderStatus.TabIndex = 4;
            this.lbl_RenderStatus.Text = "准备就绪";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "缓冲总长:";
            // 
            // lbl_RenderTotal
            // 
            this.lbl_RenderTotal.AutoSize = true;
            this.lbl_RenderTotal.Location = new System.Drawing.Point(132, 79);
            this.lbl_RenderTotal.Name = "lbl_RenderTotal";
            this.lbl_RenderTotal.Size = new System.Drawing.Size(59, 12);
            this.lbl_RenderTotal.TabIndex = 2;
            this.lbl_RenderTotal.Text = "00:00:000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "播放时间:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "光标时间:";
            // 
            // lbl_RenderCurrent
            // 
            this.lbl_RenderCurrent.AutoSize = true;
            this.lbl_RenderCurrent.Location = new System.Drawing.Point(132, 55);
            this.lbl_RenderCurrent.Name = "lbl_RenderCurrent";
            this.lbl_RenderCurrent.Size = new System.Drawing.Size(59, 12);
            this.lbl_RenderCurrent.TabIndex = 0;
            this.lbl_RenderCurrent.Text = "00:00:000";
            // 
            // lbl_PosCurrent
            // 
            this.lbl_PosCurrent.AutoSize = true;
            this.lbl_PosCurrent.Location = new System.Drawing.Point(132, 30);
            this.lbl_PosCurrent.Name = "lbl_PosCurrent";
            this.lbl_PosCurrent.Size = new System.Drawing.Size(59, 12);
            this.lbl_PosCurrent.TabIndex = 0;
            this.lbl_PosCurrent.Text = "00:00:000";
            // 
            // btnMuteUnmute
            // 
            this.btnMuteUnmute.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMuteUnmute.Font = new System.Drawing.Font("Wingdings", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnMuteUnmute.Location = new System.Drawing.Point(16, 89);
            this.btnMuteUnmute.Name = "btnMuteUnmute";
            this.btnMuteUnmute.Size = new System.Drawing.Size(32, 32);
            this.btnMuteUnmute.TabIndex = 7;
            this.btnMuteUnmute.Text = ">";
            this.btnMuteUnmute.UseVisualStyleBackColor = true;
            // 
            // tbVolume
            // 
            this.tbVolume.Location = new System.Drawing.Point(20, 22);
            this.tbVolume.Maximum = 100;
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbVolume.Size = new System.Drawing.Size(45, 71);
            this.tbVolume.TabIndex = 6;
            this.tbVolume.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbVolume.Value = 100;
            this.tbVolume.Scroll += new System.EventHandler(this.tbVolume_Scroll);
            this.tbVolume.ValueChanged += new System.EventHandler(this.tbVolume_ValueChanged);
            // 
            // AttributesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 361);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.PropertyViewer);
            this.MinimumSize = new System.Drawing.Size(220, 400);
            this.Name = "AttributesWindow";
            this.Text = "AttributesWindow";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVolume)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyViewer;
        private System.Windows.Forms.Timer MemoryCleaner;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbl_RenderStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbl_RenderTotal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_RenderCurrent;
        private System.Windows.Forms.Label lbl_PosCurrent;
        private System.Windows.Forms.Button btnMuteUnmute;
        private System.Windows.Forms.TrackBar tbVolume;
    }
}