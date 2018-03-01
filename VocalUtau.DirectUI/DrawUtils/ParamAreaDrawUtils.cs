﻿using BalthasarLib.D2DPainter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.DrawUtils
{
    public class ParamAreaDrawUtils : DrawUtils
    {
        PianoProperties pprops;
        BalthasarLib.D2DPainter.D2DPaintEventArgs D2DArgs;
        internal ParamAreaDrawUtils(BalthasarLib.D2DPainter.D2DPaintEventArgs e, RollConfigures rconf, PianoProperties pprops)
            : base(e,rconf)
        {
            
            this.pprops = pprops;
            this.D2DArgs = e;
        }

        public Rectangle ClipRectangle
        {
            get
            {
                return baseEvent.ClipRectangle;
            }
        }

        private Point Tick2Point(long tick, SortedDictionary<long, double> DPair, long MinTick, long MaxTick)
        {
            long ETick = tick - MinTick;//获得左边界距离启绘点距离；
            int NodeXPixel = baseEvent.ClipRectangle.X;      
            if (ETick >= 0)
            {
                long draw_pixel = (long)Math.Round(pprops.dertTick2dertPixel(ETick), 0);
                NodeXPixel = baseEvent.ClipRectangle.X + (int)draw_pixel;
            }

            int NodeYPixel =  (int)((1 - DPair[tick]) * D2DArgs.ClipRectangle.Height);
            if (NodeYPixel < 0) NodeYPixel = 0;
            return new Point(NodeXPixel, NodeYPixel);
           // return new Point((int)(NodeXPixel*1.265), NodeYPixel);
        }




        private int Tick2PixelX(long Tick, long MinTick, long MaxTick)
        {
            long ETick = Tick - MinTick;//获得左边界距离启绘点距离；
            int NodeXPixel = baseEvent.ClipRectangle.X;
            if (ETick < 0)
            {
                //起绘制点小于0;
                NodeXPixel = baseEvent.ClipRectangle.X - (int)Math.Round(pprops.dertTick2dertPixel(-ETick), 0);
            }
            else
            {
                NodeXPixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(ETick), 0);
            }

            return NodeXPixel;
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
                    long NodeXPixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(ETick), 0);
                    g.DrawLine(new Point((int)NodeXPixel, baseEvent.ClipRectangle.Top), new Point((int)NodeXPixel, baseEvent.ClipRectangle.Bottom), LineColor, LineWidth, LineStyle);
                }
            }

        }
        public void DrawYLine(double Percent, Color LineColor, float LineWidth = 1, System.Drawing.Drawing2D.DashStyle LineStyle = System.Drawing.Drawing2D.DashStyle.Solid)
        {
            if (Percent > 1) Percent = 1;
            if (Percent < 0) Percent = 0;
            D2DGraphics g = baseEvent.D2DGraphics;
            double rp = 1 - Percent;
            int PixelY = (int)(baseEvent.ClipRectangle.Height * rp);
            g.DrawLine(new Point(rconf.Const_RollWidth, PixelY), new Point(rconf.Const_RollWidth + baseEvent.ClipRectangle.Width, PixelY), LineColor, LineWidth, LineStyle); 
        }
        public void DrawPitchLine(List<PitchObject> SortedPitchPointSilk, double MaxValue, Color LineColor, float LineWidth = 1, System.Drawing.Drawing2D.DashStyle LineStyle = System.Drawing.Drawing2D.DashStyle.Solid, ulong AntiBordTick = 480)
        {
            D2DGraphics g = baseEvent.D2DGraphics;

            long MinTick = pprops.PianoStartTick;
            long MaxTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;

            int BaseZero = baseEvent.ClipRectangle.Height / 2;

            List<Point> PixelSilkLine = new List<Point>();

            for (int i = 0; i < SortedPitchPointSilk.Count; i++)
            {
                if (SortedPitchPointSilk[i].Tick >= MinTick - (long)AntiBordTick && SortedPitchPointSilk[i].Tick <= MaxTick + (long)AntiBordTick)
                {
                    double bfb = (double)SortedPitchPointSilk[i].PitchValue.PitchValue / (double)MaxValue;
                    int CalcY = (int)(bfb * BaseZero);
                    int PointY = BaseZero - CalcY;
                    int PointX = Tick2PixelX(SortedPitchPointSilk[i].Tick, MinTick, MaxTick);
                    PixelSilkLine.Add(new Point(PointX, PointY));
                }
            }

            if (PixelSilkLine.Count > 1) g.DrawPathGeometrySink(PixelSilkLine, LineColor, LineWidth, LineStyle, false);
        }
        public void FillPitchLine(List<PitchObject> SortedPitchPointSilk, double MaxValue, Color AreaColor, ulong AntiBordTick = 480)
        {
            D2DGraphics g = baseEvent.D2DGraphics;

            long MinTick = pprops.PianoStartTick;
            long MaxTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;

            int BaseZero = baseEvent.ClipRectangle.Height/2;
            
            List<Point> PixelSilkLine = new List<Point>();

            bool isFirst = true;
            Point PE = new Point(rconf.Const_RollWidth + baseEvent.ClipRectangle.Width, baseEvent.ClipRectangle.Height);
            for (int i = 0; i < SortedPitchPointSilk.Count; i++)
            {
                if (SortedPitchPointSilk[i].Tick >= MinTick - (long)AntiBordTick && SortedPitchPointSilk[i].Tick <= MaxTick + (long)AntiBordTick)
                {
                    double bfb = (double)SortedPitchPointSilk[i].PitchValue.PitchValue / (double)MaxValue;
                    int CalcY = (int)(bfb * BaseZero);
                    int PointY = BaseZero - CalcY;
                    int PointX = Tick2PixelX(SortedPitchPointSilk[i].Tick, MinTick, MaxTick);
                    if (isFirst)
                    {
                        PixelSilkLine.Add(new Point(PointX, baseEvent.ClipRectangle.Height));
                        isFirst = false;
                    }
                    PixelSilkLine.Add(new Point(PointX, PointY));
                    PE = new Point(PointX, baseEvent.ClipRectangle.Height);
                }
            }
            PixelSilkLine.Add(PE);

            if (PixelSilkLine.Count > 1) g.FillPathGeometrySink(PixelSilkLine, AreaColor);
        }
        public void FillSelect(long Tick1, long Tick2, Color AreaColor)
        {
            long MinSTick = Math.Min(Tick1, Tick2);
            long MaxSTick = Math.Max(Tick1, Tick2);

            long MinTick = pprops.PianoStartTick;
            long MaxTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;

            int PX1=Tick2PixelX(MinSTick,MinTick,MaxTick);
            int PX2 = Tick2PixelX(MaxSTick, MinTick, MaxTick);

            List<Point> PArr = new List<Point>();
            PArr.Add(new Point(PX1, 0));
            PArr.Add(new Point(PX2, 0));
            PArr.Add(new Point(PX2, baseEvent.ClipRectangle.Height));
            PArr.Add(new Point(PX1, baseEvent.ClipRectangle.Height));

            D2DGraphics g = baseEvent.D2DGraphics;
            g.FillPathGeometrySink(PArr, AreaColor);
        }
        public void DrawHillLine(SortedDictionary<long, double> SortedPitchPointSilk, Color AreaColor)
        {
            //计算X相位边界
            long MinTick = pprops.PianoStartTick;
            long MaxTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;
            
            List<Point> PixelSilkLine = new List<Point>();
           
            long[] KeyArr=SortedPitchPointSilk.Keys.ToArray();

            
            Point PS = Tick2Point(KeyArr[0], SortedPitchPointSilk, MinTick, MaxTick);
            PS.Y=0;
            Point PE = Tick2Point(KeyArr[KeyArr.Length-1], SortedPitchPointSilk, MinTick, MaxTick);
            PE.Y = baseEvent.ClipRectangle.Height;
            Rectangle rb = new Rectangle(PS.X, PS.Y, PE.X - PS.X, PE.Y - PS.Y);
            PS.Y = baseEvent.ClipRectangle.Height;

            D2DGraphics g = baseEvent.D2DGraphics;
            g.FillRectangle(rb, Color.Black);

            PixelSilkLine.Add(PS);

            for (int i = 0; i < KeyArr.Length; i++)
            {
                if (KeyArr[i] > MinTick && KeyArr[i] < MaxTick)
                {
                    Point P = Tick2Point(KeyArr[i], SortedPitchPointSilk, MinTick, MaxTick);
                    PixelSilkLine.Add(P);
                }
            }
            PixelSilkLine.Add(PE);

            if (PixelSilkLine.Count > 1) g.FillPathGeometrySink(PixelSilkLine, AreaColor);
        }
        
    }
}
