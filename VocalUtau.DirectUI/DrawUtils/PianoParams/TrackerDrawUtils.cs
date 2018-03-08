using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI.DrawUtils
{
    public class TrackerDrawUtils
    {
        internal BalthasarLib.D2DPainter.D2DPaintEventArgs baseEvent;
        public BalthasarLib.D2DPainter.D2DPaintEventArgs D2DPaintEventArgs { get { return baseEvent; } set { baseEvent = value; } }
        internal TrackerConfigures rconf;
        internal TrackerDrawUtils(BalthasarLib.D2DPainter.D2DPaintEventArgs e, TrackerConfigures rconf)
        {
            baseEvent = e;
            this.rconf=rconf;
        }
    }
}
