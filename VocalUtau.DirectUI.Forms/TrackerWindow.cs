using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace VocalUtau.DirectUI.Forms
{
    public partial class TrackerWindow : DockContent
    {
        public TrackerWindow()
        {
            InitializeComponent();
        }

        private void TrackerWindow_Load(object sender, EventArgs e)
        {

        }
        public void ShowOnDock(DockPanel DockPanel)
        {
            this.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockTop);
        }
    }
}
