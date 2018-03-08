using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI.DrawUtils
{
    public class PianoDrawUtils
    {
        internal BalthasarLib.D2DPainter.D2DPaintEventArgs baseEvent;
        public BalthasarLib.D2DPainter.D2DPaintEventArgs D2DPaintEventArgs { get { return baseEvent; } set { baseEvent = value; } }
        internal RollConfigures rconf;
        internal PianoDrawUtils(BalthasarLib.D2DPainter.D2DPaintEventArgs e, RollConfigures rconf)
        {
            baseEvent = e;
            this.rconf=rconf;
        }
    }
}
