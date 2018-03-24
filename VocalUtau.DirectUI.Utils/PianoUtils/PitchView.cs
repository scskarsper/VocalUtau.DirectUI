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
using VocalUtau.Formats.Model.BaseObject;
using VocalUtau.Formats.Model.VocalObject;
using VocalUtau.Formats.Model.VocalObject.ParamTranslater;

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
      /*  private List<PitchObject> PitchBendsList
        {
            get
            {
                if (PartsObject == null) return new List<PitchObject>();
                return PartsObject.PitchBendsList;
            }
        }*/
        /*
        private List<PitchObject> PitchList
        {
            get
            {
                if (PartsObject == null) return new List<PitchObject>();
                return PartsObject.PitchList;
            }
        }*/
        public PitchCompiler PitchCompiler
        {
            get
            {
                return PartsObject.PitchCompiler;
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
            List<PitchObject>[] PitchArray=getPitchObjLists();
            foreach (List<PitchObject> PitchObjList in PitchArray)
            {
                utils.DrawPitchLine(PitchObjList, Color.Red);
            }
            /*
            List<PitchObject> PitchObjList=getShownPitchLine();
            utils.DrawPitchLine(PitchObjList, Color.Red);
            */
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
            if (_HandleEvents)
            {
                switch (_PitchToolsStatus)
                {
                    case PitchView.PitchDragingType.DrawLine: utils.DrawString(new Point(utils.ClipRectangle.Width - 210, 35), Color.FromArgb(60, 0, 0, 0), "Pitch Draw Line", 25, FontStyle.Bold); break;
                    case PitchView.PitchDragingType.DrawGraphJ: utils.DrawString(new Point(utils.ClipRectangle.Width - 170, 35), Color.FromArgb(60, 0, 0, 0), "Pitch Draw J", 25, FontStyle.Bold); break;
                    case PitchView.PitchDragingType.DrawGraphR: utils.DrawString(new Point(utils.ClipRectangle.Width - 170, 35), Color.FromArgb(60, 0, 0, 0), "Pitch Draw R", 25, FontStyle.Bold); break;
                    case PitchView.PitchDragingType.DrawGraphS: utils.DrawString(new Point(utils.ClipRectangle.Width - 170, 35), Color.FromArgb(60, 0, 0, 0), "Pitch Draw S", 25, FontStyle.Bold); break;
                    case PitchView.PitchDragingType.EarseArea: utils.DrawString(new Point(utils.ClipRectangle.Width - 160, 35), Color.FromArgb(60, 0, 0, 0), "Pitch Earse", 25, FontStyle.Bold); break;
                }
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
            if (e.MouseEventArgs.Button != MouseButtons.Left) return;
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
                case PitchDragingType.DrawLine: PartsObject.PitchCompiler.ReplaceRealPitchLine(PitchMathUtils.CalcLineSilk(PitchStP1, PitchEdP2)); break;
                case PitchDragingType.DrawGraphJ: PartsObject.PitchCompiler.ReplaceRealPitchLine(PitchMathUtils.CalcGraphJ(PitchStP1, PitchEdP2)); break;
                case PitchDragingType.DrawGraphR: PartsObject.PitchCompiler.ReplaceRealPitchLine(PitchMathUtils.CalcGraphR(PitchStP1, PitchEdP2)); break;
                case PitchDragingType.DrawGraphS: PartsObject.PitchCompiler.ReplaceRealPitchLine(PitchMathUtils.CalcGraphS(PitchStP1, PitchEdP2)); break;
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
            if (e.Tick == PitchStP1.Tick) return;
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
      /*      List<PitchObject> PN = PitchBendsList;
            PitchActionUtils.earsePitchLine(ref PN, StartTick, Pitchs[Pitchs.Count - 1].Tick + StartTick);
            for (int i = 0; i < Pitchs.Count; i++)
            {
                PitchBendsList.Add(new PitchObject(Pitchs[i].Tick + StartTick,Pitchs[i].PitchValue.PitchValue));
            }
            PitchBendsList.Sort();*/
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


        List<PitchObject>[] getPitchObjLists(long MinTick = -1, long MaxTick = -1)
        {
            long nt = MinTick > 0 ? MinTick : PianoWindow.MinShownTick;
            long mt = MaxTick>nt ? MaxTick : PianoWindow.MaxShownTick;
                  

            List<KeyValuePair<long, long>> Partsy = new List<KeyValuePair<long, long>>();
            long st = -1;
            long et = -1;
            for (int i = 0; i < NoteList.Count; i++)
            {
                NoteObject PN = NoteList[i];
                if (PN.Tick >= mt) break;
                if (PN.Tick + PN.Length < nt) continue;
                if (et != -1 && PN.Tick - et > AntiBordTick)//AntiBordTick在这里为音符间最大间隔
                {
                    if (st != -1)
                    {
                        Partsy.Add(new KeyValuePair<long, long>(st, et));
                        st = -1;
                        et = -1;
                    }
                }
                if (st == -1) st = PN.Tick;
                et = PN.Tick + PN.Length;
            }
            if (st != -1 && et != -1)
            {
                Partsy.Add(new KeyValuePair<long, long>(st, et));
                st = -1;
                et = -1;
            }
            List<List<PitchObject>> Ret = new List<List<PitchObject>>();
            foreach (KeyValuePair<long, long> SE in Partsy)
            {
                if (SE.Key < SE.Value)
                {
                    List<PitchObject> PList = new List<PitchObject>();
                    for (long i = TickSortList<PitchObject>.TickFormat(SE.Key); i <= TickSortList<PitchObject>.TickFormat(SE.Value); i=i+TickSortList<PitchObject>.TickStep)
                    {
                        PartsObject po = PartsObject;
                        PList.Add(new PitchObject(i, po.PitchCompiler.getRealPitch(i)));
                    }
                    Ret.Add(PList);
                }
            }
            return Ret.ToArray();
        }
        
        private void earsePitchLine(PitchView.BlockDia NoteDia,bool ModeV2=false)
        {
            long StartTick = -1;
            long EndTick = -1;
            for (long i = NoteDia.TickStart; i <= NoteDia.TickEnd; i++)
            {
                double curPv = PartsObject.PitchCompiler.getRealPitch(i);
                int PN = (int)Math.Floor(curPv);
                if (PN < NoteDia.BottomNoteNum || PN > NoteDia.TopNoteNum)
                {
                    //区域外
                    if (StartTick > -1 && EndTick > StartTick)
                    {
                        PartsObject.PitchCompiler.ClearPitchLine(StartTick, EndTick);
                        StartTick = -1;
                        EndTick = -1;
                    }
                }
                else
                {
                    //区域内
                    if (StartTick < 0) StartTick = i;
                    EndTick = i;
                }
            } 
            if (StartTick > -1 && EndTick > StartTick)
            {
                PartsObject.PitchCompiler.ClearPitchLine(StartTick, EndTick);
                StartTick = -1;
                EndTick = -1;
            }
        }

        public void ReleaseCache()
        {
            PartsObject.PitchCompiler.ClearCache();
        }
    }
}
