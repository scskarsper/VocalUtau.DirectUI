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
using VocalUtau.DirectUI.Utils.PianoUtils;
using VocalUtau.Formats.Model.USTs.Original;
using VocalUtau.Formats.Model.VocalObject;

namespace Demo.USTViewer
{
    public partial class Form1 : Form
    {
        ViewObjects RollObjects = new ViewObjects();
        NoteView NV = null;

        public enum NoteDragingType
        {
            None,
            NoteMove,
            NoteLength,
            AreaSelect
        }
        public class BlockDia
        {
            public long TickStart
            {
                get
                {
                    return Math.Min(P1,P2);
                }
            }
            public long TickEnd
            {
                get
                {
                    return Math.Max(P1,P2);
                }
            }
            public uint TopNoteNum
            {
                get
                {
                    return Math.Max(N1,N2);
                }
            }
            public uint BottomNoteNum
            {
                get
                {
                    return Math.Min(N1,N2);
                }
            }

            long P1,P2;
            uint N1,N2;
            public void setStartPoint(long Tick,uint NoteNum)
            {
                P1=Tick;
                N1=NoteNum;
            }
            public void setEndPoint(long Tick,uint NoteNum)
            {
                P2=Tick;
                N2=NoteNum;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }
        ~Form1()
        {
        }

        List<int> NoteSelectIndexs = new List<int>();
        int CurrentNoteIndex = -1;
        long NoteTempTick = 0;
        NoteDragingType NoteDragingWork = NoteDragingType.None;
        List<BlockDia> NoteDias = new List<BlockDia>();


        void LoadUST(string FilePath)
        {
            USTOriginalProject USTPO = USTOriginalSerializer.Deserialize(FilePath);
            PartsObject pro = USTOriginalSerializer.UST2Parts(USTPO);

            foreach (KeyValuePair<long, NoteObject> po in pro.NoteList)
            {
                PianoNote PNote = new PianoNote(po.Key, po.Value.TickLength,
                        new VocalUtau.DirectUI.PitchValuePair((uint)po.Value.NoteNum, 0));
                PNote.OctaveType = VocalUtau.DirectUI.PitchValuePair.OctaveTypeEnum.Piano;
                byte[] byteArray = System.Text.Encoding.Default.GetBytes (po.Value.Lyric);
                PNote.Lyric = System.Text.Encoding.GetEncoding("Shift-JIS").GetString(byteArray);
                RollObjects.NoteList.Add(PNote);
                PitchNode PitN = new PitchNode(po.Key, po.Value.NoteNum);
                RollObjects.PitchList.Add(PitN);
                PitN = new PitchNode(po.Key+po.Value.TickLength, po.Value.NoteNum);
                RollObjects.PitchList.Add(PitN);
            }
            hScrollBar1.Maximum = (int)pro.TickLength;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadUST(@"D:\VocalUtau\IncludeLib\UTAUMixer\DemoUSTS\Sakurane2.Tracks\Track-4b158252-eb7f-4223-b7b0-d78f32e044ec.ust");
            NV = new NoteView(RollObjects.NotelistPtr, this.pianoRollWindow1);
            NV.HandleEvents = true;
            this.pianoRollWindow1.TrackPaint+=TrackPaint;
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
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pianoRollWindow1.setPianoStartTick(hScrollBar1.Value);
        }

        private void pianoRollWindow1_RollMouseDown(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            PlayNote((int)e.PitchValue.NoteNumber);
        }
        #endregion

        private void TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.TrackDrawUtils utils)
        {
            utils.DrawPitchLine(RollObjects.PitchList, Color.Red);
        }
        private void pianoRollWindow1_TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.TrackDrawUtils utils)
        {
            //utils.DrawPitchLine(RollObjects.PitchList, Color.Red);
            return;
            long mt = pianoRollWindow1.MaxShownTick;
            long nt = pianoRollWindow1.MinShownTick;
            for (int i = 0; i < RollObjects.NoteList.Count; i++)
            {
                PianoNote PN = RollObjects.NoteList[i];
                if (PN.Tick >= mt) break;
                if (PN.Tick + PN.Length < nt) continue;
                utils.DrawNote(PN, (NoteSelectIndexs.IndexOf(i)>-1)?Color.Green:Color.SkyBlue);
            }
            if(NoteDias.Count!=0)
            {
                foreach (BlockDia NoteDia in NoteDias)
                {
                    utils.DrawDia(NoteDia.TickStart, NoteDia.TickEnd, NoteDia.TopNoteNum, NoteDia.BottomNoteNum);
                }
            }
        }

