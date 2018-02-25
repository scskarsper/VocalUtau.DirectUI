using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI;
using VocalUtau.DirectUI.Models;
using VocalUtau.DirectUI.Utils.MathUtils;

namespace VocalUtau.DirectUI.Utils.PianoUtils
{
    public class PitchView
    {
        const long AntiBordTick = 480;
        IntPtr NoteListPtr = IntPtr.Zero;
        IntPtr PitchListPtr = IntPtr.Zero;
        PianoRollWindow PianoWindow;
        bool _EarseModeV2 = true;

        public bool EarseModeV2
        {
            get { return _EarseModeV2; }
            set { _EarseModeV2 = value; }
        }

        public enum PitchDragingType
        {
            None,
            DrawLine,
            DrawGraphJ,
            DrawGraphR,
            DrawGraphS,
            EarseArea
        }
        PitchDragingType _PitchToolsStatus = PitchDragingType.EarseArea;

        public PitchDragingType PitchToolsStatus
        {
            get { return _PitchToolsStatus; }
            set { _PitchToolsStatus = value;
                if (_PitchToolsStatus == PitchDragingType.None)
                {
                    PianoWindow.ParentForm.Cursor = Cursors.Arrow;
                }
                else
                {
                    PianoWindow.ParentForm.Cursor = Cursors.Cross;
                }
            }
        }
        PitchDragingType PitchDragingStatus = PitchDragingType.None;
        PitchNode PitchStP1 = null;
        PitchNode PitchTmpP0 = null;


        bool _HandleEvents = false;

        public bool HandleEvents
        {
            get { return _HandleEvents; }
            set { _HandleEvents = value; }
        }

        long _TickStepTick = 5;
        public long TickStepTick
        {
            get { return _TickStepTick; }
            set { if (value < 0)_TickStepTick = 5;else _TickStepTick = value; }
        }

        public PitchView(IntPtr NoteListPtr, IntPtr PitchListPtr, PianoRollWindow PianoWindow)
        {
            this.PianoWindow = PianoWindow;
            setNoteListPtr(NoteListPtr);
            setPitchListPtr(PitchListPtr);
            hookPianoWindow();
        }

        public void hookPianoWindow()
        {
            PianoWindow.TrackPaint += PianoWindow_TrackPaint;
            PianoWindow.TrackMouseDown += PianoWindow_TrackMouseDown;
            PianoWindow.TrackMouseUp += PianoWindow_TrackMouseUp;
            PianoWindow.TrackMouseMove += PianoWindow_TrackMouseMove;
        }
        private List<PianoNote> NoteList
        {
            get
            {
                List<PianoNote> ret = new List<PianoNote>();
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(NoteListPtr);
                    ret = (List<PianoNote>)handle.Target;
                    if (ret == null) ret = new List<PianoNote>();
                }
                catch { ;}
                return ret;
            }
        }
        private List<PitchNode> PitchList
        {
            get
            {
                List<PitchNode> ret = new List<PitchNode>();
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(PitchListPtr);
                    ret = (List<PitchNode>)handle.Target;
                    if (ret == null) ret = new List<PitchNode>();
                }
                catch { ;}
                return ret;
            }
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

