using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.Models
{
    [DefaultProperty("Name")]
    public class PartAttributes
    {
        IntPtr PartsObjectPtr = IntPtr.Zero;
        public PartAttributes(IntPtr PartsObjectPtr)
        {
            this.PartsObjectPtr = PartsObjectPtr;
        }
        public void setPartsObjectPtr(IntPtr PartsObjectPtr)
        {
            this.PartsObjectPtr = PartsObjectPtr;
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
    }
}
