using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.TrackerUtils;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;
using WeifenLuo.WinFormsUI.Docking;

namespace VocalUtau.DirectUI.Forms
{
    public partial class TrackerWindow : DockContent
    {
        public AttributesWindow AttributeWindow = null;
        public delegate void OnTotalTimePosChangeHandler(double Time);
        public event OnTotalTimePosChangeHandler TotalTimePosChange;
        public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnSelectingPartChangeHandler SelectingPartChanged;
        public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnSelectingWavPartChangeHandler SelectingWavePartChanged;
        public class ViewController
        {   
            bool Alloced = false;
            TrackerRollWindow TrackerWindow;
            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnSelectingPartChangeHandler SelectingPartChanged;
            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnSelectingWavPartChangeHandler SelectingWavePartChanged;
            
            public ViewController(ref TrackerRollWindow TrackerWin)
            {
                this.TrackerWindow = TrackerWin;
            }
            public void AddNewTrack(bool isBackTrack)
            {
                _Track_View.AddNewTrack(isBackTrack);
            }
            public void AddNewPart()
            {
                VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.PartLocation pl=_Track_View.getSelectingParts();
                if(pl!=null) _Track_View.AddNewPart(pl.TrackLocation);
            }
            public void DeletePart()
            {
                VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.PartLocation pl = _Track_View.getSelectingParts();
                if (pl != null) _Track_View.DeleteAPart(pl);
            }
            public void DeleteTrack()
            {
                VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.PartLocation pl = _Track_View.getSelectingParts();
                if (pl != null) _Track_View.DeleteATrack(pl.TrackLocation);
            }
            public void ImportAsPart(string file)
            {
                VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.PartLocation pl = _Track_View.getSelectingParts();
                if (pl != null) _Track_View.ImportWavAsPart(pl.TrackLocation, file);
            }
            public void ImportAsTrack(string file)
            {
                _Track_View.ImportWavAsTrack(file);
            }
            public void AllocView(IntPtr ObjectPtr)
            {
                if (Alloced)
                {
                    _Track_View.setProjectObjectPtr(ObjectPtr);
                }
                else
                {
                    _Track_View = new TrackerView(ObjectPtr, this.TrackerWindow);
                    _Action_View = new TrackerActionView(ObjectPtr, this.TrackerWindow);
                    _Track_View.TrackerActionBegin += Track_View_TrackerActionBegin;
                    _Track_View.TrackerActionEnd += Track_View_TrackerActionEnd;
                    _Track_View.ShowingEditorChanged += Track_View_ShowingEditorChanged;
                    _Track_View.ShowingEditorStartPosMoved += _Track_View_ShowingEditorStartPosMoved;
                    _Action_View.TickPosChange += _Action_View_TickPosChange;
                    _Track_View.SelectingPartChanged += _Track_View_SelectingPartChanged;
                    _Track_View.SelectingWavePartChanged += _Track_View_SelectingWavePartChanged;
                    _Track_View.HandleEvents = true;
                    Alloced = true;
                    _Track_View.ResetShowingParts();
                }
                try
                {
                    this.TrackerWindow.RedrawPiano();
                }
                catch { ;}
            }

            void _Track_View_SelectingWavePartChanged(WavePartsObject PartObject)
            {
                if (SelectingWavePartChanged != null)
                {
                    SelectingWavePartChanged(PartObject);
                }
            }

            void _Track_View_SelectingPartChanged(PartsObject PartObject,bool isEditing)
            {
                if (SelectingPartChanged != null)
                {
                    SelectingPartChanged(PartObject,isEditing);
                }
            }

            void _Track_View_ShowingEditorStartPosMoved()
            {
                _Action_View.RealarmTickPosition();
            }
            
            void _Action_View_TickPosChange(long Tick, double Time)
            {
                if (TickPosChange != null) TickPosChange(Tick, Time);
            }

            void Track_View_ShowingEditorChanged(PartsObject PartObject)
            {
                if (ShowingEditorChanged != null) ShowingEditorChanged(PartObject);
            }

            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnPartsEventHandler TrackerActionEnd;
            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnPartsEventHandler TrackerActionBegin;
            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnShowingEditorChangeHandler ShowingEditorChanged;
            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerActionView.OnTickPosChangeHandler TickPosChange;
            void Track_View_TrackerActionEnd(TrackerView.PartsDragingType eventType)
            {
                if (TrackerActionEnd != null) TrackerActionEnd(eventType);
            }

            void Track_View_TrackerActionBegin(TrackerView.PartsDragingType eventType)
            {
                if (TrackerActionBegin != null) TrackerActionBegin(eventType);
            }

            public void setTimePos(double Time)
            {
                if (_Action_View.TimePos != Time)
                {
                    _Action_View.TimePos = Time;
                    TrackerWindow.RedrawPiano();
                }
            }
            #region
            TrackerView _Track_View;
            TrackerActionView _Action_View;

            public TrackerActionView Action_View
            {
                get { return _Action_View; }
                set { _Action_View = value; }
            }

            public TrackerView Track_View
            {
                get { return _Track_View; }
            }
            #endregion

            #region
            #endregion


        }

        public ViewController BaseController
        {
            get { return Controller; }
        }
        ViewController Controller;
        ObjectAlloc<ProjectObject> OAC = new ObjectAlloc<ProjectObject>();
        public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnShowingEditorChangeHandler ShowingEditorChanged;

