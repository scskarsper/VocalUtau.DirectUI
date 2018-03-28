using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.DirectUI.Utils.AttributeUtils.SingerTools;
using VocalUtau.Formats.Model.Database;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.SingerUtils
{
    public class SingerLyricSpliter
    {
        SingerIndexer Indexer;
        ObjectAlloc<ProjectObject> ProjectBeeper = new ObjectAlloc<ProjectObject>();
        public SingerLyricSpliter(ref ProjectObject proj,SingerIndexer Indexer)
        {
            this.Indexer = Indexer;
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
        public void SetupPhonemes(ref PartsObject parts, NoteObject curObj)
        {
            //VocalUtau.Program.GlobalPackage
            string singerGUID = parts.SingerGUID;
            string folder=SingerFinder.getSingerDir(singerGUID, ProjectBeeper.IntPtr);
            VocalIndexObject vio=Indexer.getIndex(folder);
            if (vio == null)
            {
                if (!curObj.LockPhoneme)
                {
                    curObj.PhonemeAtoms.Clear();
                    curObj.PhonemeAtoms.Add(new NoteAtomObject());
                    curObj.PhonemeAtoms[0].AtomLength = 0;
                    curObj.PhonemeAtoms[0].PhonemeAtom = curObj.Lyric;
                }
            }else
            {
                vio.SplitDictionary.UpdateLyrics(ref parts, curObj);
            }
        }
    }
}
