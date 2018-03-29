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
using VocalUtau.DirectUI.Utils.SingerUtils;
using VocalUtau.Formats.Model.BaseObject;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.PianoUtils
{
    public class NoteView
    {
        public delegate void OnNoteEventHandler(NoteDragingType eventType,bool Callback=false);
        public event OnNoteEventHandler NoteActionEnd;
        public event OnNoteEventHandler NoteActionBegin;

        public delegate void OnNoteSelectListChangedHandler(List<int> SelectedIndexs);
        public event OnNoteSelectListChangedHandler NoteSelectListChange;

        public delegate void OnNoteSelectHandler(int SelectedNoteIndex);
        public event OnNoteSelectHandler NoteSelecting;

        const int AntiShakePixel = 3;

        bool _HandleEvents = false;
        
        public bool HandleEvents
        {
            get { return _HandleEvents; }
            set { _HandleEvents = value; }
        }

        IntPtr PartsObjectPtr = IntPtr.Zero;
        PianoRollWindow PianoWindow;
        SingerDataFinder SingerDataFinder=null;
        
        long _TickStepTick = 1;

        public long TickStepTick
        {
            get { return _TickStepTick; }
            set { if (value < 0)_TickStepTick = 1;else _TickStepTick = value; }
        }

        public NoteView(IntPtr PartsObjectPtr, PianoRollWindow PianoWindow)
        {
            this.PianoWindow = PianoWindow;
            this.PartsObjectPtr = PartsObjectPtr;
            hookPianoWindow();
        }
        public void setPartsObjectPtr(IntPtr PartsObjectPtr)
        {
            this.PartsObjectPtr = PartsObjectPtr;
        }
        public void setPianoWindowPtr(PianoRollWindow PianoWindow)
        {
            this.PianoWindow = PianoWindow;
        }
        public void setSingerDataFinder(SingerDataFinder SpliterInstance)
        {
            this.SingerDataFinder = SpliterInstance;
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
            if (e.KeyCode == Keys.Delete)
            {
                NoteDelete();
            }
        }

        public void NoteDelete()
        {
            if (NoteSelectIndexs.Count > 0)
            {
                if (NoteActionBegin != null) NoteActionBegin(NoteDragingType.NoteDelete);
                NoteSelectIndexs.Sort();
                int First = NoteSelectIndexs[0];
                int Last = NoteSelectIndexs[NoteSelectIndexs.Count-1];
                for (int i = Last; i >= First; i--)
                {
                    NoteList.RemoveAt(i);
                }
                int Sp = First - 1;
                int Se = First;
                if (Sp < 0) Sp = 0;
                ClearSelect();
                PartsObject.PitchCompiler.SetupBasePitch_Aysnc(new Formats.Model.VocalObject.ParamTranslater.PitchCompiler.AsyncWorkCallbackHandler((F, L) =>
                {
                    this.PianoWindow.Invoke(new Action(() => { this.PianoWindow.RedrawPiano(); }));
                }), Sp, Se);
                if (this.SingerDataFinder != null)
                {
                    PartsObject po = PartsObject;
                    int Lp = Se;
                    if (Sp > 0) Lp = Sp;
                    this.SingerDataFinder.GetPhonemesDictionary(PartsObject).UpdateLyrics_Aysnc(new Formats.Model.Database.VocalDatabase.SplitDictionary.AsyncWorkCallbackHandler((P,F,L)=>{
                        
                    }), ref po, Lp, Lp); ;
                }
                if (NoteActionEnd != null) NoteActionEnd(NoteDragingType.NoteDelete);
            }
        }

        public void ClearSelect()
        {
            bool isSingle = false;
            if (NoteSelectIndexs.Count == 1)
            {
                isSingle = true;
            }
            NoteSelectIndexs.Clear();
            if (isSingle && NoteSelecting!=null)
            {
                NoteSelecting(-1);
            }
            if (NoteSelectListChange != null) NoteSelectListChange(NoteSelectIndexs);
        }

        public void EditNoteLyric(int StartIndex=-1)
        {
            if(StartIndex==-1 && CurrentNoteIndex > -1)
            {
                StartIndex = CurrentNoteIndex;
            }
            else if (StartIndex == -1 && NoteSelectIndexs.Count > 0)
            {
                StartIndex = NoteSelectIndexs[0];
            }
            if (StartIndex > -1)
            {
                int BeginIndex = StartIndex;
                string Lyric2 = "";
                if (NoteSelectIndexs.IndexOf(StartIndex) != -1)
                {
                    List<string> Lyrics = new List<string>();
                    int MinIndex = StartIndex;
                    int MaxIndex = StartIndex;
                    for (int i = 0; i < NoteSelectIndexs.Count; i++)
                    {
                        MinIndex = Math.Min(NoteSelectIndexs[i], MinIndex);
                        MaxIndex = Math.Max(NoteSelectIndexs[i], MaxIndex);
                    }
                    for (int i = MinIndex; i <= MaxIndex; i++)
                    {
                        Lyrics.Add(NoteList[i].Lyric);
                    }

                    Lyric2 = Microsoft.VisualBasic.Interaction.InputBox("Input New Lyric", "Input Lyric", String.Join(" ", Lyrics.ToArray()));
                    BeginIndex = MinIndex;
                }
                else
                {
                    Lyric2 = Microsoft.VisualBasic.Interaction.InputBox("Input New Lyric", "Input Lyric", NoteList[StartIndex].Lyric);
                    BeginIndex = StartIndex;
                }
                if (Lyric2 != "")
                {
                    if (NoteActionBegin != null) NoteActionBegin(NoteDragingType.LyricEdit);
                    string[] NLyric = Lyric2.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < NLyric.Length; i++)
                    {
                        if ((BeginIndex + i) >= NoteList.Count) break;
                        if (NoteList[BeginIndex + i].Lyric != NLyric[i])
                        {
                            NoteList[BeginIndex + i].Lyric = NLyric[i];
                            if (this.SingerDataFinder != null)
                            {
                                PartsObject po = PartsObject;
                                this.SingerDataFinder.GetPhonemesDictionary(PartsObject).UpdateLyrics(ref po, BeginIndex + i);
                            }
                        }
                    }
                    if (NoteActionEnd != null) NoteActionEnd(NoteDragingType.LyricEdit);
                }
            }
        }

        public int SelectedCount
        {
            get
            {
                return NoteSelectIndexs.Count;
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
                        EditNoteLyric(CurrentNoteIndex);
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
                            ClearSelect();
                        if (Control.ModifierKeys == Keys.Shift && NoteSelectIndexs.Count>0)
                        {
                            for (int i = Math.Min(NoteSelectIndexs[NoteSelectIndexs.Count - 1], CurrentNoteIndex); i <= Math.Max(NoteSelectIndexs[NoteSelectIndexs.Count - 1], CurrentNoteIndex); i++)
                            {
                                if(!NoteSelectIndexs.Contains(i))NoteSelectIndexs.Add(i);
                            }
                            if (NoteSelectListChange != null) NoteSelectListChange(NoteSelectIndexs);
                        }
                        else
                        {
                            NoteSelectIndexs.Add(CurrentNoteIndex);
                            if (NoteSelecting!=null && NoteSelectIndexs.Count == 1)
                            {
                                NoteSelecting(NoteSelectIndexs[0]);
                            }
                            if (NoteSelectListChange != null) NoteSelectListChange(NoteSelectIndexs);
                        }

                    }
                }
            }
        }


        private PartsObject PartsObject
        {
            get
            {
                PartsObject ret = null;
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(PartsObjectPtr);
                    ret = (PartsObject)handle.Target;
                }
                catch { ;}
                return ret;
            }
        }
        private List<NoteObject> NoteList
        {
            get
            {
                if (PartsObject == null) return new List<NoteObject>();
                return PartsObject.NoteList;
            }
        }
    
        public enum NoteDragingType
        {
            None,
            NoteMove,
            NoteLength,
            AreaSelect,
            NoteAdd,
            NoteDelete,
            LyricEdit
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
            Select
        }
        NoteToolsType _NoteToolsStatus = NoteToolsType.Select;

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
            NoteSelectIndexs.Sort();
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
        public List<PitchObject> getSelectRealPitchs(bool independentBlock=false)
        {
            List<PitchObject> ret = new List<PitchObject>();
            if (NoteSelectIndexs.Count == 0) return ret;
            NoteSelectIndexs.Sort();
            long RL = NoteSelectIndexs.Count > 0 ? NoteList[NoteSelectIndexs[0]].Tick : -1;
            
            long TS = NoteList[NoteSelectIndexs[0]].Tick;
            long TE = NoteList[NoteSelectIndexs[NoteSelectIndexs.Count - 1]].Tick + NoteList[NoteSelectIndexs[NoteSelectIndexs.Count - 1]].Length;

            double lastPV = double.NaN;
            for (long i = TS; i <= TE; i ++)
            {
                double cPV = PartsObject.PitchCompiler.getRealPitch(i);
                if ((lastPV != cPV) || (i==TS || i==TE))
                {
                    ret.Add(new PitchObject(i - RL, cPV));
                    lastPV = cPV;
                }
            }
            return ret;
        }
        public bool AddNotes(long LeftTick, List<NoteObject> Notes)
        {
            if (Notes.Count <= 0) return false;
            Notes.Sort();
            long RL = LeftTick+Notes[0].Tick;
            long RR = LeftTick+Notes[Notes.Count - 1].Tick + Notes[Notes.Count - 1].Length;
            bool isAvaliable = true;
            int Sfx = PartsObject.NoteCompiler.FindTickIndex(RL, 0, NoteList.Count);
            for (int i = Sfx; i < NoteList.Count; i++)
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
                ClearSelect();
                int SIndex=NoteList.IndexOf(Notes[0]);
                PartsObject.PitchCompiler.SetupBasePitch(SIndex, SIndex + Notes.Count - 1);
                PianoWindow.RedrawPiano();
            }
            return isAvaliable;
        }

        private void PianoWindow_TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.TrackDrawUtils utils)
        {
            long mt = PianoWindow.MaxShownTick;
            long nt = PianoWindow.MinShownTick;
            int Sfx = PartsObject.NoteCompiler.FindTickIndex(nt, 0, NoteList.Count);
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
                    if (NoteDia.TickEnd - NoteDia.TickStart >= 32)
                    {
                        utils.DrawDia(NoteDia.TickStart, NoteDia.TickEnd, NoteDia.TopNoteNum, NoteDia.BottomNoteNum);
                    }
                }
            }
            if (_HandleEvents)
            {
                switch (_NoteToolsStatus)
                {
                    case NoteToolsType.Add: utils.DrawString(new Point(utils.ClipRectangle.Width - 160, 35), Color.FromArgb(60, 0, 0, 0), "Note Add", 25, FontStyle.Bold); break;
                    case NoteToolsType.Select: utils.DrawString(new Point(utils.ClipRectangle.Width - 160, 35), Color.FromArgb(60, 0, 0, 0), "Note Select", 25, FontStyle.Bold); break;
                }
            }
        }

        private void PianoWindow_TrackMouseDown(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (e.MouseEventArgs.Button != MouseButtons.Left) return;

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
                if (NoteToolsStatus == NoteToolsType.Add)
                {
                    NoteDragingWork = NoteDragingType.NoteAdd;
                    NoteDias.Clear();
                    BlockDia NoteDia = new BlockDia();
                    NoteDia.setStartPoint(e.Tick, e.PitchValue.NoteNumber);
                    NoteDia.setEndPoint(e.Tick, e.PitchValue.NoteNumber);
                    NoteDias.Add(NoteDia);
                }
                else if (NoteToolsStatus == NoteToolsType.Select)
                {
                    NoteDragingWork = NoteDragingType.AreaSelect;
                    BlockDia NoteDia = new BlockDia();
                    NoteDia.setStartPoint(e.Tick, e.PitchValue.NoteNumber);
                    NoteDia.setEndPoint(e.Tick, e.PitchValue.NoteNumber);
                    NoteDias.Add(NoteDia);
                }
            }
            if (NoteDragingWork != NoteDragingType.AreaSelect) if (NoteActionBegin != null)
                {
                    if (NoteDragingWork == NoteDragingType.NoteMove)
                    {
                        NoteActionBegin(NoteDragingWork,true);
                    }
                    else
                    {
                        NoteActionBegin(NoteDragingWork);
                    }
                }
        }

        private void PianoWindow_TrackMouseUp(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents)
            {
                return;
            }
            bool NotMove = false;
            if (NoteDragingWork != NoteDragingType.None)
            {
                if (NoteDragingWork == NoteDragingType.NoteLength)
                {
                    long NewSize = e.Tick - NoteList[CurrentNoteIndex].Tick;
                    NewSize = (long)(NewSize / _TickStepTick) * _TickStepTick;
                    if (NewSize >= 32)
                    {
                        NoteList[CurrentNoteIndex].Length = NewSize;
                        int StartPX = Math.Max(0, CurrentNoteIndex);
                        int EndPX = Math.Min(NoteList.Count-1, CurrentNoteIndex+1);
                        PartsObject.PitchCompiler.SetupBasePitch_Aysnc(new Formats.Model.VocalObject.ParamTranslater.PitchCompiler.AsyncWorkCallbackHandler((F, L) =>
                        {
                            this.PianoWindow.Invoke(new Action(() => { this.PianoWindow.RedrawPiano(); }));
                        }), StartPX, EndPX);
                        if (this.SingerDataFinder != null)
                        {
                            PartsObject po = PartsObject;
                            this.SingerDataFinder.GetPhonemesDictionary(PartsObject).UpdateLyrics_Aysnc(new Formats.Model.Database.VocalDatabase.SplitDictionary.AsyncWorkCallbackHandler((P, F, L) =>
                            {

                            }), ref po, CurrentNoteIndex, CurrentNoteIndex); ;
                        }
                    }
                    NoteDias.Clear();

                }
                else if (NoteDragingWork == NoteDragingType.NoteMove)
                {
                    long minTickChange = (long)PianoWindow.PianoProps.dertPixel2dertTick(AntiShakePixel);
                    List<NoteObject> SelectingNoteCache = new List<NoteObject>();
                    for (int i = 0; i < NoteSelectIndexs.Count; i++) SelectingNoteCache.Add(PartsObject.NoteList[NoteSelectIndexs[i]]);
                    bool TickChanged = false;

                    if (NoteSelectIndexs.IndexOf(CurrentNoteIndex) == -1)
                    {
                        long NewTick = e.Tick - NoteTempTick;
                        NewTick = (long)(NewTick / _TickStepTick) * _TickStepTick;
                        long TickDert = NoteList[CurrentNoteIndex].Tick - NewTick;
                        long NoteDert = NoteList[CurrentNoteIndex].PitchValue.NoteNumber - e.PitchValue.NoteNumber;
                        
                        if (NoteList[CurrentNoteIndex].PitchValue.NoteNumber != e.PitchValue.NoteNumber)
                        {
                            NoteList[CurrentNoteIndex].PitchValue = new PitchAtomObject(e.PitchValue.NoteNumber, NoteList[CurrentNoteIndex].PitchValue.PitchWheel);

                        }
                        TickChanged = Math.Abs(TickDert) >= minTickChange;
                        NotMove = (NoteDert == 0 && Math.Abs(TickDert) < minTickChange);

                        NoteObject OldNoteSt=NoteList[CurrentNoteIndex];
                        long oldStartTick = OldNoteSt.Tick;
                        long oldEndTick = OldNoteSt.Tick + OldNoteSt.Length;
                        if (Math.Abs(TickDert) > minTickChange)
                        {
                            NoteList[CurrentNoteIndex].Tick = NoteList[CurrentNoteIndex].Tick - TickDert;
                            NoteList.Sort();
                        }
                        long AStartTick = Math.Min(oldStartTick,OldNoteSt.Tick);
                        long AEndTick = Math.Max(oldEndTick,OldNoteSt.Tick + OldNoteSt.Length);

                        int ONI = NoteList.IndexOf(OldNoteSt);
                        if (!NotMove)
                        {
                            int StartPX = ONI;
                            int EndPX = ONI;

                            List<NoteObject> PO=PartsObject.NoteList;

                            int FSIdx = PartsObject.NoteCompiler.FindTickIndex(AStartTick, 0, PO.Count);
                            int FEIdx = PartsObject.NoteCompiler.FindTickIndex(AEndTick, FSIdx, PO.Count);
                            StartPX = Math.Min(StartPX, FSIdx);
                            EndPX = Math.Max(EndPX, FEIdx);
                            StartPX = Math.Max(0, StartPX);
                            EndPX = Math.Min(NoteList.Count - 1, EndPX);

                            PartsObject.PitchCompiler.SetupBasePitch_Aysnc(new Formats.Model.VocalObject.ParamTranslater.PitchCompiler.AsyncWorkCallbackHandler((F, L) =>
                            {
                                this.PianoWindow.Invoke(new Action(() => { this.PianoWindow.RedrawPiano(); }));
                            }), StartPX, EndPX);

                            if (this.SingerDataFinder != null)
                            {
                                PartsObject po = PartsObject;
                                this.SingerDataFinder.GetPhonemesDictionary(PartsObject).UpdateLyrics_Aysnc(new Formats.Model.Database.VocalDatabase.SplitDictionary.AsyncWorkCallbackHandler((P, F, L) =>
                                {

                                }), ref po, ONI , ONI); ;
                            }
                        }
                    }
                    else
                    {
                        long NewTick = e.Tick - NoteTempTick;
                        NewTick = (long)(NewTick / _TickStepTick) * _TickStepTick;
                        long TickDert = NoteList[CurrentNoteIndex].Tick - NewTick;
                        long NoteDert = NoteList[CurrentNoteIndex].PitchValue.NoteNumber - e.PitchValue.NoteNumber;
                        NotMove = (NoteDert == 0 && Math.Abs(TickDert) < minTickChange);
                        TickChanged = Math.Abs(TickDert) >= minTickChange;

                        NoteObject OldNoteSt = NoteList[NoteSelectIndexs[0]];
                        NoteObject LastNoteEd = NoteList[NoteSelectIndexs[NoteSelectIndexs.Count - 1]];

                        long oldStartTick = OldNoteSt.Tick;
                        long oldEndTick = LastNoteEd.Tick + LastNoteEd.Length;


                        for (int i = 0; i < SelectingNoteCache.Count; i++)
                        {
                            uint NewNoteNumber = (uint)(SelectingNoteCache[i].PitchValue.NoteNumber - NoteDert);
                            if (SelectingNoteCache[i].PitchValue.NoteNumber != NewNoteNumber)
                            {
                                SelectingNoteCache[i].PitchValue = new PitchAtomObject(NewNoteNumber, SelectingNoteCache[i].PitchValue.PitchWheel);
                            }
                            if (Math.Abs(TickDert) > minTickChange)
                            {
                                SelectingNoteCache[i].Tick = SelectingNoteCache[i].Tick - TickDert;
                            }
                        }
                        if (Math.Abs(TickDert) > minTickChange) NoteList.Sort();
                        long AStartTick = Math.Min(oldStartTick, OldNoteSt.Tick);
                        long AEndTick = Math.Max(oldEndTick, LastNoteEd.Tick + LastNoteEd.Length);

                        int First = Math.Min(NoteList.IndexOf(SelectingNoteCache[0]), NoteList.IndexOf(OldNoteSt));
                        int Last = Math.Max(NoteList.IndexOf(SelectingNoteCache[SelectingNoteCache.Count - 1]), NoteList.IndexOf(LastNoteEd));
                        if (First <= Last)
                        {
                            if (!NotMove)
                            {
                                int StartPX = First;
                                int EndPX = Last;
                                
                                int FSIdx = PartsObject.NoteCompiler.FindTickIndex(AStartTick, 0, PartsObject.NoteList.Count);
                                int FEIdx = PartsObject.NoteCompiler.FindTickIndex(AEndTick,FSIdx, PartsObject.NoteList.Count) + 1;
                                StartPX = Math.Min(StartPX, FSIdx);
                                EndPX = Math.Max(EndPX, FEIdx);
                                StartPX = Math.Max(0, StartPX);
                                EndPX = Math.Min(NoteList.Count - 1, EndPX);
                                
                                PartsObject.PitchCompiler.SetupBasePitch_Aysnc(new Formats.Model.VocalObject.ParamTranslater.PitchCompiler.AsyncWorkCallbackHandler((F, L) =>
                                {
                                    this.PianoWindow.Invoke(new Action(() => { this.PianoWindow.RedrawPiano(); }));
                                }), StartPX, EndPX);

                                if (this.SingerDataFinder != null)
                                {
                                    PartsObject po = PartsObject;
                                    this.SingerDataFinder.GetPhonemesDictionary(PartsObject).UpdateOutboundsLyric_Aysnc(new Formats.Model.Database.VocalDatabase.SplitDictionary.AsyncWorkCallbackHandler((P, F, L) =>
                                    {

                                    }), ref po, StartPX, EndPX); ;
                                }
                            }
                        }
                    }
                    if (TickChanged)
                    {
                        NoteSelectIndexs.Clear();
                        for (int i = 0; i < SelectingNoteCache.Count; i++)
                        {
                            NoteSelectIndexs.Add(PartsObject.NoteList.IndexOf(SelectingNoteCache[i]));
                        }
                    }
                    NoteDias.Clear();
                }
                else if (NoteDragingWork == NoteDragingType.NoteAdd)
                {
                    NoteObject nPN = new NoteObject(NoteDias[0].TickStart, NoteDias[0].TickEnd - NoteDias[0].TickStart, NoteDias[0].TopNoteNum);
                    if (nPN.Length >= 32)
                    {
                        nPN.InitNote();
                        NoteList.Add(nPN);
                        NoteList.Sort();
                        NoteDias.Clear();
                        int NIndex = NoteList.IndexOf(nPN);
                        PartsObject.PitchCompiler.SetupBasePitch_Aysnc(new Formats.Model.VocalObject.ParamTranslater.PitchCompiler.AsyncWorkCallbackHandler((F, L) =>
                        {
                            this.PianoWindow.Invoke(new Action(() => { this.PianoWindow.RedrawPiano(); }));
                        }), NIndex, NIndex);
                        if (this.SingerDataFinder != null)
                        {
                            PartsObject po = PartsObject;
                            this.SingerDataFinder.GetPhonemesDictionary(PartsObject).UpdateLyrics_Aysnc(new Formats.Model.Database.VocalDatabase.SplitDictionary.AsyncWorkCallbackHandler((P, F, L) =>
                            {

                            }), ref po, NIndex, NIndex); ;
                        }
                    }else
                    {
                        NoteDragingWork = NoteDragingType.AreaSelect;
                    }
                }
                else if (NoteDragingWork == NoteDragingType.AreaSelect)
                {
                    ClearSelect();
                    long mt = NoteDias[0].TickEnd;
                    long nt = NoteDias[0].TickStart;
                    if (e.Tick >= nt && e.Tick <= mt)
                    {
                        int Sfx=PartsObject.NoteCompiler.FindTickIndex(nt,0,NoteList.Count);
                        for (int i = Sfx; i < NoteList.Count; i++)
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
                    if (NoteSelectListChange != null) NoteSelectListChange(NoteSelectIndexs);
                    NoteDias.Clear();
                }
                if (NoteDragingWork != NoteDragingType.AreaSelect && NoteActionEnd != null)
                {
                    if (NotMove)
                    {
                        NoteActionEnd(NoteDragingWork,true);
                    }
                    else
                    {
                        NoteActionEnd(NoteDragingWork);
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
            if (NoteDragingWork == NoteDragingType.AreaSelect)
            {
                NoteDias[0].setEndPoint(e.Tick, e.PitchValue.NoteNumber);
            }
            else if (NoteDragingWork == NoteDragingType.NoteAdd)
            {
                NoteDias[0].setEndPoint(e.Tick, e.PitchValue.NoteNumber);
                NoteDias[0].setStartPoint(NoteDias[0].TickStart == e.Tick ? NoteDias[0].TickEnd : NoteDias[0].TickStart, e.PitchValue.NoteNumber);
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
                    int Sfx = PartsObject.NoteCompiler.FindTickIndex(nt, 0, NoteList.Count);
                    for (int i = Sfx; i < NoteList.Count; i++)
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
                                  /*  bool isVBL = false;
                                    if (PN.VerbPrecent > 0 && PN.VerbPrecent<1)
                                    {
                                        long VBP = (long)(PN.Length * (1 - PN.VerbPrecent));
                                        if (e.Tick > PN.Tick + VBP - 5 && e.Tick < PN.Tick + VBP + 5)
                                        {
                                            isVBL = true;
                                        }
                                    }
                                    if (isVBL)
                                    {
                                        PianoWindow.ParentForm.Cursor = Cursors.SizeWE;
                                    }
                                    else*/
                                    {
                                        PianoWindow.ParentForm.Cursor = Cursors.SizeAll;
                                    }
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
