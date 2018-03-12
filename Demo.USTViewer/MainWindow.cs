﻿using System;
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
        SingerWindow sw = new SingerWindow();
        AttributesWindow aw = new AttributesWindow();
        TrackerWindow tw = new TrackerWindow();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            Demo.USTViewer.BarkUST bu = new Demo.USTViewer.BarkUST();
            ProjectObject poj = bu.GetTest(true);
            PartsObject PO = poj.TrackerList[0].PartList[0];

            sw.ShowOnDock(this.MainDock);
            aw.ShowOnDock(this.MainDock);
            tw.ShowOnDock(this.MainDock);
            tw.ShowingEditorChanged += tw_ShowingEditorChanged;

            tw.LoadProjectObject(ref poj);
        }

        void tw_ShowingEditorChanged(PartsObject PartObject)
        {
            sw.LoadParts(ref PartObject);
        }

    }
}
