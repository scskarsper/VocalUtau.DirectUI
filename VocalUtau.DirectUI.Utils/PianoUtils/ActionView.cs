using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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

        IntPtr PartsObjectPtr = IntPtr.Zero;
        public ActionView(IntPtr PartsObjectPtr, PianoRollWindow PianoWindow)
        {
            this.PartsObjectPtr = PartsObjectPtr;
            this.PianoWindow = PianoWindow;
            hookPianoWindow();
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
            PianoWindow.TrackPaint += PianoWindow_TrackPaint;
            PianoWindow.TitlePaint += PianoWindow_TitlePaint;
        }

        void PianoWindow_TitlePaint(object sender, DrawUtils.TitleDrawUtils utils)
        {
            if (_TickPos >= PianoWindow.MinShownTick && _TickPos<=PianoWindow.MaxShownTick)
            {
                utils.DrawXLine(_TickPos, Color.LightBlue);
            }
        }

        void PianoWindow_TrackPaint(object sender, DrawUtils.TrackDrawUtils utils)
        {
            if (_TickPos >= PianoWindow.MinShownTick && _TickPos <= PianoWindow.MaxShownTick)
            {
                utils.DrawXLine(_TickPos, Color.LightBlue);
            }
        }
    }
}
