﻿namespace Demo.USTViewer
{
    partial class IntroduceSwap
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
            this.Introduce = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SwanSplit = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.projectPwd = new System.Windows.Forms.TextBox();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            ((System.ComponentModel.ISupportInitialize)(this.SwanSplit)).BeginInit();
            this.SwanSplit.Panel1.SuspendLayout();
            this.SwanSplit.Panel2.SuspendLayout();
            this.SwanSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // Introduce
            // 
            this.Introduce.BackColor = System.Drawing.Color.White;
            this.Introduce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Introduce.Location = new System.Drawing.Point(0, 0);
            this.Introduce.Multiline = true;
            this.Introduce.Name = "Introduce";
            this.Introduce.ReadOnly = true;
            this.Introduce.Size = new System.Drawing.Size(568, 245);
            this.Introduce.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(299, 359);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 34);
            this.button1.TabIndex = 1;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(435, 359);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 34);
            this.button2.TabIndex = 2;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // SwanSplit
            // 
            this.SwanSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SwanSplit.IsSplitterFixed = true;
            this.SwanSplit.Location = new System.Drawing.Point(2, 36);
            this.SwanSplit.Name = "SwanSplit";
            this.SwanSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // SwanSplit.Panel1
            // 
            this.SwanSplit.Panel1.Controls.Add(this.Introduce);
            // 
            // SwanSplit.Panel2
            // 
            this.SwanSplit.Panel2.Controls.Add(this.projectPwd);
            this.SwanSplit.Panel2.Controls.Add(this.label2);
            this.SwanSplit.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.SwanSplit.Size = new System.Drawing.Size(568, 317);
            this.SwanSplit.SplitterDistance = 245;
            this.SwanSplit.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-1, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "工程信息：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "工程启动密码：";
            // 
            // projectPwd
            // 
            this.projectPwd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectPwd.Location = new System.Drawing.Point(128, 20);
            this.projectPwd.Name = "projectPwd";
            this.projectPwd.PasswordChar = '*';
            this.projectPwd.Size = new System.Drawing.Size(427, 25);
            this.projectPwd.TabIndex = 1;
            this.projectPwd.TextChanged += new System.EventHandler(this.projectPwd_TextChanged);
            this.projectPwd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.projectPwd_KeyDown);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(569, 405);
            this.shapeContainer1.TabIndex = 5;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 3;
            this.lineShape1.X2 = 563;
            this.lineShape1.Y1 = 355;
            this.lineShape1.Y2 = 355;
            // 
            // IntroduceSwap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 405);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SwanSplit);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.shapeContainer1);
            this.Name = "IntroduceSwap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工程须知";
            this.Load += new System.EventHandler(this.IntroduceSwap_Load);
            this.Shown += new System.EventHandler(this.IntroduceSwap_Shown);
            this.SwanSplit.Panel1.ResumeLayout(false);
            this.SwanSplit.Panel1.PerformLayout();
            this.SwanSplit.Panel2.ResumeLayout(false);
            this.SwanSplit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SwanSplit)).EndInit();
            this.SwanSplit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Introduce;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.SplitContainer SwanSplit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox projectPwd;
        private System.Windows.Forms.Label label2;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
    }
}