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
            this.SuspendLayout();
            // 
            // PropertyViewer
            // 
            this.PropertyViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyViewer.LineColor = System.Drawing.SystemColors.ControlDark;
            this.PropertyViewer.Location = new System.Drawing.Point(4, 2);
            this.PropertyViewer.Margin = new System.Windows.Forms.Padding(4);
            this.PropertyViewer.Name = "PropertyViewer";
            this.PropertyViewer.Size = new System.Drawing.Size(372, 432);
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
            // AttributesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 546);
            this.Controls.Add(this.PropertyViewer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AttributesWindow";
            this.Text = "AttributesWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid PropertyViewer;
        private System.Windows.Forms.Timer MemoryCleaner;
    }
}