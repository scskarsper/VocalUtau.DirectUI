using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.TrackerUtils
{
    public class TrackerActionView
    {
        public delegate void OnTickPosChangeHandler(long Tick, double Time);
        public event OnTickPosChangeHandler TickPosChange;
        long _TickPos = -1;

        long _PlayTickPos = -1;

        public long PlayTickPos
        {
            get { return _PlayTickPos; }
            set { _PlayTickPos = value; }
        }

        public long TickPos
        {
            get { return _TickPos; }
            set { _TickPos = value; }
        }

        public double BaseTempo
        {
            get
            {
                return ProjectObject.BaseTempo;
            }
            set
            {
                ProjectObject.BaseTempo = value;
            }
        }

        bool HookTracker = false;
        public double TimePos
        {
            get
            {
                return MidiMathUtils.Tick2Time(_TickPos, ProjectObject.BaseTempo);
            }
            set
            {
                _TickPos = MidiMathUtils.Time2Tick(value, ProjectObject.BaseTempo);
            }
        }
        public double PlayTimePos
        {
            get
            {
                return MidiMathUtils.Tick2Time(_PlayTickPos, ProjectObject.BaseTempo);
            }
            set
            {
                _PlayTickPos = MidiMathUtils.Time2Tick(value, ProjectObject.BaseTempo);
            }
        }
        public void KeepTickPosShown(long TickPos)
        {
            if (TickPos < TrackerWindow.MinShownTick || TickPos > TrackerWindow.MaxShownTick)
            {
                TrackerWindow.setPianoStartTick(TickPos);
            }
//        TrackerWindow.MinShownTick || e.Tick > TrackerWindow.MaxShownTick
        }

        IntPtr ProjectObjectPtr = IntPtr.Zero;
        TrackerRollWindow TrackerWindow;

        public TrackerActionView(IntPtr ProjectObjectPtr, TrackerRollWindow TrackerWindow)
        {
            this.TrackerWindow = TrackerWindow;
            this.ProjectObjectPtr = ProjectObjectPtr;
            hookTrackerWindow();
        }
        public void setProjectObjectPtr(IntPtr ProjectObjectPtr)
        {
            this.ProjectObjectPtr = ProjectObjectPtr;
        }

        private ProjectObject ProjectObject
        {
            get
            {
                ProjectObject ret = null;
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(ProjectObjectPtr);
                    ret = (ProjectObject)handle.Target;
                }
                catch { ;}
                return ret;
            }
        }
        private List<TrackerObject> TrackerList
        {
            get
            {
                if (ProjectObject == null) return new List<TrackerObject>();
                return ProjectObject.TrackerList;
            }
        }
        private List<BackerObject> BackerList
        {
            get
            {
                if (ProjectObject == null) return new List<BackerObject>();
                return ProjectObject.BackerList;
            }
        }
        public void hookTrackerWindow()
        {
            try
            {
                TrackerWindow.TTitlePaint += TrackerWindow_TitlePaint;
                TrackerWindow.TPartsPaint += TrackerWindow_PartsPaint;

                TrackerWindow.PartsMouseEnter += TrackerWindow_PartsMouseEnter;
                TrackerWindow.PartsMouseLeave += TrackerWindow_PartsMouseLeave;
                TrackerWindow.PartsMouseMove += TrackerWindow_PartsMouseMove;
                TrackerWindow.KeyDown += TrackerWindow_KeyDown;
                TrackerWindow.ParentForm.MouseLeave += ParentForm_MouseLeave;
                TrackerWindow.MouseLeave += TrackerWindow_MouseLeave;
            }
            catch { ;}
        }

        void TrackerWindow_MouseLeave(object sender, EventArgs e)
        {
            DisableMousePost();
        }

        void TrackerWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (HookTracker)
            {
                if (Control.ModifierKeys == Keys.Control)
                {
                    if (e.KeyCode == Keys.G)
                    {
                        SetupMouseTick();
                    }
                }
            }
        }

        void TrackerWindow_PartsMouseMove(object sender, Models.TrackerMouseEventArgs e)
        {
         //   this.TrackerWindow.ParentForm.Activate();
            this.TrackerWindow.Focus();
            if (e.Tick < TrackerWindow.MinShownTick || e.Tick > TrackerWindow.MaxShownTick)
            {
                HookTracker = false;
                DisableMousePost();
            }
            else
            {
                HookTracker = true;
            }
            MouseTick = e.Tick;
        }

        void TrackerWindow_PartsMouseLeave(object sender, EventArgs e)
        {
            HookTracker = false;
            DisableMousePost();
        }

        void TrackerWindow_PartsMouseEnter(object sender, EventArgs e)
        {
            HookTracker = true;
        }



        void ParentForm_MouseLeave(object sender, EventArgs e)
        {
            DisableMousePost();
        }

        void DisableMousePost()
        {
            if (ShownMousePost)
            {
                ShownMousePost = false;
                HookTracker = false;
                TrackerWindow.RedrawPiano();
            }
        }
        long MouseTick = 0;

        public void SetupMouseTick()
        {
            if (TickPos != MouseTick)
            {
                TickPos = MouseTick;
                try
                {
                    TrackerWindow.RedrawPiano();
                }
                catch { ;}
                if (TickPosChange != null) TickPosChange(TickPos, TimePos);
            }
        }

        public void RealarmTickPosition()
        {
            if (TickPosChange != null) TickPosChange(TickPos, TimePos);
        }

        Color CurPost = Color.LightBlue;
        Color MousePost = Color.LightPink;
        Color PlayPost = Color.LightGreen;
        bool ShownMousePost = false;
        void TrackerWindow_TitlePaint(object sender, DrawUtils.TrackerTitlesDrawUtils utils)
        {
            try
            {
                if (_TickPos >= TrackerWindow.MinShownTick && _TickPos <= TrackerWindow.MaxShownTick)
                {
                    utils.DrawXLine(_TickPos, CurPost);
                }
                if (_PlayTickPos >= TrackerWindow.MinShownTick && _PlayTickPos <= TrackerWindow.MaxShownTick)
                {
                    utils.DrawXLine(_PlayTickPos, PlayPost);
                }
                if (HookTracker && !TrackerWindow.DisableMouse)
                {
                    if (MouseTick >= TrackerWindow.MinShownTick && MouseTick <= TrackerWindow.MaxShownTick)
                    {
                        ShownMousePost = true;
                        utils.DrawXLine(MouseTick, MousePost);
                    }
                }
            }
            catch { ;}
        }

        void TrackerWindow_PartsPaint(object sender, DrawUtils.TrackerPartsDrawUtils utils)
        {
            try
            {
                if (_TickPos >= TrackerWindow.MinShownTick && _TickPos <= TrackerWindow.MaxShownTick)
                {
                    utils.DrawXLine(_TickPos, CurPost);
                }
                if (_PlayTickPos >= TrackerWindow.MinShownTick && _PlayTickPos <= TrackerWindow.MaxShownTick)
                {
                    utils.DrawXLine(_PlayTickPos, PlayPost);
                }
                if (HookTracker && !TrackerWindow.DisableMouse)
                {
                    if (MouseTick >= TrackerWindow.MinShownTick && MouseTick <= TrackerWindow.MaxShownTick)
                    {
                        ShownMousePost = true;
                        utils.DrawXLine(MouseTick, MousePost);
                    }
                }
            }
            catch { ;}
        }

    }
}
