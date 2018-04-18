namespace VocalUtau.DirectUI.Forms
{
    partial class SoundMixer
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelMain = new System.Windows.Forms.FlowLayoutPanel();
            this.gbApplications = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelTrack = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.topMost = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.gbApplications.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanelMain);
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(143, 335);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device";
            // 
            // flowLayoutPanelMain
            // 
            this.flowLayoutPanelMain.AutoScroll = true;
            this.flowLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelMain.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelMain.Name = "flowLayoutPanelMain";
            this.flowLayoutPanelMain.Size = new System.Drawing.Size(137, 315);
            this.flowLayoutPanelMain.TabIndex = 1;
            // 
            // gbApplications
            // 
            this.gbApplications.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbApplications.Controls.Add(this.flowLayoutPanelTrack);
            this.gbApplications.Location = new System.Drawing.Point(154, 12);
            this.gbApplications.Name = "gbApplications";
            this.gbApplications.Size = new System.Drawing.Size(491, 335);
            this.gbApplications.TabIndex = 5;
            this.gbApplications.TabStop = false;
            this.gbApplications.Text = "Applications";
            // 
            // flowLayoutPanelTrack
            // 
            this.flowLayoutPanelTrack.AutoScroll = true;
            this.flowLayoutPanelTrack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelTrack.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelTrack.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanelTrack.Name = "flowLayoutPanelTrack";
            this.flowLayoutPanelTrack.Size = new System.Drawing.Size(485, 315);
            this.flowLayoutPanelTrack.TabIndex = 0;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(10, 355);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(137, 21);
            this.btnUpdate.TabIndex = 6;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // topMost
            // 
            this.topMost.AutoSize = true;
            this.topMost.Location = new System.Drawing.Point(157, 358);
            this.topMost.Name = "topMost";
            this.topMost.Size = new System.Drawing.Size(48, 16);
            this.topMost.TabIndex = 7;
            this.topMost.Text = "置顶";
            this.topMost.UseVisualStyleBackColor = true;
            this.topMost.CheckedChanged += new System.EventHandler(this.topMost_CheckedChanged);
            // 
            // SoundMixer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 388);
            this.Controls.Add(this.topMost);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbApplications);
            this.Controls.Add(this.btnUpdate);
            this.Name = "SoundMixer";
            this.Text = "SoundMixer";
            this.groupBox1.ResumeLayout(false);
            this.gbApplications.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMain;
        private System.Windows.Forms.GroupBox gbApplications;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTrack;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.CheckBox topMost;
    }
}