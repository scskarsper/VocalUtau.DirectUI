using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.CategoryForms
{
    [DefaultProperty("Note_Lyric")]
    public class BasicPhonemeAttrModels
    {
        private NoteAtomObject _obj;

        private NoteAtomObject BaseObj
        {
            get { return _obj; }
        }
        private int _Index = 0;

        private int TickIndex
        {
            get { return _Index; }
            set { _Index = value; }
        }
        public BasicPhonemeAttrModels(ref NoteAtomObject BasicObj)
        {
            this._obj = BasicObj;
        }


        [CategoryAttribute("发音时长"), DisplayName("声音时长")]
        public long AtomLength
        {
            get { return BaseObj.AtomLength; }
        //    set { BaseObj.AtomLength = value; }
        }
        [CategoryAttribute("发音时长"), DisplayName("是否百分比")]
        public bool LengthIsPercent
        {
            get { return BaseObj.LengthIsPercent; }
        }

        [CategoryAttribute("发音符号"), DisplayName("发音符号")]
        public string PhonemeAtom
        {
            get { return BaseObj.PhonemeAtom; }
            set { BaseObj.PhonemeAtom = value; }
        }

        [CategoryAttribute("自定义发音属性"), DisplayName("预处理标记(Flags)")]
        public string Flags
        {
            get { return BaseObj.Flags; }
            set { BaseObj.Flags = value; }
        }

        [CategoryAttribute("自定义发音属性"), DisplayName("先行发音(PreUtterance)")]
        public double PreUtterance
        {
            get { return BaseObj.PreUtterance; }
            set { BaseObj.PreUtterance = value; }
        }
        double _Intensity;

        [CategoryAttribute("自定义发音属性"), DisplayName("感情强度(Intensity)")]
        public double Intensity
        {
            get { return _Intensity; }
            set { _Intensity = value; }
        }
        double _Modulation;

        [CategoryAttribute("自定义发音属性"), DisplayName("移调幅度(Modulation)")]
        public double Modulation
        {
            get { return _Modulation; }
            set { _Modulation = value; }
        }

        double _StartPoint;
        [CategoryAttribute("自定义发音属性"), DisplayName("采样偏移(Offset)")]
        public double StartPoint
        {
            get { return _StartPoint; }
            set { _StartPoint = value; }
        }
        double _Velocity;
        [CategoryAttribute("自定义发音属性"), DisplayName("发音速度(Velocity)")]
        public double Velocity
        {
            get { return _Velocity; }
            set { _Velocity = value; }
        }

        /*混合时重叠区域=Overlap=EnvA.P3=EnvB.P2*/

        /* double _Overlap;

         public double Overlap
         {
             get { return _Overlap; }
             set { _Overlap = value; }
         }
        long _fadeInLengthMs = 5;

        public long FadeInLengthMs
        {
            get { return _fadeInLengthMs; }
            set { _fadeInLengthMs = value; }
        }

        long _fadeOutLengthMs = 35;

        public long FadeOutLengthMs
        {
            get { return _fadeOutLengthMs; }
            set { _fadeOutLengthMs = value; }
        }

        long _volumePercentInt = 100;

        public long VolumePercentInt
        {
            get { return _volumePercentInt; }
            set { _volumePercentInt = value; }
        }*/
    }
}
