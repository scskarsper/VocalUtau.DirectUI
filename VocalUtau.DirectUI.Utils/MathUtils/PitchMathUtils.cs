using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.DirectUI.Models;

namespace VocalUtau.DirectUI.Utils.MathUtils
{
    public class PitchMathUtils
    {
        const double pi = 3.1415926;
        public static List<PitchNode> CalcLineSilk(PitchNode S1, PitchNode S2,long MinTickDert=1)
        {
            if (S1 == null) return new List<PitchNode>();
            if (S2 == null) return new List<PitchNode>();
            if (MinTickDert < 1) MinTickDert = 1;

            PitchNode M1 = S1;
            PitchNode M2 = S2;
            if (S1.Tick >= S2.Tick)
            {
                M1 = S2;
                M2 = S1;
            }

            double pK, pB;
            if (M1.PitchValue.PitchValue == M2.PitchValue.PitchValue) pK = 0;
            else pK = (M2.PitchValue.PitchValue - M1.PitchValue.PitchValue) / (M2.Tick - M1.Tick);
            pB = M2.PitchValue.PitchValue - pK * M2.Tick;
            double P1 = pK * M1.Tick + pB;
            double P2 = pK * M2.Tick + pB;
            List<PitchNode> ret = new List<PitchNode>();
            for (long i = M1.Tick; i <= M2.Tick; i += MinTickDert)
            {
                ret.Add(new PitchNode(i, pK*i+pB));
            }
            return ret;
        }
        public static List<PitchNode> CalcGraphR(PitchNode S1, PitchNode S2, long MinTickDert = 1)
        {
            return CalcGraphJ(S2, S1, MinTickDert);
        }
        public static List<PitchNode> CalcGraphJ(PitchNode S1, PitchNode S2, long MinTickDert = 1)
        {
            if (S1 == null) return new List<PitchNode>();
            if (S2 == null) return new List<PitchNode>();
            if (MinTickDert < 1) MinTickDert = 1;
            //系数计算
            //0点坐标
            double B = pi/(2*((double)S2.Tick - (double)S1.Tick));
            double C = -B * S1.Tick;
            double A = Math.Abs(S1.PitchValue.PitchValue - S2.PitchValue.PitchValue);

            List<PitchNode> ret = new List<PitchNode>();
            for (long i = Math.Min(S1.Tick, S2.Tick); i <= Math.Max(S1.Tick, S2.Tick); i += MinTickDert)
            {
                if (S2.PitchValue.PitchValue <= S1.PitchValue.PitchValue)
                {
                    ret.Add(new PitchNode(i, S2.PitchValue.PitchValue + A * Math.Cos(B * i + C)));
                }
                else
                {
                    ret.Add(new PitchNode(i, S1.PitchValue.PitchValue + A-A * Math.Cos(B * i + C)));
                }
            }
            return ret;
        }
        public static List<PitchNode> CalcGraphS(PitchNode S1, PitchNode S2, long MinTickDert = 1)
        {
            if (S1 == null) return new List<PitchNode>();
            if (S2 == null) return new List<PitchNode>();
            if (MinTickDert < 1) MinTickDert = 1;
            //系数计算
            //0点坐标
            double B = pi / (((double)S2.Tick - (double)S1.Tick));
            double C = -B * S1.Tick;
            double A = Math.Abs(S1.PitchValue.PitchValue - S2.PitchValue.PitchValue)/2;

            List<PitchNode> ret = new List<PitchNode>();
            for (long i = Math.Min(S1.Tick, S2.Tick); i <= Math.Max(S1.Tick, S2.Tick); i += MinTickDert)
            {
                if (S2.PitchValue.PitchValue <= S1.PitchValue.PitchValue)
                {
                    ret.Add(new PitchNode(i, S2.PitchValue.PitchValue + A + A * Math.Cos(B * i + C)));
                }
                else
                {
                    ret.Add(new PitchNode(i, S1.PitchValue.PitchValue + A - A * Math.Cos(B * i + C)));
                }
            }
            return ret;
        }
    }
}
