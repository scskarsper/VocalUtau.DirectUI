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
using VocalUtau.DirectUI.Utils.SingerUtils;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;
using WeifenLuo.WinFormsUI.Docking;

namespace VocalUtau.DirectUI.Forms
{
    public partial class SingerWindow : DockContent
    {
        public AttributesWindow AttributeWindow = null;
        public event VocalUtau.DirectUI.Utils.PianoUtils.NoteView.OnNoteSelectListChangedHandler NoteSelectListChange;
        public event VocalUtau.DirectUI.Forms.SingerWindow.ViewController.OnNoteCopyMemoryChangedHandler NoteCopyMemoryChanged;

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
                    Track_NoteView.setLyricSpliter(LyricSpliters);
                }
                else
                {
                    Track_NoteView = new NoteView(ObjectPtr, this.PianoWindow);
                    Track_NoteView.setLyricSpliter(LyricSpliters);
                    Track_PitchView = new PitchView(ObjectPtr, this.PianoWindow);
                    Param_PitchView = new PITParamView(ObjectPtr, this.ParamWindow);
                    Param_DynamicView = new DYNParamView(ObjectPtr, this.ParamWindow);
                    Global_ActionView = new ActionView(ObjectPtr, this.PianoWindow, this.ParamWindow);

                    CopyPasteController = new CopyPaste(ref Track_NoteView, ref Track_PitchView);
                    CopyPasteController.NoteCopyMemoryChanged += CopyPasteController_NoteCopyMemoryChanged;

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

                    Track_NoteView.NoteSelectListChange += Track_NoteView_NoteSelectListChange;

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

            void CopyPasteController_NoteCopyMemoryChanged(bool isCopyed)
            {
                if (NoteCopyMemoryChanged != null) NoteCopyMemoryChanged(isCopyed);
            }

            void Track_NoteView_NoteSelectListChange(List<int> SelectedIndexs)
            {
                if (NoteSelectListChange != null) NoteSelectListChange(SelectedIndexs);
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
            public delegate void OnNoteCopyMemoryChangedHandler(bool isCopyed);
            public event OnNoteCopyMemoryChangedHandler NoteCopyMemoryChanged;
            public event VocalUtau.DirectUI.Utils.PianoUtils.NoteView.OnNoteSelectListChangedHandler NoteSelectListChange;
            public delegate void OnToolStatusChangeHandler(object StatusEnum);
            public event OnToolStatusChangeHandler ToolStatusChange;
            #endregion

            public PitchView Track_PitchView;
            public NoteView Track_NoteView;
            public ActionView Global_ActionView;
            public DYNParamView Param_DynamicView;
            public PITParamView Param_PitchView;
            SingerLyricSpliter LyricSpliters;
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

            public void ResetLyricSpliter(ref SingerLyricSpliter LyricSpliter)
            {
                this.LyricSpliters = LyricSpliter;
                if (Track_NoteView != null)
                {
                    Track_NoteView.setLyricSpliter(LyricSpliters);
                }
            }

            public void SetNoteViewTool(NoteView.NoteToolsType Tool)
            {
                Track_NoteView.NoteToolsStatus = Tool;
                SwitchRollAction(RollActionType.Note);
                if (ToolStatusChange != null) ToolStatusChange(Tool);
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
                if (ToolStatusChange != null) ToolStatusChange(Tool);
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
                    if (ToolStatusChange != null) ToolStatusChange(Tool);
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
                public event OnNoteCopyMemoryChangedHandler NoteCopyMemoryChanged;
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
                    if (NoteCopyMemoryChanged != null) NoteCopyMemoryChanged(isCopyed);
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
                    if (!R)
                    {
                        if (NoteCopyMemoryChanged != null) NoteCopyMemoryChanged(isCopyed);
                        return false;
                    }
                    else
                    {
                        PV.AddPitchs(StartTick, PPV);
                        if (NoteCopyMemoryChanged != null) NoteCopyMemoryChanged(isCopyed);
                        return true;
                    }
                }
            }

        }
        public delegate void OnTotalTimePosChangeHandler(double Time);
        public event OnTotalTimePosChangeHandler TotalTimePosChange;

        ViewController Controller;

        public ViewController BaseController
        {
            get { return Controller; }
        }
        ObjectAlloc<PartsObject> OAC = new ObjectAlloc<PartsObject>();
        ObjectAlloc<ProjectObject> ProjectBeeper = new ObjectAlloc<ProjectObject>();

