using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.DirectUI.Models;
using VocalUtau.DirectUI.Utils.PianoUtils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.ActionUtils
{
    class ControlActionUtils_Old
    {
        public static List<ControlObject> getShownControlLine(ref List<ControlObject> ControlList, long MinTick = -1, long MaxTick = -1)
        {
            if (MinTick < 0) MinTick = 0;
            if (MaxTick < MinTick) MaxTick = ControlList[ControlList.Count - 1].Tick + 1;
            List<ControlObject> ret = new List<ControlObject>();
            bool isFirst = true;
            if (ControlList.Count == 0)
            {
                ControlList.Add(new ControlObject(0, 0));
            }
            ControlObject LastObj = ControlList[0];
            for (int i = 0; i < ControlList.Count; i++)
            {
                if (ControlList[i].Tick < MinTick) continue;
                if (ControlList[i].Tick > MaxTick) break;
                if (isFirst && i > 0)
                {
                    LastObj = ControlList[i-1];
                    if (ControlList[i].Tick > MinTick)
                    {
                        ret.Add(new ControlObject(MinTick, LastObj.Value));
                    }
                    isFirst = false;
                }
                ret.Add(new ControlObject(ControlList[i].Tick, ControlList[i].Value));
                LastObj = ControlList[i];
            }
            if (ret.Count == 0)
            {
                ret.Add(new ControlObject(MinTick, LastObj.Value));
            }
            if (LastObj.Tick < MaxTick)
            {
                ret.Add(new ControlObject(MaxTick, LastObj.Value));
            }
            return ret;
        }
        public static void earseControlLine(ref List<ControlObject> ControlList, long StartTick, long EndTick, bool ModeV2 = false)
        {
            if (ModeV2)
            {
                List<ControlObject> PRR = new List<ControlObject>();
                PRR.Add(new ControlObject(StartTick, 0));
                PRR.Add(new ControlObject(EndTick, 0));
                replaceControlLine(ref ControlList, PRR);
            }
            else
            {
                earseArea(ref ControlList, new ControlObject(StartTick, 60), new ControlObject(EndTick, 60),false);
            }
        }
        private static void earseArea(ref List<ControlObject> ControlList, ControlObject St, ControlObject Et, bool SetStp = true)
        {
            int DelIdx = -1;
            double LastSt = ControlList.Count == 0 ? 0 : ControlList[0].Value;
            for (int i = 0; i < ControlList.Count; i++)
            {
                LastSt = ControlList[i].Value;
                if (ControlList[i].Tick <= St.Tick - 1) continue;
                if (ControlList[i].Tick > Et.Tick + 1) break;
                DelIdx = i;
                break;
            }
            double LastEt = LastSt;
            double LastEt2 = LastSt;
            long LastLt2 = 0;
            if (DelIdx > -1)
            {
                while (DelIdx < ControlList.Count)
                {
                    if (ControlList[DelIdx].Tick > Et.Tick + 1)
                    {
                        LastLt2 = ControlList[DelIdx].Tick-1;
                        LastEt2 = ControlList[DelIdx].Value;
                        break;
                    }
                    LastEt = ControlList[DelIdx].Value;
                    ControlList.RemoveAt(DelIdx);
                }
            }
            ControlList.Add(new ControlObject(St.Tick - 1, LastSt));
            if (SetStp)
            {
                ControlList.Add(new ControlObject(Et.Tick + 1, LastEt));
            }
            else
            {
                ControlList.Add(new ControlObject(LastLt2, LastEt2));
            }
        }
        public static void replaceControlLine(ref List<ControlObject> ControlList, List<ControlObject> newControlLine)
        {
            if (newControlLine.Count < 2) return;
            long StartTick = newControlLine[0].Tick;
            long EndTick = newControlLine[newControlLine.Count - 1].Tick;
            earseArea(ref ControlList, newControlLine[0], newControlLine[newControlLine.Count - 1]);

            for (int i = 0; i < newControlLine.Count; i++)
            {
                ControlList.Add(new ControlObject(newControlLine[i].Tick, newControlLine[i].Value));
            }
            ControlList.Sort();
        }
    }
}
