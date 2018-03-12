using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VocalUtau.DirectUI.Models
{
    public class GridesMouseEventArgs
    {
        MouseEventArgs _me;
        public MouseEventArgs MouseEventArgs
        {
            get { return _me; }
        }
        public GridesMouseEventArgs(MouseEventArgs e)
        {
            _me = e;
        }
        public enum GridesAreaType
        {
            None,
            NameArea,
            VerticalBtnsAdd,
            VerticalBtnsDec,
            VolumeArea
        }
        private GridesAreaType _area;
        public GridesAreaType Area { get { return _area; } }
        
        internal void CalcArea(TrackerProperties pprops, TrackerConfigures rconf, GridesMouseEventArgs cache)
        {
            if (cache != null && cache.MouseEventArgs.X == _me.X && cache.MouseEventArgs.Y == _me.Y)
            {
                _area = cache.Area;
            }
            else
            {
                CalcArea(pprops, rconf);
            }
        }
        internal void CalcArea(TrackerProperties pprops, TrackerConfigures rconf)
        {
            if (_me.X > rconf.Const_GridVolumeWidth && _me.X < rconf.Const_GridWidth)
            {
                //GRID
                if (_me.X < rconf.Const_GridVolumeWidth + rconf.Const_GridButtonWidth)
                {
                    //Button
                    double cid = (_me.Y - rconf.Const_TitleHeight) / (double)(rconf.Const_TrackHeight / 2);
                    if (((int)cid) % 2 == 0)
                    {
                        _area = GridesAreaType.VerticalBtnsDec;
                    }
                    else
                    {
                        _area = GridesAreaType.VerticalBtnsAdd;
                    }
                }
                else
                {
                    _area = GridesAreaType.NameArea;
                }
            }
            else if (_me.X < rconf.Const_GridVolumeWidth && _me.X >= 0)
            {
                _area = GridesAreaType.VolumeArea;
            }
            else
            {
                _area = GridesAreaType.None;
                //NAN
            }

        }
    }
}
