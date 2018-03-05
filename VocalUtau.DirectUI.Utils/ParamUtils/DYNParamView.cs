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
        ControlObject DynStP1 = null;
        ControlObject DynTmpP0 = null;

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
        private List<ControlObject> DynList
        {
            get
            {
                if (PartsObject == null) return new List<ControlObject>();
                return PartsObject.DynList;
            }
        }

        public void hookParamWindow()
        {
            ParamWindow.ParamAreaPaint += ParamWindow_TrackPaint;
            ParamWindow.ParamAreaMouseDown += ParamWindow_ParamAreaMouseDown;
            ParamWindow.ParamAreaMouseUp += ParamWindow_ParamAreaMouseUp;
            ParamWindow.ParamAreaMouseMove += ParamWindow_ParamAreaMouseMove;
            /* PianoWindow.TrackPaint += PianoWindow_TrackPaint;
             PianoWindow.TrackMouseDown += PianoWindow_TrackMouseDown;
             PianoWindow.TrackMouseUp += PianoWindow_TrackMouseUp;
             PianoWindow.TrackMouseMove += PianoWindow_TrackMouseMove;*/
            ParamWindow.ParamAreaMouseLeave += ParamWindow_ParamAreaMouseLeave;
            ParamWindow.ParamAreaMouseEnter += ParamWindow_ParamAreaMouseEnter;
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
            if (DynDragingStatus == PitchView.PitchDragingType.None)
            {
                return;
            }
            DynTmpP0 = new ControlObject(e.Tick, e.TallPercent * 100 * Zoom - DynBase);
            if (_DynToolsStatus == PitchView.PitchDragingType.None)
            {
                ParamWindow.ParentForm.Cursor = Cursors.Arrow;
            }
            else
            {
                ParamWindow.ParentForm.Cursor = Cursors.Cross;
            }
        }

        public void replaceControlLine(List<ControlObject> newPitchLine)
        {
            List<ControlObject> PN = DynList;
            ControlActionUtils.replaceControlLine(ref PN, newPitchLine);
        }
        public void earseControlLine(ControlObject P1, ControlObject P2, bool isModeV2)
        {
            List<ControlObject> PN = DynList;
            ControlActionUtils.earseControlLine(ref PN, Math.Min(P1.Tick, P2.Tick), Math.Max(P1.Tick, P2.Tick), isModeV2);
        }

        void ParamWindow_ParamAreaMouseUp(object sender, ParamMouseEventArgs e)
        {
            if (!_HandleEvents) return;
            if (DynDragingStatus == PitchView.PitchDragingType.None) return;
            ControlObject DynEdP2 = new ControlObject(e.Tick, e.TallPercent * 100 * Zoom - DynBase);

            switch (DynDragingStatus)
            {
                case PitchView.PitchDragingType.DrawLine: replaceControlLine(ControlMathUtils.CalcLineSilk(DynStP1, DynEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphJ: replaceControlLine(ControlMathUtils.CalcGraphJ(DynStP1, DynEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphR: replaceControlLine(ControlMathUtils.CalcGraphR(DynStP1, DynEdP2)); break;
                case PitchView.PitchDragingType.DrawGraphS: replaceControlLine(ControlMathUtils.CalcGraphS(DynStP1, DynEdP2)); break;
                case PitchView.PitchDragingType.EarseArea: earseControlLine(DynStP1, DynEdP2,_EarseModeV2); break;
                    
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
            DynStP1 = new ControlObject(e.Tick, e.TallPercent*100*Zoom - 100);
            DynDragingStatus = _DynToolsStatus;
            if (DynActionBegin != null) DynActionBegin(DynDragingStatus);
        }


        public List<ControlObject> getShownDynLine(long MinTick = -1, long MaxTick = -1)
        {
            MinTick = MinTick < AntiBordTick ? 0 : ParamWindow.MinShownTick- AntiBordTick;
            if (MaxTick <= MinTick) MaxTick = ParamWindow.MaxShownTick + AntiBordTick;
            List<ControlObject> DL = DynList;
            List<ControlObject> ret = ControlActionUtils.getShownControlLine(ref DL, MinTick, MaxTick);
            return ret;
        }
        private void ParamWindow_TrackPaint(object sender, VocalUtau.DirectUI.DrawUtils.ParamAreaDrawUtils utils)
        {
            if (!_HandleEvents) return;
            if (DynDragingStatus == PitchView.PitchDragingType.EarseArea)
            {
                utils.FillSelect(DynStP1.Tick, DynTmpP0.Tick,Color.DarkSalmon);
            }

            utils.FillDynLine(getShownDynLine(ParamWindow.MinShownTick,ParamWindow.MaxShownTick),DynBase,(100*Zoom), Color.Green, AntiBordTick);

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
        }
    }
}
