namespace VocalUtau.DirectUI
{
    partial class TrackerRollWindow
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.noteScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.d2DPainterBox1 = new BalthasarLib.D2DPainter.D2DPainterBox();
            this.SuspendLayout();
            // 
            // noteScrollBar1
            // 
            this.noteScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noteScrollBar1.LargeChange = 1;
            this.noteScrollBar1.Location = new System.Drawing.Point(233, 0);
            this.noteScrollBar1.Maximum = 127;
            this.noteScrollBar1.Name = "noteScrollBar1";
            this.noteScrollBar1.Size = new System.Drawing.Size(19, 226);
            this.noteScrollBar1.TabIndex = 3;
            this.noteScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.noteScrollBar1_Scroll);
            // 
            // d2DPainterBox1
            // 
            this.d2DPainterBox1.Antialias = false;
            this.d2DPainterBox1.BackColor = System.Drawing.Color.Black;
            this.d2DPainterBox1.Location = new System.Drawing.Point(0, 0);
            this.d2DPainterBox1.Margin = new System.Windows.Forms.Padding(4);
            this.d2DPainterBox1.Name = "d2DPainterBox1";
            this.d2DPainterBox1.Size = new System.Drawing.Size(233, 226);
            this.d2DPainterBox1.TabIndex = 2;
            this.d2DPainterBox1.D2DPaint += new BalthasarLib.D2DPainter.D2DPainterBox.OnD2DPaintHandler(this.d2DPainterBox1_D2DPaint);
            this.d2DPainterBox1.Load += new System.EventHandler(this.d2DPainterBox1_Load);
            this.d2DPainterBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.d2DPainterBox1_KeyDown);
            this.d2DPainterBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.d2DPainterBox1_KeyPress);
            this.d2DPainterBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.d2DPainterBox1_KeyUp);
            this.d2DPainterBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.d2DPainterBox1_MouseClick);
            this.d2DPainterBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.d2DPainterBox1_MouseDoubleClick);
            this.d2DPainterBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.d2DPainterBox1_MouseDown);
            this.d2DPainterBox1.MouseEnter += new System.EventHandler(this.d2DPainterBox1_MouseEnter);
            this.d2DPainterBox1.MouseLeave += new System.EventHandler(this.d2DPainterBox1_MouseLeave);
            this.d2DPainterBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.d2DPainterBox1_MouseMove);
            this.d2DPainterBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.d2DPainterBox1_MouseUp);
            // 
            // TrackerRollWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.noteScrollBar1);
            this.Controls.Add(this.d2DPainterBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TrackerRollWindow";
            this.Size = new System.Drawing.Size(252, 226);
            this.Resize += new System.EventHandler(this.TrackerRollWindow_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar noteScrollBar1;
        private BalthasarLib.D2DPainter.D2DPainterBox d2DPainterBox1;
    }
}
