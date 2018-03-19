using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.AbilityUtils;
using VocalUtau.DirectUI.Utils.ParamUtils;
using VocalUtau.DirectUI.Utils.PianoUtils;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;
using WeifenLuo.WinFormsUI.Docking;

namespace VocalUtau.DirectUI.Forms
{
    public partial class SingerWindow : DockContent
    {
        public AttributesWindow AttributeWindow = null;

        public enum ParamViewType
        {
            Dynamic,
            Pitch
        }
        public enum RollActionType
        {
            Note,
            Pitch
        }
        public class ViewController
        {
            bool _BindRollAndParam = false;

            public bool BindRollAndParam
            {
                get { return _BindRollAndParam; }
                set
                {
                    _BindRollAndParam = value;
                    if(value){
                        Param_PitchView.PitchToolsStatus = Track_PitchView.PitchToolsStatus;
                        Param_DynamicView.DynToolsStatus = Track_PitchView.PitchToolsStatus;
                    }
                }
            }
            bool Alloced = false;
            PianoRollWindow PianoWindow;
            ParamCurveWindow ParamWindow;
            public ViewController(ref PianoRollWindow PianoWin,ref ParamCurveWindow ParamWin)
            {
                this.PianoWindow = PianoWin;
                this.ParamWindow = ParamWin;
            }
            public void AllocView(IntPtr ObjectPtr)
            {
                if (Alloced)
                {
                    Track_PitchView.setPartsObjectPtr(ObjectPtr);
                    Track_NoteView.setPartsObjectPtr(ObjectPtr);
                    Global_ActionView.setPartsObjectPtr(ObjectPtr);
                    Param_PitchView.setPartsObjectPtr(ObjectPtr);
                    Param_DynamicView.setPartsObjectPtr(ObjectPtr);
                }
                else
                {
                    Track_NoteView = new NoteView(ObjectPtr, this.PianoWindow);
                    Track_PitchView = new PitchView(ObjectPtr, this.PianoWindow);
                    Param_PitchView = new PITParamView(ObjectPtr, this.ParamWindow);
                    Param_DynamicView = new DYNParamView(ObjectPtr, this.ParamWindow);
                    Global_ActionView = new ActionView(ObjectPtr, this.PianoWindow, this.ParamWindow);

                    CopyPasteController = new CopyPaste(ref Track_NoteView, ref Track_PitchView);

                    Track_NoteView.NoteActionBegin += Track_NoteView_NoteActionBegin;
                    Track_NoteView.NoteActionEnd += Track_NoteView_NoteActionEnd;
                    Track_NoteView.NoteSelecting += Track_NoteView_NoteSelecting;
                    Track_PitchView.PitchActionBegin += Track_PitchView_PitchActionBegin;
                    Track_PitchView.PitchActionEnd += Track_PitchView_PitchActionEnd;
                    Param_DynamicView.DynActionBegin += Param_DynamicView_DynActionBegin;
                    Param_DynamicView.DynActionEnd += Param_DynamicView_DynActionEnd;
                    Param_PitchView.PitchActionBegin += Param_PitchView_PitchActionBegin;
                    Param_PitchView.PitchActionEnd += Param_PitchView_PitchActionEnd;

                    Global_ActionView.TickPosChange += Global_ActionView_TickPosChange;

                    Alloced = true;

                    SetNoteViewTool(NoteView.NoteToolsType.Select);
                    SetParamGraphicTool(PitchView.PitchDragingType.DrawGraphS);
                    SwitchParamView(ParamViewType.Dynamic);
                }
                try
                {
                    ParamWindow.RedrawPiano();
                    PianoWindow.RedrawPiano();
                    LastSelectIndex = -1;
                }
                catch { ;}
            }

            public void RealaramNoteSelecting()
            {
                Track_NoteView_NoteSelecting(LastSelectIndex);
            }

            int LastSelectIndex = -1;
            void Track_NoteView_NoteSelecting(int SelectedNoteIndex)
            {
                if (NoteSelecting != null)
                {
                    NoteSelecting(SelectedNoteIndex);
                    LastSelectIndex = SelectedNoteIndex;
                }
            }
                        
