using BalthasarLib.D2DPainter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.DrawUtils
{
    public class TrackerPartsDrawUtils : TrackerDrawUtils
    {
        TrackerConfigures rconf;
        TrackerProperties pprops;
        internal TrackerPartsDrawUtils(BalthasarLib.D2DPainter.D2DPaintEventArgs e, TrackerConfigures rconf, TrackerProperties pprops)
            : base(e,rconf)
        {
            baseEvent = e;
            this.rconf = rconf;
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

        public void FillParts(System.Drawing.Rectangle TrackArea, List<IPartsInterface> Parts, Color PartsColor, int PartIndex = -1, long OffsetTick=  0)
        {
            bool isSingle = false;
            if (PartIndex >= 0 && PartIndex < Parts.Count)
            {
                isSingle = true;
            }
            int pIndex=0;
            foreach (IPartsInterface part in Parts)
            {
                pIndex++;
                if (isSingle && (pIndex-1) != PartIndex)
                {
                    continue;
                }
                long LeftTick = part.getAbsoluteStartTick(pprops.Tempo);
                long RightTick = part.getAbsoluteEndTick(pprops.Tempo);

                long LeftRectangleTick = OffsetTick+pprops.PianoStartTick;
                long RightRectangleTick = OffsetTick+pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;

                if (RightTick <= LeftRectangleTick || LeftTick >= RightRectangleTick)
                {
                    //抛弃音符（超界）
                    return;
                }

                long StartTick = LeftTick - LeftRectangleTick;//获得左边界距离启绘点距离；
                long EndTick = RightTick - LeftRectangleTick;//获得右边界距离启绘点距离；

                int PartsX1Pixel = baseEvent.ClipRectangle.X;
                int PartsX2Pixel = PartsX1Pixel;
                if (StartTick < 0)
                {
                    //起绘制点小于0;
                    PartsX1Pixel = baseEvent.ClipRectangle.X - (int)Math.Round(pprops.dertTick2dertPixel(-StartTick), 0);
                    PartsX2Pixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);
                }
                else
                {
                    PartsX1Pixel = PartsX1Pixel + (int)Math.Round(pprops.dertTick2dertPixel(StartTick), 0);
                    PartsX2Pixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);
                }

                Rectangle PartsRect = new Rectangle(
                    new Point(PartsX1Pixel, TrackArea.Top + 2),
                    new Size(PartsX2Pixel - PartsX1Pixel, TrackArea.Height - 4)
                    );

                baseEvent.D2DGraphics.DrawRectangle(PartsRect, Color.White);
                baseEvent.D2DGraphics.FillRectangle(PartsRect, Color.FromArgb(90, PartsColor));
                baseEvent.D2DGraphics.DrawText(" " + part.getPartName(), PartsRect, Color.White,
                            new System.Drawing.Font("Tahoma", 10));
            }
        }
        public void FillParts(System.Drawing.Rectangle TrackArea, List<PartsObject> Parts, Color PartsColor,int PartIndex=-1)
        {
            List<IPartsInterface> Parties = new List<IPartsInterface>();
            Parties.AddRange(Parts.ToArray());
            FillParts(TrackArea, Parties, PartsColor,PartIndex);
        }
        public void FillParts(System.Drawing.Rectangle TrackArea, List<WavePartsObject> WavParts, Color PartsColor, int PartIndex = -1)
        {
            List<IPartsInterface> Parties = new List<IPartsInterface>();
            Parties.AddRange(WavParts.ToArray());
            FillParts(TrackArea, Parties, PartsColor, PartIndex);
        }

        public void DrawParts(System.Drawing.Rectangle TrackArea, double StartTime, double DurTime, Color BlockColor)
        {
            long LeftTick = pprops.Time2Tick(StartTime);
            long RightTick = pprops.Time2Tick(StartTime + DurTime);

            long LeftRectangleTick = pprops.PianoStartTick;
            long RightRectangleTick = pprops.PianoStartTick + (long)Math.Round(pprops.dertPixel2dertTick(baseEvent.ClipRectangle.Width), 0) + 1;

            if (RightTick <= LeftRectangleTick || LeftTick >= RightRectangleTick)
            {
                //抛弃音符（超界）
                return;
            }

            long StartTick = LeftTick - LeftRectangleTick;//获得左边界距离启绘点距离；
            long EndTick = RightTick - LeftRectangleTick;//获得右边界距离启绘点距离；

            int PartsX1Pixel = baseEvent.ClipRectangle.X;
            int PartsX2Pixel = PartsX1Pixel;
            if (StartTick < 0)
            {
                //起绘制点小于0;
                PartsX1Pixel = baseEvent.ClipRectangle.X - (int)Math.Round(pprops.dertTick2dertPixel(-StartTick), 0);
                PartsX2Pixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);
            }
            else
            {
                PartsX1Pixel = PartsX1Pixel + (int)Math.Round(pprops.dertTick2dertPixel(StartTick), 0);
                PartsX2Pixel = baseEvent.ClipRectangle.X + (int)Math.Round(pprops.dertTick2dertPixel(EndTick), 0);
            }

            Rectangle PartsRect = new Rectangle(
                new Point(PartsX1Pixel, TrackArea.Top + 2),
                new Size(PartsX2Pixel - PartsX1Pixel, TrackArea.Height - 4)
                );

            baseEvent.D2DGraphics.DrawRectangle(PartsRect, BlockColor, 2);

        }
        
        public class TrackPainterArgs
        {
            public TrackPainterArgs(int AbsoluteIndex, int TrackIndex, object TrackObject, System.Drawing.Rectangle TrackArea)
            {
                this._AbsoluteIndex = AbsoluteIndex;
                this._TrackArea = TrackArea;
                this._TrackIndex = TrackIndex;
                this._TrackObject = TrackObject;
            }
            int _AbsoluteIndex;

            public int AbsoluteIndex
            {
                get { return _AbsoluteIndex; }
                set { _AbsoluteIndex = value; }
            }
            int _TrackIndex;

            public int TrackIndex
            {
                get { return _TrackIndex; }
                set { _TrackIndex = value; }
            }
            object _TrackObject;

            public object TrackObject
            {
                get { return _TrackObject; }
                set { _TrackObject = value; }
            }
            System.Drawing.Rectangle _TrackArea;

            public System.Drawing.Rectangle TrackArea
            {
                get { return _TrackArea; }
                set { _TrackArea = value; }
            }
        }
        public delegate void OneTrackPaintHandler(TrackPainterArgs Args,TrackerPartsDrawUtils utils);
        public void DrawTracks(List<TrackerObject> VocalTracks, List<BackerObject> BackTracks,OneTrackPaintHandler TrackPaintCallBack)
        {
            int ShownTrackCount = baseEvent.ClipRectangle.Height / rconf.Const_TrackHeight;
            int y = rconf.Const_TitleHeight;//绘制点纵坐标
            for (uint i = pprops.TopTrackId; i < VocalTracks.Count + BackTracks.Count; i++)
            {
                if (y > baseEvent.ClipRectangle.Top + baseEvent.ClipRectangle.Height)
                {
                    break;
                }
                try
                {
                    System.Drawing.Rectangle TrackArea = new Rectangle(new Point(baseEvent.ClipRectangle.Left, y), new Size(baseEvent.ClipRectangle.Width, rconf.Const_TrackHeight));
                    uint j = 0;
                    if (i >= VocalTracks.Count)
                    {
                        j = (uint)(i - VocalTracks.Count);
                        //BackerObject
                        BackerObject TObject = BackTracks[(int)j];
                        //baseEvent.D2DGraphics.FillRectangle(TrackArea, Color.FromArgb(90,Color.Blue));
                        if (TrackPaintCallBack != null)
                        {
                            TrackPaintCallBack(new TrackPainterArgs((int)i,(int)j, TObject, TrackArea), this);
                        }
                    }
                    else
                    {
                        j = i;
                        //TrackerObject
                        TrackerObject TObject = VocalTracks[(int)j];
                        //baseEvent.D2DGraphics.FillRectangle(TrackArea, Color.FromArgb(90, Color.YellowGreen));
                        if (TrackPaintCallBack != null)
                        {
                            TrackPaintCallBack(new TrackPainterArgs((int)i, (int)j, TObject, TrackArea), this);
                        }
                    }
                }
                catch { ;}

                baseEvent.D2DGraphics.DrawLine(
                    new Point(baseEvent.ClipRectangle.Left, y + rconf.Const_TrackHeight),
                    new Point(baseEvent.ClipRectangle.Right, y + rconf.Const_TrackHeight),
                        rconf.TitleColor_Marker
                );
                y += rconf.Const_TrackHeight;
            }
        }
    }
}
