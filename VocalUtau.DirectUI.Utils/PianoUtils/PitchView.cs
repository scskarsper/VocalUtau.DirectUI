using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI;
using VocalUtau.DirectUI.Models;
using VocalUtau.DirectUI.Utils.ActionUtils;
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

        bool _ShowNoteSpace = true;

        public bool ShowNoteSpace
        {
            get { return _ShowNoteSpace; }
            set { _ShowNoteSpace = value; }
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

        long _TickStepTick = 1;
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
        public List<PitchNode> getShownPitchLine(long MinTick=-1,long MaxTick=-1)
        {
            MinTick = MinTick<AntiBordTick?0:PianoWindow.MinShownTick - AntiBordTick;
            if(MaxTick<=MinTick)MaxTick = PianoWindow.MaxShownTick + AntiBordTick;
            List<PianoNote> NL = NoteList;
            List<PitchNode> PN = PitchList;
            return PitchActionUtils.getShownPitchLine(ref NL, ref PN, MinTick, MaxTick, _ShowNoteSpace);
        }
        public void earsePitchLine(PitchView.BlockDia NoteDia,bool ModeV2=false)
        {
            List<PianoNote> NL = NoteList;
            List<PitchNode> PN = PitchList;
            PitchActionUtils.earsePitchLine(ref NL, ref PN, NoteDia,ModeV2);
        }
        public void replacePitchLine(List<PitchNode> newPitchLine)
        {
            List<PianoNote> NL = NoteList;
            List<PitchNode> PN = PitchList;
            PitchActionUtils.replacePitchLine(ref NL, ref PN, newPitchLine); 
        }
    
    }
}