            void Global_ActionView_TickPosChange(long Tick, double Time)
            {
                if (TickPosChange != null) TickPosChange(Tick, Time);
            }

            #region
            void Param_PitchView_PitchActionEnd(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (PitchActionEnd != null) PitchActionEnd(eventType);
                PianoWindow.RedrawPiano();
            }
            void Param_PitchView_PitchActionBegin(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (PitchActionBegin != null) PitchActionBegin(eventType);
            }
            void Param_DynamicView_DynActionEnd(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (DynActionEnd != null) DynActionEnd(eventType);
            }
            void Param_DynamicView_DynActionBegin(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (DynActionBegin != null) DynActionBegin(eventType);
            }
            void Track_PitchView_PitchActionEnd(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (PitchActionEnd != null) PitchActionEnd(eventType);
                if (GetParamType() == ParamViewType.Pitch)
                {
                    ParamWindow.RedrawPiano();
                }
            }
            void Track_PitchView_PitchActionBegin(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (PitchActionBegin != null) PitchActionBegin(eventType);
            }
            void Track_NoteView_NoteActionEnd(NoteView.NoteDragingType eventType, bool Callback = false)
            {
                if (eventType == NoteView.NoteDragingType.None) return;
                if (eventType == NoteView.NoteDragingType.NoteMove)
                {
                    if (!Callback)
                    {
                        if (NoteActionEnd != null) NoteActionEnd(eventType);
                    }
                }
                else
                {
                    if (NoteActionEnd != null) NoteActionEnd(eventType);
                }
            }
            void Track_NoteView_NoteActionBegin(NoteView.NoteDragingType eventType, bool Callback = false)
            {
                if (eventType == NoteView.NoteDragingType.None) return;
                if (NoteActionBegin != null) NoteActionBegin(eventType);
            }
            public delegate void OnNoteEventHandler(VocalUtau.DirectUI.Utils.PianoUtils.NoteView.NoteDragingType eventType);
            public event OnNoteEventHandler NoteActionEnd;
            public event OnNoteEventHandler NoteActionBegin;
            public event VocalUtau.DirectUI.Utils.ParamUtils.DYNParamView.OnPitchEventHandler DynActionEnd;
            public event VocalUtau.DirectUI.Utils.ParamUtils.DYNParamView.OnPitchEventHandler DynActionBegin;
            public event VocalUtau.DirectUI.Utils.PianoUtils.PitchView.OnPitchEventHandler PitchActionEnd;
            public event VocalUtau.DirectUI.Utils.PianoUtils.PitchView.OnPitchEventHandler PitchActionBegin;
            public event VocalUtau.DirectUI.Utils.PianoUtils.ActionView.OnTickPosChangeHandler TickPosChange;
            public event VocalUtau.DirectUI.Utils.PianoUtils.NoteView.OnNoteSelectHandler NoteSelecting;
            #endregion

