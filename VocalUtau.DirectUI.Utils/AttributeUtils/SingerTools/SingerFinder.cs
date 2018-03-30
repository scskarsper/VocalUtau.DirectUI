using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.DirectUI.Utils.AttributeUtils.Models;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.SingerTools
{
    public class SingerFinder
    {
        private static ProjectObject getProjectObject(IntPtr ProjectObjectPtr)
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
        public static string getSingerName(string SingerGUID,IntPtr ProjectPtr)
        {
            string ret = "";
            ProjectObject poj=getProjectObject(ProjectPtr);
            for (int i = 0; i < poj.SingerList.Count; i++)
            {
                if (ret == "") ret = poj.SingerList[i].VocalName;
                if (poj.SingerList[i].getGuid() == SingerGUID)
                {
                    return poj.SingerList[i].VocalName;
                }
            }
            return ret;
        }
        public static SingerObject getSingerObject(string SingerGUID, IntPtr ProjectPtr)
        {
            SingerObject ret = null;
            ProjectObject poj = getProjectObject(ProjectPtr);
            for (int i = 0; i < poj.SingerList.Count; i++)
            {
                if (ret == null) ret = poj.SingerList[i];
                if (poj.SingerList[i].getGuid() == SingerGUID)
                {
                    return poj.SingerList[i];
                }
            }
            return ret;
        }
        public static string getSingerGuid(string SingerName, IntPtr ProjectPtr)
        {
            string ret = "";
            ProjectObject poj = getProjectObject(ProjectPtr);
            for (int i = 0; i < poj.SingerList.Count; i++)
            {
                if (ret == "") ret = poj.SingerList[i].getGuid();
                if (poj.SingerList[i].VocalName == SingerName)
                {
                    return poj.SingerList[i].getGuid();
                }
            }
            return ret;
        }
        public static string getSingerDir(string SingerGUID, IntPtr ProjectPtr)
        {
            string ret = "";
            ProjectObject poj = getProjectObject(ProjectPtr);
            for (int i = 0; i < poj.SingerList.Count; i++)
            {
                if (ret == "") ret = poj.SingerList[i].getRealSingerFolder();
                if (poj.SingerList[i].getGuid() == SingerGUID)
                {
                    return poj.SingerList[i].getRealSingerFolder();
                }
            }
            return ret;
        }
    }

    public class SingerItemConverter : StringConverter
    {
        public SingerItemConverter()
        {
        }  

        //true enable,false disable
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance is PartAttributes)
            {

                List<string> values = new System.Collections.Generic.List<string>();
                values.Clear();

                for (int i = 0; i < (context.Instance as PartAttributes).ProjectObject.SingerList.Count; i++)
                {
                    values.Add((context.Instance as PartAttributes).ProjectObject.SingerList[i].VocalName);
                }
                values.Add("添加/删除歌手...");
                return new StandardValuesCollection(values);

            }

            return base.GetStandardValues(context);

        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
