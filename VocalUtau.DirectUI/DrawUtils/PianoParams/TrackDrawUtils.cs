﻿using BalthasarLib.D2DPainter;
using VocalUtau.DirectUI.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.DrawUtils
{
    public class TrackDrawUtils : PianoDrawUtils
    {
        PianoProperties pprops;
        internal TrackDrawUtils(BalthasarLib.D2DPainter.D2DPaintEventArgs e, RollConfigures rconf, PianoProperties pprops)
            : base(e,rconf)
        {
            this.pprops = pprops;
        }

        public Rectangle ClipRectangle
        {
            get
            {
                return baseEvent.ClipRectangle;
            }
        }

        public void DrawPianoMouseAxis(Color LineColor, float LineWidth)
        {
            DrawPianoMouseAxis(LineColor, LineWidth, System.Drawing.Drawing2D.DashStyle.Dash);
        }
        public void DrawPianoMouseAxis(Color LineColor, float LineWidth, System.Drawing.Drawing2D.DashStyle LineStyle)
        {
            D2DGraphics g = baseEvent.D2DGraphics;
            //横线
            Point L1_p1 = new Point(baseEvent.ClipRectangle.Left, baseEvent.MousePoint.Y);
            Point L1_p2 = new Point(baseEvent.ClipRectangle.Left+baseEvent.ClipRectangle.Width, baseEvent.MousePoint.Y);
            //竖线
            Point L2_p1 = new Point(baseEvent.MousePoint.X, baseEvent.ClipRectangle.Top);
            Point L2_p2 = new Point(baseEvent.MousePoint.X, baseEvent.ClipRectangle.Top+baseEvent.ClipRectangle.Height);
            g.DrawLine(L1_p1, L1_p2, LineColor,LineWidth,LineStyle);
            g.DrawLine(L2_p1, L2_p2, LineColor, LineWidth, LineStyle);
        }

        public void DrawXLine(long Tick,Color LineColor,float LineWidth=2, System.Drawing.Drawing2D.DashStyle LineStyle=System.Drawing.Drawing2D.DashStyle.Solid)
        {
            D2DGraphics g = baseEvent.D2DGraphics;
            long MinTick = pprops.PianoStartTick;
            long MaxTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;
            if (Tick <= MaxTick && Tick>=MinTick)
            {
                long ETick = Tick - MinTick;//获得左边界距离启绘点距离；
                if (ETick >= 0)
                {
                    long NodeXPixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(ETick), 0);
                    g.DrawLine(new Point((int)NodeXPixel, baseEvent.ClipRectangle.Top), new Point((int)NodeXPixel, baseEvent.ClipRectangle.Bottom), LineColor, LineWidth, LineStyle);
                }
            }

        }

        public void DrawMask(long LeftTick, long RightTick)
        {
            long LeftRectangleTick = pprops.PianoStartTick;
            long RightRectangleTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;
            if (LeftTick < LeftRectangleTick) LeftTick = LeftRectangleTick;
            if (RightTick > RightRectangleTick) RightTick = RightRectangleTick;
            long StartTick = LeftTick - LeftRectangleTick;//获得左边界距离启绘点距离；
            long EndTick = RightTick - LeftRectangleTick;//获得右边界距离启绘点距离；


            int X1 = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(StartTick), 0);
            int X2 = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);

            Rectangle Rect = new Rectangle(
                new Point(X1, baseEvent.ClipRectangle.Top),
                new Size(X2 - X1, baseEvent.ClipRectangle.Height)
                );
            baseEvent.D2DGraphics.DrawLine(new Point(X1, baseEvent.ClipRectangle.Top), new Point(X1, baseEvent.ClipRectangle.Bottom), Color.FromArgb(90, Color.Black));
            baseEvent.D2DGraphics.FillRectangle(Rect, Color.FromArgb(70,Color.Black));
        }

        public void DrawNote(NoteObject Note, Color NoteColor)
        {
            DrawNote(Note, NoteColor, Color.Black);
        }
        public void DrawNote(NoteObject Note, Color NoteColor, Color LyricColor)
        {
            //判断X相位是否抛弃
            long LeftTick = Note.Tick;
            long RightTick = Note.Tick + Note.Length;

            long LeftRectangleTick = pprops.PianoStartTick;
            long RightRectangleTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;

            if (RightTick <= LeftRectangleTick || LeftTick>=RightRectangleTick)
            {
                //抛弃音符（超界）
                return;
            }

            //判断Y相位是否抛弃
            double MaxNoteCount = (double)baseEvent.ClipRectangle.Height / rconf.Const_RollNoteHeight;
            if (MaxNoteCount > (int)MaxNoteCount) MaxNoteCount = (int)MaxNoteCount+1;
            uint MaxNote = pprops.PianoTopNote;
            uint MinNote = MaxNote - (uint)MaxNoteCount;

            if (Note.PitchValue.NoteNumber < MinNote || Note.PitchValue.NoteNumber > MaxNote)
            {
                //抛弃音符（超界）
                return;
            }

            //计算X坐标

            long StartTick = LeftTick - LeftRectangleTick;//获得左边界距离启绘点距离；
            long EndTick = RightTick - LeftRectangleTick;//获得右边界距离启绘点距离；
            int NoteX1Pixel = baseEvent.ClipRectangle.X;
            int NoteX2Pixel = NoteX1Pixel;
            if (StartTick < 0)
            {
                //起绘制点小于0;
                NoteX1Pixel=baseEvent.ClipRectangle.X - (int)Math.Round(pprops.dertTick2dertPixel(-StartTick),0);
                NoteX2Pixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);
            }
            else
            {
                NoteX1Pixel = NoteX1Pixel + (int)Math.Round(pprops.dertTick2dertPixel(StartTick), 0);
                NoteX2Pixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);
            }

            //计算Y坐标
            uint NoteDistance = MaxNote - Note.PitchValue.NoteNumber;
            int NoteYPixel = baseEvent.ClipRectangle.Top + (int)NoteDistance * rconf.Const_RollNoteHeight;

            double verb = Note.VerbPrecent;

            Rectangle NoteRect=new Rectangle(
                new Point(NoteX1Pixel, NoteYPixel),
                new Size(NoteX2Pixel - NoteX1Pixel, rconf.Const_RollNoteHeight)
                );

            Rectangle NoteVerbRect = new Rectangle(
                new Point((int)((NoteX2Pixel - NoteX1Pixel) * (1 - verb)) + NoteX1Pixel, NoteYPixel),
                new Size((int)((NoteX2Pixel - NoteX1Pixel)*(verb)), rconf.Const_RollNoteHeight)
                );

            Rectangle LyricRect = new Rectangle(
                new Point(NoteX1Pixel+5, NoteYPixel+1),
                new Size(NoteX2Pixel - NoteX1Pixel-5, rconf.Const_RollNoteHeight-1)
                );

            D2DGraphics g = baseEvent.D2DGraphics;
            g.FillRectangle(NoteRect, NoteColor);
            g.FillRectangle(NoteVerbRect, Color.FromArgb(40,Color.Black));
            try
            {
                g.DrawText(Note.Lyric, LyricRect, LyricColor, new System.Drawing.Font("Tahoma", 9));
            }
            catch { ;}
            g.DrawRectangle(NoteRect, rconf.RollColor_NoteBorderColor);
        }

        public void DrawDia(long TickStart, long TickEnd, uint TopNoteNum, uint BottomNoteNum)
        {
            //判断X相位是否抛弃
            long LeftTick = TickStart;
            long RightTick = TickEnd;

            long LeftRectangleTick = pprops.PianoStartTick;
            long RightRectangleTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;

            if (RightTick <= LeftRectangleTick || LeftTick >= RightRectangleTick)
            {
                //抛弃音符（超界）
                return;
            }

            //判断Y相位是否抛弃
            double MaxNoteCount = (double)baseEvent.ClipRectangle.Height / rconf.Const_RollNoteHeight;
            if (MaxNoteCount > (int)MaxNoteCount) MaxNoteCount = (int)MaxNoteCount + 1;
            uint MaxNote = pprops.PianoTopNote;
            uint MinNote = MaxNote - (uint)MaxNoteCount;

            if (TopNoteNum < MinNote) TopNoteNum = MinNote; else if (TopNoteNum > MaxNote) TopNoteNum = MaxNote;
            if (BottomNoteNum < MinNote) BottomNoteNum = MinNote; else if (BottomNoteNum > MaxNote) BottomNoteNum = MaxNote;

            //计算X坐标

            long StartTick = LeftTick - LeftRectangleTick;//获得左边界距离启绘点距离；
            long EndTick = RightTick - LeftRectangleTick;//获得右边界距离启绘点距离；
            int NoteX1Pixel = baseEvent.ClipRectangle.X;
            int NoteX2Pixel = NoteX1Pixel;
            if (StartTick < 0)
            {
                //起绘制点小于0;
                NoteX1Pixel = baseEvent.ClipRectangle.X - (int)Math.Round(pprops.dertTick2dertPixel(-StartTick), 0);
                NoteX2Pixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);
            }
            else
            {
                NoteX1Pixel = NoteX1Pixel + (int)Math.Round(pprops.dertTick2dertPixel(StartTick), 0);
                NoteX2Pixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);
            }

            //计算Y坐标
            uint NoteDistance = MaxNote - TopNoteNum;
            int NoteYTopPixel = baseEvent.ClipRectangle.Top + (int)NoteDistance * rconf.Const_RollNoteHeight;
            NoteDistance = MaxNote - BottomNoteNum;
            int NoteYBtmPixel = baseEvent.ClipRectangle.Top + (int)NoteDistance * rconf.Const_RollNoteHeight;
            Rectangle DiaRect = new Rectangle(
                new Point(NoteX1Pixel, NoteYTopPixel),
                new Size(NoteX2Pixel - NoteX1Pixel, NoteYBtmPixel - NoteYTopPixel + rconf.Const_RollNoteHeight)
                );
            D2DGraphics g = baseEvent.D2DGraphics;
            g.DrawRectangle(DiaRect, rconf.RollColor_NoteBorderColor);
        }

        public void DrawPitchLine(List<PitchObject> SortedPitchPointSilk, Color LineColor)
        {
            DrawPitchLine(SortedPitchPointSilk, LineColor, 1);
        }
        public void DrawPitchLine(List<PitchObject> SortedPitchPointSilk, Color LineColor, float LineWidth)
        {
            DrawPitchLine(SortedPitchPointSilk, LineColor, LineWidth, System.Drawing.Drawing2D.DashStyle.Solid);
        }

        public void DrawString(System.Drawing.Point LeftTopAxis, System.Drawing.Color FontColor, string Text, int FontSize = 9, FontStyle FontStyles = FontStyle.Regular)
        {

            try
            {
                D2DGraphics g = baseEvent.D2DGraphics;
                g.DrawText(Text, new System.Drawing.Rectangle(baseEvent.ClipRectangle.Left + LeftTopAxis.X, LeftTopAxis.Y, baseEvent.ClipRectangle.Width - LeftTopAxis.X, baseEvent.ClipRectangle.Height - LeftTopAxis.Y), FontColor, new System.Drawing.Font("Tahoma", FontSize, FontStyles));
            }
            catch { ;}
        }

        private Point PitchObject2Point(PitchObject Node,long MinTick,long MaxTick,uint MinNote,uint MaxNote)
        {
            long ETick = Node.Tick - MinTick;//获得左边界距离启绘点距离；
            int NodeXPixel = baseEvent.ClipRectangle.X;
            if (ETick < 0)
            {
                //起绘制点小于0;
                NodeXPixel=baseEvent.ClipRectangle.X - (int)Math.Round(pprops.dertTick2dertPixel(-ETick),0);
            }
            else
            {
                NodeXPixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(ETick), 0);
            }

            uint NoteDistance = MaxNote - Node.PitchValue.NoteNumber;
            double PitchDistance=(double)Node.PitchValue.PitchValue-Node.PitchValue.NoteNumber;
            int NodeYPixel=baseEvent.ClipRectangle.Top +  (int)(((double)NoteDistance-PitchDistance+0.5) * rconf.Const_RollNoteHeight);
            return new Point(NodeXPixel,NodeYPixel);
        }
        public void DrawPitchLine(List<PitchObject> SortedPitchPointSilk, Color LineColor, float LineWidth, System.Drawing.Drawing2D.DashStyle LineStyle)
        {
            //计算X相位边界
            long MinTick = pprops.PianoStartTick;
            long MaxTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;
            //计算Y相位边界
            double MaxNoteCount = (double)baseEvent.ClipRectangle.Height / rconf.Const_RollNoteHeight;
            if (MaxNoteCount > (int)MaxNoteCount) MaxNoteCount = (int)MaxNoteCount + 1;
            uint MaxNote = pprops.PianoTopNote;
            uint MinNote = MaxNote - (uint)MaxNoteCount;

            List<Point> PixelSilkLine = new List<Point>();
            bool First = true;
            for (int i = 1; i < SortedPitchPointSilk.Count; i++)
            {
                if (SortedPitchPointSilk[i].Tick > MinTick && SortedPitchPointSilk[i - 1].Tick < MaxTick)
                {
                    PitchObject pn = SortedPitchPointSilk[i];
                    PitchObject pn2 = SortedPitchPointSilk[i-1];
                    if (First)
                    {
                        Point StartP = PitchObject2Point(pn2, MinTick, MaxTick, MinNote, MaxNote);
                        PixelSilkLine.Add(StartP);
                        First = false;
                    }
                    Point EndP = PitchObject2Point(pn, MinTick, MaxTick, MinNote, MaxNote);
                    PixelSilkLine.Add(EndP);
                }
            }

            D2DGraphics g = baseEvent.D2DGraphics;
                if (PixelSilkLine.Count > 1) g.DrawPathGeometrySink(PixelSilkLine, LineColor, LineWidth, LineStyle, false);
        }


