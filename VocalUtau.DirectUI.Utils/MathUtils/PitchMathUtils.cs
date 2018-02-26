using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.DirectUI.Models;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.MathUtils
{
    public class PitchMathUtils
    {
        const double pi = 3.1415926;
        public static List<PitchObject> CalcLineSilk(PitchObject S1, PitchObject S2,long MinTickDert=1)
        {
            if (S1 == null) return new List<PitchObject>();
            if (S2 == null) return new List<PitchObject>();
            if (MinTickDert < 1) MinTickDert = 1;

            PitchObject M1 = S1;
            PitchObject M2 = S2;
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
            List<PitchObject> ret = new List<PitchObject>();
            for (long i = M1.Tick; i <= M2.Tick; i += MinTickDert)
            {
                ret.Add(new PitchObject(i, pK*i+pB));
            }
            return ret;
        }
        public static List<PitchObject> CalcGraphR(PitchObject S1, PitchObject S2, long MinTickDert = 1)
        {
            return CalcGraphJ(S2, S1, MinTickDert);
        }
        public static List<PitchObject> CalcGraphJ(PitchObject S1, PitchObject S2, long MinTickDert = 1)
        {
            if (S1 == null) return new List<PitchObject>();
            if (S2 == null) return new List<PitchObject>();
            if (MinTickDert < 1) MinTickDert = 1;
            //系数计算
            //0点坐标
            double B = pi/(2*((double)S2.Tick - (double)S1.Tick));
            double C = -B * S1.Tick;
            double A = Math.Abs(S1.PitchValue.PitchValue - S2.PitchValue.PitchValue);

            List<PitchObject> ret = new List<PitchObject>();
            for (long i = Math.Min(S1.Tick, S2.Tick); i <= Math.Max(S1.Tick, S2.Tick); i += MinTickDert)
            {
                if (S2.PitchValue.PitchValue <= S1.PitchValue.PitchValue)
                {
                    ret.Add(new PitchObject(i, S2.PitchValue.PitchValue + A * Math.Cos(B * i + C)));
                }
                else
                {
                    ret.Add(new PitchObject(i, S1.PitchValue.PitchValue + A-A * Math.Cos(B * i + C)));
                }
            }
            return ret;
        }
        public static List<PitchObject> CalcGraphS(PitchObject S1, PitchObject S2, long MinTickDert = 1)
        {
            if (S1 == null) return new List<PitchObject>();
            if (S2 == null) return new List<PitchObject>();
            if (MinTickDert < 1) MinTickDert = 1;
            //系数计算
            //0点坐标
            double B = pi / (((double)S2.Tick - (double)S1.Tick));
            double C = -B * S1.Tick;
            double A = Math.Abs(S1.PitchValue.PitchValue - S2.PitchValue.PitchValue)/2;

            List<PitchObject> ret = new List<PitchObject>();
            for (long i = Math.Min(S1.Tick, S2.Tick); i <= Math.Max(S1.Tick, S2.Tick); i += MinTickDert)
            {
                if (S2.PitchValue.PitchValue <= S1.PitchValue.PitchValue)
                {
                    ret.Add(new PitchObject(i, S2.PitchValue.PitchValue + A + A * Math.Cos(B * i + C)));
                }
                else
                {
                    ret.Add(new PitchObject(i, S1.PitchValue.PitchValue + A - A * Math.Cos(B * i + C)));
                }
            }
            return ret;
        }
    }
}