        private void pianoRollWindow1_TrackMouseDown(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            return;
            if (CurrentNoteIndex != -1)
            {
                if (e.Tick > RollObjects.NoteList[CurrentNoteIndex].Tick + RollObjects.NoteList[CurrentNoteIndex].Length - 20)
                {
                    NoteDragingWork = NoteDragingType.NoteLength;
                }
                else
                {
                    NoteDragingWork = NoteDragingType.NoteMove;
                    NoteTempTick = e.Tick - RollObjects.NoteList[CurrentNoteIndex].Tick;
                }
            }
            else
            {
                NoteDragingWork = NoteDragingType.AreaSelect;
                BlockDia NoteDia = new BlockDia();
                NoteDia.setStartPoint(e.Tick, e.PitchValue.NoteNumber);
                NoteDia.setEndPoint(e.Tick, e.PitchValue.NoteNumber);
                NoteDias.Add(NoteDia);
            }
        }

        private void pianoRollWindow1_TrackMouseUp(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            return;
            if (NoteDragingWork != NoteDragingType.None)
            {
                if (NoteDragingWork == NoteDragingType.NoteLength)
                {
                    long NewSize = e.Tick - RollObjects.NoteList[CurrentNoteIndex].Tick;
                    if (NewSize >= 32)
                    {
                        RollObjects.NoteList[CurrentNoteIndex].Length = NewSize;
                    }
                    NoteDias.Clear();
                }
                else if (NoteDragingWork == NoteDragingType.NoteMove)
                {
                    if (NoteSelectIndexs.IndexOf(CurrentNoteIndex) == -1)
                    {
                        if (RollObjects.NoteList[CurrentNoteIndex].PitchValue.NoteNumber != e.PitchValue.NoteNumber)
                        {
                            RollObjects.NoteList[CurrentNoteIndex].PitchValue = new VocalUtau.DirectUI.PitchValuePair(e.PitchValue.NoteNumber, RollObjects.NoteList[CurrentNoteIndex].PitchValue.PitchWheel);
                        }
                        RollObjects.NoteList[CurrentNoteIndex].Tick = e.Tick - NoteTempTick;
                    }
                    else
                    {
                        long NewTick = e.Tick - NoteTempTick;
                        long TickDert = RollObjects.NoteList[CurrentNoteIndex].Tick - NewTick;
                        long NoteDert = RollObjects.NoteList[CurrentNoteIndex].PitchValue.NoteNumber - e.PitchValue.NoteNumber;
                        for (int i = 0; i < NoteSelectIndexs.Count; i++)
                        {
                            uint NewNoteNumber = (uint)(RollObjects.NoteList[NoteSelectIndexs[i]].PitchValue.NoteNumber - NoteDert);
                            if (RollObjects.NoteList[NoteSelectIndexs[i]].PitchValue.NoteNumber != NewNoteNumber)
                            {
                                RollObjects.NoteList[NoteSelectIndexs[i]].PitchValue = new VocalUtau.DirectUI.PitchValuePair(NewNoteNumber, RollObjects.NoteList[NoteSelectIndexs[i]].PitchValue.PitchWheel);
                            }
                            RollObjects.NoteList[NoteSelectIndexs[i]].Tick = RollObjects.NoteList[NoteSelectIndexs[i]].Tick - TickDert;
                        }
                    }
                    NoteDias.Clear();
                }
                else if (NoteDragingWork == NoteDragingType.AreaSelect)
                {
                    NoteSelectIndexs.Clear();
                    long mt = NoteDias[0].TickEnd;
                    long nt = NoteDias[0].TickStart;
                    if (e.Tick >= nt && e.Tick <= mt)
                    {
                        for (int i = 0; i < RollObjects.NoteList.Count; i++)
                        {
                            PianoNote PN = RollObjects.NoteList[i];
                            if (PN.Tick >= mt) break;
                            if (PN.Tick + PN.Length < nt) continue;
                            if (PN.PitchValue.NoteNumber >= NoteDias[0].BottomNoteNum && PN.PitchValue.NoteNumber <= NoteDias[0].TopNoteNum)
                            {
                                NoteSelectIndexs.Add(i);
                            }
                        }
                    }
                    NoteDias.Clear();
                }
                NoteDragingWork = NoteDragingType.None;
            }
        }

