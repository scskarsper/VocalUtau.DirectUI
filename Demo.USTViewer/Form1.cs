using Demo.USTViewer.Model.Objects;
using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Models;
using VocalUtau.DirectUI.Utils.ParamUtils;
using VocalUtau.DirectUI.Utils.PianoUtils;
using VocalUtau.Formats.Model.USTs.Original;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace Demo.USTViewer
{
    public partial class Form1 : Form
    {
        ObjectAlloc<PartsObject> OAC = new ObjectAlloc<PartsObject>();
        NoteView NV = null;
        PitchView PV = null;
        ActionView AV = null;

        PITParamView PitV = null;

        public Form1()
        {
            InitializeComponent();
        }
        ~Form1()
        {
        }


        ProjectObject LoadUST(string FilePath)
        {
            USTOriginalProject USTPO = USTOriginalSerializer.Deserialize(FilePath);
            PartsObject pro = USTOriginalSerializer.UST2Parts(USTPO);

            ProjectObject poj = new ProjectObject();
            poj.InitEmpty();
            poj.TrackerList[1].PartList[0] = pro;

            foreach (NoteObject po in pro.NoteList)
            {
                byte[] bt=System.Text.Encoding.Default.GetBytes(po.Lyric);
                string Str = System.Text.Encoding.GetEncoding("Shift-JIS").GetString(bt);
                po.Lyric = Str;
            }
            hScrollBar1.Maximum = (int)pro.TickLength;
            int sg = 1;
            for (long i = 1; i <= pro.TickLength; i += 32)//
            {
                sg = sg * -1;
                pro.PitchBendsList.Add(new PitchObject(i,sg*0.5));
            }

            string abc=ProjectObject.Serializer.Serialize(poj);

            return poj;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ProjectObject poj = LoadUST(@"D:\VocalUtau\VocalUtau.DebugExampleFiles\DemoUSTS\Sakurane2.Tracks\Track-4b158252-eb7f-4223-b7b0-d78f32e044ec.ust");
            OAC.ReAlloc(poj.TrackerList[1].PartList[0]);

            NV = new NoteView(OAC.IntPtr, this.pianoRollWindow1);
            PV = new PitchView(OAC.IntPtr, this.pianoRollWindow1);

            PitV = new PITParamView(OAC.IntPtr, this.paramCurveWindow1);


            AV = new ActionView(OAC.IntPtr, this.pianoRollWindow1, this.paramCurveWindow1);

            NV.HandleEvents = false;
            PV.HandleEvents = true;
            PV.EarseModeV2 = true;
            PitV.HandleEvents = true;
            NV.NoteActionEnd += NV_NoteActionEnd;
            PV.PitchActionEnd += PV_PitchActionEnd;
            PitV.PitchActionEnd += PitV_PitchActionEnd;
            this.pianoRollWindow1.TrackPaint += TrackPaint;
            InitEventAction();

            AV.TickPos = 480;
        }

        void PitV_PitchActionEnd(PitchView.PitchDragingType eventType)
        {
            InitEventAction();
        }

        void PV_PitchActionEnd(PitchView.PitchDragingType eventType)
        {
            InitEventAction();
        }

        void NV_NoteActionEnd(NoteView.NoteDragingType eventType)
        {
            InitEventAction();
        }

        /// <summary>
        /// BaseWork
        /// </summary>
        #region
        private void PlayNote(int NoteNumber)
        {
            System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart((note) =>
            {
                try
                {
                    using (MidiOut midiOut = new MidiOut(0))
                    {
                        midiOut.Volume = 65535;
                        midiOut.Send(MidiMessage.StartNote(NoteNumber, 127, 1).RawData);
                        System.Threading.Thread.Sleep(500);
                        midiOut.Send(MidiMessage.StopNote(NoteNumber, 127, 1).RawData);
                    }
                }
                catch { ;}
            }));
            th.Start(NoteNumber);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pianoRollWindow1.setCrotchetSize((uint)trackBar1.Value);
            paramCurveWindow1.setCrotchetSize((uint)trackBar1.Value);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pianoRollWindow1.setPianoStartTick(hScrollBar1.Value);
            paramCurveWindow1.setPianoStartTick(hScrollBar1.Value);
        }

        private void pianoRollWindow1_RollMouseDown(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            PlayNote((int)e.PitchValue.NoteNumber);
        }
        #endregion

        private void TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.TrackDrawUtils utils)
        {
           /* utils.DrawPitchLine(PV.getShownPitchLine(), Color.Red);

            switch (PitchDragingStatus)
            {
                case PitchDragingType.DrawLine: utils.DrawPitchLine(PitchMathUtils.CalcLineSilk(PitchStP1, PitchTmpP0), Color.DarkCyan); break;
                case PitchDragingType.DrawGraphJ: utils.DrawPitchLine(PitchMathUtils.CalcGraphJ(PitchStP1, PitchTmpP0), Color.DarkCyan); break;
                case PitchDragingType.DrawGraphR: utils.DrawPitchLine(PitchMathUtils.CalcGraphR(PitchStP1, PitchTmpP0), Color.DarkCyan); break;
                case PitchDragingType.DrawGraphS: utils.DrawPitchLine(PitchMathUtils.CalcGraphS(PitchStP1, PitchTmpP0), Color.DarkCyan); break;
                case PitchDragingType.EarseArea:
                        if(PitchStP1!=null && PitchTmpP0!=null)
                        {
                            PitchView.BlockDia PitchDia = new PitchView.BlockDia();
                            PitchDia.setStartPoint(PitchStP1.Tick,PitchStP1.PitchValue.NoteNumber);
                            PitchDia.setEndPoint(PitchTmpP0.Tick,PitchTmpP0.PitchValue.NoteNumber);
                            utils.DrawDia(PitchDia.TickStart, PitchDia.TickEnd, PitchDia.TopNoteNum, PitchDia.BottomNoteNum);
                        }
                    break;
            }*/
        }

        private void pianoRollWindow1_TrackMouseDown(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            
        }

        private void pianoRollWindow1_TrackMouseUp(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            
        }

        private void pianoRollWindow1_TrackMouseMove(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            
        }

        private void pianoRollWindow1_Load(object sender, EventArgs e)
        {

        }

        void InitEventAction()
        {
            NV.HandleEvents = true;
            NV.NoteToolsStatus = NoteView.NoteToolsType.Select;
            PV.HandleEvents = false;
            PitV.PitchToolsStatus = PitchView.PitchDragingType.None;
            toolStripButton1.BackColor = System.Drawing.SystemColors.ControlDark;
            toolStripButton2.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton3.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton4.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton5.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton6.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton7.BackColor = System.Drawing.SystemColors.Control;
            pianoRollWindow1.Refresh();
            paramCurveWindow1.Refresh();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            InitEventAction();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            NV.HandleEvents = false;
            PV.HandleEvents = true;
            PV.PitchToolsStatus = PitchView.PitchDragingType.DrawLine;
            PitV.PitchToolsStatus = PV.PitchToolsStatus;
            toolStripButton1.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton2.BackColor = System.Drawing.SystemColors.ControlDark;
            toolStripButton3.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton4.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton5.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton6.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton7.BackColor = System.Drawing.SystemColors.Control;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

            NV.HandleEvents = false;
            PV.HandleEvents = true;
            PV.PitchToolsStatus = PitchView.PitchDragingType.DrawGraphJ;
            PitV.PitchToolsStatus = PV.PitchToolsStatus;
            toolStripButton1.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton2.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton3.BackColor = System.Drawing.SystemColors.ControlDark;
            toolStripButton4.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton5.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton6.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton7.BackColor = System.Drawing.SystemColors.Control;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            NV.HandleEvents = false;
            PV.HandleEvents = true;
            PV.PitchToolsStatus = PitchView.PitchDragingType.DrawGraphR;
            PitV.PitchToolsStatus = PV.PitchToolsStatus;
            toolStripButton1.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton2.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton3.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton4.BackColor = System.Drawing.SystemColors.ControlDark;
            toolStripButton5.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton6.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton7.BackColor = System.Drawing.SystemColors.Control;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            NV.HandleEvents = false;
            PV.HandleEvents = true;
            PV.PitchToolsStatus = PitchView.PitchDragingType.DrawGraphS;
            PitV.PitchToolsStatus = PV.PitchToolsStatus;
            toolStripButton1.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton2.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton3.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton4.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton5.BackColor = System.Drawing.SystemColors.ControlDark;
            toolStripButton6.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton7.BackColor = System.Drawing.SystemColors.Control;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            NV.HandleEvents = false;
            PV.HandleEvents = true;
            PV.PitchToolsStatus = PitchView.PitchDragingType.EarseArea;
            PitV.PitchToolsStatus = PV.PitchToolsStatus;
            toolStripButton1.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton2.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton3.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton4.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton5.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton6.BackColor = System.Drawing.SystemColors.ControlDark;
            toolStripButton7.BackColor = System.Drawing.SystemColors.Control;
        }

        private void LSize_Scroll(object sender, ScrollEventArgs e)
        {
            pianoRollWindow1.setNoteHeight((uint)LSize.Value);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            NV.HandleEvents = true;
            NV.NoteToolsStatus = NoteView.NoteToolsType.Add;
            PV.HandleEvents = false;
            toolStripButton1.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton2.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton3.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton4.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton5.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton6.BackColor = System.Drawing.SystemColors.Control;
            toolStripButton7.BackColor = System.Drawing.SystemColors.ControlDark;
        }
        List<NoteObject> PNV = new List<NoteObject>();
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            PNV = NV.getSelectNotes(true);
            toolStripButton8.BackColor = System.Drawing.SystemColors.Control;
            if (PNV.Count > 0) toolStripButton8.BackColor = System.Drawing.SystemColors.ControlDark;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            bool R=NV.AddNotes(0, PNV);
            PNV.Clear();
            toolStripButton8.BackColor = System.Drawing.SystemColors.Control;
            if (PNV.Count > 0) toolStripButton8.BackColor = System.Drawing.SystemColors.ControlDark;
            if (!R) MessageBox.Show("Paste Error");
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            PitV.Zoom = (uint)trackBar2.Value;
        }

    }
}
