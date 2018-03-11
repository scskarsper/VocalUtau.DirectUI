namespace VocalUtau.DirectUI.Forms
{
    partial class TrackerWindow
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
            this.trackerRollWindow1 = new VocalUtau.DirectUI.TrackerRollWindow();
            this.ctl_Scroll_LeftPos = new System.Windows.Forms.HScrollBar();
            this.ctl_Track_PianoWidth = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_PianoWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // trackerRollWindow1
            // 
            this.trackerRollWindow1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackerRollWindow1.BackColor = System.Drawing.Color.Black;
            this.trackerRollWindow1.Location = new System.Drawing.Point(0, 0);
            this.trackerRollWindow1.Name = "trackerRollWindow1";
            this.trackerRollWindow1.Size = new System.Drawing.Size(1114, 242);
            this.trackerRollWindow1.TabIndex = 0;
            // 
            // ctl_Scroll_LeftPos
            // 
            this.ctl_Scroll_LeftPos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_Scroll_LeftPos.Location = new System.Drawing.Point(122, 245);
            this.ctl_Scroll_LeftPos.Maximum = 20000000;
            this.ctl_Scroll_LeftPos.Name = "ctl_Scroll_LeftPos";
            this.ctl_Scroll_LeftPos.Size = new System.Drawing.Size(992, 18);
            this.ctl_Scroll_LeftPos.SmallChange = 10;
            this.ctl_Scroll_LeftPos.TabIndex = 5;
            this.ctl_Scroll_LeftPos.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ctl_Scroll_LeftPos_Scroll);
            // 
            // ctl_Track_PianoWidth
            // 
            this.ctl_Track_PianoWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ctl_Track_PianoWidth.AutoSize = false;
            this.ctl_Track_PianoWidth.BackColor = System.Drawing.SystemColors.Control;
            this.ctl_Track_PianoWidth.Location = new System.Drawing.Point(0, 245);
            this.ctl_Track_PianoWidth.Maximum = 300;
            this.ctl_Track_PianoWidth.Minimum = 32;
            this.ctl_Track_PianoWidth.Name = "ctl_Track_PianoWidth";
            this.ctl_Track_PianoWidth.Size = new System.Drawing.Size(118, 24);
            this.ctl_Track_PianoWidth.TabIndex = 6;
            this.ctl_Track_PianoWidth.TickFrequency = 10;
            this.ctl_Track_PianoWidth.Value = 100;
            // 
            // TrackerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 272);
            this.Controls.Add(this.ctl_Scroll_LeftPos);
            this.Controls.Add(this.ctl_Track_PianoWidth);
            this.Controls.Add(this.trackerRollWindow1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "TrackerWindow";
            this.Text = "TrackerWindow";
            this.Load += new System.EventHandler(this.TrackerWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_PianoWidth)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TrackerRollWindow trackerRollWindow1;
        private System.Windows.Forms.HScrollBar ctl_Scroll_LeftPos;
        private System.Windows.Forms.TrackBar ctl_Track_PianoWidth;
    }
}