using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI
{
    class TrackerConfigures
    {
        public int Const_TrackHeight = 40;
        public int Const_GridWidth = 200;//头表宽
        public int Const_GridFontTop = 13;
        public int Const_GridVolumeWidth = 70;
        public int Const_TitleHeight = 28;//标题头大小
        public int Const_TitleHeightSpliter = 2;//标题头分割线先高
        public int Const_TitleLineTop = 6;//标题头分割线先高
        public int Const_TitleRulerTop = 20;//标题头分割线先高
        public int Const_VScrollBarWidth = 19;

        public void setNoteHeight(uint NewValue)
        {
            Const_TrackHeight = (int)(NewValue > 20 ? NewValue : 20);
        }


        public Color PianoColor_WhiteKey;
        public Color TitleColor_Line;
        public Color TitleColor_Ruler;
        public Color TitleColor_Marker;
        public TrackerConfigures()
        {
            PianoColor_WhiteKey = Color.FromArgb(240, 240, 240);
            TitleColor_Line = Color.FromArgb(35, 105, 107);
            TitleColor_Ruler = Color.FromArgb(91,91,91);
            TitleColor_Marker = Color.FromArgb(131,131,131);
        }
    }
}
