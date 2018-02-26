﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.DirectUI.Models;
using VocalUtau.DirectUI.Utils.PianoUtils;

namespace VocalUtau.DirectUI.Utils.ActionUtils
{
    class PitchActionUtils
    {
        public static List<PitchNode> getShownPitchLine(ref List<PianoNote> NoteList, ref List<PitchNode> PitchList, long MinTick, long MaxTick, bool ShowNoteSpace=true)
        {
            List<PitchNode> ret = new List<PitchNode>();
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
                PianoNote PN = NoteList[i];
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
                        PitchNode PNP = new PitchNode(PitchList[NextPitchIndex].Tick, PN.PitchValue.NoteNumber + PitchList[NextPitchIndex].PitchValue.PitchValue);
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
                    PitchNode PNS = new PitchNode(StartPoint, PN.PitchValue.PitchValue);
                    PitchNode PNE = new PitchNode(EndPoint, PN.PitchValue.PitchValue);
                    ret.Add(PNS);
                    ret.Add(PNE);
                }
                else
                {
                    //判定如果音符内有CP，则设置音高
                    PitchNode PNS = null;
                    PitchNode PNE = null;
                    while (NextPitchIndex < PitchList.Count && PitchList[NextPitchIndex].Tick <= EndPoint && PitchList[NextPitchIndex].Tick < MaxTick)
                    {
                        if (PitchList[NextPitchIndex].Tick < StartPoint)
                        {
                            NextPitchIndex++;
                            continue;
                        }
                        if (PNS == null && PitchList[NextPitchIndex].Tick > StartPoint)
                        {
                            PNS = new PitchNode(StartPoint, PN.PitchValue.NoteNumber + PitchList[NextPitchIndex].PitchValue.PitchValue);
                            ret.Add(PNS);
                        }
                        PitchNode PNP = new PitchNode(PitchList[NextPitchIndex].Tick, PN.PitchValue.NoteNumber + PitchList[NextPitchIndex].PitchValue.PitchValue);
                        ret.Add(PNP);
                        NextPitchIndex++;
                    }
                    if (ret.Count > 0)
                    {
                        if (ret[ret.Count - 1].Tick < EndPoint)
                        {
                            PNE = new PitchNode(EndPoint, ret[ret.Count - 1].PitchValue);
                            ret.Add(PNE);
                        }
                    }
                }
                RightLimit = Math.Max(RightLimit, PN.Tick+PN.Length);
            }
            return ret;

        }
        public static void earsePitchLine(ref List<PianoNote> NoteList, ref List<PitchNode> PitchList, PitchView.BlockDia NoteDia, bool ModeV2 = false)
        {
            long mt = NoteDia.TickEnd;
            long nt = NoteDia.TickStart;
            for (int i = 0; i < NoteList.Count; i++)
            {
                PianoNote PN = NoteList[i];
                if (PN.Tick >= mt) break;
                if (PN.Tick + PN.Length < nt) continue;
                if (PN.PitchValue.NoteNumber >= NoteDia.BottomNoteNum && PN.PitchValue.NoteNumber <= NoteDia.TopNoteNum)
                {
                    long St = PN.Tick;
                    long Ed = PN.Tick + PN.Length;
                    if (nt > St && nt < Ed) St = nt;
                    if (mt < Ed && mt > St) Ed = mt;
                    earseArea(ref PitchList,new PitchNode(St, PN.PitchValue.PitchValue), new PitchNode(Ed, PN.PitchValue.PitchValue));
                    if (ModeV2)
                    {
                        replacePitchLine(ref NoteList,ref PitchList,new List<PitchNode>() { 
                            new PitchNode(St, PN.PitchValue.NoteNumber), new PitchNode(Ed-1, PN.PitchValue.NoteNumber)
                        });
                    }
                    else
                    {
                        earseArea(ref PitchList, new PitchNode(St, PN.PitchValue.PitchValue), new PitchNode(Ed - 1, PN.PitchValue.PitchValue));
                    }
                }
            }
        }
        private static void earseArea(ref List<PitchNode> PitchList, PitchNode St, PitchNode Et)
        {
            int DelIdx = -1;
            for (int i = 0; i < PitchList.Count; i++)
            {
                if (PitchList[i].Tick < St.Tick) continue;
                if (PitchList[i].Tick > Et.Tick) break;
                DelIdx = i;
                break;
            }
            if (DelIdx > -1)
            {
                while (DelIdx < PitchList.Count)
                {
                    if (PitchList[DelIdx].Tick > Et.Tick) break;
                    PitchList.RemoveAt(DelIdx);
                }
            }
        }
        public static void replacePitchLine(ref List<PianoNote> NoteList, ref List<PitchNode> PitchList, List<PitchNode> newPitchLine)
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
                PitchList.Add(new PitchNode(newPitchLine[i].Tick, newPitchLine[i].PitchValue.PitchValue - BaseNote[i]));
            }
            PitchList.Sort();
        }
    }
}
