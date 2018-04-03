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
            this.ctl_Scroll_LeftPos = new System.Windows.Forms.HScrollBar();
            this.ctl_Track_PianoWidth = new System.Windows.Forms.TrackBar();
            this.ctl_Track_TrackHeight = new System.Windows.Forms.TrackBar();
            this.menu_TrackEditor = new System.Windows.Forms.ContextMenuStrip();
            this.track_AddNewTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.track_AddNewBackerTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.track_AddParts = new System.Windows.Forms.ToolStripMenuItem();
            this.track_ImportWav = new System.Windows.Forms.ToolStripMenuItem();
            this.track_ImportAsTrack = new System.Windows.Forms.ToolStripMenuItem();
            this.track_ImportAsPart = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.track_DelTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.track_DelectParts = new System.Windows.Forms.ToolStripMenuItem();
            this.trackerRollWindow1 = new VocalUtau.DirectUI.TrackerRollWindow();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_PianoWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_TrackHeight)).BeginInit();
            this.menu_TrackEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctl_Scroll_LeftPos
            // 
            this.ctl_Scroll_LeftPos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_Scroll_LeftPos.Location = new System.Drawing.Point(108, 204);
            this.ctl_Scroll_LeftPos.Maximum = 20000000;
            this.ctl_Scroll_LeftPos.Name = "ctl_Scroll_LeftPos";
            this.ctl_Scroll_LeftPos.Size = new System.Drawing.Size(775, 18);
            this.ctl_Scroll_LeftPos.SmallChange = 10;
            this.ctl_Scroll_LeftPos.TabIndex = 5;
            this.ctl_Scroll_LeftPos.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ctl_Scroll_LeftPos_Scroll);
            // 
            // ctl_Track_PianoWidth
            // 
            this.ctl_Track_PianoWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ctl_Track_PianoWidth.AutoSize = false;
            this.ctl_Track_PianoWidth.BackColor = System.Drawing.SystemColors.Control;
            this.ctl_Track_PianoWidth.Location = new System.Drawing.Point(0, 204);
            this.ctl_Track_PianoWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ctl_Track_PianoWidth.Maximum = 300;
            this.ctl_Track_PianoWidth.Minimum = 32;
            this.ctl_Track_PianoWidth.Name = "ctl_Track_PianoWidth";
            this.ctl_Track_PianoWidth.Size = new System.Drawing.Size(105, 20);
            this.ctl_Track_PianoWidth.TabIndex = 6;
            this.ctl_Track_PianoWidth.TickFrequency = 10;
            this.ctl_Track_PianoWidth.Value = 100;
            this.ctl_Track_PianoWidth.Scroll += new System.EventHandler(this.ctl_Track_PianoWidth_Scroll);
            // 
            // ctl_Track_TrackHeight
            // 
            this.ctl_Track_TrackHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_Track_TrackHeight.AutoSize = false;
            this.ctl_Track_TrackHeight.BackColor = System.Drawing.SystemColors.Control;
            this.ctl_Track_TrackHeight.LargeChange = 18;
            this.ctl_Track_TrackHeight.Location = new System.Drawing.Point(885, 204);
            this.ctl_Track_TrackHeight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ctl_Track_TrackHeight.Maximum = 108;
            this.ctl_Track_TrackHeight.Minimum = 18;
            this.ctl_Track_TrackHeight.Name = "ctl_Track_TrackHeight";
            this.ctl_Track_TrackHeight.Size = new System.Drawing.Size(105, 20);
            this.ctl_Track_TrackHeight.SmallChange = 18;
            this.ctl_Track_TrackHeight.TabIndex = 7;
            this.ctl_Track_TrackHeight.TickFrequency = 18;
            this.ctl_Track_TrackHeight.Value = 18;
            this.ctl_Track_TrackHeight.Scroll += new System.EventHandler(this.ctl_Track_TrackHeight_Scroll);
            // 
            // menu_TrackEditor
            // 
            this.menu_TrackEditor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.track_AddNewTrack,
            this.track_AddNewBackerTrack,
            this.toolStripSeparator2,
            this.track_AddParts,
            this.track_ImportWav,
            this.toolStripSeparator1,
            this.track_DelTracks,
            this.track_DelectParts});
            this.menu_TrackEditor.Name = "menu_TrackEditor";
            this.menu_TrackEditor.Size = new System.Drawing.Size(214, 182);
            this.menu_TrackEditor.Opening += new System.ComponentModel.CancelEventHandler(this.menu_TrackEditor_Opening);
            // 
            // track_AddNewTrack
            // 
            this.track_AddNewTrack.Name = "track_AddNewTrack";
            this.track_AddNewTrack.Size = new System.Drawing.Size(213, 24);
            this.track_AddNewTrack.Text = "添加新声轨";
            this.track_AddNewTrack.Click += new System.EventHandler(this.track_AddNewTrack_Click);
            // 
            // track_AddNewBackerTrack
            // 
            this.track_AddNewBackerTrack.Name = "track_AddNewBackerTrack";
            this.track_AddNewBackerTrack.Size = new System.Drawing.Size(213, 24);
            this.track_AddNewBackerTrack.Text = "添加新伴奏轨";
            this.track_AddNewBackerTrack.Click += new System.EventHandler(this.track_AddNewBackerTrack_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(210, 6);
            // 
            // track_AddParts
            // 
            this.track_AddParts.Name = "track_AddParts";
            this.track_AddParts.Size = new System.Drawing.Size(213, 24);
            this.track_AddParts.Text = "在选中轨道添加区块";
            this.track_AddParts.Click += new System.EventHandler(this.track_AddParts_Click);
            // 
            // track_ImportWav
            // 
            this.track_ImportWav.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.track_ImportAsTrack,
            this.track_ImportAsPart});
            this.track_ImportWav.Name = "track_ImportWav";
            this.track_ImportWav.Size = new System.Drawing.Size(213, 24);
            this.track_ImportWav.Text = "导入伴奏";
            // 
            // track_ImportAsTrack
            // 
            this.track_ImportAsTrack.Name = "track_ImportAsTrack";
            this.track_ImportAsTrack.Size = new System.Drawing.Size(243, 24);
            this.track_ImportAsTrack.Text = "导入为新的伴奏轨";
            this.track_ImportAsTrack.Click += new System.EventHandler(this.track_ImportAsTrack_Click);
            // 
            // track_ImportAsPart
            // 
            this.track_ImportAsPart.Name = "track_ImportAsPart";
            this.track_ImportAsPart.Size = new System.Drawing.Size(243, 24);
            this.track_ImportAsPart.Text = "在当前轨道末尾导入伴奏";
            this.track_ImportAsPart.Click += new System.EventHandler(this.track_ImportAsPart_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(210, 6);
            // 
            // track_DelTracks
            // 
            this.track_DelTracks.Name = "track_DelTracks";
            this.track_DelTracks.Size = new System.Drawing.Size(213, 24);
            this.track_DelTracks.Text = "删除选中的轨道";
            this.track_DelTracks.Click += new System.EventHandler(this.track_DelTracks_Click);
            // 
            // track_DelectParts
            // 
            this.track_DelectParts.Name = "track_DelectParts";
            this.track_DelectParts.Size = new System.Drawing.Size(213, 24);
            this.track_DelectParts.Text = "删除选中区块";
            this.track_DelectParts.Click += new System.EventHandler(this.track_DelectParts_Click);
            // 
            // trackerRollWindow1
            // 
            this.trackerRollWindow1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackerRollWindow1.BackColor = System.Drawing.Color.Black;
            this.trackerRollWindow1.Location = new System.Drawing.Point(0, 0);
            this.trackerRollWindow1.Margin = new System.Windows.Forms.Padding(1);
            this.trackerRollWindow1.Name = "trackerRollWindow1";
            this.trackerRollWindow1.Size = new System.Drawing.Size(991, 201);
            this.trackerRollWindow1.TabIndex = 0;
            // 
            // TrackerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 226);
            this.Controls.Add(this.ctl_Track_TrackHeight);
            this.Controls.Add(this.ctl_Scroll_LeftPos);
            this.Controls.Add(this.ctl_Track_PianoWidth);
            this.Controls.Add(this.trackerRollWindow1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "TrackerWindow";
            this.Text = "TrackerWindow";
            this.Load += new System.EventHandler(this.TrackerWindow_Load);
            this.Enter += new System.EventHandler(this.TrackerWindow_Enter);
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_PianoWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_TrackHeight)).EndInit();
            this.menu_TrackEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TrackerRollWindow trackerRollWindow1;
        private System.Windows.Forms.HScrollBar ctl_Scroll_LeftPos;
        private System.Windows.Forms.TrackBar ctl_Track_PianoWidth;
        private System.Windows.Forms.TrackBar ctl_Track_TrackHeight;
        private System.Windows.Forms.ContextMenuStrip menu_TrackEditor;
        private System.Windows.Forms.ToolStripMenuItem track_AddNewTrack;
        private System.Windows.Forms.ToolStripMenuItem track_AddNewBackerTrack;
        private System.Windows.Forms.ToolStripMenuItem track_DelTracks;
        private System.Windows.Forms.ToolStripMenuItem track_AddParts;
        private System.Windows.Forms.ToolStripMenuItem track_DelectParts;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem track_ImportWav;
        private System.Windows.Forms.ToolStripMenuItem track_ImportAsTrack;
        private System.Windows.Forms.ToolStripMenuItem track_ImportAsPart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}