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
                    Track_View.setProjectObjectPtr(ObjectPtr);
                }
                else
                {
                    Track_View = new TrackerView(ObjectPtr, this.TrackerWindow);
                    Track_View.TrackerActionBegin += Track_View_TrackerActionBegin;
                    Track_View.TrackerActionEnd += Track_View_TrackerActionEnd;
                    Track_View.HandleEvents = true;
                    Alloced = true;

                }
                try
                {
                    this.TrackerWindow.RedrawPiano();
                }
                catch { ;}
            }

            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnPartsEventHandler TrackerActionEnd;
            public event VocalUtau.DirectUI.Utils.TrackerUtils.TrackerView.OnPartsEventHandler TrackerActionBegin;
            void Track_View_TrackerActionEnd(TrackerView.PartsDragingType eventType)
            {
                if (TrackerActionEnd != null) TrackerActionEnd(eventType);
            }

            void Track_View_TrackerActionBegin(TrackerView.PartsDragingType eventType)
            {
                if (TrackerActionBegin != null) TrackerActionBegin(eventType);
            }

            #region
            TrackerView Track_View;
            #endregion

            #region
            #endregion


        }

        ViewController Controller;
        ObjectAlloc<ProjectObject> OAC = new ObjectAlloc<ProjectObject>();
        public TrackerWindow()
        {
            InitializeComponent();
            Controller = new ViewController(ref this.trackerRollWindow1);
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
    }
}
