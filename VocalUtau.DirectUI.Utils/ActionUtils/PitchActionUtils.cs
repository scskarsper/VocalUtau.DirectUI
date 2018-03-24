using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.DirectUI.Models;
using VocalUtau.DirectUI.Utils.PianoUtils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.ActionUtils
{
    class PitchActionUtils_Drop
    {
        public static List<PitchObject> getShownPitchLine(ref List<NoteObject> NoteList, ref List<PitchObject> PitchList, long MinTick, long MaxTick, bool ShowNoteSpace=true)
        {
            List<PitchObject> ret = new List<PitchObject>();
            if (MinTick < 0) MinTick = 0;
            if (MaxTick <= MinTick) return ret;
            int NextPitchIndex = -1;
            for (int i = 0; i < PitchList.Count; i++)
            {
                if (PitchList[i].Tick < MinTick) continue;
                NextPitchIndex = i;
                break;
            }
            long RightLimit = -1;
            for (int i = 0; i < NoteList.Count; i++)
            {
                //查找和设置音符起始点
                NoteObject PN = NoteList[i];
                if (RightLimit >= PN.Tick + PN.Length) continue;
                if (PN.Tick >= MaxTick) break;
                if (PN.Tick + PN.Length < MinTick) continue;
                long StartPoint = PN.Tick;
                long EndPoint = PN.Tick + PN.Length;
                //设置初始点
                if (ret.Count > 0)
                {
                    long LE = ret[ret.Count - 1].Tick;
                    if (LE >= StartPoint)
                    {
                        StartPoint = ret[ret.Count - 1].Tick + 1;//Tick?
                    }
                }
                if (ShowNoteSpace)
                {
                    //对于两音符间的CP，以后者音高优先（考虑到Overlap参数）
                    while (NextPitchIndex > -1 && NextPitchIndex < PitchList.Count && PitchList[NextPitchIndex].Tick < PN.Tick)
                    {
                        PitchObject PNP = new PitchObject(PitchList[NextPitchIndex].Tick, PN.PitchValue.NoteNumber + PitchList[NextPitchIndex].PitchValue.PitchValue);
                        ret.Add(PNP);
                        NextPitchIndex++;
                    }
                }
                //判定如果音符内没有CP，则设置音符两端
                if (NextPitchIndex == -1 || NextPitchIndex == PitchList.Count || PitchList[NextPitchIndex].Tick >= MaxTick ||
                    PitchList[NextPitchIndex].Tick > EndPoint
                    )
                {
                    //NoPitch
                    PitchObject PNS = new PitchObject(StartPoint, PN.PitchValue.PitchValue);
                    PitchObject PNE = new PitchObject(EndPoint, PN.PitchValue.PitchValue);
                    ret.Add(PNS);
                    ret.Add(PNE);
                }
                else
                {
                    //判定如果音符内有CP，则设置音高
                    PitchObject PNS = null;
                    PitchObject PNE = null;
                    while (NextPitchIndex < PitchList.Count && PitchList[NextPitchIndex].Tick <= EndPoint && PitchList[NextPitchIndex].Tick < MaxTick)
                    {
                        if (PitchList[NextPitchIndex].Tick < StartPoint)
                        {
                            NextPitchIndex++;
                            continue;
                        }
                        if (PNS == null && PitchList[NextPitchIndex].Tick > StartPoint)
                        {
                            PNS = new PitchObject(StartPoint, PN.PitchValue.NoteNumber + PitchList[NextPitchIndex].PitchValue.PitchValue);
                            ret.Add(PNS);
                        }
                        PitchObject PNP = new PitchObject(PitchList[NextPitchIndex].Tick, PN.PitchValue.NoteNumber + PitchList[NextPitchIndex].PitchValue.PitchValue);
                        ret.Add(PNP);
                        NextPitchIndex++;
                    }
                    if (ret.Count > 0)
                    {
                        if (ret[ret.Count - 1].Tick < EndPoint)
                        {
                            PNE = new PitchObject(EndPoint, ret[ret.Count - 1].PitchValue);
                            ret.Add(PNE);
                        }
                    }
                }
                RightLimit = Math.Max(RightLimit, PN.Tick+PN.Length);
            }
            return ret;

        }
        public static List<PitchObject> getShownPitchLine(ref List<PitchObject> PitchList, long MinTick=-1,long MaxTick=-1)
        {
            if (MinTick < 0) MinTick = 0;
            if (MaxTick < MinTick) MaxTick = PitchList[PitchList.Count - 1].Tick + 1;
            List<PitchObject> ret = new List<PitchObject>();
            bool isFirst = true;
            if (PitchList.Count == 0)
            {
                PitchList.Add(new PitchObject(0, 0));
            }
            PitchObject LastObj = PitchList[0];
            PitchObject PrevObj = LastObj;
            for (int i = 0; i < PitchList.Count; i++)
            {
                if (PitchList[i].Tick < MinTick)
                {
                    PrevObj = PitchList[i];
                    continue;
                }
                if (PitchList[i].Tick > MaxTick) break;
                if (isFirst && i > 0)
                {
                    LastObj = PitchList[i-1];
                    if (PitchList[i].Tick > MinTick)
                    {
                        ret.Add(new PitchObject(MinTick, LastObj.PitchValue));
                    }
                    isFirst = false;
                }
                ret.Add(new PitchObject(PitchList[i].Tick, PitchList[i].PitchValue.PitchValue));
                LastObj = PitchList[i];
            }
            if (isFirst && PrevObj.Tick<= MinTick)
            {
                ret.Add(new PitchObject(MinTick, PrevObj.PitchValue));
            }
            if (LastObj.Tick < MaxTick)
            {
                ret.Add(new PitchObject(MaxTick, LastObj.PitchValue));
            }
            return ret;
        }
        public static void earsePitchLine(ref List<NoteObject> NoteList, ref List<PitchObject> PitchList, PitchView.BlockDia NoteDia, bool ModeV2 = false)
        {
            long mt = NoteDia.TickEnd;
            long nt = NoteDia.TickStart;
            for (int i = 0; i < NoteList.Count; i++)
            {
                NoteObject PN = NoteList[i];
                if (PN.Tick >= mt) break;
                if (PN.Tick + PN.Length < nt) continue;
                if (PN.PitchValue.NoteNumber >= NoteDia.BottomNoteNum && PN.PitchValue.NoteNumber <= NoteDia.TopNoteNum)
                {
                    long St = PN.Tick;
                    long Ed = PN.Tick + PN.Length;
                    if (nt > St && nt < Ed) St = nt;
                    if (mt < Ed && mt > St) Ed = mt;
                    earseArea(ref PitchList,new PitchObject(St, PN.PitchValue.PitchValue), new PitchObject(Ed, PN.PitchValue.PitchValue));
                    if (ModeV2)
                    {
                        replacePitchLine(ref NoteList,ref PitchList,new List<PitchObject>() { 
                            new PitchObject(St+1, PN.PitchValue.NoteNumber), new PitchObject(Ed, PN.PitchValue.NoteNumber)
                        });
                    }
                    else
                    {
                        earseArea(ref PitchList, new PitchObject(St, PN.PitchValue.PitchValue), new PitchObject(Ed - 1, PN.PitchValue.PitchValue));
                    }
                }
            }
        }
        public static void earsePitchLine(ref List<PitchObject> PitchList, long StartTick, long EndTick, bool ModeV2 = false)
        {
            if (ModeV2)
            {
                List<PitchObject> PRR = new List<PitchObject>();
                PRR.Add(new PitchObject(StartTick, 0));
                PRR.Add(new PitchObject(EndTick, 0));
                replacePitchLine(ref PitchList, PRR);
            }
            else
            {
                earseArea(ref PitchList, new PitchObject(StartTick, 60), new PitchObject(EndTick, 60), false);
            }
        }
        private static void earseArea(ref List<PitchObject> PitchList, PitchObject St, PitchObject Et, bool SetStp = true)
        {
            int DelIdx = -1;
            double LastSt = PitchList.Count == 0 ? 0 : PitchList[0].PitchValue.PitchValue;
            for (int i = 0; i < PitchList.Count; i++)
            {
                LastSt = PitchList[i].PitchValue.PitchValue;
                if (PitchList[i].Tick < St.Tick - 1) continue;
                if (PitchList[i].Tick > Et.Tick + 1) break;
                DelIdx = i;
                break;
            }
            double LastEt = LastSt;
            double LastEt2 = LastSt;
            long LastLt2 = 0;
            if (DelIdx > -1)
            {
                while (DelIdx < PitchList.Count)
                {
                    if (PitchList[DelIdx].Tick > Et.Tick + 1)
                    {
                        LastLt2 = PitchList[DelIdx].Tick - 1;
                        LastEt2 = PitchList[DelIdx].PitchValue.PitchValue;
                        break;
                    }
                    LastEt = PitchList[DelIdx].PitchValue.PitchValue;
                    PitchList.RemoveAt(DelIdx);
                }
            }
            PitchList.Add(new PitchObject(St.Tick - 1, LastSt));
            if (SetStp)
            {
                PitchList.Add(new PitchObject(Et.Tick + 1, LastEt));
            }
            else
            {
                PitchList.Add(new PitchObject(LastLt2, LastEt2));
            }
        }
        public static void replacePitchLine(ref List<PitchObject> PitchList, List<PitchObject> newPitchLine)
        {
            if (newPitchLine.Count < 2) return;
            long StartTick = newPitchLine[0].Tick;
            long EndTick = newPitchLine[newPitchLine.Count - 1].Tick;
            earseArea(ref PitchList, newPitchLine[0], newPitchLine[newPitchLine.Count - 1]);
            for (int i = 0; i < newPitchLine.Count; i++)
            {
                PitchList.Add(new PitchObject(newPitchLine[i].Tick, newPitchLine[i].PitchValue.PitchValue));
            }
            PitchList.Sort();
        }
        public static void replacePitchLine(ref List<NoteObject> NoteList, ref List<PitchObject> PitchList, List<PitchObject> newPitchLine)
        {
            if (newPitchLine.Count < 2) return;
            long StartPoint = newPitchLine[0].Tick;
            long EndPoint = newPitchLine[newPitchLine.Count - 1].Tick;
            int NearSNoteIdx = -1;
            List<uint> BaseNote = new List<uint>();
            for (int i = 0; i < NoteList.Count; i++)
            {
                if (NoteList[i].Tick + NoteList[i].Length < StartPoint) continue;//在之前的，掠过
                NearSNoteIdx = i;
                break;
            }
            if (NearSNoteIdx == -1) return;

            if (NoteList[NearSNoteIdx].Tick < StartPoint)
            {
                BaseNote.Add(NoteList[NearSNoteIdx].PitchValue.NoteNumber);
            }
            for (int i = NearSNoteIdx; i < NoteList.Count; i++)
            {
                if (NoteList[i].Tick > EndPoint) break;
                int BNC = BaseNote.Count - 1;
                if (BNC == -1) BNC = 0;
                for (int j = BNC; j < newPitchLine.Count; j++)
                {
                    if (newPitchLine[j].Tick < NoteList[i].Tick + NoteList[i].Length)
                    {
                        BaseNote.Add(NoteList[i].PitchValue.NoteNumber);
                    }
                }
            }
            if (BaseNote.Count > 0 && BaseNote.Count < newPitchLine.Count)
            {
                for (int i = BaseNote.Count; i < newPitchLine.Count; i++)
                {
                    BaseNote.Add(BaseNote[BaseNote.Count - 1]);
                }
            }
            earseArea(ref PitchList,newPitchLine[0], newPitchLine[newPitchLine.Count - 1]);
            for (int i = 0; i < newPitchLine.Count; i++)
            {
                PitchList.Add(new PitchObject(newPitchLine[i].Tick, newPitchLine[i].PitchValue.PitchValue - BaseNote[i]));
            }
            PitchList.Sort();
        }
    }
}
