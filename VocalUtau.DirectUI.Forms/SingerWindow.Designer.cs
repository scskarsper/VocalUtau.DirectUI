namespace VocalUtau.DirectUI.Forms
{
    partial class SingerWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingerWindow));
            this.btn_SelectCurve = new System.Windows.Forms.Button();
            this.ctl_Param_RZoom = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ctl_Track_NoteHeight = new System.Windows.Forms.TrackBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btn_PianoRollAction = new System.Windows.Forms.Button();
            this.pianoRollWindow1 = new VocalUtau.DirectUI.PianoRollWindow();
            this.ctl_Scroll_LeftPos = new System.Windows.Forms.HScrollBar();
            this.ctl_Track_PianoWidth = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_SelectSinger = new System.Windows.Forms.Button();
            this.ctl_Param_LZoom = new System.Windows.Forms.TrackBar();
            this.paramCurveWindow1 = new VocalUtau.DirectUI.ParamCurveWindow();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.ParamCurveTypeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CurveSelector_PIT = new System.Windows.Forms.ToolStripMenuItem();
            this.CurveSelector_DYN = new System.Windows.Forms.ToolStripMenuItem();
            this.PianoRollActionMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RollTool_NoteSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_NoteAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_DrawLine = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_DrawJ = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_DrawR = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_DrawS = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_Earse = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.RollAction_NoteCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.RollAction_NotePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.RollAction_EditLyrics = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.RollAction_SetCurrentPos = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Param_RZoom)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_NoteHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_PianoWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Param_LZoom)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.ParamCurveTypeMenu.SuspendLayout();
            this.PianoRollActionMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_SelectCurve
            // 
            this.btn_SelectCurve.BackColor = System.Drawing.Color.Black;
            this.btn_SelectCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SelectCurve.ForeColor = System.Drawing.Color.White;
            this.btn_SelectCurve.Location = new System.Drawing.Point(-1, 0);
            this.btn_SelectCurve.Name = "btn_SelectCurve";
            this.btn_SelectCurve.Size = new System.Drawing.Size(59, 22);
            this.btn_SelectCurve.TabIndex = 9;
            this.btn_SelectCurve.Text = "Type>";
            this.btn_SelectCurve.UseVisualStyleBackColor = false;
            this.btn_SelectCurve.Click += new System.EventHandler(this.btn_SelectCurve_Click);
            // 
            // ctl_Param_RZoom
            // 
            this.ctl_Param_RZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_Param_RZoom.AutoSize = false;
            this.ctl_Param_RZoom.BackColor = System.Drawing.Color.Black;
            this.ctl_Param_RZoom.LargeChange = 1;
            this.ctl_Param_RZoom.Location = new System.Drawing.Point(-3, 19);
            this.ctl_Param_RZoom.Margin = new System.Windows.Forms.Padding(2);
            this.ctl_Param_RZoom.Maximum = 12;
            this.ctl_Param_RZoom.Minimum = 1;
            this.ctl_Param_RZoom.Name = "ctl_Param_RZoom";
            this.ctl_Param_RZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ctl_Param_RZoom.Size = new System.Drawing.Size(17, 94);
            this.ctl_Param_RZoom.TabIndex = 10;
            this.ctl_Param_RZoom.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.ctl_Param_RZoom.Value = 1;
            this.ctl_Param_RZoom.Scroll += new System.EventHandler(this.ctl_Param_RZoom_Scroll);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ctl_Param_RZoom);
            this.panel1.Location = new System.Drawing.Point(924, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(23, 229);
            this.panel1.TabIndex = 11;
            // 
            // ctl_Track_NoteHeight
            // 
            this.ctl_Track_NoteHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_Track_NoteHeight.AutoSize = false;
            this.ctl_Track_NoteHeight.BackColor = System.Drawing.SystemColors.Control;
            this.ctl_Track_NoteHeight.LargeChange = 13;
            this.ctl_Track_NoteHeight.Location = new System.Drawing.Point(864, 327);
            this.ctl_Track_NoteHeight.Margin = new System.Windows.Forms.Padding(2);
            this.ctl_Track_NoteHeight.Maximum = 130;
            this.ctl_Track_NoteHeight.Minimum = 13;
            this.ctl_Track_NoteHeight.Name = "ctl_Track_NoteHeight";
            this.ctl_Track_NoteHeight.Size = new System.Drawing.Size(79, 16);
            this.ctl_Track_NoteHeight.SmallChange = 13;
            this.ctl_Track_NoteHeight.TabIndex = 5;
            this.ctl_Track_NoteHeight.TickFrequency = 13;
            this.ctl_Track_NoteHeight.Value = 13;
            this.ctl_Track_NoteHeight.Scroll += new System.EventHandler(this.ctl_Track_NoteHeight_Scroll);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Black;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.btn_PianoRollAction);
            this.splitContainer1.Panel1.Controls.Add(this.ctl_Track_NoteHeight);
            this.splitContainer1.Panel1.Controls.Add(this.pianoRollWindow1);
            this.splitContainer1.Panel1.Controls.Add(this.ctl_Scroll_LeftPos);
            this.splitContainer1.Panel1.Controls.Add(this.ctl_Track_PianoWidth);
            this.splitContainer1.Panel1MinSize = 120;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.btn_SelectSinger);
            this.splitContainer1.Panel2.Controls.Add(this.btn_SelectCurve);
            this.splitContainer1.Panel2.Controls.Add(this.ctl_Param_LZoom);
            this.splitContainer1.Panel2.Controls.Add(this.paramCurveWindow1);
            this.splitContainer1.Panel2MinSize = 120;
            this.splitContainer1.Size = new System.Drawing.Size(945, 488);
            this.splitContainer1.SplitterDistance = 345;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 9;
            // 
            // btn_PianoRollAction
            // 
            this.btn_PianoRollAction.BackColor = System.Drawing.Color.Black;
            this.btn_PianoRollAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PianoRollAction.ForeColor = System.Drawing.Color.White;
            this.btn_PianoRollAction.Location = new System.Drawing.Point(1, 0);
            this.btn_PianoRollAction.Name = "btn_PianoRollAction";
            this.btn_PianoRollAction.Size = new System.Drawing.Size(81, 27);
            this.btn_PianoRollAction.TabIndex = 13;
            this.btn_PianoRollAction.Text = "Action>";
            this.btn_PianoRollAction.UseVisualStyleBackColor = false;
            this.btn_PianoRollAction.Click += new System.EventHandler(this.btn_PianoRollAction_Click);
            // 
            // pianoRollWindow1
            // 
            this.pianoRollWindow1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pianoRollWindow1.BackColor = System.Drawing.Color.Black;
            this.pianoRollWindow1.Location = new System.Drawing.Point(1, 1);
            this.pianoRollWindow1.Margin = new System.Windows.Forms.Padding(1);
            this.pianoRollWindow1.Name = "pianoRollWindow1";
            this.pianoRollWindow1.OctaveType = VocalUtau.Formats.Model.VocalObject.PitchAtomObject.OctaveTypeEnum.Voice;
            this.pianoRollWindow1.Size = new System.Drawing.Size(943, 325);
            this.pianoRollWindow1.TabIndex = 0;
            this.pianoRollWindow1.RollMouseDown += new VocalUtau.DirectUI.PianoRollWindow.OnMouseEventHandler(this.pianoRollWindow1_RollMouseDown);
            // 
            // ctl_Scroll_LeftPos
            // 
            this.ctl_Scroll_LeftPos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_Scroll_LeftPos.Location = new System.Drawing.Point(81, 327);
            this.ctl_Scroll_LeftPos.Maximum = 20000000;
            this.ctl_Scroll_LeftPos.Name = "ctl_Scroll_LeftPos";
            this.ctl_Scroll_LeftPos.Size = new System.Drawing.Size(781, 18);
            this.ctl_Scroll_LeftPos.SmallChange = 10;
            this.ctl_Scroll_LeftPos.TabIndex = 3;
            this.ctl_Scroll_LeftPos.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ctl_Scroll_LeftPos_Scroll);
            // 
            // ctl_Track_PianoWidth
            // 
            this.ctl_Track_PianoWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ctl_Track_PianoWidth.AutoSize = false;
            this.ctl_Track_PianoWidth.BackColor = System.Drawing.SystemColors.Control;
            this.ctl_Track_PianoWidth.Location = new System.Drawing.Point(0, 327);
            this.ctl_Track_PianoWidth.Margin = new System.Windows.Forms.Padding(2);
            this.ctl_Track_PianoWidth.Maximum = 300;
            this.ctl_Track_PianoWidth.Minimum = 32;
            this.ctl_Track_PianoWidth.Name = "ctl_Track_PianoWidth";
            this.ctl_Track_PianoWidth.Size = new System.Drawing.Size(79, 16);
            this.ctl_Track_PianoWidth.TabIndex = 4;
            this.ctl_Track_PianoWidth.TickFrequency = 10;
            this.ctl_Track_PianoWidth.Value = 100;
            this.ctl_Track_PianoWidth.Scroll += new System.EventHandler(this.ctl_Track_PianoWidth_Scroll);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(4, 50);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(51, 50);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // btn_SelectSinger
            // 
            this.btn_SelectSinger.BackColor = System.Drawing.Color.Black;
            this.btn_SelectSinger.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SelectSinger.ForeColor = System.Drawing.Color.White;
            this.btn_SelectSinger.Location = new System.Drawing.Point(-1, 21);
            this.btn_SelectSinger.Name = "btn_SelectSinger";
            this.btn_SelectSinger.Size = new System.Drawing.Size(59, 22);
            this.btn_SelectSinger.TabIndex = 9;
            this.btn_SelectSinger.Text = "Singer>";
            this.btn_SelectSinger.UseVisualStyleBackColor = false;
            // 
            // ctl_Param_LZoom
            // 
            this.ctl_Param_LZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ctl_Param_LZoom.AutoSize = false;
            this.ctl_Param_LZoom.BackColor = System.Drawing.Color.Black;
            this.ctl_Param_LZoom.LargeChange = 1;
            this.ctl_Param_LZoom.Location = new System.Drawing.Point(63, 17);
            this.ctl_Param_LZoom.Margin = new System.Windows.Forms.Padding(2);
            this.ctl_Param_LZoom.Maximum = 12;
            this.ctl_Param_LZoom.Minimum = 1;
            this.ctl_Param_LZoom.Name = "ctl_Param_LZoom";
            this.ctl_Param_LZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ctl_Param_LZoom.Size = new System.Drawing.Size(16, 94);
            this.ctl_Param_LZoom.TabIndex = 7;
            this.ctl_Param_LZoom.Value = 1;
            this.ctl_Param_LZoom.Scroll += new System.EventHandler(this.ctl_Param_LZoom_Scroll);
            // 
            // paramCurveWindow1
            // 
            this.paramCurveWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramCurveWindow1.Location = new System.Drawing.Point(0, 0);
            this.paramCurveWindow1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.paramCurveWindow1.Name = "paramCurveWindow1";
            this.paramCurveWindow1.Size = new System.Drawing.Size(945, 133);
            this.paramCurveWindow1.TabIndex = 0;
            // 
            // toolStripButton11
            // 
            this.toolStripButton11.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton11.Image")));
            this.toolStripButton11.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton11.Name = "toolStripButton11";
            this.toolStripButton11.Size = new System.Drawing.Size(84, 22);
            this.toolStripButton11.Text = "Repeat(0)";
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton10.Image")));
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Size = new System.Drawing.Size(75, 22);
            this.toolStripButton10.Text = "Undo(0)";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(83, 22);
            this.toolStripButton6.Text = "EarseLine";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(72, 22);
            this.toolStripButton4.Text = "GraphR";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(69, 22);
            this.toolStripButton3.Text = "GraphJ";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton2.Text = "Line";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton9.Image")));
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Size = new System.Drawing.Size(88, 22);
            this.toolStripButton9.Text = "NotePaste";
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(87, 22);
            this.toolStripButton8.Text = "NoteCopy";
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton7.Text = "NoteAdd";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(91, 22);
            this.toolStripButton1.Text = "NoteSelect";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton7,
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSeparator2,
            this.toolStripButton10,
            this.toolStripButton11});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(945, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(71, 22);
            this.toolStripButton5.Text = "GraphS";
            // 
            // ParamCurveTypeMenu
            // 
            this.ParamCurveTypeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurveSelector_PIT,
            this.CurveSelector_DYN});
            this.ParamCurveTypeMenu.Name = "SwitchTypeBtn";
            this.ParamCurveTypeMenu.ShowCheckMargin = true;
            this.ParamCurveTypeMenu.ShowImageMargin = false;
            this.ParamCurveTypeMenu.Size = new System.Drawing.Size(103, 48);
            // 
            // CurveSelector_PIT
            // 
            this.CurveSelector_PIT.Name = "CurveSelector_PIT";
            this.CurveSelector_PIT.Size = new System.Drawing.Size(102, 22);
            this.CurveSelector_PIT.Text = "PIT";
            this.CurveSelector_PIT.Click += new System.EventHandler(this.CurveSelector_PIT_Click);
            // 
            // CurveSelector_DYN
            // 
            this.CurveSelector_DYN.Name = "CurveSelector_DYN";
            this.CurveSelector_DYN.Size = new System.Drawing.Size(102, 22);
            this.CurveSelector_DYN.Text = "DYN";
            this.CurveSelector_DYN.Click += new System.EventHandler(this.CurveSelector_DYN_Click);
            // 
            // PianoRollActionMenu
            // 
            this.PianoRollActionMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RollAction_SetCurrentPos,
            this.toolStripSeparator5,
            this.RollTool_NoteSelect,
            this.RollTool_NoteAdd,
            this.toolStripSeparator3,
            this.RollTool_DrawLine,
            this.RollTool_DrawJ,
            this.RollTool_DrawR,
            this.RollTool_DrawS,
            this.RollTool_Earse,
            this.toolStripSeparator4,
            this.RollAction_NoteCopy,
            this.RollAction_NotePaste,
            this.RollAction_EditLyrics});
            this.PianoRollActionMenu.Name = "SwitchTypeBtn";
            this.PianoRollActionMenu.ShowCheckMargin = true;
            this.PianoRollActionMenu.ShowImageMargin = false;
            this.PianoRollActionMenu.Size = new System.Drawing.Size(264, 264);
            // 
            // RollTool_NoteSelect
            // 
            this.RollTool_NoteSelect.Name = "RollTool_NoteSelect";
            this.RollTool_NoteSelect.Size = new System.Drawing.Size(263, 22);
            this.RollTool_NoteSelect.Text = "Select Notes";
            this.RollTool_NoteSelect.Click += new System.EventHandler(this.RollTool_NoteSelect_Click);
            // 
            // RollTool_NoteAdd
            // 
            this.RollTool_NoteAdd.Name = "RollTool_NoteAdd";
            this.RollTool_NoteAdd.Size = new System.Drawing.Size(263, 22);
            this.RollTool_NoteAdd.Text = "Add New Note";
            this.RollTool_NoteAdd.Click += new System.EventHandler(this.RollTool_NoteAdd_Click);
            // 
            // RollTool_DrawLine
            // 
            this.RollTool_DrawLine.Name = "RollTool_DrawLine";
            this.RollTool_DrawLine.Size = new System.Drawing.Size(263, 22);
            this.RollTool_DrawLine.Text = "Draw PitchBends with Line";
            this.RollTool_DrawLine.Click += new System.EventHandler(this.RollTool_DrawLine_Click);
            // 
            // RollTool_DrawJ
            // 
            this.RollTool_DrawJ.Name = "RollTool_DrawJ";
            this.RollTool_DrawJ.Size = new System.Drawing.Size(263, 22);
            this.RollTool_DrawJ.Text = "Draw PitchBends with Graphic J";
            this.RollTool_DrawJ.Click += new System.EventHandler(this.RollTool_DrawJ_Click);
            // 
            // RollTool_DrawR
            // 
            this.RollTool_DrawR.Name = "RollTool_DrawR";
            this.RollTool_DrawR.Size = new System.Drawing.Size(263, 22);
            this.RollTool_DrawR.Text = "Draw PitchBends with Graphic R";
            this.RollTool_DrawR.Click += new System.EventHandler(this.RollTool_DrawR_Click);
            // 
            // RollTool_DrawS
            // 
            this.RollTool_DrawS.Name = "RollTool_DrawS";
            this.RollTool_DrawS.Size = new System.Drawing.Size(263, 22);
            this.RollTool_DrawS.Text = "Draw PitchBends with Graphic S";
            this.RollTool_DrawS.Click += new System.EventHandler(this.RollTool_DrawS_Click);
            // 
            // RollTool_Earse
            // 
            this.RollTool_Earse.Name = "RollTool_Earse";
            this.RollTool_Earse.Size = new System.Drawing.Size(263, 22);
            this.RollTool_Earse.Text = "Select And Earse PitchBends";
            this.RollTool_Earse.Click += new System.EventHandler(this.RollTool_Earse_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(260, 6);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(260, 6);
            // 
            // RollAction_NoteCopy
            // 
            this.RollAction_NoteCopy.Name = "RollAction_NoteCopy";
            this.RollAction_NoteCopy.Size = new System.Drawing.Size(263, 22);
            this.RollAction_NoteCopy.Text = "Copy Selected Notes";
            this.RollAction_NoteCopy.Click += new System.EventHandler(this.RollAction_NoteCopy_Click);
            // 
            // RollAction_NotePaste
            // 
            this.RollAction_NotePaste.Name = "RollAction_NotePaste";
            this.RollAction_NotePaste.Size = new System.Drawing.Size(263, 22);
            this.RollAction_NotePaste.Text = "Paste Copyed Notes";
            this.RollAction_NotePaste.Click += new System.EventHandler(this.RollAction_NotePaste_Click);
            // 
            // RollAction_EditLyrics
            // 
            this.RollAction_EditLyrics.Name = "RollAction_EditLyrics";
            this.RollAction_EditLyrics.Size = new System.Drawing.Size(263, 22);
            this.RollAction_EditLyrics.Text = "Edit Selected Lyrics";
            this.RollAction_EditLyrics.Click += new System.EventHandler(this.RollAction_EditLyrics_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(260, 6);
            // 
            // RollAction_SetCurrentPos
            // 
            this.RollAction_SetCurrentPos.Name = "RollAction_SetCurrentPos";
            this.RollAction_SetCurrentPos.Size = new System.Drawing.Size(263, 22);
            this.RollAction_SetCurrentPos.Text = "Setup Current Postion to Mouse";
            this.RollAction_SetCurrentPos.Click += new System.EventHandler(this.RollAction_SetCurrentPos_Click);
            // 
            // SingerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(945, 513);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SingerWindow";
            this.Text = "SingerWindow";
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Param_RZoom)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_NoteHeight)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_PianoWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Param_LZoom)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ParamCurveTypeMenu.ResumeLayout(false);
            this.PianoRollActionMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_SelectCurve;
        private ParamCurveWindow paramCurveWindow1;
        private System.Windows.Forms.TrackBar ctl_Param_RZoom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar ctl_Track_NoteHeight;
        private PianoRollWindow pianoRollWindow1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.HScrollBar ctl_Scroll_LeftPos;
        private System.Windows.Forms.TrackBar ctl_Track_PianoWidth;
        private System.Windows.Forms.TrackBar ctl_Param_LZoom;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ContextMenuStrip ParamCurveTypeMenu;
        private System.Windows.Forms.ToolStripMenuItem CurveSelector_PIT;
        private System.Windows.Forms.ToolStripMenuItem CurveSelector_DYN;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_SelectSinger;
        private System.Windows.Forms.Button btn_PianoRollAction;
        private System.Windows.Forms.ContextMenuStrip PianoRollActionMenu;
        private System.Windows.Forms.ToolStripMenuItem RollTool_NoteSelect;
        private System.Windows.Forms.ToolStripMenuItem RollTool_NoteAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem RollTool_DrawLine;
        private System.Windows.Forms.ToolStripMenuItem RollTool_DrawJ;
        private System.Windows.Forms.ToolStripMenuItem RollTool_DrawR;
        private System.Windows.Forms.ToolStripMenuItem RollTool_DrawS;
        private System.Windows.Forms.ToolStripMenuItem RollTool_Earse;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem RollAction_SetCurrentPos;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem RollAction_NoteCopy;
        private System.Windows.Forms.ToolStripMenuItem RollAction_NotePaste;
        private System.Windows.Forms.ToolStripMenuItem RollAction_EditLyrics;
    }
}