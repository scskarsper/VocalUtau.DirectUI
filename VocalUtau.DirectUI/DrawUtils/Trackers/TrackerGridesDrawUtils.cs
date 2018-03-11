﻿using BalthasarLib.D2DPainter;
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
            D2DGraphics g = baseEvent.D2DGraphics;
            g.DrawText(Text, new System.Drawing.Rectangle(LeftTopAxis.X, LeftTopAxis.Y, baseEvent.ClipRectangle.Width - LeftTopAxis.X, baseEvent.ClipRectangle.Height - LeftTopAxis.Y), FontColor, new System.Drawing.Font("Tahoma", FontSize, FontStyles));
        }


        public class GridePainterArgs
        {
            public GridePainterArgs(int AbsoluteIndex, int TrackIndex, object TrackObject, System.Drawing.Rectangle TrackArea)
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
                    if (i >= VocalTracks.Count)
                    {
                        j = (uint)(i - VocalTracks.Count);
                        //BackerObject
                        BackerObject TObject = BackTracks[(int)j];
                        baseEvent.D2DGraphics.DrawText(
                            " "+TObject.Name,
                            new Rectangle(
                                new Point(rconf.Const_GridVolumeWidth+TrackArea.X,TrackArea.Y+rconf.Const_GridFontTop),
                                new Size(TrackArea.Width - rconf.Const_GridVolumeWidth, TrackArea.Height - rconf.Const_GridFontTop)
                                ),
                            Color.White,
                            new System.Drawing.Font("Tahoma", 10));

                        if (GridePaintCallBack != null)
                        {
                            GridePaintCallBack(new GridePainterArgs((int)i, (int)j, TObject, new Rectangle(new Point(TrackArea.X,TrackArea.Y),new Size(rconf.Const_GridVolumeWidth,TrackArea.Height))), this);
                        }
                    }
                    else
                    {
                        j = i;
                        //TrackerObject
                        TrackerObject TObject = VocalTracks[(int)j];
                        baseEvent.D2DGraphics.DrawText(
                            " " + TObject.Name,
                            new Rectangle(
                                new Point(rconf.Const_GridVolumeWidth + TrackArea.X, TrackArea.Y + rconf.Const_GridFontTop),
                                new Size(TrackArea.Width - rconf.Const_GridVolumeWidth, TrackArea.Height - rconf.Const_GridFontTop)
                                ),
                            Color.White,
                            new System.Drawing.Font("Tahoma", 10));
                        if (GridePaintCallBack != null)
                        {
                            GridePaintCallBack(new GridePainterArgs((int)i, (int)j, TObject, new Rectangle(new Point(TrackArea.X,TrackArea.Y),new Size(rconf.Const_GridVolumeWidth,TrackArea.Height))), this);
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
    }
}
