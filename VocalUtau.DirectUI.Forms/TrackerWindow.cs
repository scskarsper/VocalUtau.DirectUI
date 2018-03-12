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
        public class ViewController
        {   
            bool Alloced = false;
            TrackerRollWindow TrackerWindow;
            public ViewController(ref TrackerRollWindow TrackerWin)
            {
                this.TrackerWindow = TrackerWin;
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
                    _Track_View.TrackerActionBegin += Track_View_TrackerActionBegin;
                    _Track_View.TrackerActionEnd += Track_View_TrackerActionEnd;
                    _Track_View.ShowingEditorChanged += Track_View_ShowingEditorChanged;
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

            void Track_View_ShowingEditorChanged(PartsObject PartObject)
            {
                if (ShowingEditorChanged != null) ShowingEditorChanged(PartObject);
            }

            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnPartsEventHandler TrackerActionEnd;
            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnPartsEventHandler TrackerActionBegin;
            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnShowingEditorChangeHandler ShowingEditorChanged;
            void Track_View_TrackerActionEnd(TrackerView.PartsDragingType eventType)
            {
                if (TrackerActionEnd != null) TrackerActionEnd(eventType);
            }

            void Track_View_TrackerActionBegin(TrackerView.PartsDragingType eventType)
            {
                if (TrackerActionBegin != null) TrackerActionBegin(eventType);
            }

            #region
            TrackerView _Track_View;

            public TrackerView Track_View
            {
                get { return _Track_View; }
            }
            #endregion

            #region
            #endregion


        }

        ViewController Controller;
        ObjectAlloc<ProjectObject> OAC = new ObjectAlloc<ProjectObject>();
        public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnShowingEditorChangeHandler ShowingEditorChanged;

        public TrackerWindow()
        {
            InitializeComponent();
            Controller = new ViewController(ref this.trackerRollWindow1);
            Controller.ShowingEditorChanged += Controller_ShowingEditorChanged;
        }

        void Controller_ShowingEditorChanged(PartsObject PartObject)
        {
            if (ShowingEditorChanged != null) ShowingEditorChanged(PartObject);
        }

        private void TrackerWindow_Load(object sender, EventArgs e)
        {

        }
        public void ShowOnDock(DockPanel DockPanel)
        {
            this.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockTop);
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
    }
}
