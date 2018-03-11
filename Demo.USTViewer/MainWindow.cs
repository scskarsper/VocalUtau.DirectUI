using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.USTs.Original;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Forms
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            Demo.USTViewer.BarkUST bu = new Demo.USTViewer.BarkUST();
            ProjectObject poj = bu.GetTest(true);
            PartsObject PO = poj.TrackerList[0].PartList[0];
            SingerWindow sw = new SingerWindow();
            sw.LoadParts(ref PO);
            sw.ShowOnDock(this.MainDock);

            AttributesWindow aw = new AttributesWindow();
            aw.ShowOnDock(this.MainDock);

            TrackerWindow tw = new TrackerWindow();
            tw.LoadProjectObject(ref poj);
            tw.ShowOnDock(this.MainDock);
        }

    }
}