#region
        /*抛弃的代码段
        public void DrawPitchLine_droped(List<PitchObject> SortedPitchPointSilk,Color LineColor, float LineWidth, System.Drawing.Drawing2D.DashStyle LineStyle)
        {
            //计算X相位边界
            long MinTick = pprops.PianoStartTick;
            long MaxTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;
            //计算Y相位边界
            double MaxNoteCount = (double)baseEvent.ClipRectangle.Height / rconf.Const_RollNoteHeight;
            if (MaxNoteCount > (int)MaxNoteCount) MaxNoteCount = (int)MaxNoteCount + 1;
            uint MaxNote = pprops.PianoTopNote;
            uint MinNote = MaxNote - (uint)MaxNoteCount;

            //点群转线
            Dictionary<PitchObject[], int> PitchSegments = new Dictionary<PitchObject[], int>();
            for (int i = 1; i < SortedPitchPointSilk.Count; i++)
            {
                if (SortedPitchPointSilk[i].Tick > MinTick && SortedPitchPointSilk[i - 1].Tick < MaxTick)
                {
                    PitchSegments.Add(new PitchObject[2] { SortedPitchPointSilk[i - 1], SortedPitchPointSilk[i] }, 0);//0为未分组的
                }
            }

            //剔除超界线段
            int SegmentIndex = 0;
            bool LastSegmentIsHidden = true;
            PitchObject[][] KeyArray = PitchSegments.Keys.ToArray();
            for (int i = 0; i < KeyArray.Length;i++ )
            {
                PitchObject[] PitchSegment = KeyArray[i];
                bool StartOver = false;//起点越界
                bool EndOver = false;//终点越界
                PitchObject Sp = PitchSegment[0];
                PitchObject Ep = PitchSegment[1];
                if (Sp.PitchValue.NoteNumber > MaxNote || Sp.PitchValue.NoteNumber < MinNote || Sp.Tick > MaxTick || Sp.Tick < MinTick)
                {
                    StartOver = true;
                }
                if (Ep.PitchValue.NoteNumber > MaxNote || Ep.PitchValue.NoteNumber < MinNote || Ep.Tick > MaxTick || Ep.Tick < MinTick)
                {
                    EndOver = true;
                }
                if (StartOver && EndOver)
                {
                    LastSegmentIsHidden = true;
                }
                else
                {
                    if (LastSegmentIsHidden)
                    {
                        SegmentIndex++;
                        LastSegmentIsHidden = false;
                    }
                    PitchSegments[PitchSegment] = SegmentIndex;
                }
            }
            //转换成渲染序列
            List<List<Point>> PixelSilks = new List<List<Point>>();
            SegmentIndex = 0;
            foreach (KeyValuePair<PitchObject[], int> KeyValue in PitchSegments)
            {
                int Value = KeyValue.Value;
                if (Value == 0) continue;
                PitchObject[] Segment = KeyValue.Key;
                if (Value != SegmentIndex)
                {
                    SegmentIndex = Value;
                    PixelSilks.Add(new List<Point>());
                    Point StartP = PitchObject2Point(Segment[0], MinTick, MaxTick, MinNote, MaxNote);
                    PixelSilks[PixelSilks.Count - 1].Add(StartP);
                }
                Point EndP = PitchObject2Point(Segment[1], MinTick, MaxTick, MinNote, MaxNote);
                PixelSilks[PixelSilks.Count - 1].Add(EndP);
            }
            D2DGraphics g = baseEvent.D2DGraphics;
            foreach (List<Point> PixelSilkLine in PixelSilks)
            {
                if(PixelSilkLine.Count>1) g.DrawPathGeometrySink(PixelSilkLine, LineColor, LineWidth, LineStyle, false);
            }
        }
         */
#endregion
    }
}
