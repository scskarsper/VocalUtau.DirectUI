using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.SingerUtils
{
    public class SingerLyricSpliter
    {
        ObjectAlloc<ProjectObject> ProjectBeeper = new ObjectAlloc<ProjectObject>();
        public SingerLyricSpliter(ref ProjectObject proj)
        {
            ProjectBeeper.ReAlloc(proj);
        }
        public void LoadProjectObject(ref ProjectObject proj)
        {
            ProjectBeeper.ReAlloc(proj);
        }
        public ProjectObject ProjectObject
        {
            get
            {
                ProjectObject ret = null;
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(ProjectBeeper.IntPtr);
                    ret = (ProjectObject)handle.Target;
                }
                catch { ;}
                return ret;
            }
        }
        public List<NoteAtomObject> SetupPhonemes(string SingerGUID, string Lyric)
        {
            List<NoteAtomObject> ret = new List<NoteAtomObject>();
            Lyric = Lyric.Replace("ang", "ang|ng_R");
            Lyric = Lyric.Replace("eng", "eng|ng_R");
            Lyric = Lyric.Replace("ong", "ong|ng_R");
            Lyric = Lyric.Replace("ing", "ing|ng_R");
            string[] LyricArr = Lyric.Split('|');
            for (int i = 0; i < LyricArr.Length; i++)
            {
                ret.Add(new NoteAtomObject(LyricArr[i]));
            }
            return ret;
        }
    }
}