            public PitchView Track_PitchView;
            public NoteView Track_NoteView;
            public ActionView Global_ActionView;
            public DYNParamView Param_DynamicView;
            public PITParamView Param_PitchView;
            public void SwitchParamView(ParamViewType type)
            {
                if (type == ParamViewType.Pitch)
                {
                    if (!Param_PitchView.HandleEvents)
                    {
                        Param_PitchView.HandleEvents = true;
                        Param_DynamicView.HandleEvents = false;
                        ParamWindow.Refresh();
                    }
                }
                else
                {
                    if (!Param_DynamicView.HandleEvents)
                    {
                        Param_PitchView.HandleEvents =  false;
                        Param_DynamicView.HandleEvents = true;
                        ParamWindow.Refresh();
                    }
                }
            }
            public void SwitchRollAction(RollActionType type)
            {
                if (type == RollActionType.Pitch)
                {
                    if (!Track_PitchView.HandleEvents)
                    {
                        Track_NoteView.HandleEvents = false; 
                        Track_PitchView.HandleEvents = true;
                        PianoWindow.Refresh();
                    }
                }
                else
                {
                    if (!Track_NoteView.HandleEvents)
                    {
                        Track_NoteView.HandleEvents = true;
                        Track_PitchView.HandleEvents = false;
                        PianoWindow.Refresh();
                    }
                }
            }
            public ParamViewType GetParamType()
            {
                if (Param_PitchView.HandleEvents)return ParamViewType.Pitch;
                return ParamViewType.Dynamic;
            }
            public RollActionType GetRollActionType()
            {
                if (Track_PitchView.HandleEvents) return RollActionType.Pitch;
                return RollActionType.Note;
            }
            public uint ParamZoom
            {
                get
                {
                    if (GetParamType() == ParamViewType.Pitch)
                    {
                        return Param_PitchView.Zoom;
                    }
                    else
                    {
                        return Param_DynamicView.Zoom;
                    }
                }
                set
                {
                    if (GetParamType() == ParamViewType.Pitch)
                    {
                        if(value>0)Param_PitchView.Zoom=value;
                    }
                    else
                    {
                        if (value > 0) Param_DynamicView.Zoom = value;
                    }
                }
            }

            public void SetNoteViewTool(NoteView.NoteToolsType Tool)
            {
                Track_NoteView.NoteToolsStatus = Tool;
                SwitchRollAction(RollActionType.Note);
            }
            public void SetPitchViewTool(PitchView.PitchDragingType Tool)
            {
                Track_PitchView.PitchToolsStatus = Tool;
                SwitchRollAction(RollActionType.Pitch);
                if (_BindRollAndParam)
                {
                    Param_PitchView.PitchToolsStatus = Tool;
                    Param_DynamicView.DynToolsStatus = Tool;
                    ParamWindow.RedrawPiano();
                }
            }
            public void SetParamGraphicTool(PitchView.PitchDragingType Tool)
            {
                Param_PitchView.PitchToolsStatus = Tool;
                Param_DynamicView.DynToolsStatus = Tool;
                ParamWindow.RedrawPiano();
                if (_BindRollAndParam)
                {
                    SwitchRollAction(RollActionType.Pitch);
                    Track_PitchView.PitchToolsStatus = Tool;
                    PianoWindow.RedrawPiano();
                }
            }
            public void setTimePos(double Time)
            {
                if (Global_ActionView.TimePos != Time)
                {
                    Global_ActionView.TimePos = Time;
                    PianoWindow.RedrawPiano();
                    ParamWindow.RedrawPiano();
                }
            }
            public long getTickPos()
            {
                return Global_ActionView.TickPos;
            }

            public CopyPaste CopyPasteController;
            public class CopyPaste
            {
                NoteView NV;
                PitchView PV;
                public CopyPaste(ref NoteView nNV, ref PitchView pPV)
                {
                    this.NV = nNV;
                    this.PV = pPV;
                }

                List<NoteObject> PNV = new List<NoteObject>();
                List<PitchObject> PPV = new List<PitchObject>();
                public bool CopySelectsNote()
                {
                    PNV = NV.getSelectNotes(true);
                    if (PNV.Count <= 0) return false;
                    PPV = NV.getSelectPitchs(true);
                    return true;
                }
                public bool isCopyed
                {
                    get
                    {
                        return PNV.Count > 0;
                    }
                }
                public bool PasteNotes(long StartTick)
                {
                    bool R = NV.AddNotes(StartTick, PNV);
                    PNV.Clear();
                    if (!R) return false;
                    else
                    {
                        PV.AddPitchs(StartTick, PPV);
                    }
                    return true;
                }
            }

        }
        public delegate void OnTotalTimePosChangeHandler(double Time);
        public event OnTotalTimePosChangeHandler TotalTimePosChange;

        ViewController Controller;
        ObjectAlloc<PartsObject> OAC = new ObjectAlloc<PartsObject>();
        ObjectAlloc<ProjectObject> ProjectBeeper = new ObjectAlloc<ProjectObject>();

