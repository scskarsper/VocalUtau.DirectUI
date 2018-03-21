using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.DirectUI.Utils.AttributeUtils.SingerTools;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.Models
{
    [DefaultProperty("Part_Name")]
    public class PartAttributes
    {
        IntPtr PartsObjectPtr = IntPtr.Zero;
        IntPtr ProjectObjectPtr = IntPtr.Zero;
        public PartAttributes(IntPtr PartsObjectPtr,IntPtr ProjectObjectPtr)
        {
            this.PartsObjectPtr = PartsObjectPtr;
            this.ProjectObjectPtr = ProjectObjectPtr;
            isCurrentEditing = true;
        }
        public void setPartsObjectPtr(IntPtr PartsObjectPtr) 
        {
            this.PartsObjectPtr = PartsObjectPtr;
            isCurrentEditing = true;
        }
        public void setProjectObjectPtr(IntPtr ProjectObjectPtr)
        {
            this.ProjectObjectPtr = ProjectObjectPtr;
        }
        bool isCurrentEditing = false;
        public void setIsCurrent(bool value)
        {
            isCurrentEditing = value;
        }
        private PartsObject PartsObject
        {
            get
            {
                PartsObject ret = null;
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(PartsObjectPtr);
                    ret = (PartsObject)handle.Target;
                }
                catch { ;}
                return ret;
            }
        }

        [Browsable(false)]
        public ProjectObject ProjectObject
        {
            get
            {
                ProjectObject ret = null;
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(ProjectObjectPtr);
                    ret = (ProjectObject)handle.Target;
                }
                catch { ;}
                return ret;
            }
        }

        [CategoryAttribute("段落信息"), DisplayName("段落名称")]
        public string Part_Name
        {
            get
            {
                return PartsObject.getPartName();
            }
            set
            {
                PartsObject.setPartName(value);
            }
        }


        [CategoryAttribute("段落信息"), DisplayName("正在编辑")]
        public bool Current_Editing
        {
            get
            {
                return isCurrentEditing;
            }
        }

        public string Value2String(double value)
        {
            if (value == double.NaN) return "(默认)";
            if (double.IsNaN(value)) return "(默认)";
            return value.ToString();
        }
        public string Flg2String(string value)
        {
            if (value == "") return "(默认)";
            if (value == null) return "(默认)";
            return value.ToString();
        }
        public double String2Value(string str)
        {
            if (str.IndexOf("默认") != -1) return double.NaN;
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
        public string String2Flg(string str)
        {
            return str.Replace("(", "").Replace(")", "").Replace("默", "").Replace("认", "");
        }

        [CategoryAttribute("段落信息"), DisplayName("段落总长")]
        public long Tick_Length
        {
            get
            {
                return PartsObject.TickLength;
            }
        }

        [CategoryAttribute("段落歌手属性"), DisplayName("歌手")]
        [TypeConverterAttribute(typeof(SingerItemConverter))] 
        public string Part_Singer
        {
            get { return SingerFinder.getSingerName(PartsObject.SingerGUID,ProjectObjectPtr); }
            set { if (value == "添加/删除歌手...") { SingerAtomCategoryWindow sacw = new SingerAtomCategoryWindow(ProjectObjectPtr); sacw.ShowDialog(); } else PartsObject.SingerGUID = SingerFinder.getSingerGuid(value, ProjectObjectPtr); }
        }

        [CategoryAttribute("段落歌手属性"), DisplayName("自定义预处理标记(Flags)")]
        public string Part_Flags
        {
            get { return Flg2String(PartsObject.Flags); }
            set { PartsObject.Flags = String2Flg(value); }
        }

        [CategoryAttribute("段落歌手属性"), DisplayName("自定义重采样引擎(Resampler)")]
        public string Part_Resampler
        {
            get { return Flg2String(PartsObject.PartResampler); }
            set { PartsObject.PartResampler = String2Flg(value); }
        }

        [CategoryAttribute("段落信息"), DisplayName("段落曲速")]
        public string Part_Tempo
        {
            get { return Value2String(PartsObject.getRealTempo()); }
            set { PartsObject.Tempo = String2Value(value); }
        }

        [CategoryAttribute("工程信息"), DisplayName("默认曲速")]
        public double Project_Temp
        {
            get { return ProjectObject.BaseTempo; }
            set
            {
                if(ProjectObject.BaseTempo != value)
                {
                    ProjectObject.BaseTempo = value;
                }
            }
        }
    }
}
