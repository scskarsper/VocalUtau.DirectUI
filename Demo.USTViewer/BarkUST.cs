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
        ProjectObject LoadUST(string FilePath)
        {
            USTOriginalProject USTPO = USTOriginalSerializer.Deserialize(FilePath);
            PartsObject pro = USTOriginalSerializer.UST2Parts(USTPO);

            ProjectObject poj = new ProjectObject();
            poj.InitEmpty();
            poj.TrackerList[0].PartList[0] = pro;

            foreach (NoteObject po in pro.NoteList)
            {
                byte[] bt = System.Text.Encoding.Default.GetBytes(po.Lyric);
                string Str = System.Text.Encoding.GetEncoding("Shift-JIS").GetString(bt);
                po.Lyric = Str;
            }
            int sg = 1;
            for (long i = 1; i <= pro.TickLength; i += 32)//
            {
                sg = sg * -1;
                pro.PitchBendsList.Add(new PitchObject(i, sg * 0.5));
            }

            poj.TrackerList.Add(new TrackerObject(1));
            poj.TrackerList[1].PartList.Add(new PartsObject());
            poj.TrackerList[1].PartList[0].StartTime = 0.2;
            poj.TrackerList[1].PartList[0].TickLength = 1920;
            poj.TrackerList[1].PartList[0].PartName = "NAME";

            poj.TrackerList[1].PartList.Add(new PartsObject());
            poj.TrackerList[1].PartList[1].StartTime = 2.5;
            poj.TrackerList[1].PartList[1].TickLength = 1920;
            poj.TrackerList[1].PartList[1].PartName = "NAME2";


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




            return poj;
        }
        ProjectObject BarkIt(string file,bool renew=false)
        {
            if (renew)
            {
                ProjectObject POJB = LoadUST(file);
                ProjectObject.Serializer.SerializeToFile(POJB, file + ".json");
                return POJB;
            }
            try
            {
                ObjectDeserializer<ProjectObject> DPO = new ObjectDeserializer<ProjectObject>();
                ProjectObject OOP = DPO.DeserializeFromFile(file + ".json");
                return OOP;
            }
            catch
            {
                return BarkIt(file, true);
            }
        }
        public ProjectObject GetTest(bool renew=false)
        {
            ProjectObject poj = BarkIt(@"D:\VocalUtau\VocalUtau.DebugExampleFiles\DemoUSTS\Sakurane2.Tracks\Track-4b158252-eb7f-4223-b7b0-d78f32e044ec.ust",renew);
            return poj;
        }
    }
}
