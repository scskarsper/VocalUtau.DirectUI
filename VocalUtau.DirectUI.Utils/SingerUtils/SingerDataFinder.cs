using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.DirectUI.Utils.AttributeUtils.SingerTools;
using VocalUtau.Formats.Model.Database;
using VocalUtau.Formats.Model.Database.VocalDatabase;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.SingerUtils
{
    public class SingerDataFinder
    {
        SingerIndexer Indexer;
        ObjectAlloc<ProjectObject> ProjectBeeper = new ObjectAlloc<ProjectObject>();
        public SingerDataFinder(ref ProjectObject proj,SingerIndexer Indexer)
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
        public SplitDictionary GetPhonemesDictionary(PartsObject parts)
        {
            //VocalUtau.Program.GlobalPackage
            string singerGUID = parts.SingerGUID;
            string folder=SingerFinder.getSingerDir(singerGUID, ProjectBeeper.IntPtr);
            VocalIndexObject vio=Indexer.getIndex(folder);
            return vio.SplitDictionary;
        }
        public VocalIndexObject GetVocalIndexObject(PartsObject parts)
        {
            //VocalUtau.Program.GlobalPackage
            string singerGUID = parts.SingerGUID;
            string folder = SingerFinder.getSingerDir(singerGUID, ProjectBeeper.IntPtr);
            VocalIndexObject vio = Indexer.getIndex(folder);
            return vio;
        }
        public SingerObject GetSingerObject(PartsObject parts)
        {
            string singerGUID = parts.SingerGUID;
            return SingerFinder.getSingerObject(singerGUID, ProjectBeeper.IntPtr);
        }
        public string GetSingerFolder(PartsObject parts)
        {
            //VocalUtau.Program.GlobalPackage
            string singerGUID = parts.SingerGUID;
            string folder = SingerFinder.getSingerDir(singerGUID, ProjectBeeper.IntPtr);
            if (System.IO.Directory.Exists(folder))
            {
                return folder;
            }
            else
            {
                return "";
            }
        }
    }
}
