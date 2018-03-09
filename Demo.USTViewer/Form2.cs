using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.VocalObject;

namespace Demo.USTViewer
{
    public partial class Form2 : Form
    {
        List<TrackerObject> TrackObject = new List<TrackerObject>();
        List<BackerObject> BackerObject = new List<BackerObject>();

        List<PartsObject> PObject = new List<PartsObject>();
        public Form2()
        {
            InitializeComponent();
            trackerRollWindow1.TrackerProps.Tempo = 120.0;
            trackerRollWindow1.TGridePaint += trackerRollWindow1_TGridePaint;
            trackerRollWindow1.TPartsPaint += trackerRollWindow1_TPartsPaint;
            trackerRollWindow1.GridsMouseDown += trackerRollWindow1_GridsMouseDown;
            trackerRollWindow1.PartsMouseDown += trackerRollWindow1_PartsMouseDown;
            //INIT
            TrackObject.Add(new TrackerObject(0));
            TrackObject.Add(new TrackerObject(1));
            BackerObject.Add(new BackerObject(0));
            BackerObject.Add(new BackerObject(1));
            //Parts
            TrackObject[0].PartList.Add(new PartsObject());
            TrackObject[0].PartList[0].StartTime = 0.2;
            TrackObject[0].PartList[0].TickLength = 1920;
            TrackObject[0].PartList[0].PartName = "NAME";
        }

        void trackerRollWindow1_PartsMouseDown(object sender, VocalUtau.DirectUI.Models.TrackerMouseEventArgs e)
        {
            MessageBox.Show(e.AbsoluteTrackID.ToString());
        }

        void trackerRollWindow1_GridsMouseDown(object sender, VocalUtau.DirectUI.Models.TrackerMouseEventArgs e)
        {
        }
        void SingleTrackPaint(VocalUtau.DirectUI.DrawUtils.TrackerPartsDrawUtils.TrackPainterArgs Args, VocalUtau.DirectUI.DrawUtils.TrackerPartsDrawUtils Utils)
        {
            if (Args.TrackObject.GetType() == typeof(TrackerObject))
            {
                Utils.DrawParts(Args.TrackArea,TrackObject[Args.TrackIndex].PartList,Color.BlueViolet);
            }
        }
        void trackerRollWindow1_TPartsPaint(object sender, VocalUtau.DirectUI.DrawUtils.TrackerPartsDrawUtils utils)
        {
            utils.DrawTracks(TrackObject, BackerObject, new VocalUtau.DirectUI.DrawUtils.TrackerPartsDrawUtils.OneTrackPaintHandler(SingleTrackPaint));
        }

        void trackerRollWindow1_TGridePaint(object sender, VocalUtau.DirectUI.DrawUtils.TrackerGridesDrawUtils utils)
        {
            utils.DrawTracks(TrackObject,BackerObject);
        }

        private void trackerRollWindow1_Load(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            trackerRollWindow1.setPianoStartTick(hScrollBar1.Value);
        }
    }
}