        private void PianoWindow_TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.TrackDrawUtils utils)
        {
            utils.DrawPitchLine(getShownPitchLine(), Color.Red);

            switch (PitchDragingStatus)
            {
                case PitchDragingType.DrawLine: utils.DrawPitchLine(PitchMathUtils.CalcLineSilk(PitchStP1, PitchTmpP0), Color.DarkCyan); break;
                case PitchDragingType.DrawGraphJ: utils.DrawPitchLine(PitchMathUtils.CalcGraphJ(PitchStP1, PitchTmpP0), Color.DarkCyan); break;
                case PitchDragingType.DrawGraphR: utils.DrawPitchLine(PitchMathUtils.CalcGraphR(PitchStP1, PitchTmpP0), Color.DarkCyan); break;
                case PitchDragingType.DrawGraphS: utils.DrawPitchLine(PitchMathUtils.CalcGraphS(PitchStP1, PitchTmpP0), Color.DarkCyan); break;
                case PitchDragingType.EarseArea:
                    if (PitchStP1 != null && PitchTmpP0 != null)
                    {
                        PitchView.BlockDia PitchDia = new PitchView.BlockDia();
                        PitchDia.setStartPoint(PitchStP1.Tick, PitchStP1.PitchValue.NoteNumber);
                        PitchDia.setEndPoint(PitchTmpP0.Tick, PitchTmpP0.PitchValue.NoteNumber);
                        utils.DrawDia(PitchDia.TickStart, PitchDia.TickEnd, PitchDia.TopNoteNum, PitchDia.BottomNoteNum);
                    }
                    break;
            }
        }

        private void PianoWindow_TrackMouseDown(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents)
            {
                PitchDragingStatus = PitchDragingType.None;
                PitchStP1 = null;
                PitchTmpP0 = null;
                return;
            }
            if (_PitchToolsStatus == PitchDragingType.None) return;
            if (PitchDragingStatus != PitchDragingType.None) return;
            PitchStP1 = new PitchNode(e.Tick, e.PitchValue.PitchValue);
            PitchDragingStatus = _PitchToolsStatus;
        }

        private void PianoWindow_TrackMouseUp(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (PitchDragingStatus == PitchDragingType.None) return;
            PitchNode PitchEdP2 = new PitchNode(e.Tick, e.PitchValue.PitchValue);

            switch (PitchDragingStatus)
            {
                case PitchDragingType.DrawLine: replacePitchLine(PitchMathUtils.CalcLineSilk(PitchStP1, PitchEdP2)); break;
                case PitchDragingType.DrawGraphJ: replacePitchLine(PitchMathUtils.CalcGraphJ(PitchStP1, PitchEdP2)); break;
                case PitchDragingType.DrawGraphR: replacePitchLine(PitchMathUtils.CalcGraphR(PitchStP1, PitchEdP2)); break;
                case PitchDragingType.DrawGraphS: replacePitchLine(PitchMathUtils.CalcGraphS(PitchStP1, PitchEdP2)); break;
                case PitchDragingType.EarseArea:
                    if (PitchStP1 != null && PitchTmpP0 != null)
                    {
                        PitchView.BlockDia PitchDia = new PitchView.BlockDia();
                        PitchDia.setStartPoint(PitchStP1.Tick, PitchStP1.PitchValue.NoteNumber);
                        PitchDia.setEndPoint(PitchTmpP0.Tick, PitchTmpP0.PitchValue.NoteNumber);
                        earsePitchLine(PitchDia, _EarseModeV2);
                    }
                    break;
            }

            PitchDragingStatus = PitchDragingType.None;
            PitchStP1 = null;
            PitchTmpP0 = null;
        }

        private void PianoWindow_TrackMouseMove(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (PitchDragingStatus == PitchDragingType.None)
            {
                return;
            }
            PitchTmpP0 = new PitchNode(e.Tick, e.PitchValue.PitchValue);
            if (_PitchToolsStatus == PitchDragingType.None)
            {
                PianoWindow.ParentForm.Cursor = Cursors.Arrow;
            }
            else
            {
                PianoWindow.ParentForm.Cursor = Cursors.Cross;
            }
        }
    

        //ShouldPrivate
        public class BlockDia
        {
            public long TickStart
            {
                get
                {
                    return Math.Min(P1, P2);
                }
            }
            public long TickEnd
            {
                get
                {
                    return Math.Max(P1, P2);
                }
            }
            public uint TopNoteNum
            {
                get
                {
                    return Math.Max(N1, N2);
                }
            }
            public uint BottomNoteNum
            {
                get
                {
                    return Math.Min(N1, N2);
                }
            }

            long P1, P2;
            uint N1, N2;
            public void setStartPoint(long Tick, uint NoteNum)
            {
                P1 = Tick;
                N1 = NoteNum;
            }
            public void setEndPoint(long Tick, uint NoteNum)
            {
                P2 = Tick;
                N2 = NoteNum;
            }
        }
        public List<PitchNode> getShownPitchLine()
        {
            List<PitchNode> ret = new List<PitchNode>();
            long MinTick = PianoWindow.MinShownTick - AntiBordTick;
            long MaxTick = PianoWindow.MaxShownTick + AntiBordTick;
            if (MinTick < 0) MinTick = 0;
            if (MaxTick <= MinTick) return ret;
            int NextPitchIndex = -1;
            for (int i = 0; i < PitchList.Count; i++)
            {
                if (PitchList[i].Tick < MinTick) continue;
                NextPitchIndex = i;
                break;
            }
            for (int i = 0; i < NoteList.Count; i++)
            {
                PianoNote PN = NoteList[i];
                if (PN.Tick >= MaxTick) break;
                if (PN.Tick + PN.Length < MinTick) continue;
                long StartPoint = PN.Tick;
                long EndPoint = PN.Tick+PN.Length;
                if (ret.Count > 0)
                {
                    long LE = ret[ret.Count - 1].Tick;
                    if (LE >= StartPoint)
                    {
                        StartPoint = ret[ret.Count - 1].Tick+1;//Tick?
                    }
                }
                if (NextPitchIndex == -1 || NextPitchIndex == PitchList.Count || PitchList[NextPitchIndex].Tick >= MaxTick || 
                    PitchList[NextPitchIndex].Tick > EndPoint
                    )
                {
                    //NoPitch
                    PitchNode PNS = new PitchNode(StartPoint, PN.PitchValue.PitchValue);
                    PitchNode PNE = new PitchNode(EndPoint, PN.PitchValue.PitchValue);
                    ret.Add(PNS);
                    ret.Add(PNE);
                }else
                {
                    PitchNode PNS = null;
                    PitchNode PNE = null;
                    while (NextPitchIndex < PitchList.Count && PitchList[NextPitchIndex].Tick <= EndPoint && PitchList[NextPitchIndex].Tick < MaxTick)
                    {
                        if (PitchList[NextPitchIndex].Tick < StartPoint)
                        {
                            NextPitchIndex++;
                            continue;
                        }
                        if (PNS == null && PitchList[NextPitchIndex].Tick > StartPoint)
                        {
                            PNS = new PitchNode(StartPoint, PN.PitchValue.NoteNumber + PitchList[NextPitchIndex].PitchValue.PitchValue);
                            ret.Add(PNS);
                        }
                        PitchNode PNP = new PitchNode(PitchList[NextPitchIndex].Tick, PN.PitchValue.NoteNumber+PitchList[NextPitchIndex].PitchValue.PitchValue);
                        ret.Add(PNP);
                        NextPitchIndex++;
                    }
                    if(ret.Count>0)
                    {
                        if(ret[ret.Count-1].Tick<EndPoint)
                        {
                            PNE = new PitchNode(EndPoint, ret[ret.Count - 1].PitchValue);
                            ret.Add(PNE);
                        }
                    }
                }
            }
            return ret;

        }
        public void earsePitchLine(PitchView.BlockDia NoteDia,bool ModeV2=false)
        {
            long mt = NoteDia.TickEnd;
            long nt = NoteDia.TickStart;
            for (int i = 0; i < NoteList.Count; i++)
            {
                PianoNote PN = NoteList[i];
                if (PN.Tick >= mt) break;
                if (PN.Tick + PN.Length < nt) continue;
                if (PN.PitchValue.NoteNumber >= NoteDia.BottomNoteNum && PN.PitchValue.NoteNumber <= NoteDia.TopNoteNum)
                {
                    long St = PN.Tick;
                    long Ed = PN.Tick + PN.Length;
                    if (nt > St && nt < Ed) St = nt;
                    if (mt < Ed && mt > St) Ed = mt;
                    earseArea(new PitchNode(St,PN.PitchValue.PitchValue),new PitchNode(Ed,PN.PitchValue.PitchValue));
                    if (ModeV2)
                    {
                        replacePitchLine(new List<PitchNode>() { 
                            new PitchNode(St, PN.PitchValue.NoteNumber), new PitchNode(Ed-1, PN.PitchValue.NoteNumber)
                        });
                    }
                    else
                    {
                        earseArea(new PitchNode(St, PN.PitchValue.PitchValue), new PitchNode(Ed-1, PN.PitchValue.PitchValue));
                    }
                }
            }
        }
        private void earseArea(PitchNode St, PitchNode Et)
        {
            int DelIdx = -1;
            for (int i = 0; i < PitchList.Count; i++)
            {
                if (PitchList[i].Tick < St.Tick) continue;
                if (PitchList[i].Tick > Et.Tick) break;
                DelIdx = i;
                break;
            }
            if (DelIdx > -1)
            {
                while (DelIdx < PitchList.Count)
                {
                    if (PitchList[DelIdx].Tick > Et.Tick) break;
                    PitchList.RemoveAt(DelIdx);
                }
            }
        }
        public void replacePitchLine(List<PitchNode> newPitchLine)
        {
            if (newPitchLine.Count < 2) return;
            long StartPoint = newPitchLine[0].Tick;
            long EndPoint = newPitchLine[newPitchLine.Count - 1].Tick;
            int NearSNoteIdx = -1;
            List<uint> BaseNote = new List<uint>();
            for (int i = 0; i < NoteList.Count; i++)
            {
                if (NoteList[i].Tick + NoteList[i].Length < StartPoint) continue;//在之前的，掠过
                NearSNoteIdx = i;
                break;
            }
            if (NearSNoteIdx == -1) return;

            if (NoteList[NearSNoteIdx].Tick < StartPoint)
            {
                BaseNote.Add(NoteList[NearSNoteIdx].PitchValue.NoteNumber);
            }
            for (int i = NearSNoteIdx; i < NoteList.Count; i++)
            {
                if (NoteList[i].Tick > EndPoint) break;
                int BNC = BaseNote.Count - 1;
                if (BNC == -1) BNC = 0;
                for (int j = BNC; j < newPitchLine.Count; j++)
                {
                    if (newPitchLine[j].Tick < NoteList[i].Tick + NoteList[i].Length)
                    {
                        BaseNote.Add(NoteList[i].PitchValue.NoteNumber);
                    }
                }
            }
            if (BaseNote.Count > 0 && BaseNote.Count < newPitchLine.Count)
            {
                for (int i = BaseNote.Count; i < newPitchLine.Count; i++)
                {
                    BaseNote.Add(BaseNote[BaseNote.Count - 1]);
                }
            }
            earseArea(newPitchLine[0], newPitchLine[newPitchLine.Count - 1]);
            for (int i = 0; i < newPitchLine.Count; i++)
            {
                PitchList.Add(new PitchNode(newPitchLine[i].Tick,newPitchLine[i].PitchValue.PitchValue-BaseNote[i]));
            }
            PitchList.Sort();
        }
    
    }
}
