using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.MathUtils;
using VocalUtau.DirectUI.Utils.PianoUtils;
using VocalUtau.Formats.Model.BaseObject;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.ParamUtils
{
    public class DYNParamView
    {
        public delegate void OnPitchEventHandler(PitchView.PitchDragingType eventType);
        public event OnPitchEventHandler DynActionEnd;
        public event OnPitchEventHandler DynActionBegin;
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

        PitchView.PitchDragingType _DynToolsStatus = PitchView.PitchDragingType.EarseArea;

        public PitchView.PitchDragingType DynToolsStatus
        {
            get { return _DynToolsStatus; }
            set
            {
                _DynToolsStatus = value;
            }
        }
        PitchView.PitchDragingType DynDragingStatus = PitchView.PitchDragingType.None;
        TickControlObject DynStP1 = null;
        TickControlObject DynTmpP0 = null;
        double CurValue = 0;

        bool _HandleEvents = false;

        public bool HandleEvents
        {
            get { return _HandleEvents; }
            set { _HandleEvents = value; }
        }

        IntPtr PartsObjectPtr = IntPtr.Zero;
        ParamCurveWindow ParamWindow;
        public DYNParamView(IntPtr PartsObjectPtr, ParamCurveWindow ParamWindow)
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
        private int DynBase
        {
            get
            {
                if (PartsObject == null) return 100;
                return PartsObject.DynBaseValue;
            }
        }

        public void hookParamWindow()
        {
            ParamWindow.ParamAreaPaint += ParamWindow_TrackPaint;
            ParamWindow.ParamBtnsPaint += ParamWindow_ParamBtnsPaint;
            ParamWindow.ParamAreaMouseDown += ParamWindow_ParamAreaMouseDown;
            ParamWindow.ParamAreaMouseUp += ParamWindow_ParamAreaMouseUp;
            ParamWindow.ParamBtnsMouseUp += ParamWindow_ParamAreaMouseUp;
            ParamWindow.ParamAreaMouseMove += ParamWindow_ParamAreaMouseMove;
            ParamWindow.ParamBtnsMouseMove += ParamWindow_ParamBtnsMouseMove;
            ParamWindow.ParamAreaMouseLeave += ParamWindow_ParamAreaMouseLeave;
            ParamWindow.ParamAreaMouseEnter += ParamWindow_ParamAreaMouseEnter;
        }


        void ParamWindow_ParamBtnsMouseMove(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (DynDragingStatus == PitchView.PitchDragingType.None) return;
            ParamWindow_ParamAreaMouseMove(sender, e);
        }

        void ParamWindow_ParamBtnsMouseUp(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (DynDragingStatus == PitchView.PitchDragingType.None) return;
            ParamWindow_ParamAreaMouseUp(sender, e);
        }

        void ParamWindow_ParamAreaMouseLeave(object sender, EventArgs e)
        {
            ParamWindow.ParentForm.Cursor = Cursors.Arrow;
        }

        void ParamWindow_ParamAreaMouseEnter(object sender, EventArgs e)
        {
            if (!_HandleEvents) return;
            if (_DynToolsStatus == PitchView.PitchDragingType.None)
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
            CurValue = (e.TallPercent * 100 * Zoom);
            if (DynDragingStatus == PitchView.PitchDragingType.None)
            {
                return;
            }
            if (e.Tick == DynStP1.Tick) return;
            DynTmpP0 = new TickControlObject(e.Tick, e.TallPercent * 100 * Zoom - DynBase);
            if (_DynToolsStatus == PitchView.PitchDragingType.None)
            {
                ParamWindow.ParentForm.Cursor = Cursors.Arrow;
            }
            else
            {
                ParamWindow.ParentForm.Cursor = Cursors.Cross;
            }
        }

        public void replaceControlLine(List<TickControlObject> newPitchLine)
        {
            for (int i = 0; i < newPitchLine.Count; i++)
            {
                if (newPitchLine[i].Value < -100)
                {
                    newPitchLine[i].Value = -100;
                }
            }
            PartsObject.DynCompiler.ReplaceDynLine(newPitchLine);
        }
        public void earseControlLine(TickControlObject P1, TickControlObject P2)
        {
            PartsObject.DynCompiler.ClearDynLine(Math.Min(P1.Tick, P2.Tick), Math.Max(P1.Tick, P2.Tick));
        }

        void ParamWindow_ParamAreaMouseUp(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (DynDragingStatus == PitchView.PitchDragingType.None) return;
            TickControlObject DynEdP2 = new TickControlObject(e.Tick, e.TallPercent * 100 * Zoom - DynBase);

            switch (DynDragingStatus)
            {
                case PitchView.PitchDragingType.DrawLine: replaceControlLine(ControlMathUtils.CalcLineSilk(DynStP1, DynEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphJ: replaceControlLine(ControlMathUtils.CalcGraphJ(DynStP1, DynEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphR: replaceControlLine(ControlMathUtils.CalcGraphR(DynStP1, DynEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphS: replaceControlLine(ControlMathUtils.CalcGraphS(DynStP1, DynEdP2)); break;
                case PitchView.PitchDragingType.EarseArea: earseControlLine(DynStP1, DynEdP2); break;
                    
            }
            PitchView.PitchDragingType EDStatus = DynDragingStatus;
            DynDragingStatus = PitchView.PitchDragingType.None;
            DynStP1 = null;
            DynTmpP0 = null;
            if (DynActionEnd != null) DynActionEnd(EDStatus);
            if (_DynToolsStatus == PitchView.PitchDragingType.None)
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
                DynDragingStatus = PitchView.PitchDragingType.None;
                DynStP1 = null;
                DynTmpP0 = null;
                return;
            }
            if (_DynToolsStatus == PitchView.PitchDragingType.None) return;
            if (DynDragingStatus != PitchView.PitchDragingType.None) return;
            if (e.MouseEventArgs.Button != MouseButtons.Left) return;
            DynStP1 = new TickControlObject(e.Tick, e.TallPercent*100*Zoom - 100);
            DynDragingStatus = _DynToolsStatus;
            if (DynActionBegin != null) DynActionBegin(DynDragingStatus);
        }


        public List<TickControlObject> getShownPitchLine(long MinTick = -1, long MaxTick = -1)
        {
            if (MinTick < 0) MinTick = ParamWindow.MinShownTick;
            if (MaxTick <= MinTick) MaxTick = ParamWindow.MaxShownTick;
            List<TickControlObject> ret = new List<TickControlObject>();
            for (long i = TickSortList<TickControlObject>.TickFormat(MinTick); i < TickSortList<TickControlObject>.TickFormat(MaxTick); i = i + TickSortList<TickControlObject>.TickStep)
            {
                ret.Add(new TickControlObject(i, PartsObject.DynCompiler.getDynValue(i)));
            }
            return ret;
        }
        private void ParamWindow_TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.ParamAreaDrawUtils utils)
        {
            if (!_HandleEvents) return;
            if (DynDragingStatus == PitchView.PitchDragingType.EarseArea)
            {
                utils.FillSelect(DynStP1.Tick, DynTmpP0.Tick,Color.DarkSalmon);
            }

            utils.FillDynLine(getShownPitchLine(),DynBase,(100*Zoom), Color.Green, AntiBordTick);

            switch (DynDragingStatus)
            {
                case PitchView.PitchDragingType.DrawLine:utils.DrawDynLine(ControlMathUtils.CalcLineSilk(DynStP1, DynTmpP0), DynBase, (100 * Zoom), Color.LightPink, 2);  break;
                case PitchView.PitchDragingType.DrawGraphJ: utils.DrawDynLine(ControlMathUtils.CalcGraphJ(DynStP1, DynTmpP0), DynBase, (100 * Zoom), Color.LightPink, 2); break;
                case PitchView.PitchDragingType.DrawGraphR: utils.DrawDynLine(ControlMathUtils.CalcGraphR(DynStP1, DynTmpP0), DynBase, (100 * Zoom), Color.LightPink, 2); break;
                case PitchView.PitchDragingType.DrawGraphS: utils.DrawDynLine(ControlMathUtils.CalcGraphS(DynStP1, DynTmpP0), DynBase, (100 * Zoom), Color.LightPink, 2); break;
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

            utils.DrawString(new Point(5, 0), Color.FromArgb(80, 255, 255, 255), (100 * Zoom).ToString()+" %", 10, FontStyle.Bold);
            utils.DrawString(new Point(5, utils.ClipRectangle.Height / 2 - 8), Color.FromArgb(80, 255, 255, 255), (50 * Zoom).ToString() + " %", 10, FontStyle.Bold);
            utils.DrawString(new Point(5, utils.ClipRectangle.Height - 15), Color.FromArgb(80, 255, 255, 255), "0 %" , 10, FontStyle.Bold);

            utils.DrawString(new Point(utils.ClipRectangle.Width - 150, 0), Color.FromArgb(80, 255, 255, 255), "DYN", 50, FontStyle.Bold);

            switch (_DynToolsStatus)
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
            utils.DrawString(new Point(0, utils.ClipRectangle.Height - 15), Color.FromArgb(100, 255, 255, 255), Math.Round(CurValue<0?0:CurValue, 2).ToString().PadLeft(22, ' '), 10, FontStyle.Bold);
        }
    }
}