        public SingerWindow()
        {
            InitializeComponent();
            Controller = new ViewController(ref this.pianoRollWindow1, ref this.paramCurveWindow1);
            Controller.TickPosChange += Controller_TickPosChange;
            Controller.NoteSelecting += Controller_NoteSelecting;
            Controller.NoteSelectListChange += Controller_NoteSelectListChange;
            Controller.NoteCopyMemoryChanged += Controller_NoteCopyMemoryChanged;
            Controller.NoteActionEnd += Controller_NoteActionEnd;
            this.pianoRollWindow1.TrackMouseClick += pianoRollWindow1_TrackMouseClick;
            this.paramCurveWindow1.ParamAreaMouseClick += paramCurveWindow1_ParamAreaMouseClick;
            ResetComponent();
        }

        void Controller_NoteActionEnd(NoteView.NoteDragingType eventType)
        {
            if (eventType == NoteView.NoteDragingType.LyricEdit)
            {
                this.AttributeWindow.GuiRefresh();
            }
        }

        void Controller_NoteCopyMemoryChanged(bool isCopyed)
        {
            if (NoteCopyMemoryChanged != null) NoteCopyMemoryChanged(isCopyed);
        }

        void Controller_NoteSelectListChange(List<int> SelectedIndexs)
        {
            if (NoteSelectListChange != null) NoteSelectListChange(SelectedIndexs);
        }

        void ResetComponent()
        {
            this.btn_SelectCurve.Location = new System.Drawing.Point(-1, 93);
            this.btn_SelectCurve.Size = new System.Drawing.Size(59, 22);
            this.panel1.Location = new System.Drawing.Point(924, -3);
            this.panel1.Size = new System.Drawing.Size(23, 207);
            this.ctl_Track_NoteHeight.Location = new System.Drawing.Point(864, 344);
            this.ctl_Track_NoteHeight.Margin = new System.Windows.Forms.Padding(2);
            this.ctl_Track_NoteHeight.Size = new System.Drawing.Size(79, 16);
            this.btn_PianoRollAction.Size = new System.Drawing.Size(81, 27);
            this.ctl_Scroll_LeftPos.Location = new System.Drawing.Point(81, 344);
            this.ctl_Scroll_LeftPos.Size = new System.Drawing.Size(781, 18);
            this.ctl_Track_PianoWidth.Location = new System.Drawing.Point(0, 344);
            this.ctl_Track_PianoWidth.Margin = new System.Windows.Forms.Padding(2);
            this.ctl_Track_PianoWidth.Size = new System.Drawing.Size(79, 16);
            this.UtauPic.Location = new System.Drawing.Point(7, 11);
            this.UtauPic.Size = new System.Drawing.Size(51, 50);
            this.btn_SelectAction.Location = new System.Drawing.Point(-1, 72);
            this.btn_SelectAction.Size = new System.Drawing.Size(59, 22);
            this.ParamCurveTypeMenu.Size = new System.Drawing.Size(103, 48);
            this.CurveSelector_PIT.Size = new System.Drawing.Size(102, 22);
            this.CurveSelector_DYN.Size = new System.Drawing.Size(102, 22);
            this.PianoRollActionMenu.Size = new System.Drawing.Size(264, 292);
            this.RollAction_SetCurrentPos.Size = new System.Drawing.Size(263, 22);
            this.toolStripSeparator5.Size = new System.Drawing.Size(260, 6);
            this.RollTool_NoteSelect.Size = new System.Drawing.Size(263, 22);
            this.RollTool_NoteAdd.Size = new System.Drawing.Size(263, 22);
            this.toolStripSeparator3.Size = new System.Drawing.Size(260, 6);
            this.RollTool_DrawLine.Size = new System.Drawing.Size(263, 22);
            this.RollTool_DrawJ.Size = new System.Drawing.Size(263, 22);
            this.RollTool_DrawR.Size = new System.Drawing.Size(263, 22);
            this.RollTool_DrawS.Size = new System.Drawing.Size(263, 22);
            this.RollTool_Earse.Size = new System.Drawing.Size(263, 22);
            this.toolStripSeparator4.Size = new System.Drawing.Size(260, 6);
            this.RollAction_NoteCopy.Size = new System.Drawing.Size(263, 22);
            this.RollAction_NotePaste.Size = new System.Drawing.Size(263, 22);
            this.RollAction_EditLyrics.Size = new System.Drawing.Size(263, 22);
            this.ParamCurveTollMenu.Size = new System.Drawing.Size(264, 170);
            this.CurveAction_SetupCurrentToMouse.Size = new System.Drawing.Size(263, 22);
            this.CurveAction_SetupCurrentToMouse_Separator.Size = new System.Drawing.Size(260, 6);
            this.CurveTool_DrawLine.Size = new System.Drawing.Size(263, 22);
            this.CurveTool_DrawJ.Size = new System.Drawing.Size(263, 22);
            this.CurveTool_DrawR.Size = new System.Drawing.Size(263, 22);
            this.CurveTool_DrawS.Size = new System.Drawing.Size(263, 22);
            this.CurveTool_EarseSelect.Size = new System.Drawing.Size(263, 22);
            this.toolStripSeparator1.Size = new System.Drawing.Size(260, 6);

            this.ctl_Param_LZoom.Location = new System.Drawing.Point(63, 2);
            this.ctl_Param_LZoom.Margin = new System.Windows.Forms.Padding(2);
            this.ctl_Param_LZoom.Size = new System.Drawing.Size(16, 128);

            this.ctl_Param_RZoom.Location = new System.Drawing.Point(-3, 4);
            this.ctl_Param_RZoom.Margin = new System.Windows.Forms.Padding(2);
            this.ctl_Param_RZoom.Size = new System.Drawing.Size(17, 128);

            this.paramCurveWindow1.Size = new System.Drawing.Size(945, 141);
            this.paramCurveWindow1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);