        public SingerWindow()
        {
            InitializeComponent();
            Controller = new ViewController(ref this.pianoRollWindow1, ref this.paramCurveWindow1);
            Controller.TickPosChange += Controller_TickPosChange;
            Controller.NoteSelecting += Controller_NoteSelecting;
            this.pianoRollWindow1.TrackMouseClick += pianoRollWindow1_TrackMouseClick;
            this.paramCurveWindow1.ParamAreaMouseClick += paramCurveWindow1_ParamAreaMouseClick;
        }
        
        void Controller_NoteSelecting(int SelectedNoteIndex)
        {
            if (AttributeWindow != null)
            {
                PartsObject PO = (PartsObject)OAC.AllocedObject;
                if (SelectedNoteIndex < 0)
                {
                    AttributeWindow.LoadPartsPtr(ref PO);
                }
                else
                {
                    List<NoteObject> NoteList = ((PartsObject)OAC.AllocedObject).NoteList;
                    if (NoteList.Count > SelectedNoteIndex)
                    {
                        NoteObject NoteObj = NoteList[SelectedNoteIndex];
                        AttributeWindow.LoadNotesPtr(ref PO, ref NoteObj);
                    }
                }
            }
        }

        public void BindAttributeWindow(AttributesWindow attrwin)
        {
            this.AttributeWindow = attrwin;
            this.AttributeWindow.AttributeChange += AttributeWindow_AttributeChange;
        }

        void AttributeWindow_AttributeChange()
        {
            GuiRefresh();
        }


        void Controller_TickPosChange(long Tick, double Time)
        {
            if (TotalTimePosChange != null) TotalTimePosChange(Time+((PartsObject)OAC.AllocedObject).getStartTime());
        }

        public void setCurrentTimePos(double Time)
        {
            double StartTime = ((PartsObject)OAC.AllocedObject).getStartTime();
            double EndTime = StartTime + ((PartsObject)OAC.AllocedObject).getDuringTime();
            double CurrTime = Time - StartTime;
            if (Time > EndTime) CurrTime = -1;
            if (Time < StartTime) CurrTime = -1;
            Controller.setTimePos(CurrTime);
            long CurTick=Controller.getTickPos();
            long PreStartTick=CurTick-(this.pianoRollWindow1.MaxShownTick-this.pianoRollWindow1.MinShownTick)/2;
            if(PreStartTick<0)PreStartTick=0;
            if (PreStartTick > ((PartsObject)OAC.AllocedObject).TickLength) return;
            if (CurTick < this.pianoRollWindow1.MinShownTick || CurTick > this.pianoRollWindow1.MaxShownTick)
            {
                ctl_Scroll_LeftPos.Value = (int)PreStartTick;
                ctl_Scroll_LeftPos_Scroll(null, null);
            }
        }

        void paramCurveWindow1_ParamAreaMouseClick(object sender, ParamMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button == MouseButtons.Right)
            {
                SetCurveActionMenu();
                CurveAction_SetupCurrentToMouse.Visible = true;
                CurveAction_SetupCurrentToMouse_Separator.Visible = true;
                ParamCurveTollMenu.Show(PointToScreen(new Point(e.MouseEventArgs.X, e.MouseEventArgs.Y + MainPianoSplitContainer.Top + MainPianoSplitContainer.SplitterDistance + MainPianoSplitContainer.SplitterWidth)), ToolStripDropDownDirection.AboveRight);
            }
        }
        public void GuiRefresh()
        {
            this.paramCurveWindow1.RedrawPiano();
            this.pianoRollWindow1.RedrawPiano();
            try
            {
                this.Text = ((PartsObject)OAC.AllocedObject).PartName;
                setupSingerIcon();
            }
            catch { ;}
        }

