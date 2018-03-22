using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.Models
{
    [DefaultProperty("WavPart_Name")]
    public class WavePartAttributes
    {
        IntPtr PartsObjectPtr = IntPtr.Zero;
        IntPtr ProjectObjectPtr = IntPtr.Zero;
        public WavePartAttributes(IntPtr PartsObjectPtr,IntPtr ProjectObjectPtr)
        {
            this.PartsObjectPtr = PartsObjectPtr;
            this.ProjectObjectPtr = ProjectObjectPtr;
        }
        public void setPartsObjectPtr(IntPtr PartsObjectPtr) 
        {
            this.PartsObjectPtr = PartsObjectPtr;
        }
        public void setProjectObjectPtr(IntPtr ProjectObjectPtr)
        {
            this.ProjectObjectPtr = ProjectObjectPtr;
        }
        private WavePartsObject PartsObject
        {
            get
            {
                WavePartsObject ret = null;
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(PartsObjectPtr);
                    ret = (WavePartsObject)handle.Target;
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

        [CategoryAttribute("伴奏段落信息"), DisplayName("段落名称")]
        public string WavPart_Name
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

        [CategoryAttribute("伴奏段落信息"), DisplayName("音频文件")]
        [Editor(typeof(VocalUtau.DirectUI.Utils.AttributeUtils.WavPartTools.WavPartEditors.FileSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string WavPart_File
        {
            get
            {
                return PartsObject.WavFileName;
            }
            set
            {
                if (System.IO.File.Exists(value))
                {
                    PartsObject.WavFileName = value;
                    if (PartsObject.getRealDuringTime(true) * 1000 != PartsObject.DuringTime)
                    {
                        if (MessageBox.Show("当前音频文件长度与段落长度不匹配，要重设段落长度为音频文件长度么？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            PartsObject.DuringTime = PartsObject.getRealDuringTime();
                        };
                    }
                }
            }
        }

        [CategoryAttribute("伴奏段落设置"), DisplayName("文件时长")]
        public TimeSpan WavPart_RealFileDuring
        {
            get
            {
                return new TimeSpan(0, 0, 0, 0, (int)(PartsObject.getRealDuringTime() * 1000));
            }
        }

        [CategoryAttribute("伴奏段落设置"), DisplayName("段落时长")]
        [Editor(typeof(VocalUtau.DirectUI.Utils.AttributeUtils.WavPartTools.WavPartEditors.TimeReload), typeof(System.Drawing.Design.UITypeEditor))]
        public string WavPart_FileDuring
        {
            get
            {
                return (new TimeSpan(0, 0, 0, 0, (int)(PartsObject.DuringTime * 1000))).ToString();
            }
            set
            {
                TimeSpan vle = new TimeSpan();
                if (TimeSpan.TryParse(value, out vle))
                {
                    if (vle.TotalSeconds > 0)
                    {
                        PartsObject.DuringTime = vle.TotalSeconds;
                    }
                    else
                    {
                        PartsObject.DuringTime = PartsObject.getRealDuringTime(false);
                    }
                    if (PartsObject.DuringTime <= 0)
                    {
                        PartsObject.DuringTime = 1;
                    }
                }
                else
                {
                    PartsObject.DuringTime = PartsObject.getRealDuringTime(false);
                }
            }
        }
    }
}
