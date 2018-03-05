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
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.PianoUtils
{
    public class PitchView
    {
        public delegate void OnPitchEventHandler(PitchDragingType eventType);
        public event OnPitchEventHandler PitchActionEnd;
        public event OnPitchEventHandler PitchActionBegin;

        const long AntiBordTick = 480;
        IntPtr PartsObjectPtr = IntPtr.Zero;
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
            }
        }
        PitchDragingType PitchDragingStatus = PitchDragingType.None;
        PitchObject PitchStP1 = null;
        PitchObject PitchTmpP0 = null;


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

        public PitchView(IntPtr PartsObjectPtr, PianoRollWindow PianoWindow)
        {
            this.PianoWindow = PianoWindow;
            this.PartsObjectPtr = PartsObjectPtr;
            hookPianoWindow();
        }

        public void hookPianoWindow()
        {
            PianoWindow.TrackPaint += PianoWindow_TrackPaint;
            PianoWindow.TrackMouseDown += PianoWindow_TrackMouseDown;
            PianoWindow.TrackMouseUp += PianoWindow_TrackMouseUp;
            PianoWindow.TrackMouseMove += PianoWindow_TrackMouseMove;
            PianoWindow.TrackMouseLeave += PianoWindow_TrackMouseLeave;
            PianoWindow.TrackMouseEnter += PianoWindow_TrackMouseEnter;
        }

        void PianoWindow_TrackMouseLeave(object sender, EventArgs e)
        {
            PianoWindow.ParentForm.Cursor = Cursors.Arrow;
        }

        void PianoWindow_TrackMouseEnter(object sender, EventArgs e)
        {
            if (!_HandleEvents) return;
            if (_PitchToolsStatus == PitchDragingType.None)
            {
                PianoWindow.ParentForm.Cursor = Cursors.Arrow;
            }
            else
            {
                PianoWindow.ParentForm.Cursor = Cursors.Cross;
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
        private List<PitchObject> PitchList
        {
            get
            {
                if (PartsObject == null) return new List<PitchObject>();
                return PartsObject.PitchBendsList;
            }
        }


        public void setPartsObjectPtr(IntPtr PartsObjectPtr)
        {
            this.PartsObjectPtr = PartsObjectPtr;
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
            PitchStP1 = new PitchObject(e.Tick, e.PitchValue.PitchValue);
            PitchDragingStatus = _PitchToolsStatus;
            if(PitchActionBegin!=null)PitchActionBegin(PitchDragingStatus);
        }

        private void PianoWindow_TrackMouseUp(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (PitchDragingStatus == PitchDragingType.None) return;
            PitchObject PitchEdP2 = new PitchObject(e.Tick, e.PitchValue.PitchValue);

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
            PitchDragingType EDStatus = PitchDragingStatus;
            PitchDragingStatus = PitchDragingType.None;
            PitchStP1 = null;
            PitchTmpP0 = null;
            if(PitchActionEnd!=null)PitchActionEnd(EDStatus);
        }

        private void PianoWindow_TrackMouseMove(object sender, VocalUtau.DirectUI.PianoMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (PitchDragingStatus == PitchDragingType.None)
            {
                return;
            }
            PitchTmpP0 = new PitchObject(e.Tick, e.PitchValue.PitchValue);
            if (_PitchToolsStatus == PitchDragingType.None)
            {
                PianoWindow.ParentForm.Cursor = Cursors.Arrow;
            }
            else
            {
                PianoWindow.ParentForm.Cursor = Cursors.Cross;
            }
        }

        
        public void AddPitchs(long StartTick, List<PitchObject> Pitchs)
        {
            if (Pitchs.Count == 0) return;
            List<PitchObject> PN = PitchList;
            PitchActionUtils.earsePitchLine(ref PN, StartTick, Pitchs[Pitchs.Count - 1].Tick + StartTick);
            for (int i = 0; i < Pitchs.Count; i++)
            {
                PitchList.Add(new PitchObject(Pitchs[i].Tick + StartTick,Pitchs[i].PitchValue.PitchValue));
            }
            PitchList.Sort();
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
        public List<PitchObject> getShownPitchLine(long MinTick=-1,long MaxTick=-1)
        {
            MinTick = MinTick<AntiBordTick?0:PianoWindow.MinShownTick - AntiBordTick;
            if(MaxTick<=MinTick)MaxTick = PianoWindow.MaxShownTick + AntiBordTick;
            List<NoteObject> NL = NoteList;
            List<PitchObject> PN = PitchList;
            return PitchActionUtils.getShownPitchLine(ref NL, ref PN, MinTick, MaxTick, _ShowNoteSpace);
        }
        public void earsePitchLine(PitchView.BlockDia NoteDia,bool ModeV2=false)
        {
            List<NoteObject> NL = NoteList;
            List<PitchObject> PN = PitchList;
            PitchActionUtils.earsePitchLine(ref NL, ref PN, NoteDia,ModeV2);
        }
        public void replacePitchLine(List<PitchObject> newPitchLine)
        {
            List<NoteObject> NL = NoteList;
            List<PitchObject> PN = PitchList;
            PitchActionUtils.replacePitchLine(ref NL, ref PN, newPitchLine); 
        }
    
    }
}