        public TrackerWindow()
        {
            InitializeComponent();
            Controller = new ViewController(ref this.trackerRollWindow1);
            Controller.TickPosChange += Controller_TickPosChange;
            Controller.ShowingEditorChanged += Controller_ShowingEditorChanged;
            Controller.SelectingPartChanged += Controller_SelectingPartChanged;
            Controller.SelectingWavePartChanged += Controller_SelectingWavePartChanged;
            Controller.TrackerActionBegin += Controller_TrackerActionBegin;
            Controller.TrackerActionEnd += Controller_TrackerActionEnd;
            this.trackerRollWindow1.PartsMouseClick += trackerRollWindow1_PartsMouseClick;
        }

        public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnPartsEventHandler TrackerActionEnd;
        public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnPartsEventHandler TrackerActionBegin;
        void Controller_TrackerActionEnd(TrackerView.PartsDragingType eventType)
        {
            if(TrackerActionEnd!=null)TrackerActionEnd(eventType);
        }

        void Controller_TrackerActionBegin(TrackerView.PartsDragingType eventType)
        {
            if (TrackerActionBegin != null) TrackerActionBegin(eventType);
        }

        void trackerRollWindow1_PartsMouseClick(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Point p=PointToScreen(new Point(e.MouseEventArgs.X, e.MouseEventArgs.Y));
                VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.PartLocation pl=Controller.Track_View.getSelectingParts();
                if (pl == null)
                {
                    track_DelectParts.Enabled = false;
                    track_DelTracks.Enabled = false;
                    track_AddParts.Enabled = false;
                    track_ImportAsPart.Enabled = false;
                }
                else
                {
                    track_DelectParts.Enabled = true;
                    track_DelTracks.Enabled = true;
                    track_AddParts.Enabled = true;
                    track_ImportAsPart.Enabled = pl.TrackLocation.Type == TrackerView.TrackLocation.TrackType.Barker;
                }
                menu_TrackEditor.Show(p);
            }
        }

        void Controller_SelectingWavePartChanged(WavePartsObject PartObject)
        {
            if (SelectingWavePartChanged != null)
            {
                SelectingWavePartChanged(PartObject);
            }
        }

        void Controller_SelectingPartChanged(PartsObject PartObject, bool isEditing)
        {
            if (SelectingPartChanged != null)
            {
                SelectingPartChanged(PartObject,isEditing);
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
            if (TotalTimePosChange != null) TotalTimePosChange(Time);
        }

        void Controller_ShowingEditorChanged(PartsObject PartObject)
        {
            if (ShowingEditorChanged != null) ShowingEditorChanged(PartObject);
        }

        private void TrackerWindow_Load(object sender, EventArgs e)
        {

        }

        public void RealarmTickPosition()
        {
            Controller.Action_View.RealarmTickPosition();
        }

        public void GuiRefresh()
        {
            Controller.Track_View.reloadBaseTempo();
            ProjectObject Po = (ProjectObject)OAC.AllocedObject;
            this.Text = Po.ProjectName;
            int MaxL=(int)Math.Ceiling(1920 + (double)Po.Time2Tick(Po.MaxLength));
            if (ctl_Scroll_LeftPos.Value > MaxL)
            {
                ctl_Scroll_LeftPos.Value = MaxL;
                trackerRollWindow1.setPianoStartTick(ctl_Scroll_LeftPos.Value);
            }
            ctl_Scroll_LeftPos.Maximum = MaxL;
            this.trackerRollWindow1.RedrawPiano();
        }

        public void ShowOnDock(DockPanel DockPanel)
        {
            this.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockTop);
        }

        public void setCurrentTimePos(double Time)
        {
            Controller.setTimePos(Time);
        }
        public void LoadProjectObject(ref ProjectObject projects)
        {
            OAC.ReAlloc(projects);
            Controller.AllocView(OAC.IntPtr);
            ctl_Scroll_LeftPos.Maximum = (int)Math.Ceiling(1920 + (double)projects.Time2Tick(projects.MaxLength));
            this.Text = projects.ProjectName;
        }

        private void ctl_Scroll_LeftPos_Scroll(object sender, ScrollEventArgs e)
        {
            trackerRollWindow1.setPianoStartTick(ctl_Scroll_LeftPos.Value);
        }

        private void ctl_Track_PianoWidth_Scroll(object sender, EventArgs e)
        {
            this.trackerRollWindow1.setCrotchetSize((uint)ctl_Track_PianoWidth.Value);
        }

        private void ctl_Track_TrackHeight_Scroll(object sender, EventArgs e)
        {
            this.trackerRollWindow1.setTrackHeight((uint)ctl_Track_TrackHeight.Value);
            Controller.Track_View.ResetScrollBar();
        }

        private void TrackerWindow_Enter(object sender, EventArgs e)
        {
        }

        private void track_AddNewBackerTrack_Click(object sender, EventArgs e)
        {
            Controller.AddNewTrack(true);
        }

        private void track_AddNewTrack_Click(object sender, EventArgs e)
        {
            Controller.AddNewTrack(false);
        }

        private void track_AddParts_Click(object sender, EventArgs e)
        {
            Controller.AddNewPart();
        }

        private void track_DelTracks_Click(object sender, EventArgs e)
        {
            Controller.DeleteTrack();
        }

        private void track_DelectParts_Click(object sender, EventArgs e)
        {
            Controller.DeletePart();
        }

        private void track_ImportAsTrack_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Wav音频文件|*.wav|所有文件|*.*";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Controller.ImportAsTrack(ofd.FileName);
            }
        }

        private void track_ImportAsPart_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Wav音频文件|*.wav|所有文件|*.*";
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Controller.ImportAsPart(ofd.FileName);
            }
        }
    }
}