            this.pianoRollWindow1.Size = new System.Drawing.Size(943, 342);

            this.MainPianoSplitContainer.Size = new System.Drawing.Size(945, 513);
            this.MainPianoSplitContainer.SplitterDistance = 362;
            this.MainPianoSplitContainer.SplitterWidth = 10;
            this.BindPianoRoll.Size = new System.Drawing.Size(263, 22);
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(945, 513);
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

        void AttributeWindow_AttributeChange(PropertyValueChangedEventArgs e, ProjectObject oldObj)
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
        public void SetupLyricSpliter(ref SingerLyricSpliter sls)
        {
            this.Controller.ResetLyricSpliter(ref sls);
        }
        public void LoadProjectObject(ref ProjectObject proj)
        {
            ProjectBeeper.ReAlloc(proj);
        }
        
        public void LoadParts(ref PartsObject parts,bool KeepSelecting=false)
        {
            OAC.ReAlloc(parts);
            Controller.AllocView(OAC.IntPtr);
            ctl_Scroll_LeftPos.Maximum = (int)parts.TickLength;
            ctl_Scroll_LeftPos.Value = 0;
            ctl_Scroll_LeftPos_Scroll(null, null);
            this.Text = parts.PartName;
            AttributeWindow.LoadPartsPtr(ref parts);
            if (KeepSelecting)
            {
                Controller.RealaramNoteSelecting();
            }
            else
            {

                Controller.Track_NoteView.ClearSelect();
            }
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
            CopyNotes();
        }

        public void CopyNotes()
        {
            Controller.CopyPasteController.CopySelectsNote();
        }
        public void PasteNotes()
        {
            if (Controller.CopyPasteController.isCopyed)
            {
                if (!Controller.CopyPasteController.PasteNotes(Controller.Global_ActionView.TickPos))
                {
                    MessageBox.Show("粘贴错误,没有足够的空白用于粘贴剪贴板内段落!");
                }
                else
                {
                    this.pianoRollWindow1.RedrawPiano();
                    this.paramCurveWindow1.RedrawPiano();
                }
            }
        }
        private void RollAction_NotePaste_Click(object sender, EventArgs e)
        {
            PasteNotes();
        }
        public void EditLyrics()
        {
            Controller.Track_NoteView.EditNoteLyric();
            this.pianoRollWindow1.RedrawPiano();
            this.paramCurveWindow1.RedrawPiano();
            Controller.RealaramNoteSelecting();
        }
        public void NoteDeletes()
        {
            Controller.Track_NoteView.NoteDelete();
            this.pianoRollWindow1.RedrawPiano();
            this.paramCurveWindow1.RedrawPiano();
        }
        private void RollAction_EditLyrics_Click(object sender, EventArgs e)
        {
            EditLyrics();
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
