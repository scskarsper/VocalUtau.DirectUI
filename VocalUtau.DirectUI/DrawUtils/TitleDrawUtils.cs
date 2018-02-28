using BalthasarLib.D2DPainter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI.DrawUtils
{
    public class TitleDrawUtils : DrawUtils
    {
        PianoProperties pprops;
        internal TitleDrawUtils(BalthasarLib.D2DPainter.D2DPaintEventArgs e, RollConfigures rconf, PianoProperties pprops)
            : base(e,rconf)
        {
            this.pprops = pprops;
            baseEvent = e;
        }

        public void DrawXLine(long Tick, Color LineColor, float LineWidth = 2, System.Drawing.Drawing2D.DashStyle LineStyle = System.Drawing.Drawing2D.DashStyle.Solid)
        {
            D2DGraphics g = baseEvent.D2DGraphics;
            long MinTick = pprops.PianoStartTick;
            long MaxTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;
            if (Tick <= MaxTick && Tick >= MinTick)
            {
                long ETick = Tick - MinTick;//获得左边界距离启绘点距离；
                if (ETick >= 0)
                {
                    long NodeXPixel = rconf.Const_RollWidth + baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(ETick), 0);
                    g.DrawLine(new Point((int)NodeXPixel, baseEvent.ClipRectangle.Top), new Point((int)NodeXPixel, baseEvent.ClipRectangle.Bottom), LineColor, LineWidth, LineStyle);
                }
            }

        }
    }
}
