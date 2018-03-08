using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.PianoUtils
{
    public class ActionView
    {
        long _TickPos = -1;

        public long TickPos
        {
            get { return _TickPos; }
            set { _TickPos = value; }
        }

        public double TracksTime
        {
            set
            {
                long TTick = MidiMathUtils.Time2Tick(value, PartsObject.Tempo);
                long AST=PartsObject.AbsoluteStartTick;
                if (AST >= TTick && TTick <= AST + PartsObject.TickLength)
                {
                    _TickPos = AST - TTick;
                }
                else
                {
                    _TickPos = -1;
                }
            }
        }

        PianoRollWindow PianoWindow;
        ParamCurveWindow ParamWindow;

        IntPtr PartsObjectPtr = IntPtr.Zero;
        public ActionView(IntPtr PartsObjectPtr, PianoRollWindow PianoWindow, ParamCurveWindow ParamWindow)
        {
            this.PartsObjectPtr = PartsObjectPtr;
            this.PianoWindow = PianoWindow;
            this.ParamWindow = ParamWindow;
            hookPianoWindow();
        }
        public void setPartsObjectPtr(IntPtr PartsObjectPtr)
        {
            this.PartsObjectPtr = PartsObjectPtr;
        }
        public void setPianoWindowPtr(PianoRollWindow PianoWindow)
        {
            this.PianoWindow = PianoWindow;
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
        public void hookPianoWindow()
        {
            try
            {
                PianoWindow.TrackPaint += PianoWindow_TrackPaint;
                PianoWindow.TitlePaint += PianoWindow_TitlePaint;
                PianoWindow.TrackMouseEnter += PianoWindow_TrackMouseEnter;
                PianoWindow.TrackMouseLeave += PianoWindow_TrackMouseLeave;
                PianoWindow.TrackMouseMove += PianoWindow_TrackMouseMove;
                PianoWindow.KeyDown += PianoWindow_KeyDown;
                PianoWindow.ParentForm.MouseLeave += ParentForm_MouseLeave;
            }
            catch { ;}
            try
            {
                ParamWindow.ParamAreaPaint += ParamWindow_ParamAreaPaint;
                ParamWindow.ParamAreaMouseEnter += ParamWindow_ParamAreaMouseEnter;
                ParamWindow.ParamAreaMouseLeave += ParamWindow_ParamAreaMouseLeave;
                ParamWindow.ParamAreaMouseMove += ParamWindow_ParamAreaMouseMove;
                ParamWindow.KeyDown += ParamWindow_KeyDown;
            }
            catch { ;}
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
                PianoWindow.RedrawPiano();
                ParamWindow.RedrawPiano();
            }
        }
        void ParamWindow_ParamAreaMouseMove(object sender, ParamMouseEventArgs e)
        {
            HookPiano = false;
            if (e.Tick < PianoWindow.MinShownTick || e.Tick > PianoWindow.MaxShownTick)
            {
                HookParam = false;
                DisableMousePost();
            }
            else
            {
                HookParam = true;
            }
            MouseTick = e.Tick;
        }
        long MouseTick = 0;
        void PianoWindow_TrackMouseMove(object sender, PianoMouseEventArgs e)
        {
            HookParam = false; 
            if (e.Tick < PianoWindow.MinShownTick || e.Tick > PianoWindow.MaxShownTick)
            {
                HookPiano = false;
                DisableMousePost();
            }
            else
            {
                HookPiano = true;
            }
            MouseTick = e.Tick;
        }

        void ParamWindow_ParamAreaMouseLeave(object sender, EventArgs e)
        {
            HookParam = false;
            if (!HookPiano)
                DisableMousePost();
        }

        void ParamWindow_ParamAreaMouseEnter(object sender, EventArgs e)
        {
            HookParam = true;
        }
        bool HookPiano = false;
        bool HookParam = false;
        void PianoWindow_TrackMouseLeave(object sender, EventArgs e)
        {
            HookPiano = false;
            if (!HookParam)
                DisableMousePost();
        }

        void PianoWindow_TrackMouseEnter(object sender, EventArgs e)
        {
            HookPiano = true;
        }


        void ParamWindow_KeyDown(object sender, KeyEventArgs e)
        {
            PianoWindow_KeyDown(null, e);
        }
        public void SetupMouseTick()
        {
            if (TickPos != MouseTick)
            {
                TickPos = MouseTick;
                try
                {
                    PianoWindow.RedrawPiano();
                }
                catch { ;}
                try
                {
                    ParamWindow.RedrawPiano();
                }
                catch { ;}
            }
        }
        void PianoWindow_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (HookParam || HookPiano)
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
        Color CurPost = Color.LightBlue;
        Color MousePost = Color.LightPink;
        bool ShownMousePost = false;
        void ParamWindow_ParamAreaPaint(object sender, DrawUtils.ParamAreaDrawUtils utils)
        {
            if (_TickPos >= ParamWindow.MinShownTick && _TickPos <= ParamWindow.MaxShownTick)
            {
                utils.DrawXLine(_TickPos, CurPost);
            }
            if (HookParam || HookPiano)
            {
                if (MouseTick >= ParamWindow.MinShownTick && MouseTick <= ParamWindow.MaxShownTick)
                {
                    ShownMousePost = true;
                    utils.DrawXLine(MouseTick, MousePost);
                    if (HookParam) PianoWindow.RedrawPiano();
                }
            }
        }

        void PianoWindow_TitlePaint(object sender, DrawUtils.TitleDrawUtils utils)
        {
            if (_TickPos >= ParamWindow.MinShownTick && _TickPos <= ParamWindow.MaxShownTick)
            {
                utils.DrawXLine(_TickPos, CurPost);
            }
            if (HookParam || HookPiano)
            {
                if (MouseTick >= ParamWindow.MinShownTick && MouseTick <= ParamWindow.MaxShownTick)
                {
                    ShownMousePost = true;
                    utils.DrawXLine(MouseTick, MousePost);
                    if (HookPiano) ParamWindow.RedrawPiano();
                }
            }
        }

        void PianoWindow_TrackPaint(object sender, DrawUtils.TrackDrawUtils utils)
        {
            if (_TickPos >= PianoWindow.MinShownTick && _TickPos <= PianoWindow.MaxShownTick)
            {
                utils.DrawXLine(_TickPos, CurPost);
            }
            if (HookParam || HookPiano)
            {
                if (MouseTick >= PianoWindow.MinShownTick && MouseTick <= PianoWindow.MaxShownTick)
                {
                    ShownMousePost = true;
                    utils.DrawXLine(MouseTick, MousePost);
                    if (HookPiano) ParamWindow.RedrawPiano();
                }
            }
            utils.DrawString(new Point(10, 30), Color.FromArgb(60, 0, 0, 0), "Tempo:"+PartsObject.Tempo.ToString(), 15, FontStyle.Bold);
        }
    }
}
