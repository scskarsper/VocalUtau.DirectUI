using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.ActionUtils;
using VocalUtau.DirectUI.Utils.MathUtils;
using VocalUtau.DirectUI.Utils.PianoUtils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.ParamUtils
{
    public class PITParamView
    {
        public delegate void OnPitchEventHandler(PitchView.PitchDragingType eventType);
        public event OnPitchEventHandler PitchActionEnd;
        public event OnPitchEventHandler PitchActionBegin;
        const long AntiBordTick = 480;

        uint _Zoom = 1;

        public uint Zoom
        {
            get { return _Zoom; }
            set { _Zoom = value;
                if (_Zoom < 1) _Zoom = 1;
                ParamWindow.Refresh();
            }
        }
        bool _EarseModeV2 = true;

        public bool EarseModeV2
        {
            get { return _EarseModeV2; }
            set { _EarseModeV2 = value; }
        }

        PitchView.PitchDragingType _PitchToolsStatus = PitchView.PitchDragingType.EarseArea;

        public PitchView.PitchDragingType PitchToolsStatus
        {
            get { return _PitchToolsStatus; }
            set
            {
                _PitchToolsStatus = value;
            }
        }
        PitchView.PitchDragingType PitchDragingStatus = PitchView.PitchDragingType.None;
        PitchObject PitchStP1 = null;
        PitchObject PitchTmpP0 = null;

        bool _HandleEvents = false;

        public bool HandleEvents
        {
            get { return _HandleEvents; }
            set { _HandleEvents = value; }
        }

        IntPtr PartsObjectPtr = IntPtr.Zero;
        ParamCurveWindow ParamWindow;
        double CurValue = 0;
        public PITParamView(IntPtr PartsObjectPtr, ParamCurveWindow ParamWindow)
        {
            this.ParamWindow = ParamWindow;
            this.PartsObjectPtr = PartsObjectPtr;
            hookParamWindow();
        }
        public void setPartsObjectPtr(IntPtr PartsObjectPtr)
        {
            this.PartsObjectPtr = PartsObjectPtr;
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
        private List<PitchObject> PitchList
        {
            get
            {
                if (PartsObject == null) return new List<PitchObject>();
                return PartsObject.PitchBendsList;
            }
        }

        public void hookParamWindow()
        {
            ParamWindow.ParamAreaPaint += ParamWindow_TrackPaint;
            ParamWindow.ParamAreaMouseDown += ParamWindow_ParamAreaMouseDown;
            ParamWindow.ParamAreaMouseUp += ParamWindow_ParamAreaMouseUp;
            ParamWindow.ParamBtnsMouseUp += ParamWindow_ParamBtnsMouseUp;
            ParamWindow.ParamAreaMouseMove += ParamWindow_ParamAreaMouseMove;
            ParamWindow.ParamBtnsMouseMove += ParamWindow_ParamBtnsMouseMove;
            ParamWindow.ParamAreaMouseLeave += ParamWindow_ParamAreaMouseLeave;
            ParamWindow.ParamAreaMouseEnter += ParamWindow_ParamAreaMouseEnter;
            ParamWindow.ParamBtnsPaint += ParamWindow_ParamBtnsPaint;
        }

        void ParamWindow_ParamBtnsMouseMove(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (PitchDragingStatus == PitchView.PitchDragingType.None) return;
            ParamWindow_ParamAreaMouseMove(sender, e);
        }

        void ParamWindow_ParamBtnsMouseUp(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (PitchDragingStatus == PitchView.PitchDragingType.None) return;
            ParamWindow_ParamAreaMouseUp(sender, e);
        }

        void ParamWindow_ParamAreaMouseLeave(object sender, EventArgs e)
        {
            ParamWindow.ParentForm.Cursor = Cursors.Arrow;
        }

        void ParamWindow_ParamAreaMouseEnter(object sender, EventArgs e)
        {
            if (!_HandleEvents) return;
            if (_PitchToolsStatus == PitchView.PitchDragingType.None)
            {
                ParamWindow.ParentForm.Cursor = Cursors.Arrow;
            }
            else
            {
                ParamWindow.ParentForm.Cursor = Cursors.Cross;
            }
        }

        void ParamWindow_ParamAreaMouseMove(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            CurValue = e.MidPercent * 0.5 * Zoom;
            if (PitchDragingStatus == PitchView.PitchDragingType.None)
            {
                return;
            }
            if (e.Tick == PitchStP1.Tick) return;
            PitchTmpP0 = new PitchObject(e.Tick, e.MidPercent * 0.5 * Zoom);
            if (_PitchToolsStatus == PitchView.PitchDragingType.None)
            {
                ParamWindow.ParentForm.Cursor = Cursors.Arrow;
            }
            else
            {
                ParamWindow.ParentForm.Cursor = Cursors.Cross;
            }
        }

        public void replacePitchLine(List<PitchObject> newPitchLine)
        {
            List<PitchObject> PN = PitchList;
            PitchActionUtils.replacePitchLine(ref PN, newPitchLine);
        }
        public void earsePitchLine(PitchObject P1,PitchObject P2,bool isModeV2)
        {
            List<PitchObject> PN = PitchList;
            PitchActionUtils.earsePitchLine(ref PN, Math.Min(P1.Tick, P2.Tick), Math.Max(P1.Tick, P2.Tick), isModeV2);
        }

        void ParamWindow_ParamAreaMouseUp(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (PitchDragingStatus == PitchView.PitchDragingType.None) return;
            PitchObject PitchEdP2 = new PitchObject(e.Tick, e.MidPercent * 0.5 * Zoom);

            switch (PitchDragingStatus)
            {
                case PitchView.PitchDragingType.DrawLine: replacePitchLine(PitchMathUtils.CalcLineSilk(PitchStP1, PitchEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphJ: replacePitchLine(PitchMathUtils.CalcGraphJ(PitchStP1, PitchEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphR: replacePitchLine(PitchMathUtils.CalcGraphR(PitchStP1, PitchEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphS: replacePitchLine(PitchMathUtils.CalcGraphS(PitchStP1, PitchEdP2)); break;
                case PitchView.PitchDragingType.EarseArea: earsePitchLine(PitchStP1, PitchEdP2,_EarseModeV2); break;
                    
            }
            PitchView.PitchDragingType EDStatus = PitchDragingStatus;
            PitchDragingStatus = PitchView.PitchDragingType.None;
            PitchStP1 = null;
            PitchTmpP0 = null;
            if (PitchActionEnd != null) PitchActionEnd(EDStatus);
            if (_PitchToolsStatus == PitchView.PitchDragingType.None)
            {
                ParamWindow.ParentForm.Cursor = Cursors.Arrow;
            }
            else
            {
                ParamWindow.ParentForm.Cursor = Cursors.Cross;
            }
        }

        void ParamWindow_ParamAreaMouseDown(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents)
            {
                PitchDragingStatus = PitchView.PitchDragingType.None;
                PitchStP1 = null;
                PitchTmpP0 = null;
                return;
            }
            if (_PitchToolsStatus == PitchView.PitchDragingType.None) return;
            if (PitchDragingStatus != PitchView.PitchDragingType.None) return;
            if (e.MouseEventArgs.Button != MouseButtons.Left) return;
            PitchStP1 = new PitchObject(e.Tick, e.MidPercent * 0.5 * Zoom);
            PitchDragingStatus = _PitchToolsStatus;
            if (PitchActionBegin != null) PitchActionBegin(PitchDragingStatus);
        }


        public List<PitchObject> getShownPitchLine(long MinTick = -1, long MaxTick = -1)
        {
            if (MinTick < 0) MinTick = ParamWindow.MinShownTick;
            MinTick = MinTick < AntiBordTick ? 0 : ParamWindow.MinShownTick- AntiBordTick;
            if (MaxTick <= MinTick) MaxTick = ParamWindow.MaxShownTick + AntiBordTick;
            List<PitchObject> PO = PitchList;
            return PitchActionUtils.getShownPitchLine(ref PO, MinTick, MaxTick);
        }
        private void ParamWindow_TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.ParamAreaDrawUtils utils)
        {
            if (!_HandleEvents) return;
            if (PitchDragingStatus == PitchView.PitchDragingType.EarseArea)
            {
                utils.FillSelect(PitchStP1.Tick, PitchTmpP0.Tick,Color.DarkSalmon);
            }

            utils.FillPitchLine(getShownPitchLine(), (0.5 * Zoom), Color.Green, AntiBordTick);

            switch (PitchDragingStatus)
            {
                case PitchView.PitchDragingType.DrawLine: utils.DrawPitchLine(PitchMathUtils.CalcLineSilk(PitchStP1, PitchTmpP0), (0.5 * Zoom), Color.LightPink, 2); break;
                case PitchView.PitchDragingType.DrawGraphJ: utils.DrawPitchLine(PitchMathUtils.CalcGraphJ(PitchStP1, PitchTmpP0), (0.5 * Zoom), Color.LightPink, 2); break;
                case PitchView.PitchDragingType.DrawGraphR: utils.DrawPitchLine(PitchMathUtils.CalcGraphR(PitchStP1, PitchTmpP0), (0.5 * Zoom), Color.LightPink, 2); break;
                case PitchView.PitchDragingType.DrawGraphS: utils.DrawPitchLine(PitchMathUtils.CalcGraphS(PitchStP1, PitchTmpP0), (0.5 * Zoom), Color.LightPink, 2); break;
            }

            uint SplitCount = 2*Zoom;
            if (SplitCount > 2)
            {
                for (int i = 1; i < SplitCount; i++)
                {
                    double Pcent = (double)i / (double)SplitCount;
                    utils.DrawYLine(Pcent, Color.FromArgb(91, 91, 91));
                }
            }
            utils.DrawString(new Point(5, 0), Color.FromArgb(80, 255, 255, 255), (0.5 * Zoom).ToString() + " Semitone ", 10, FontStyle.Bold);
            utils.DrawString(new Point(5, utils.ClipRectangle.Height / 2 - 8), Color.FromArgb(80, 255, 255, 255), "0 Semitone", 10, FontStyle.Bold);
            utils.DrawString(new Point(5, utils.ClipRectangle.Height - 15), Color.FromArgb(80, 255, 255, 255), (-0.5 * Zoom).ToString() + " Semitone", 10, FontStyle.Bold);

            utils.DrawString(new Point(utils.ClipRectangle.Width - 150, 0), Color.FromArgb(80, 255, 255, 255), "PIT", 50, FontStyle.Bold);


            switch (_PitchToolsStatus)
            {
                case PitchView.PitchDragingType.DrawLine: utils.DrawString(new Point(utils.ClipRectangle.Width - 160, 65), Color.FromArgb(80, 255, 255, 255), "Draw Line", 25, FontStyle.Bold); break;
                case PitchView.PitchDragingType.DrawGraphJ: utils.DrawString(new Point(utils.ClipRectangle.Width - 130, 65), Color.FromArgb(80, 255, 255, 255), "Draw J", 25, FontStyle.Bold); break;
                case PitchView.PitchDragingType.DrawGraphR: utils.DrawString(new Point(utils.ClipRectangle.Width - 130, 65), Color.FromArgb(80, 255, 255, 255), "Draw R", 25, FontStyle.Bold); break;
                case PitchView.PitchDragingType.DrawGraphS: utils.DrawString(new Point(utils.ClipRectangle.Width - 130, 65), Color.FromArgb(80, 255, 255, 255), "Draw S", 25, FontStyle.Bold); break;
                case PitchView.PitchDragingType.EarseArea: utils.DrawString(new Point(utils.ClipRectangle.Width - 130, 65), Color.FromArgb(80, 255, 255, 255), "Earse", 25, FontStyle.Bold); break;
            }
        }
        void ParamWindow_ParamBtnsPaint(object sender, DrawUtils.ParamBtnsDrawUtils utils)
        {
            if (!_HandleEvents) return;
            utils.DrawString(new Point(0, utils.ClipRectangle.Height - 15), Color.FromArgb(100, 255, 255, 255), Math.Round(CurValue, 2).ToString().PadLeft(22, ' '), 10, FontStyle.Bold);
        }
    }
}
