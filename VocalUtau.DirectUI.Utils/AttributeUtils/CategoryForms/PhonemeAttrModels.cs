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

        public string Value2String(double value)
        {
            if (value == double.NaN) return "";
            if (double.IsNaN(value)) return "";
            return value.ToString();
        }
        public double String2Value(string str)
        {
            if (str == "") return double.NaN;
            double t;
            if (double.TryParse(str, out t))
            {
                return t;
            }
            else
            {
                return double.NaN;
            }
        }
        [CategoryAttribute("自定义发音属性"), DisplayName("感情强度(Intensity)")]
        public string Intensity
        {
            get { return Value2String(BaseObj.Intensity); }
            set { BaseObj.Intensity = String2Value(value); }
        }

        [CategoryAttribute("自定义发音属性"), DisplayName("移调幅度(Modulation)")]
        public string Modulation
        {
            get { return Value2String(BaseObj.Modulation); }
            set { BaseObj.Modulation = String2Value(value); }
        }

        [CategoryAttribute("自定义发音属性"), DisplayName("采样偏移(Offset)")]
        public string StartPoint
        {
            get { return Value2String(BaseObj.StartPoint); }
            set { BaseObj.StartPoint = String2Value(value); }
        }

        double _Velocity;
        [CategoryAttribute("自定义发音属性"), DisplayName("发音速度(Velocity)")]
        public string Velocity
        {
            get { return Value2String(BaseObj.Velocity); }
            set { BaseObj.Velocity = String2Value(value); }
        }

        /*混合时重叠区域=Overlap=EnvA.P3=EnvB.P2*/

        [CategoryAttribute("自定义过度属性"), DisplayName("先行发音(PreUtterance)")]
        public string PreUtterance
        {
            get { return Value2String(BaseObj.PreUtterance); }
            set { BaseObj.PreUtterance = String2Value(value); }
        }
        [CategoryAttribute("自定义过度属性"), DisplayName("交叠区域(PreUtterance)")]
        public string Overlap
        {
            get { return Value2String(BaseObj.Overlap); }
            set { BaseObj.Overlap = String2Value(value); }
        }
        [CategoryAttribute("包络属性"), DisplayName("音量百分比")]
        public long VolumePercentInt
        {
            get { return BaseObj.VolumePercentInt; }
            set { BaseObj.VolumePercentInt = value; }
        }
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
        }*/
    }


    [DefaultProperty("Note_Lyric")]
    public class FirstPhonemeAttrModels : BasicPhonemeAttrModels
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
        public FirstPhonemeAttrModels(ref NoteAtomObject BasicObj)
            : base(ref BasicObj)
        {
            this._obj = BasicObj;
        }
        [CategoryAttribute("包络属性"), DisplayName("音量淡入时长(ms)")]
        public long FadeInLengthMs
        {
            get { return BaseObj.FadeInLengthMs; }
            set { BaseObj.FadeInLengthMs = value; if (BaseObj.FadeInLengthMs < 0)BaseObj.FadeInLengthMs = 0; }
        }
    }

    [DefaultProperty("Note_Lyric")]
    public class LastPhonemeAttrModels : BasicPhonemeAttrModels
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
        public LastPhonemeAttrModels(ref NoteAtomObject BasicObj)
            : base(ref BasicObj)
        {
            this._obj = BasicObj;
        }
        [CategoryAttribute("包络属性"), DisplayName("音量淡出时长(ms)")]
        public long FadeOutLengthMs
        {
            get { return BaseObj.FadeOutLengthMs; }
            set { BaseObj.FadeOutLengthMs = value; if (BaseObj.FadeOutLengthMs < 0)BaseObj.FadeOutLengthMs = 0; }
        }
    }


    [DefaultProperty("Note_Lyric")]
    public class SinglePhonemeAttrModels : BasicPhonemeAttrModels
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
        public SinglePhonemeAttrModels(ref NoteAtomObject BasicObj)
            : base(ref BasicObj)
        {
            this._obj = BasicObj;
        }
        [CategoryAttribute("包络属性"), DisplayName("音量淡出时长(ms)")]
        public long FadeOutLengthMs
        {
            get { return BaseObj.FadeOutLengthMs; }
            set { BaseObj.FadeOutLengthMs = value; if (BaseObj.FadeOutLengthMs < 0)BaseObj.FadeOutLengthMs = 0; }
        }
        [CategoryAttribute("包络属性"), DisplayName("音量淡入时长(ms)")]
        public long FadeInLengthMs
        {
            get { return BaseObj.FadeInLengthMs; }
            set { BaseObj.FadeInLengthMs = value; if (BaseObj.FadeInLengthMs < 0)BaseObj.FadeInLengthMs = 0; }
        }
    }
}
