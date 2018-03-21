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
            this.btn_SelectCurve = new System.Windows.Forms.Button();
            this.ctl_Param_RZoom = new System.Windows.Forms.TrackBar();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ctl_Track_NoteHeight = new System.Windows.Forms.TrackBar();
            this.MainPianoSplitContainer = new System.Windows.Forms.SplitContainer();
            this.btn_PianoRollAction = new System.Windows.Forms.Button();
            this.pianoRollWindow1 = new VocalUtau.DirectUI.PianoRollWindow();
            this.ctl_Scroll_LeftPos = new System.Windows.Forms.HScrollBar();
            this.ctl_Track_PianoWidth = new System.Windows.Forms.TrackBar();
            this.UtauPic = new System.Windows.Forms.PictureBox();
            this.btn_SelectAction = new System.Windows.Forms.Button();
            this.ctl_Param_LZoom = new System.Windows.Forms.TrackBar();
            this.paramCurveWindow1 = new VocalUtau.DirectUI.ParamCurveWindow();
            this.ParamCurveTypeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CurveSelector_PIT = new System.Windows.Forms.ToolStripMenuItem();
            this.CurveSelector_DYN = new System.Windows.Forms.ToolStripMenuItem();
            this.PianoRollActionMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.RollAction_SetCurrentPos = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.RollTool_NoteSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_NoteAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RollTool_DrawLine = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_DrawJ = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_DrawR = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_DrawS = new System.Windows.Forms.ToolStripMenuItem();
            this.RollTool_Earse = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.RollAction_NoteCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.RollAction_NotePaste = new System.Windows.Forms.ToolStripMenuItem();
            this.RollAction_EditLyrics = new System.Windows.Forms.ToolStripMenuItem();
            this.ParamCurveTollMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CurveAction_SetupCurrentToMouse = new System.Windows.Forms.ToolStripMenuItem();
            this.CurveAction_SetupCurrentToMouse_Separator = new System.Windows.Forms.ToolStripSeparator();
            this.CurveTool_DrawLine = new System.Windows.Forms.ToolStripMenuItem();
            this.CurveTool_DrawJ = new System.Windows.Forms.ToolStripMenuItem();
            this.CurveTool_DrawR = new System.Windows.Forms.ToolStripMenuItem();
            this.CurveTool_DrawS = new System.Windows.Forms.ToolStripMenuItem();
            this.CurveTool_EarseSelect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.BindPianoRoll = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Param_RZoom)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_NoteHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainPianoSplitContainer)).BeginInit();
            this.MainPianoSplitContainer.Panel1.SuspendLayout();
            this.MainPianoSplitContainer.Panel2.SuspendLayout();
            this.MainPianoSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_PianoWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UtauPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Param_LZoom)).BeginInit();
            this.ParamCurveTypeMenu.SuspendLayout();
            this.PianoRollActionMenu.SuspendLayout();
            this.ParamCurveTollMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_SelectCurve
            // 
            this.btn_SelectCurve.BackColor = System.Drawing.Color.Black;
            this.btn_SelectCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SelectCurve.ForeColor = System.Drawing.Color.White;
            this.btn_SelectCurve.Location = new System.Drawing.Point(-1, 116);
            this.btn_SelectCurve.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SelectCurve.Name = "btn_SelectCurve";
            this.btn_SelectCurve.Size = new System.Drawing.Size(79, 28);
            this.btn_SelectCurve.TabIndex = 9;
            this.btn_SelectCurve.Text = "参数>";
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
            this.ctl_Param_RZoom.Location = new System.Drawing.Point(-4, 5);
            this.ctl_Param_RZoom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ctl_Param_RZoom.Maximum = 5;
            this.ctl_Param_RZoom.Name = "ctl_Param_RZoom";
            this.ctl_Param_RZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ctl_Param_RZoom.Size = new System.Drawing.Size(23, 145);
            this.ctl_Param_RZoom.TabIndex = 10;
            this.ctl_Param_RZoom.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.ctl_Param_RZoom.Scroll += new System.EventHandler(this.ctl_Param_RZoom_Scroll);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ctl_Param_RZoom);
            this.panel1.Location = new System.Drawing.Point(1232, -4);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(30, 244);
            this.panel1.TabIndex = 11;
            // 
            // ctl_Track_NoteHeight
            // 
            this.ctl_Track_NoteHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_Track_NoteHeight.AutoSize = false;
            this.ctl_Track_NoteHeight.BackColor = System.Drawing.SystemColors.Control;
            this.ctl_Track_NoteHeight.LargeChange = 13;
            this.ctl_Track_NoteHeight.Location = new System.Drawing.Point(1152, 430);
            this.ctl_Track_NoteHeight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ctl_Track_NoteHeight.Maximum = 130;
            this.ctl_Track_NoteHeight.Minimum = 13;
            this.ctl_Track_NoteHeight.Name = "ctl_Track_NoteHeight";
            this.ctl_Track_NoteHeight.Size = new System.Drawing.Size(105, 20);
            this.ctl_Track_NoteHeight.SmallChange = 13;
            this.ctl_Track_NoteHeight.TabIndex = 5;
            this.ctl_Track_NoteHeight.TickFrequency = 13;
            this.ctl_Track_NoteHeight.Value = 13;
            this.ctl_Track_NoteHeight.Scroll += new System.EventHandler(this.ctl_Track_NoteHeight_Scroll);
            // 
            // MainPianoSplitContainer
            // 
            this.MainPianoSplitContainer.BackColor = System.Drawing.Color.Black;
            this.MainPianoSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPianoSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.MainPianoSplitContainer.Margin = new System.Windows.Forms.Padding(4);
            this.MainPianoSplitContainer.Name = "MainPianoSplitContainer";
            this.MainPianoSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainPianoSplitContainer.Panel1
            // 
            this.MainPianoSplitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.MainPianoSplitContainer.Panel1.Controls.Add(this.btn_PianoRollAction);
            this.MainPianoSplitContainer.Panel1.Controls.Add(this.ctl_Track_NoteHeight);
            this.MainPianoSplitContainer.Panel1.Controls.Add(this.pianoRollWindow1);
            this.MainPianoSplitContainer.Panel1.Controls.Add(this.ctl_Scroll_LeftPos);
            this.MainPianoSplitContainer.Panel1.Controls.Add(this.ctl_Track_PianoWidth);
            this.MainPianoSplitContainer.Panel1MinSize = 120;
            // 
            // MainPianoSplitContainer.Panel2
            // 
            this.MainPianoSplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.MainPianoSplitContainer.Panel2.Controls.Add(this.UtauPic);
            this.MainPianoSplitContainer.Panel2.Controls.Add(this.panel1);
            this.MainPianoSplitContainer.Panel2.Controls.Add(this.btn_SelectAction);
            this.MainPianoSplitContainer.Panel2.Controls.Add(this.btn_SelectCurve);
            this.MainPianoSplitContainer.Panel2.Controls.Add(this.ctl_Param_LZoom);
            this.MainPianoSplitContainer.Panel2.Controls.Add(this.paramCurveWindow1);
            this.MainPianoSplitContainer.Panel2MinSize = 120;
            this.MainPianoSplitContainer.Size = new System.Drawing.Size(1260, 641);
            this.MainPianoSplitContainer.SplitterDistance = 452;
            this.MainPianoSplitContainer.SplitterWidth = 12;
            this.MainPianoSplitContainer.TabIndex = 9;
            // 
            // btn_PianoRollAction
            // 
            this.btn_PianoRollAction.BackColor = System.Drawing.Color.Black;
            this.btn_PianoRollAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_PianoRollAction.ForeColor = System.Drawing.Color.White;
            this.btn_PianoRollAction.Location = new System.Drawing.Point(1, 0);
            this.btn_PianoRollAction.Margin = new System.Windows.Forms.Padding(4);
            this.btn_PianoRollAction.Name = "btn_PianoRollAction";
            this.btn_PianoRollAction.Size = new System.Drawing.Size(108, 34);
            this.btn_PianoRollAction.TabIndex = 13;
            this.btn_PianoRollAction.Text = "快捷>";
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
            this.pianoRollWindow1.Size = new System.Drawing.Size(1257, 428);
            this.pianoRollWindow1.TabIndex = 0;
            this.pianoRollWindow1.RollMouseDown += new VocalUtau.DirectUI.PianoRollWindow.OnMouseEventHandler(this.pianoRollWindow1_RollMouseDown);
            // 
            // ctl_Scroll_LeftPos
            // 
            this.ctl_Scroll_LeftPos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctl_Scroll_LeftPos.Location = new System.Drawing.Point(108, 430);
            this.ctl_Scroll_LeftPos.Maximum = 20000000;
            this.ctl_Scroll_LeftPos.Name = "ctl_Scroll_LeftPos";
            this.ctl_Scroll_LeftPos.Size = new System.Drawing.Size(1041, 18);
            this.ctl_Scroll_LeftPos.SmallChange = 10;
            this.ctl_Scroll_LeftPos.TabIndex = 3;
            this.ctl_Scroll_LeftPos.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ctl_Scroll_LeftPos_Scroll);
            // 
            // ctl_Track_PianoWidth
            // 
            this.ctl_Track_PianoWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ctl_Track_PianoWidth.AutoSize = false;
            this.ctl_Track_PianoWidth.BackColor = System.Drawing.SystemColors.Control;
            this.ctl_Track_PianoWidth.Location = new System.Drawing.Point(0, 430);
            this.ctl_Track_PianoWidth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ctl_Track_PianoWidth.Maximum = 300;
            this.ctl_Track_PianoWidth.Minimum = 32;
            this.ctl_Track_PianoWidth.Name = "ctl_Track_PianoWidth";
            this.ctl_Track_PianoWidth.Size = new System.Drawing.Size(105, 20);
            this.ctl_Track_PianoWidth.TabIndex = 4;
            this.ctl_Track_PianoWidth.TickFrequency = 10;
            this.ctl_Track_PianoWidth.Value = 100;
            this.ctl_Track_PianoWidth.Scroll += new System.EventHandler(this.ctl_Track_PianoWidth_Scroll);
            // 
            // UtauPic
            // 
            this.UtauPic.Location = new System.Drawing.Point(9, 14);
            this.UtauPic.Margin = new System.Windows.Forms.Padding(4);
            this.UtauPic.Name = "UtauPic";
            this.UtauPic.Size = new System.Drawing.Size(68, 62);
            this.UtauPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.UtauPic.TabIndex = 12;
            this.UtauPic.TabStop = false;
            // 
            // btn_SelectAction
            // 
            this.btn_SelectAction.BackColor = System.Drawing.Color.Black;
            this.btn_SelectAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_SelectAction.ForeColor = System.Drawing.Color.White;
            this.btn_SelectAction.Location = new System.Drawing.Point(-1, 90);
            this.btn_SelectAction.Margin = new System.Windows.Forms.Padding(4);
            this.btn_SelectAction.Name = "btn_SelectAction";
            this.btn_SelectAction.Size = new System.Drawing.Size(79, 28);
            this.btn_SelectAction.TabIndex = 9;
            this.btn_SelectAction.Text = "线型>";
            this.btn_SelectAction.UseVisualStyleBackColor = false;
            this.btn_SelectAction.Click += new System.EventHandler(this.btn_SelectAction_Click);
            // 
            // ctl_Param_LZoom
            // 
            this.ctl_Param_LZoom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ctl_Param_LZoom.AutoSize = false;
            this.ctl_Param_LZoom.BackColor = System.Drawing.Color.Black;
            this.ctl_Param_LZoom.LargeChange = 1;
            this.ctl_Param_LZoom.Location = new System.Drawing.Point(84, 2);
            this.ctl_Param_LZoom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ctl_Param_LZoom.Maximum = 12;
            this.ctl_Param_LZoom.Minimum = 1;
            this.ctl_Param_LZoom.Name = "ctl_Param_LZoom";
            this.ctl_Param_LZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ctl_Param_LZoom.Size = new System.Drawing.Size(21, 145);
            this.ctl_Param_LZoom.TabIndex = 7;
            this.ctl_Param_LZoom.Value = 1;
            this.ctl_Param_LZoom.Scroll += new System.EventHandler(this.ctl_Param_LZoom_Scroll);
            // 
            // paramCurveWindow1
            // 
            this.paramCurveWindow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paramCurveWindow1.Location = new System.Drawing.Point(0, 0);
            this.paramCurveWindow1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.paramCurveWindow1.Name = "paramCurveWindow1";
            this.paramCurveWindow1.Size = new System.Drawing.Size(1260, 177);
            this.paramCurveWindow1.TabIndex = 0;
            // 
            // ParamCurveTypeMenu
            // 
            this.ParamCurveTypeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurveSelector_PIT,
            this.CurveSelector_DYN});
            this.ParamCurveTypeMenu.Name = "SwitchTypeBtn";
            this.ParamCurveTypeMenu.ShowCheckMargin = true;
            this.ParamCurveTypeMenu.ShowImageMargin = false;
            this.ParamCurveTypeMenu.Size = new System.Drawing.Size(111, 52);
            // 
            // CurveSelector_PIT
            // 
            this.CurveSelector_PIT.Name = "CurveSelector_PIT";
            this.CurveSelector_PIT.Size = new System.Drawing.Size(110, 24);
            this.CurveSelector_PIT.Text = "PIT";
            this.CurveSelector_PIT.Click += new System.EventHandler(this.CurveSelector_PIT_Click);
            // 
            // CurveSelector_DYN
            // 
            this.CurveSelector_DYN.Name = "CurveSelector_DYN";
            this.CurveSelector_DYN.Size = new System.Drawing.Size(110, 24);
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
            this.PianoRollActionMenu.Size = new System.Drawing.Size(293, 308);
            // 
            // RollAction_SetCurrentPos
            // 
            this.RollAction_SetCurrentPos.Name = "RollAction_SetCurrentPos";
            this.RollAction_SetCurrentPos.Size = new System.Drawing.Size(292, 24);
            this.RollAction_SetCurrentPos.Text = "设置当前位置到鼠标位置(&G)";
            this.RollAction_SetCurrentPos.Click += new System.EventHandler(this.RollAction_SetCurrentPos_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(289, 6);
            // 
            // RollTool_NoteSelect
            // 
            this.RollTool_NoteSelect.Name = "RollTool_NoteSelect";
            this.RollTool_NoteSelect.Size = new System.Drawing.Size(292, 24);
            this.RollTool_NoteSelect.Text = "音符选择(&N)";
            this.RollTool_NoteSelect.Click += new System.EventHandler(this.RollTool_NoteSelect_Click);
            // 
            // RollTool_NoteAdd
            // 
            this.RollTool_NoteAdd.Name = "RollTool_NoteAdd";
            this.RollTool_NoteAdd.Size = new System.Drawing.Size(292, 24);
            this.RollTool_NoteAdd.Text = "音符添加(&A)";
            this.RollTool_NoteAdd.Click += new System.EventHandler(this.RollTool_NoteAdd_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(289, 6);
            // 
            // RollTool_DrawLine
            // 
            this.RollTool_DrawLine.Name = "RollTool_DrawLine";
            this.RollTool_DrawLine.Size = new System.Drawing.Size(292, 24);
            this.RollTool_DrawLine.Text = "绘制直线音高(&L)";
            this.RollTool_DrawLine.Click += new System.EventHandler(this.RollTool_DrawLine_Click);
            // 
            // RollTool_DrawJ
            // 
            this.RollTool_DrawJ.Name = "RollTool_DrawJ";
            this.RollTool_DrawJ.Size = new System.Drawing.Size(292, 24);
            this.RollTool_DrawJ.Text = "绘制J曲线音高(&J)";
            this.RollTool_DrawJ.Click += new System.EventHandler(this.RollTool_DrawJ_Click);
            // 
            // RollTool_DrawR
            // 
            this.RollTool_DrawR.Name = "RollTool_DrawR";
            this.RollTool_DrawR.Size = new System.Drawing.Size(292, 24);
            this.RollTool_DrawR.Text = "绘制R曲线音高(&R)";
            this.RollTool_DrawR.Click += new System.EventHandler(this.RollTool_DrawR_Click);
            // 
            // RollTool_DrawS
            // 
            this.RollTool_DrawS.Name = "RollTool_DrawS";
            this.RollTool_DrawS.Size = new System.Drawing.Size(292, 24);
            this.RollTool_DrawS.Text = "绘制S曲线音高(&S)";
            this.RollTool_DrawS.Click += new System.EventHandler(this.RollTool_DrawS_Click);
            // 
            // RollTool_Earse
            // 
            this.RollTool_Earse.Name = "RollTool_Earse";
            this.RollTool_Earse.Size = new System.Drawing.Size(292, 24);
            this.RollTool_Earse.Text = "擦除选中区域音高(&E)";
            this.RollTool_Earse.Click += new System.EventHandler(this.RollTool_Earse_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(289, 6);
            // 
            // RollAction_NoteCopy
            // 
            this.RollAction_NoteCopy.Enabled = false;
            this.RollAction_NoteCopy.Name = "RollAction_NoteCopy";
            this.RollAction_NoteCopy.Size = new System.Drawing.Size(292, 24);
            this.RollAction_NoteCopy.Text = "复制选中区域音符(&C)";
            this.RollAction_NoteCopy.Click += new System.EventHandler(this.RollAction_NoteCopy_Click);
            // 
            // RollAction_NotePaste
            // 
            this.RollAction_NotePaste.Name = "RollAction_NotePaste";
            this.RollAction_NotePaste.Size = new System.Drawing.Size(292, 24);
            this.RollAction_NotePaste.Text = "粘贴音符到当前坐标空白区域(&P)";
            this.RollAction_NotePaste.Click += new System.EventHandler(this.RollAction_NotePaste_Click);
            // 
            // RollAction_EditLyrics
            // 
            this.RollAction_EditLyrics.Name = "RollAction_EditLyrics";
            this.RollAction_EditLyrics.Size = new System.Drawing.Size(292, 24);
            this.RollAction_EditLyrics.Text = "编辑歌词(&I)";
            this.RollAction_EditLyrics.Click += new System.EventHandler(this.RollAction_EditLyrics_Click);
            // 
            // ParamCurveTollMenu
            // 
            this.ParamCurveTollMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CurveAction_SetupCurrentToMouse,
            this.CurveAction_SetupCurrentToMouse_Separator,
            this.CurveTool_DrawLine,
            this.CurveTool_DrawJ,
            this.CurveTool_DrawR,
            this.CurveTool_DrawS,
            this.CurveTool_EarseSelect,
            this.toolStripSeparator1,
            this.BindPianoRoll});
            this.ParamCurveTollMenu.Name = "SwitchTypeBtn";
            this.ParamCurveTollMenu.ShowCheckMargin = true;
            this.ParamCurveTollMenu.ShowImageMargin = false;
            this.ParamCurveTollMenu.Size = new System.Drawing.Size(265, 184);
            // 
            // CurveAction_SetupCurrentToMouse
            // 
            this.CurveAction_SetupCurrentToMouse.Name = "CurveAction_SetupCurrentToMouse";
            this.CurveAction_SetupCurrentToMouse.Size = new System.Drawing.Size(264, 24);
            this.CurveAction_SetupCurrentToMouse.Text = "设置当前位置到鼠标位置(&G)";
            this.CurveAction_SetupCurrentToMouse.Visible = false;
            this.CurveAction_SetupCurrentToMouse.Click += new System.EventHandler(this.RollAction_SetCurrentPos_Click);
            // 
            // CurveAction_SetupCurrentToMouse_Separator
            // 
            this.CurveAction_SetupCurrentToMouse_Separator.Name = "CurveAction_SetupCurrentToMouse_Separator";
            this.CurveAction_SetupCurrentToMouse_Separator.Size = new System.Drawing.Size(261, 6);
            this.CurveAction_SetupCurrentToMouse_Separator.Visible = false;
            // 
            // CurveTool_DrawLine
            // 
            this.CurveTool_DrawLine.Name = "CurveTool_DrawLine";
            this.CurveTool_DrawLine.Size = new System.Drawing.Size(264, 24);
            this.CurveTool_DrawLine.Text = "绘制直线参数(&L)";
            this.CurveTool_DrawLine.Click += new System.EventHandler(this.CurveTool_DrawLine_Click);
            // 
            // CurveTool_DrawJ
            // 
            this.CurveTool_DrawJ.Name = "CurveTool_DrawJ";
            this.CurveTool_DrawJ.Size = new System.Drawing.Size(264, 24);
            this.CurveTool_DrawJ.Text = "绘制J曲线参数(&J)";
            this.CurveTool_DrawJ.Click += new System.EventHandler(this.CurveTool_DrawJ_Click);
            // 
            // CurveTool_DrawR
            // 
            this.CurveTool_DrawR.Name = "CurveTool_DrawR";
            this.CurveTool_DrawR.Size = new System.Drawing.Size(264, 24);
            this.CurveTool_DrawR.Text = "绘制R曲线参数(&R)";
            this.CurveTool_DrawR.Click += new System.EventHandler(this.CurveTool_DrawR_Click);
            // 
            // CurveTool_DrawS
            // 
            this.CurveTool_DrawS.Name = "CurveTool_DrawS";
            this.CurveTool_DrawS.Size = new System.Drawing.Size(264, 24);
            this.CurveTool_DrawS.Text = "绘制S曲线参数(&S)";
            this.CurveTool_DrawS.Click += new System.EventHandler(this.CurveTool_DrawS_Click);
            // 
            // CurveTool_EarseSelect
            // 
            this.CurveTool_EarseSelect.Name = "CurveTool_EarseSelect";
            this.CurveTool_EarseSelect.Size = new System.Drawing.Size(264, 24);
            this.CurveTool_EarseSelect.Text = "擦除选中区域参数(&E)";
            this.CurveTool_EarseSelect.Click += new System.EventHandler(this.CurveTool_EarseSelect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(261, 6);
            // 
            // BindPianoRoll
            // 
            this.BindPianoRoll.Name = "BindPianoRoll";
            this.BindPianoRoll.Size = new System.Drawing.Size(264, 24);
            this.BindPianoRoll.Text = "与钢琴窗联动(&B)";
            this.BindPianoRoll.Click += new System.EventHandler(this.BindPianoRoll_Click);
            // 
            // SingerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 641);
            this.Controls.Add(this.MainPianoSplitContainer);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SingerWindow";
            this.Text = "SingerWindow";
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Param_RZoom)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_NoteHeight)).EndInit();
            this.MainPianoSplitContainer.Panel1.ResumeLayout(false);
            this.MainPianoSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainPianoSplitContainer)).EndInit();
            this.MainPianoSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Track_PianoWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UtauPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ctl_Param_LZoom)).EndInit();
            this.ParamCurveTypeMenu.ResumeLayout(false);
            this.PianoRollActionMenu.ResumeLayout(false);
            this.ParamCurveTollMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_SelectCurve;
        private ParamCurveWindow paramCurveWindow1;
        private System.Windows.Forms.TrackBar ctl_Param_RZoom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar ctl_Track_NoteHeight;
        private PianoRollWindow pianoRollWindow1;
        private System.Windows.Forms.SplitContainer MainPianoSplitContainer;
        private System.Windows.Forms.HScrollBar ctl_Scroll_LeftPos;
        private System.Windows.Forms.TrackBar ctl_Track_PianoWidth;
        private System.Windows.Forms.TrackBar ctl_Param_LZoom;
        private System.Windows.Forms.ContextMenuStrip ParamCurveTypeMenu;
        private System.Windows.Forms.ToolStripMenuItem CurveSelector_PIT;
        private System.Windows.Forms.ToolStripMenuItem CurveSelector_DYN;
        private System.Windows.Forms.PictureBox UtauPic;
        private System.Windows.Forms.Button btn_SelectAction;
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
        private System.Windows.Forms.ContextMenuStrip ParamCurveTollMenu;
        private System.Windows.Forms.ToolStripMenuItem CurveTool_DrawLine;
        private System.Windows.Forms.ToolStripMenuItem CurveTool_DrawJ;
        private System.Windows.Forms.ToolStripMenuItem CurveTool_DrawR;
        private System.Windows.Forms.ToolStripMenuItem CurveTool_DrawS;
        private System.Windows.Forms.ToolStripMenuItem CurveTool_EarseSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem BindPianoRoll;
        private System.Windows.Forms.ToolStripMenuItem CurveAction_SetupCurrentToMouse;
        private System.Windows.Forms.ToolStripSeparator CurveAction_SetupCurrentToMouse_Separator;
    }
}