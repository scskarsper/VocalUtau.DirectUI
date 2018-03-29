using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.Formats.Model.Database;

namespace VocalUtau.DirectUI.Utils.SingerUtils
{
    public class SingerIndexer
    {
        Dictionary<string, VocalIndexObject> SingerIndexerCache = new Dictionary<string, VocalIndexObject>();
        public void AddIndexer(string Path, VocalIndexObject Index)
        {
            SingerIndexerCache.Add(Path, Index);
        }
        public VocalIndexObject getIndex(string RealFolder)
        {
            if (SingerIndexerCache.ContainsKey(RealFolder))
            {
                return SingerIndexerCache[RealFolder];
            }
            else
            {
                if (System.IO.Directory.Exists(RealFolder))
                {
                    LoadSinger(RealFolder);
                }
                if (SingerIndexerCache.ContainsKey(RealFolder))
                {
                    return SingerIndexerCache[RealFolder];
                }
                else
                {
                    SingerIndexerCache.Add(RealFolder, new VocalIndexObject());
                    return SingerIndexerCache[RealFolder];
                }
            }
        }
        public void LoadSinger(string RealFolder)
        {
            if (!SingerIndexerCache.ContainsKey(RealFolder))
            {
                VocalIndexObject vio = VocalIndexObject.Deseralize(RealFolder); 
                SingerIndexerCache.Add(RealFolder,vio);
            }
        }
    }
}
