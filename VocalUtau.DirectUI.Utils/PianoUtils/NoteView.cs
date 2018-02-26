using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI;
using VocalUtau.DirectUI.Models;
using VocalUtau.DirectUI.Utils.ActionUtils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.PianoUtils
{
    public class NoteView
    {
        const int AntiShakePixel = 3;

        bool _HandleEvents = false;

        public bool HandleEvents
        {
            get { return _HandleEvents; }
            set { _HandleEvents = value; }
        }

        IntPtr NoteListPtr = IntPtr.Zero;
        IntPtr PitchListPtr = IntPtr.Zero;
        PianoRollWindow PianoWindow;

      //  List<PitchObject> Tmp = new List<PitchObject>();

        long _TickStepTick = 1;

        public long TickStepTick
        {
            get { return _TickStepTick; }
            set { if (value < 0)_TickStepTick = 1;else _TickStepTick = value; }
        }

        public NoteView(IntPtr NoteListPtr, IntPtr PitchListPtr, PianoRollWindow PianoWindow)
        {
            this.PianoWindow = PianoWindow;
            setNoteListPtr(NoteListPtr);
            hookPianoWindow();
        }
        public void setNoteListPtr(IntPtr NoteListPtr)
        {
            this.NoteListPtr = NoteListPtr;
        }
        public void setPitchListPtr(IntPtr PitchListPtr)
        {
            this.PitchListPtr = PitchListPtr;
        }
        public void setPianoWindowPtr(PianoRollWindow PianoWindow)
        {
            this.PianoWindow = PianoWindow;
        }
        public void hookPianoWindow()
        {
            PianoWindow.TrackPaint += PianoWindow_TrackPaint;
            PianoWindow.TrackMouseDown += PianoWindow_TrackMouseDown;
            PianoWindow.TrackMouseUp += PianoWindow_TrackMouseUp;
            PianoWindow.TrackMouseMove += PianoWindow_TrackMouseMove;
            PianoWindow.TrackMouseClick += PianoWindow_TrackMouseClick;
            PianoWindow.TrackMouseDoubleClick += PianoWindow_TrackMouseDoubleClick;
            PianoWindow.KeyUp += PianoWindow_KeyUp;
        }

        void PianoWindow_KeyUp(object sender, KeyEventArgs e)
        {
            if (!HandleEvents) return;
            if (e.KeyCode == Keys.Delete && NoteSelectIndexs.Count > 0)
            {
                NoteSelectIndexs.Sort();
                for (int i = 0; i < NoteSelectIndexs.Count; i++)
                {
                    NoteList.RemoveAt(NoteSelectIndexs[i]-i);
                }
                NoteSelectIndexs.Clear();
            }
        }

        void PianoWindow_TrackMouseDoubleClick(object sender, PianoMouseEventArgs e)
        {
            if (NoteDragingWork == NoteDragingType.NoteMove)
            {
                if (CurrentNoteIndex > -1)
                {
                    if (e.Tick - NoteTempTick == NoteList[CurrentNoteIndex].Tick)
                    {
                        int BeginIndex = CurrentNoteIndex;
                        string Lyric2 = "";
                        if (NoteSelectIndexs.IndexOf(CurrentNoteIndex) != -1)
                        {
                            List<string> Lyrics = new List<string>();
                            int MinIndex = CurrentNoteIndex;
                            int MaxIndex = CurrentNoteIndex;
                            for (int i = 0; i < NoteSelectIndexs.Count; i++)
                            {
                                MinIndex = Math.Min(NoteSelectIndexs[i], MinIndex);
                                MaxIndex = Math.Max(NoteSelectIndexs[i], MaxIndex);
                            }
                            for (int i = MinIndex; i <= MaxIndex; i++)
                            {
                                Lyrics.Add(NoteList[i].Lyric);
                            }

                            Lyric2 = Microsoft.VisualBasic.Interaction.InputBox("Input New Lyric", "Input Lyric", String.Join(" ",Lyrics.ToArray()));
                            BeginIndex = MinIndex;
                        }
                        else
                        {
                            Lyric2 = Microsoft.VisualBasic.Interaction.InputBox("Input New Lyric", "Input Lyric", NoteList[CurrentNoteIndex].Lyric);
                            BeginIndex = CurrentNoteIndex;
                        }
                        if (Lyric2 != "")
                        {
                            string[] NLyric = Lyric2.Split(new string[]{" "},StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < NLyric.Length; i++)
                            {
                                NoteList[BeginIndex + i].Lyric = NLyric[i];
                            }
                        }
                    }
                }
            }
        }

        void PianoWindow_TrackMouseClick(object sender, PianoMouseEventArgs e)
        {
            if (NoteDragingWork == NoteDragingType.NoteMove)
            {
                if (CurrentNoteIndex > -1)
                {   
                    if (e.Tick-NoteTempTick==NoteList[CurrentNoteIndex].Tick)
                    {
                        if (Control.ModifierKeys != Keys.Control 
                            &&Control.ModifierKeys != Keys.Shift)
                            NoteSelectIndexs.Clear();
                        if (Control.ModifierKeys == Keys.Shift && NoteSelectIndexs.Count>0)
                        {
                            for (int i = Math.Min(NoteSelectIndexs[NoteSelectIndexs.Count - 1], CurrentNoteIndex); i <= Math.Max(NoteSelectIndexs[NoteSelectIndexs.Count - 1], CurrentNoteIndex); i++)
                            {
                                if(!NoteSelectIndexs.Contains(i))NoteSelectIndexs.Add(i);
                            }
                        }
                        else
                        {
                            NoteSelectIndexs.Add(CurrentNoteIndex);
                        }

                    }
                }
            }
        }


        private List<NoteObject> NoteList
        {
            get
            {
                List<NoteObject> ret = new List<NoteObject>();
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(NoteListPtr);
                    ret = (List<NoteObject>)handle.Target;
                    if (ret == null) ret = new List<NoteObject>();
                }
                catch { ;}
                return ret;
            }
        }
        private List<PitchObject> PitchList
        {
            get
            {
                List<PitchObject> ret = new List<PitchObject>();
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(PitchListPtr);
                    ret = (List<PitchObject>)handle.Target;
                    if (ret == null) ret = new List<PitchObject>();
                }
                catch { ;}
                return ret;
            }
        }
    
        public enum NoteDragingType
        {
            None,
            NoteMove,
            NoteLength,
            AreaSelect,
            NoteAdd
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

        public enum NoteToolsType
        {
            None,
            Add,
            Edit
        }
        NoteToolsType _NoteToolsStatus = NoteToolsType.Edit;

        public NoteToolsType NoteToolsStatus
        {
            get { return _NoteToolsStatus; }
            set { _NoteToolsStatus = value; }
        }

        List<int> NoteSelectIndexs = new List<int>();
        int CurrentNoteIndex = -1;
        long NoteTempTick = 0;
        NoteDragingType NoteDragingWork = NoteDragingType.None;
        List<BlockDia> NoteDias = new List<BlockDia>();

        public List<NoteObject> getSelectNotes(bool independentBlock=false)
        {
            List<NoteObject> ret = new List<NoteObject>();
            long RL = NoteSelectIndexs.Count > 0 ? NoteList[NoteSelectIndexs[0]].Tick: -1;
            if (RL == -1) return ret;
            for (int i = 0; i < NoteSelectIndexs.Count; i++)
            {
                NoteObject PNN = (NoteObject)NoteList[NoteSelectIndexs[i]].Clone();
                PNN.Tick -= RL;
                ret.Add(PNN);
            }
            return ret;
        }
        public bool AddNotes(long LeftTick,List<NoteObject> Notes)
        {
            if (Notes.Count <= 0) return false;
            Notes.Sort();
            long RL = LeftTick+Notes[0].Tick;
            long RR = LeftTick+Notes[Notes.Count - 1].Tick + Notes[Notes.Count - 1].Length;
            bool isAvaliable = true;
            for (int i = 0; i < NoteList.Count; i++)
            {
                NoteObject PN = NoteList[i];
                if (PN.Tick > RR) break;
                if (PN.Tick + PN.Length < RL) continue;
                isAvaliable = false;
            }
            if (isAvaliable)
            {
                for(int i=0;i<Notes.Count;i++)
                {
                    NoteObject PNN = (NoteObject)Notes[i].Clone();
                    PNN.Tick += LeftTick;
                    NoteList.Add(PNN);
                }
                NoteList.Sort();
                NoteSelectIndexs.Clear();
                PianoWindow.RedrawPiano();
            }
            return isAvaliable;
        }

        private void PianoWindow_TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.TrackDrawUtils utils)
        {
            long mt = PianoWindow.MaxShownTick;
            long nt = PianoWindow.MinShownTick;
            for (int i = 0; i < NoteList.Count; i++)
            {
                NoteObject PN = NoteList[i];
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

        private void PianoWindow_TrackMouseDown(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (NoteToolsStatus == NoteToolsType.Add)
            {
                NoteDias.Clear();
                BlockDia NoteDia = new BlockDia();
                NoteDia.setStartPoint(e.Tick, e.PitchValue.NoteNumber);
                NoteDia.setEndPoint(e.Tick, e.PitchValue.NoteNumber);
                NoteDias.Add(NoteDia);
                NoteDragingWork = NoteDragingType.NoteAdd;
            }
            else if (NoteToolsStatus == NoteToolsType.Edit)
            {
                if (CurrentNoteIndex != -1)
                {
                    if (e.Tick > NoteList[CurrentNoteIndex].Tick + NoteList[CurrentNoteIndex].Length - 20)
                    {
                        NoteDragingWork = NoteDragingType.NoteLength;
                    }
                    else
                    {
                        NoteDragingWork = NoteDragingType.NoteMove;
                        NoteTempTick = e.Tick - NoteList[CurrentNoteIndex].Tick;
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
        }

        private void PianoWindow_TrackMouseUp(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents)
            {
                return;
            }
            if (NoteToolsStatus == NoteToolsType.Add)
            {
                if (NoteDragingWork == NoteDragingType.NoteAdd)
                {
                    NoteObject nPN = new NoteObject(NoteDias[0].TickStart, NoteDias[0].TickEnd - NoteDias[0].TickStart, NoteDias[0].TopNoteNum);
                    //判断是否是重叠的
                    #region
                    /*bool isAvaliable = false;
                    long mt = PianoWindow.MaxShownTick+480;
                    long nt = PianoWindow.MinShownTick-480;
                    if(nt<0)nt=0;
                    List<NoteObject> EmptyArea = new List<NoteObject>();
                    for (int i = 0; i < NoteList.Count; i++)
                    {
                        NoteObject RPN = i==0?null:NoteList[i-1];
                        NoteObject PN = NoteList[i];
                        if (PN.Tick >= mt) break;
                        if (PN.Tick <= nt) continue;
                        if (PN.Tick > 0 && RPN == null)
                        {
                            EmptyArea.Add(new NoteObject(0, PN.Tick - 1, 60));//A1;
                        }else if(i==NoteList.Count-1)
                        {
                            long ABL=mt-PN.Tick-PN.Length;
                            EmptyArea.Add(new NoteObject(PN.Tick+PN.Length,ABL<=0?1:ABL, 60));//AN;
                        }
                        else
                        {
                            long S=RPN.Tick + RPN.Length;
                            long L = PN.Tick - S;
                            if (L > 32)
                            {
                                EmptyArea.Add(new NoteObject(S, L, 60));//A1;
                            }
                        }
                    }
                        long nPNL = nPN.Tick;
                        long nPNR = nPN.Tick + nPN.Length;
                    for (int i = 0; i < EmptyArea.Count; i++)
                    {
                        long EAL = EmptyArea[i].Tick;
                        long EAR = EmptyArea[i].Tick + EmptyArea[i].Length;
                        if (EAL<=nPNL && EAR>=nPNR)
                        {
                            //PN in EA
                            isAvaliable = true;
                        }
                        else if (nPNL <= EAL && nPNR >= EAR)
                        {
                            //EA in PN
                            isAvaliable = true;
                        }else if(nPNL<EAR && nPNL>EAL)
                        {
                            isAvaliable = true;
                        }
                        else if (nPNR > EAL && nPNR < EAR)
                        {
                            isAvaliable = true;
                        }
                    }
                    if (true || isAvaliable)
                    */
                    #endregion
                    {
                        NoteList.Add(nPN);
                        NoteList.Sort();
                    }
                    NoteDias.Clear();
                }
            }
            else if (NoteToolsStatus == NoteToolsType.Edit)
            {
                if (NoteDragingWork != NoteDragingType.None)
                {
                    if (NoteDragingWork == NoteDragingType.NoteLength)
                    {
                        long NewSize = e.Tick - NoteList[CurrentNoteIndex].Tick;
                        NewSize = (long)(NewSize / _TickStepTick) * _TickStepTick;
                        if (NewSize >= 32)
                        {
                            NoteList[CurrentNoteIndex].Length = NewSize;
                        }
                        NoteDias.Clear();
                    }
                    else if (NoteDragingWork == NoteDragingType.NoteMove)
                    {
                        long minTickChange = (long)PianoWindow.PianoProps.dertPixel2dertTick(AntiShakePixel);
                        if (NoteSelectIndexs.IndexOf(CurrentNoteIndex) == -1)
                        {
                            long NewTick = e.Tick - NoteTempTick;
                            NewTick = (long)(NewTick / _TickStepTick) * _TickStepTick;
                            long TickDert = NoteList[CurrentNoteIndex].Tick - NewTick;

                            if (NoteList[CurrentNoteIndex].PitchValue.NoteNumber != e.PitchValue.NoteNumber)
                            {
                                NoteList[CurrentNoteIndex].PitchValue = new PitchAtomObject(e.PitchValue.NoteNumber, NoteList[CurrentNoteIndex].PitchValue.PitchWheel);
                            }
                            if (Math.Abs(TickDert) > minTickChange)
                            {
                                NoteList[CurrentNoteIndex].Tick = NoteList[CurrentNoteIndex].Tick - TickDert;
                            }
                        }
                        else
                        {
                            long NewTick = e.Tick - NoteTempTick;
                            NewTick = (long)(NewTick / _TickStepTick) * _TickStepTick;
                            long TickDert = NoteList[CurrentNoteIndex].Tick - NewTick;
                            long NoteDert = NoteList[CurrentNoteIndex].PitchValue.NoteNumber - e.PitchValue.NoteNumber;
                            for (int i = 0; i < NoteSelectIndexs.Count; i++)
                            {
                                uint NewNoteNumber = (uint)(NoteList[NoteSelectIndexs[i]].PitchValue.NoteNumber - NoteDert);
                                if (NoteList[NoteSelectIndexs[i]].PitchValue.NoteNumber != NewNoteNumber)
                                {
                                    NoteList[NoteSelectIndexs[i]].PitchValue = new PitchAtomObject(NewNoteNumber, NoteList[NoteSelectIndexs[i]].PitchValue.PitchWheel);
                                }
                                if (Math.Abs(TickDert) > minTickChange)
                                {
                                    NoteList[NoteSelectIndexs[i]].Tick = NoteList[NoteSelectIndexs[i]].Tick - TickDert;
                                }
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
                            for (int i = 0; i < NoteList.Count; i++)
                            {
                                NoteObject PN = NoteList[i];
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
                }
            }
            NoteDragingWork = NoteDragingType.None;
        }

        private void PianoWindow_TrackMouseMove(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents)
            {
                NoteDragingWork = NoteDragingType.None;
                NoteDias.Clear();
                return;
            }
            if (NoteToolsStatus == NoteToolsType.Add)
            {
                if (NoteDragingWork == NoteDragingType.NoteAdd)
                {
                    NoteDias[0].setEndPoint(e.Tick, e.PitchValue.NoteNumber);
                    NoteDias[0].setStartPoint(NoteDias[0].TickStart == e.Tick ? NoteDias[0].TickEnd : NoteDias[0].TickStart, e.PitchValue.NoteNumber);
                }
            }else if (NoteToolsStatus == NoteToolsType.Edit)
            {
                if (NoteDragingWork == NoteDragingType.AreaSelect)
                {
                    NoteDias[0].setEndPoint(e.Tick, e.PitchValue.NoteNumber);
                }
                else if (NoteDragingWork == NoteDragingType.NoteMove)
                {
                    long minTickChange = (long)PianoWindow.PianoProps.dertPixel2dertTick(AntiShakePixel);
                    if (NoteSelectIndexs.IndexOf(CurrentNoteIndex) == -1)
                    {
                        long NewTick = e.Tick - NoteTempTick;
                        NewTick = (long)(NewTick / _TickStepTick) * _TickStepTick;
                        long TickDert = NoteList[CurrentNoteIndex].Tick - NewTick;
                        if (NoteDias.Count == 0) NoteDias.Add(new BlockDia());

                        if (Math.Abs(TickDert) <= minTickChange)
                        {
                            TickDert = 0;
                        }
                        long CurNewTick = NoteList[CurrentNoteIndex].Tick - TickDert;
                        NoteDias[0].setStartPoint(CurNewTick, e.PitchValue.NoteNumber);
                        NoteDias[0].setEndPoint(CurNewTick + NoteList[CurrentNoteIndex].Length, e.PitchValue.NoteNumber);
                    }
                    else
                    {
                        long NewTick = e.Tick - NoteTempTick;
                        NewTick = (long)(NewTick / _TickStepTick) * _TickStepTick;
                        long TickDert = NoteList[CurrentNoteIndex].Tick - NewTick;
                        if (Math.Abs(TickDert) <= minTickChange)
                        {
                            TickDert = 0;
                        }
                        long NoteDert = NoteList[CurrentNoteIndex].PitchValue.NoteNumber - e.PitchValue.NoteNumber;
                        for (int i = 0; i < NoteSelectIndexs.Count; i++)
                        {
                            uint NewNoteNumber = (uint)(NoteList[NoteSelectIndexs[i]].PitchValue.NoteNumber - NoteDert);

                            if (NoteDias.Count < NoteSelectIndexs.Count) NoteDias.Add(new BlockDia());

                            long CurNewTick = NoteList[NoteSelectIndexs[i]].Tick - TickDert;
                            NoteDias[i].setStartPoint(CurNewTick, NewNoteNumber);
                            NoteDias[i].setEndPoint(CurNewTick + NoteList[NoteSelectIndexs[i]].Length, NewNoteNumber);
                        }
                    }
                }
                else if (NoteDragingWork == NoteDragingType.NoteLength)
                {
                    long NewSize = e.Tick - NoteList[CurrentNoteIndex].Tick;
                    NewSize = (long)(NewSize / _TickStepTick) * _TickStepTick;
                    if (NewSize >= 32)
                    {
                        if (NoteDias.Count == 0) NoteDias.Add(new BlockDia());
                        NoteDias[0].setStartPoint(NoteList[CurrentNoteIndex].Tick, e.PitchValue.NoteNumber);
                        NoteDias[0].setEndPoint(NoteList[CurrentNoteIndex].Tick + NewSize, e.PitchValue.NoteNumber);
                    }
                }
                else if (NoteDragingWork == NoteDragingType.None)
                {
                    int cnn = -1;
                    long mt = PianoWindow.MaxShownTick;
                    long nt = PianoWindow.MinShownTick;
                    if (e.Tick >= nt && e.Tick <= mt)
                    {
                        for (int i = 0; i < NoteList.Count; i++)
                        {
                            PianoWindow.ParentForm.Cursor = Cursors.Arrow;
                            NoteObject PN = NoteList[i];
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
                                        PianoWindow.ParentForm.Cursor = Cursors.SizeWE;
                                    }
                                    else
                                    {
                                        PianoWindow.ParentForm.Cursor = Cursors.SizeAll;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    CurrentNoteIndex = cnn;
                }
            }
        }
    
    }
}
