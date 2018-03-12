using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.TrackerUtils;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace Demo.USTViewer
{
    public partial class Form2 : Form
    {
        ProjectObject poj;
        ObjectAlloc<ProjectObject> OAC = new ObjectAlloc<ProjectObject>();

        TrackerView TV = null;

        public Form2()
        {
            InitializeComponent();
            BarkUST bu=new BarkUST();
            poj = bu.GetTest(true);
            OAC.ReAlloc(poj);
        }

        void trackerRollWindow1_PartsMouseDown(object sender, VocalUtau.DirectUI.Models.TrackerMouseEventArgs e)
        {
            MessageBox.Show(e.AbsoluteTrackID.ToString());
        }

        void trackerRollWindow1_GridsMouseDown(object sender, VocalUtau.DirectUI.Models.TrackerMouseEventArgs e)
        {
        }

        private void trackerRollWindow1_Load(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            trackerRollWindow1.setPianoStartTick(hScrollBar1.Value);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            TV = new TrackerView(OAC.IntPtr, this.trackerRollWindow1);
            TV.HandleEvents = true;
            hScrollBar1.Maximum = (int)Math.Ceiling(1920+(double)poj.Time2Tick(poj.MaxLength));
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.trackerRollWindow1.setTrackHeight((uint)trackBar1.Value);
        }
    }
}
