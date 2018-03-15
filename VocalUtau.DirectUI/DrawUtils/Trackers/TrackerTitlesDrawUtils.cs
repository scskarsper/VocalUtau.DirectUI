using BalthasarLib.D2DPainter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI.DrawUtils
{
    public class TrackerTitlesDrawUtils : TrackerDrawUtils
    {
        TrackerProperties pprops;
        internal TrackerTitlesDrawUtils(BalthasarLib.D2DPainter.D2DPaintEventArgs e, TrackerConfigures rconf,TrackerProperties pprops)
            : base(e,rconf)
        {
            baseEvent = e;
            this.pprops = pprops;
        }

        public Rectangle ClipRectangle
        {
            get
            {
                return baseEvent.ClipRectangle;
            }
        }

        public void DrawString(System.Drawing.Point LeftTopAxis, System.Drawing.Color FontColor, string Text, int FontSize = 9, FontStyle FontStyles = FontStyle.Regular)
        {
            D2DGraphics g = baseEvent.D2DGraphics;
            g.DrawText(Text, new System.Drawing.Rectangle(LeftTopAxis.X, LeftTopAxis.Y, baseEvent.ClipRectangle.Width - LeftTopAxis.X, baseEvent.ClipRectangle.Height - LeftTopAxis.Y), FontColor, new System.Drawing.Font("Tahoma", FontSize, FontStyles));
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
                    long NodeXPixel = rconf.Const_GridWidth+ baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(ETick), 0);
                    g.DrawLine(new Point((int)NodeXPixel, baseEvent.ClipRectangle.Top), new Point((int)NodeXPixel, baseEvent.ClipRectangle.Bottom), LineColor, LineWidth, LineStyle);
                }
            }

        }
    }
}
