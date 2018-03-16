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
            this.PropertyViewer = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // PropertyViewer
            // 
            this.PropertyViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyViewer.LineColor = System.Drawing.SystemColors.ControlDark;
            this.PropertyViewer.Location = new System.Drawing.Point(3, 16);
            this.PropertyViewer.Name = "PropertyViewer";
            this.PropertyViewer.Size = new System.Drawing.Size(279, 346);
            this.PropertyViewer.TabIndex = 0;
            this.PropertyViewer.UseCompatibleTextRendering = true;
            // 
            // AttributesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 437);
            this.Controls.Add(this.PropertyViewer);
            this.Name = "AttributesWindow";
            this.Text = "AttributesWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyViewer;
    }
}