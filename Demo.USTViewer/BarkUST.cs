using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.Formats.Model.USTs.Original;
using VocalUtau.Formats.Model.VocalObject;

namespace Demo.USTViewer
{
    public class BarkUST
    {
        ProjectObject LoadUST(string[] FilePath)
        {
            ProjectObject poj = new ProjectObject();
            poj.InitEmpty();
            try
            {
                for (int i = 0; i < FilePath.Length; i++)
                {
                    USTOriginalProject USTPO = USTOriginalSerializer.Deserialize(FilePath[i]);
                    PartsObject pro = USTOriginalSerializer.UST2Parts(USTPO);

                    if (poj.TrackerList.Count <= i)
                    {
                        poj.TrackerList.Add(new TrackerObject((uint)i));
                        poj.TrackerList[i].PartList.Add(new PartsObject());
                    }
                    poj.TrackerList[i].PartList[0] = pro;

                    foreach (NoteObject po in pro.NoteList)
                    {
                        byte[] bt = System.Text.Encoding.Default.GetBytes(po.Lyric);
                        string Str = System.Text.Encoding.GetEncoding("Shift-JIS").GetString(bt);
                        po.Lyric = Str;
                    }
                }
            }
            catch { ;}
        /*    int sg = 1;
            for (long i = 1; i <= pro.TickLength; i += 32)
            {
                sg = sg * -1;
                pro.PitchBendsList.Add(new PitchObject(i, sg * 0.5));
            }*/

            int ci = poj.TrackerList.Count;
            poj.TrackerList.Add(new TrackerObject((uint)ci));
            poj.TrackerList[ci].PartList.Add(new PartsObject());
            poj.TrackerList[ci].PartList[0].StartTime = 0.2;
          //  poj.TrackerList[ci].PartList[0].TickLength = 1920;
            poj.TrackerList[ci].PartList[0].PartName = "NAME";

            poj.TrackerList[ci].PartList.Add(new PartsObject());
            poj.TrackerList[ci].PartList[1].StartTime = 2.5;
          //  poj.TrackerList[ci].PartList[1].TickLength = 1920;
            poj.TrackerList[ci].PartList[1].PartName = "NAME2";


            poj.BackerList[0].WavPartList.Add(new WavePartsObject());
            poj.BackerList[0].WavPartList[0].StartTime = 0.2;
            poj.BackerList[0].WavPartList[0].DuringTime = 100;
            poj.BackerList[0].WavPartList[0].PartName = "WA1";


            poj.BackerList.Add(new BackerObject(1));
            poj.BackerList[1].WavPartList.Add(new WavePartsObject());
            poj.BackerList[1].WavPartList[0].StartTime = 0.2;
            poj.BackerList[1].WavPartList[0].DuringTime = 4;
            poj.BackerList[1].WavPartList[0].PartName = "WA2";

            poj.BackerList[1].WavPartList.Add(new WavePartsObject());
            poj.BackerList[1].WavPartList[1].StartTime = 5;
            poj.BackerList[1].WavPartList[1].DuringTime = 100;
            poj.BackerList[1].WavPartList[1].PartName = "WA3";

            poj.SingerList.Clear();
            poj.SingerList.Add(new SingerObject("Singer1"));
            poj.SingerList[0].PartResampler = "resampler.exe";
            poj.SingerList[0].Flags = "B0Y0";
            poj.SingerList[0].SingerFolder = @"D:\VocalUtau\VocalUtau.DebugExampleFiles\UTAUKernel\voice\uta";
            
            poj.SingerList.Add(new SingerObject("Singer2"));
            poj.SingerList[1].PartResampler = "tn_fnds.exe";
            poj.SingerList[1].Flags = "B0Y1";
            poj.SingerList[1].SingerFolder = @"D:\VocalUtau\VocalUtau.DebugExampleFiles\UTAUKernel\voice\uta";

            return poj;
        }
        ProjectObject BarkIt(string[] files,bool renew=false)
        {
            if (renew)
            {
                ProjectObject POJB = LoadUST(files);
                ProjectObject.Serializer.SerializeToZipFile(POJB, files[0] + ".json");
                return POJB;
            }
            try
            {
                ObjectDeserializer<ProjectObject> DPO = new ObjectDeserializer<ProjectObject>();
                ProjectObject OOP = DPO.DeserializeFromZipFile(files[0] + ".json");
                return OOP;
            }
            catch
            {
                return BarkIt(files, true);
            }
        }
        public ProjectObject GetTest(bool renew=false)
        {
            ProjectObject poj = BarkIt(new string[]{
                                    @"D:\VocalUtau\VocalUtau.DebugExampleFiles\DemoUSTS\Sakurane2.Tracks\Track-fc0b6027-d7fb-4c82-8ca0-6bc1e54cdfb2.ust",
                                    @"D:\VocalUtau\VocalUtau.DebugExampleFiles\DemoUSTS\Sakurane2.Tracks\Track-4b158252-eb7f-4223-b7b0-d78f32e044ec.ust"
                                },renew);
            return poj;
        }
    }
}