        void pianoRollWindow1_TrackMouseClick(object sender, PianoMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button == MouseButtons.Right)
            {
                SetPianoActionMenu();
                PianoRollActionMenu.Show(PointToScreen(new Point(e.MouseEventArgs.X, e.MouseEventArgs.Y)), ToolStripDropDownDirection.BelowRight);
            }
            else
            {
                Controller.RealaramNoteSelecting();
            }
        }

        public void ShowOnDock(DockPanel DockPanel)
        {
            this.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.Document);
        }

        public void LoadProjectObject(ref ProjectObject proj)
        {
            ProjectBeeper.ReAlloc(proj);
        }

        public void LoadParts(ref PartsObject parts)
        {
            OAC.ReAlloc(parts);
            Controller.AllocView(OAC.IntPtr);
            ctl_Scroll_LeftPos.Maximum = (int)parts.TickLength;
            ctl_Scroll_LeftPos.Value = 0;
            ctl_Scroll_LeftPos_Scroll(null, null);
            this.Text = parts.PartName;
            AttributeWindow.LoadPartsPtr(ref parts);
            setupSingerIcon();
        }

        void setupSingerIcon()
        {
            if (ProjectBeeper==null || ProjectBeeper.AllocedObject == null)
            {
                return;
            }
            if (OAC == null || OAC.AllocedObject == null)
            {
                return;
            }
            ProjectObject po = (ProjectObject)ProjectBeeper.AllocedObject;
            PartsObject pt = (PartsObject)OAC.AllocedObject;
            string avatar = "";
            if (po.SingerList.Count > 0) avatar = po.SingerList[0].Avatar;
            foreach (SingerObject so in po.SingerList)
            {
                if(so.getGuid() == pt.SingerGUID)
                {
                    avatar = so.Avatar;
                    break;
                }
            }
            if (System.IO.File.Exists(avatar))
            {
                UtauPic.Image = Image.FromFile(avatar);
            }
            else
            {
                UtauPic.Image = null;
            }
        }

        #region
        private void ctl_Scroll_LeftPos_Scroll(object sender, ScrollEventArgs e)
        {
            pianoRollWindow1.setPianoStartTick(ctl_Scroll_LeftPos.Value);
            paramCurveWindow1.setPianoStartTick(ctl_Scroll_LeftPos.Value);
        }

        private void ctl_Track_PianoWidth_Scroll(object sender, EventArgs e)
        {
            pianoRollWindow1.setCrotchetSize((uint)ctl_Track_PianoWidth.Value);
            paramCurveWindow1.setCrotchetSize((uint)ctl_Track_PianoWidth.Value);
        }

        private void ctl_Track_NoteHeight_Scroll(object sender, EventArgs e)
        {
            pianoRollWindow1.setNoteHeight((uint)ctl_Track_NoteHeight.Value);
        }
        
        public void SetupParamZoom()
        {
            Controller.ParamZoom = (uint)(ctl_Param_RZoom.Value * ctl_Param_LZoom.Maximum + ctl_Param_LZoom.Value);
        }
        public void ShowBackParamZoom()
        {
            int LZ=(int)(Controller.ParamZoom % ctl_Param_LZoom.Maximum);
            ctl_Param_LZoom.Value = LZ < 1 ? 1 : LZ;
            ctl_Param_RZoom.Value = (int)Math.Floor((decimal)Controller.ParamZoom / ctl_Param_LZoom.Maximum);
        }

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

        private void pianoRollWindow1_RollMouseDown(object sender, PianoMouseEventArgs e)
        {
            PlayNote((int)e.PitchValue.NoteNumber);
        }
        #endregion

        private void btn_SelectCurve_Click(object sender, EventArgs e)
        {
            int x = btn_SelectCurve.Left + btn_SelectCurve.Width;
            int y = btn_SelectCurve.Top + btn_SelectCurve.Height +MainPianoSplitContainer.Top + MainPianoSplitContainer.SplitterDistance + MainPianoSplitContainer.SplitterWidth;
            CurveSelector_DYN.Checked = false;
            CurveSelector_PIT.Checked = false;
            switch (Controller.GetParamType())
            {
                case ParamViewType.Dynamic: CurveSelector_DYN.Checked = true; break;
                case ParamViewType.Pitch: CurveSelector_PIT.Checked = true; break;
            }
            ParamCurveTypeMenu.Show(PointToScreen(new Point(x,y)),ToolStripDropDownDirection.AboveRight);
        }

        private void CurveSelector_PIT_Click(object sender, EventArgs e)
        {
            Controller.SwitchParamView(ParamViewType.Pitch);
            ShowBackParamZoom();
        }

        private void CurveSelector_DYN_Click(object sender, EventArgs e)
        {
            Controller.SwitchParamView(ParamViewType.Dynamic);
            ShowBackParamZoom();
        }
        void SetCurveActionMenu()
        {
            CurveAction_SetupCurrentToMouse.Visible = false;
            CurveAction_SetupCurrentToMouse_Separator.Visible = false;
            CurveTool_EarseSelect.Checked = false;
            CurveTool_DrawLine.Checked = false;
            CurveTool_DrawS.Checked = false;
            CurveTool_DrawR.Checked = false;
            CurveTool_DrawJ.Checked = false;
            PitchView.PitchDragingType PDT = Controller.Param_PitchView.PitchToolsStatus;
            if (Controller.GetParamType() == ParamViewType.Dynamic)
            {
                PDT = Controller.Param_DynamicView.DynToolsStatus;
            }
            switch (PDT)
            {
                case PitchView.PitchDragingType.EarseArea: CurveTool_EarseSelect.Checked = true; break;
                case PitchView.PitchDragingType.DrawLine: CurveTool_DrawLine.Checked = true; break;
                case PitchView.PitchDragingType.DrawGraphS: CurveTool_DrawS.Checked = true; break;
                case PitchView.PitchDragingType.DrawGraphR: CurveTool_DrawR.Checked = true; break;
                case PitchView.PitchDragingType.DrawGraphJ: CurveTool_DrawJ.Checked = true; break;
            }
            BindPianoRoll.Checked = Controller.BindRollAndParam;
        }
        private void btn_SelectAction_Click(object sender, EventArgs e)
        {
            int x = btn_SelectAction.Left + btn_SelectAction.Width;
            int y = btn_SelectAction.Top + btn_SelectAction.Height + MainPianoSplitContainer.Top + MainPianoSplitContainer.SplitterDistance + MainPianoSplitContainer.SplitterWidth;

            SetCurveActionMenu();
            
            ParamCurveTollMenu.Show(PointToScreen(new Point(x, y)), ToolStripDropDownDirection.AboveRight);
        }

        void SetPianoActionMenu()
        {
            RollTool_DrawJ.Checked = false;
            RollTool_DrawLine.Checked = false;
            RollTool_DrawR.Checked = false;
            RollTool_DrawS.Checked = false;
            RollTool_Earse.Checked = false;
            RollTool_NoteAdd.Checked = false;
            RollTool_NoteSelect.Checked = false;
            if (Controller.GetRollActionType() == RollActionType.Note)
            {
                switch (Controller.Track_NoteView.NoteToolsStatus)
                {
                    case NoteView.NoteToolsType.Select: RollTool_NoteSelect.Checked = true; break;
                    case NoteView.NoteToolsType.Add: RollTool_NoteAdd.Checked = true; break;
                }
            }
            else
            {
                switch (Controller.Track_PitchView.PitchToolsStatus)
                {
                    case PitchView.PitchDragingType.EarseArea: RollTool_Earse.Checked = true; break;
                    case PitchView.PitchDragingType.DrawLine: RollTool_DrawLine.Checked = true; break;
                    case PitchView.PitchDragingType.DrawGraphS: RollTool_DrawS.Checked = true; break;
                    case PitchView.PitchDragingType.DrawGraphR: RollTool_DrawR.Checked = true; break;
                    case PitchView.PitchDragingType.DrawGraphJ: RollTool_DrawJ.Checked = true; break;
                }
            }
            RollAction_NotePaste.Enabled = Controller.CopyPasteController.isCopyed;
            RollAction_NoteCopy.Enabled = Controller.Track_NoteView.SelectedCount > 0;
            RollAction_EditLyrics.Enabled = Controller.Track_NoteView.SelectedCount > 0;
        }
        private void btn_PianoRollAction_Click(object sender, EventArgs e)
        {
            int x = btn_PianoRollAction.Left + btn_PianoRollAction.Width;
            int y = btn_PianoRollAction.Top + MainPianoSplitContainer.Top;

            SetPianoActionMenu();
            PianoRollActionMenu.Show(PointToScreen(new Point(x, y)), ToolStripDropDownDirection.BelowRight);
        }

        private void RollAction_SetCurrentPos_Click(object sender, EventArgs e)
        {
            Controller.Global_ActionView.SetupMouseTick();
        }

        private void RollAction_NoteCopy_Click(object sender, EventArgs e)
        {
            Controller.CopyPasteController.CopySelectsNote();
        }

        private void RollAction_NotePaste_Click(object sender, EventArgs e)
        {
            if (Controller.CopyPasteController.isCopyed)
            {
                if (!Controller.CopyPasteController.PasteNotes(Controller.Global_ActionView.TickPos))
                {
                    MessageBox.Show("Paste Error! No Enough Spaces!");  
                }
            }
        }

        private void RollAction_EditLyrics_Click(object sender, EventArgs e)
        {
            Controller.Track_NoteView.EditNoteLyric();
        }

        private void RollTool_NoteSelect_Click(object sender, EventArgs e)
        {
            Controller.SetNoteViewTool(NoteView.NoteToolsType.Select);
        }

        private void RollTool_NoteAdd_Click(object sender, EventArgs e)
        {
            Controller.SetNoteViewTool(NoteView.NoteToolsType.Add);
        }

        private void RollTool_DrawLine_Click(object sender, EventArgs e)
        {
            Controller.SetPitchViewTool(PitchView.PitchDragingType.DrawLine);
        }

        private void RollTool_DrawJ_Click(object sender, EventArgs e)
        {
            Controller.SetPitchViewTool(PitchView.PitchDragingType.DrawGraphJ);
        }

        private void RollTool_DrawR_Click(object sender, EventArgs e)
        {
            Controller.SetPitchViewTool(PitchView.PitchDragingType.DrawGraphR);
        }

        private void RollTool_DrawS_Click(object sender, EventArgs e)
        {
            Controller.SetPitchViewTool(PitchView.PitchDragingType.DrawGraphS);
        }

        private void RollTool_Earse_Click(object sender, EventArgs e)
        {
            Controller.SetPitchViewTool(PitchView.PitchDragingType.EarseArea);
        }

        private void BindPianoRoll_Click(object sender, EventArgs e)
        {
            Controller.BindRollAndParam = !Controller.BindRollAndParam;
        }

        private void CurveTool_DrawLine_Click(object sender, EventArgs e)
        {
            Controller.SetParamGraphicTool(PitchView.PitchDragingType.DrawLine);
        }

        private void CurveTool_DrawJ_Click(object sender, EventArgs e)
        {
            Controller.SetParamGraphicTool(PitchView.PitchDragingType.DrawGraphJ);
        }

        private void CurveTool_DrawR_Click(object sender, EventArgs e)
        {
            Controller.SetParamGraphicTool(PitchView.PitchDragingType.DrawGraphR);
        }

        private void CurveTool_DrawS_Click(object sender, EventArgs e)
        {
            Controller.SetParamGraphicTool(PitchView.PitchDragingType.DrawGraphS);
        }

        private void CurveTool_EarseSelect_Click(object sender, EventArgs e)
        {
            Controller.SetParamGraphicTool(PitchView.PitchDragingType.EarseArea);
        }

        private void ctl_Param_RZoom_Scroll(object sender, EventArgs e)
        {
            SetupParamZoom();
        }

        private void ctl_Param_LZoom_Scroll(object sender, EventArgs e)
        {
            SetupParamZoom();
        }
        

    }
}
