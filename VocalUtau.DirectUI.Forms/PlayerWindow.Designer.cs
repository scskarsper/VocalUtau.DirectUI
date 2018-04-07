namespace VocalUtau.DirectUI.Forms
{
    partial class PlayerWindow
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
            this.BufferBfb = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // BufferBfb
            // 
            this.BufferBfb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BufferBfb.Location = new System.Drawing.Point(12, 12);
            this.BufferBfb.Name = "BufferBfb";
            this.BufferBfb.Size = new System.Drawing.Size(460, 23);
            this.BufferBfb.TabIndex = 0;
            this.BufferBfb.Click += new System.EventHandler(this.BufferBfb_Click);
            // 
            // PlayerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 253);
            this.Controls.Add(this.BufferBfb);
            this.Name = "PlayerWindow";
            this.Text = "PlayerWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayerWindow_FormClosing);
            this.Load += new System.EventHandler(this.PlayerWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar BufferBfb;
    }
}