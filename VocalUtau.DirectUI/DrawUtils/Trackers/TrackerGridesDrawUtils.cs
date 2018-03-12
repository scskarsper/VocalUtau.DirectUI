using BalthasarLib.D2DPainter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.DrawUtils
{
    public class TrackerGridesDrawUtils : TrackerDrawUtils
    {
        TrackerConfigures rconf;
        TrackerProperties pprops;
        internal TrackerGridesDrawUtils(BalthasarLib.D2DPainter.D2DPaintEventArgs e, TrackerConfigures rconf, TrackerProperties pprops)
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
            try
            {
                baseEvent.D2DGraphics.DrawText(Text, new System.Drawing.Rectangle(LeftTopAxis.X, LeftTopAxis.Y, baseEvent.ClipRectangle.Width - LeftTopAxis.X, baseEvent.ClipRectangle.Height), FontColor, new System.Drawing.Font("Tahoma", FontSize, FontStyles));
            }
            catch { ;}
        }


        public class GridePainterArgs
        {
            public GridePainterArgs(int AbsoluteIndex, int TrackIndex, ITrackerInterface TrackObject, System.Drawing.Rectangle TrackArea)
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
            ITrackerInterface _TrackObject;

            public ITrackerInterface TrackObject
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
        private void DrawButton(int Left, int Top, int Width, int Height, Color BackColor, string Text)
        {
            Rectangle retbg = new Rectangle(Left, Top, Width, Height);
            baseEvent.D2DGraphics.FillRectangle(retbg, BackColor);

            baseEvent.D2DGraphics.DrawText(Text, new System.Drawing.Rectangle(Left + Width / 2 - 4, Top + (Height-9)/2 - 1, Width, Height), Color.White, new System.Drawing.Font("Tahoma", 7, FontStyle.Bold));
        

            baseEvent.D2DGraphics.DrawRectangle(retbg, rconf.TitleColor_Marker);
        }
        public delegate void OneGridePaintHandler(GridePainterArgs Args, TrackerGridesDrawUtils utils);
        public void DrawTracks(List<TrackerObject> VocalTracks, List<BackerObject> BackTracks, OneGridePaintHandler GridePaintCallBack)
        {
            int ShownTrackCount = baseEvent.ClipRectangle.Height / rconf.Const_TrackHeight;
            int y = rconf.Const_TitleHeight;//绘制点纵坐标
            for(uint i=pprops.TopTrackId;i<VocalTracks.Count+BackTracks.Count;i++)
            {
                if(y>baseEvent.ClipRectangle.Top+baseEvent.ClipRectangle.Height)
                {
                    break;
                }
                try
                {
                    System.Drawing.Rectangle TrackArea = new Rectangle(new Point(0, y), new Size(rconf.Const_GridWidth - 1, rconf.Const_TrackHeight));
                    uint j = 0;
                    uint jcount=0;
                    ITrackerInterface TObject = null;
                    if (i >= VocalTracks.Count)
                    {
                        j = (uint)(i - VocalTracks.Count);
                        jcount=(uint)VocalTracks.Count;
                        //BackerObject
                        TObject = BackTracks[(int)j];
                    }
                    else
                    {
                        j = i;
                        jcount=(uint)VocalTracks.Count;
                        //TrackerObject
                        TObject = VocalTracks[(int)j];
                    }
                    if (TObject != null)
                    {
                        string ShowName = TObject.getName();
                        if (ShowName.Length > 22)
                        {
                            ShowName = ShowName.Substring(0, 22);
                        }
                        baseEvent.D2DGraphics.DrawText(
                            "  " + ShowName,
                            new Rectangle(
                                new Point(rconf.Const_GridVolumeWidth + TrackArea.X + rconf.Const_GridButtonWidth, TrackArea.Y + rconf.Const_GridFontTop),
                                new Size(TrackArea.Width - rconf.Const_GridVolumeWidth - rconf.Const_GridButtonWidth, TrackArea.Height - rconf.Const_GridFontTop)
                                ),
                            Color.White,
                            new System.Drawing.Font("Tahoma", 10));

                        DrawButton(rconf.Const_GridVolumeWidth + TrackArea.X, TrackArea.Y , rconf.Const_GridButtonWidth, rconf.Const_TrackHeight, Color.DarkSlateGray, "");
                        
                        if (j > 0)
                        {
                            DrawButton(rconf.Const_GridVolumeWidth + TrackArea.X, TrackArea.Y, rconf.Const_GridButtonWidth, rconf.Const_TrackHeight / 2, Color.DarkCyan, "▲");
                        }

                        if (j + 1 < jcount)
                        {
                            DrawButton(rconf.Const_GridVolumeWidth + TrackArea.X, TrackArea.Y + rconf.Const_TrackHeight / 2, rconf.Const_GridButtonWidth, rconf.Const_TrackHeight / 2, Color.DarkGoldenrod, "▼");
                        }

                        if (GridePaintCallBack != null)
                        {
                            GridePaintCallBack(new GridePainterArgs((int)i, (int)j, TObject, new Rectangle(new Point(TrackArea.X, TrackArea.Y), new Size(rconf.Const_GridVolumeWidth, TrackArea.Height))), this);
                        }
                    }
                    baseEvent.D2DGraphics.DrawLine(
                        new Point(rconf.Const_GridVolumeWidth, TrackArea.Top),
                        new Point(rconf.Const_GridVolumeWidth, TrackArea.Bottom),
                            rconf.TitleColor_Marker
                    );

                }
                catch { ;}

                baseEvent.D2DGraphics.DrawLine(
                    new Point(0, y + rconf.Const_TrackHeight),
                    new Point(rconf.Const_GridWidth, y + rconf.Const_TrackHeight),
                        rconf.TitleColor_Marker
                );
                y += rconf.Const_TrackHeight;
            }
        }

        public void DrawTrianglePercent(Rectangle Area, double Percent, Color FillColor)
        {
            if(Percent>1)Percent=1;
            if(Percent<0)Percent=0;
            List<Point> Border = new List<Point>();
            Border.Add(new Point(Area.Left, Area.Bottom));
            Border.Add(new Point(Area.Right, Area.Top));
            Border.Add(new Point(Area.Right, Area.Bottom));

            double pK = -(double)Area.Height / (double)Area.Width;
            double pB = (double)Area.Top - pK * (double)Area.Right;

            List<Point> Filler = new List<Point>();
            Filler.Add(new Point(Area.Left, Area.Bottom));
            Filler.Add(new Point((int)(Area.Left + Area.Width * Percent), (int)(pK * (Area.Left + Area.Width * Percent) + pB)));
            Filler.Add(new Point((int)(Area.Left + Area.Width * Percent),Area.Bottom));

            baseEvent.D2DGraphics.FillPathGeometrySink(Filler, FillColor);
            baseEvent.D2DGraphics.DrawPathGeometrySink(Border, Color.White, true);
        }
    }
}