        private void pianoRollWindow1_TrackMouseMove(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            return;
            if (NoteDragingWork == NoteDragingType.AreaSelect)
            {
                NoteDias[0].setEndPoint(e.Tick, e.PitchValue.NoteNumber);
            }
            else if (NoteDragingWork == NoteDragingType.NoteMove)
            {
                if (NoteSelectIndexs.IndexOf(CurrentNoteIndex) == -1)
                {
                    if (NoteDias.Count == 0) NoteDias.Add(new BlockDia());
                    NoteDias[0].setStartPoint(RollObjects.NoteList[CurrentNoteIndex].Tick, e.PitchValue.NoteNumber);
                    NoteDias[0].setEndPoint(RollObjects.NoteList[CurrentNoteIndex].Tick + RollObjects.NoteList[CurrentNoteIndex].Length, e.PitchValue.NoteNumber);
                }
                else
                {
                    long NewTick = e.Tick - NoteTempTick;
                    long TickDert = RollObjects.NoteList[CurrentNoteIndex].Tick - NewTick;
                    long NoteDert = RollObjects.NoteList[CurrentNoteIndex].PitchValue.NoteNumber - e.PitchValue.NoteNumber;
                    for (int i = 0; i < NoteSelectIndexs.Count; i++)
                    {
                        uint NewNoteNumber = (uint)(RollObjects.NoteList[NoteSelectIndexs[i]].PitchValue.NoteNumber - NoteDert);

                        if (NoteDias.Count < NoteSelectIndexs.Count) NoteDias.Add(new BlockDia());

                        NoteDias[i].setStartPoint(RollObjects.NoteList[NoteSelectIndexs[i]].Tick - TickDert, NewNoteNumber);
                        NoteDias[i].setEndPoint(RollObjects.NoteList[NoteSelectIndexs[i]].Tick - TickDert + RollObjects.NoteList[NoteSelectIndexs[i]].Length, NewNoteNumber);
                    }
                }
            }
            else if (NoteDragingWork == NoteDragingType.NoteLength)
            {
                long NewSize = e.Tick - RollObjects.NoteList[CurrentNoteIndex].Tick;
                if (NewSize >= 32)
                {
                    if (NoteDias.Count == 0) NoteDias.Add(new BlockDia());
                    NoteDias[0].setStartPoint(RollObjects.NoteList[CurrentNoteIndex].Tick, e.PitchValue.NoteNumber);
                    NoteDias[0].setEndPoint(RollObjects.NoteList[CurrentNoteIndex].Tick + NewSize, e.PitchValue.NoteNumber);
                }
            }
            else if (NoteDragingWork == NoteDragingType.None)
            {
                int cnn = -1;
                long mt = pianoRollWindow1.MaxShownTick;
                long nt = pianoRollWindow1.MinShownTick;
                if (e.Tick >= nt && e.Tick <= mt)
                {
                    for (int i = 0; i < RollObjects.NoteList.Count; i++)
                    {
                        this.Cursor = Cursors.Arrow;
                        PianoNote PN = RollObjects.NoteList[i];
                        if (PN.Tick >= mt) break;
                        if (PN.Tick + PN.Length < nt) continue;
                        if (e.PitchValue.NoteNumber == PN.PitchValue.NoteNumber)
                        {
                            if (e.Tick >= PN.Tick && e.Tick <= PN.Tick + PN.Length)
                            {
                                Console.WriteLine("Mouse in Note " + PN.Lyric);
                                cnn = i;
                                if (e.Tick > PN.Tick + PN.Length - 20)
                                {
                                    this.Cursor = Cursors.SizeWE;
                                }
                                else
                                {
                                    this.Cursor = Cursors.SizeAll;
                                }
                                break;
                            }
                        }
                    }
                }
                CurrentNoteIndex = cnn;
            }
        }

        private void pianoRollWindow1_Load(object sender, EventArgs e)
        {

        }

    }
}
