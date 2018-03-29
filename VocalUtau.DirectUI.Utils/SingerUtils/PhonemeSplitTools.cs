using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.SingerUtils
{
    public class PhonemeSplitTools
    {
        public static List<long> SplitedTickList(long TotalLength, List<NoteAtomObject> AtomList)
        {
            List<long> TickArray = new List<long>();
            long MaxLength = TotalLength / AtomList.Count;
            long autoCount = 0;
            long noSetLength = TotalLength;
            for (int i = 0; i < AtomList.Count; i++)
            {
                NoteAtomObject nao = AtomList[i];
                if (nao.AtomLength <= 0)
                {
                    TickArray.Add(0);
                    autoCount++;
                }
                else
                {
                    if (nao.LengthIsPercent)
                    {
                        long len = (long)(TotalLength * (double)nao.AtomLength / 100);
                        if (len > MaxLength) len = MaxLength;
                        TickArray.Add(len);
                        noSetLength = noSetLength - len;
                    }
                    else
                    {
                        long len = nao.AtomLength;
                        if (len > MaxLength) len = MaxLength;
                        TickArray.Add(len);
                        noSetLength = noSetLength - len;
                    }
                }
            }
            long setAtom = (long)((double)noSetLength / (double)autoCount);
            for (int i = 0; i < TickArray.Count; i++)
            {
                if (TickArray[i] == 0)
                {
                    if (autoCount > 1)
                    {
                        TickArray[i] = setAtom;
                        noSetLength = noSetLength - setAtom;
                        autoCount--;
                    }
                    else
                    {
                        TickArray[i] = noSetLength;
                    }
                }
            }
            return TickArray;
        }

    }
}
