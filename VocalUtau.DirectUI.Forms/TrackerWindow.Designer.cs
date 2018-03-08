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
            this.SuspendLayout();
            // 
            // trackerRollWindow1
            // 
            this.trackerRollWindow1.BackColor = System.Drawing.Color.Black;
            this.trackerRollWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackerRollWindow1.Location = new System.Drawing.Point(0, 0);
            this.trackerRollWindow1.Margin = new System.Windows.Forms.Padding(2);
            this.trackerRollWindow1.Name = "trackerRollWindow1";
            this.trackerRollWindow1.Size = new System.Drawing.Size(743, 181);
            this.trackerRollWindow1.TabIndex = 0;
            // 
            // TrackerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(743, 181);
            this.Controls.Add(this.trackerRollWindow1);
            this.Name = "TrackerWindow";
            this.Text = "TrackerWindow";
            this.Load += new System.EventHandler(this.TrackerWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TrackerRollWindow trackerRollWindow1;
    }
}