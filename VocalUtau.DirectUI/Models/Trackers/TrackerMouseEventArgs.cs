using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VocalUtau.DirectUI.Models
{
    public class TrackerMouseEventArgs
    {
        object _Tag;

        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }
        MouseEventArgs _me;
        public MouseEventArgs MouseEventArgs
        {
            get { return _me; }
        }
        public TrackerMouseEventArgs(MouseEventArgs e)
        {
            _me = e;
        }
        internal enum AreaType
        {
            None,
            Roll,
            Track,
            Title
        }
        private AreaType _area;
        internal AreaType Area { get { return _area; } }

        private int _trackID;
        public int AbsoluteTrackID { get { return _trackID; } }


        private long _tick;
        public long Tick { get { return _tick; } }

        internal void CalcAxis(TrackerProperties pprops, TrackerConfigures rconf, TrackerMouseEventArgs cache)
        {
            if (cache != null && cache.MouseEventArgs.X == _me.X && cache.MouseEventArgs.Y == _me.Y)
            {
                _tick = cache.Tick;
                _trackID = cache._trackID;
                _area = cache.Area;
            }
            else
            {
                CalcAxis(pprops, rconf);
            }
        }
        internal void CalcAxis(TrackerProperties pprops, TrackerConfigures rconf)
        {
            if (_me.Y <= rconf.Const_TitleHeight && _me.Y >= 0)
            {
                _area = AreaType.Title;
            }
            else if (_me.X <= rconf.Const_GridWidth && _me.X >= 0)
            {
                _area = AreaType.Roll;
            }
            else
            {
                _area = AreaType.Track;
            }
            if (_area != AreaType.Title)
            {
                double drawed_noteSpt = (double)(_me.Y - rconf.Const_TitleHeight) / rconf.Const_TrackHeight;
                _trackID = (int)Math.Floor(drawed_noteSpt) + (int)pprops.TopTrackId;
            }
            if (_area != AreaType.Roll)
            {
                long drawed_pixel = _me.X - rconf.Const_GridWidth;
                _tick = (long)Math.Round((pprops.PianoStartTick + pprops.dertPixel2dertTick(drawed_pixel)), 0);
            }
        }
    }
}
