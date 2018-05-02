using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.DirectUI.Models;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.MathUtils
{
    public class ControlMathUtils
    {
        const double pi = 3.1415926;
        public static List<TickControlObject> CalcLineSilk(TickControlObject S1, TickControlObject S2,long MinTickDert=1)
        {
            if (S1 == null) return new List<TickControlObject>();
            if (S2 == null) return new List<TickControlObject>();
            if (MinTickDert < 1) MinTickDert = 1;

            TickControlObject M1 = S1;
            TickControlObject M2 = S2;
            if (S1.Tick >= S2.Tick)
            {
                M1 = S2;
                M2 = S1;
            }

            double pK, pB;
            if (M1.Value == M2.Value) pK = 0;
            else pK = (M2.Value - M1.Value) / (M2.Tick - M1.Tick);
            pB = M2.Value - pK * M2.Tick;
            double P1 = pK * M1.Tick + pB;
            double P2 = pK * M2.Tick + pB;
            List<TickControlObject> ret = new List<TickControlObject>();
            for (long i = M1.Tick; i <= M2.Tick; i += MinTickDert)
            {
                ret.Add(new TickControlObject(i, pK*i+pB));
            }
            return ret;
        }
        public static List<TickControlObject> CalcGraphR(TickControlObject S1, TickControlObject S2, long MinTickDert = 1)
        {
            return CalcGraphJ(S2, S1, MinTickDert);
        }
        public static List<TickControlObject> CalcGraphJ(TickControlObject S1, TickControlObject S2, long MinTickDert = 1)
        {
            if (S1 == null) return new List<TickControlObject>();
            if (S2 == null) return new List<TickControlObject>();
            if (MinTickDert < 1) MinTickDert = 1;
            //系数计算
            //0点坐标
            double B = pi/(2*((double)S2.Tick - (double)S1.Tick));
            double C = -B * S1.Tick;
            double A = Math.Abs(S1.Value - S2.Value);

            List<TickControlObject> ret = new List<TickControlObject>();
            for (long i = Math.Min(S1.Tick, S2.Tick); i <= Math.Max(S1.Tick, S2.Tick); i += MinTickDert)
            {
                if (S2.Value <= S1.Value)
                {
                    ret.Add(new TickControlObject(i, S2.Value + A * Math.Cos(B * i + C)));
                }
                else
                {
                    ret.Add(new TickControlObject(i, S1.Value + A-A * Math.Cos(B * i + C)));
                }
            }
            return ret;
        }
        public static List<TickControlObject> CalcGraphS(TickControlObject S1, TickControlObject S2, long MinTickDert = 1)
        {
            if (S1 == null) return new List<TickControlObject>();
            if (S2 == null) return new List<TickControlObject>();
            if (MinTickDert < 1) MinTickDert = 1;
            //系数计算
            //0点坐标
            double B = pi / (((double)S2.Tick - (double)S1.Tick));
            double C = -B * S1.Tick;
            double A = Math.Abs(S1.Value - S2.Value)/2;

            List<TickControlObject> ret = new List<TickControlObject>();
            for (long i = Math.Min(S1.Tick, S2.Tick); i <= Math.Max(S1.Tick, S2.Tick); i += MinTickDert)
            {
                if (S2.Value <= S1.Value)
                {
                    ret.Add(new TickControlObject(i, S2.Value + A + A * Math.Cos(B * i + C)));
                }
                else
                {
                    ret.Add(new TickControlObject(i, S1.Value + A - A * Math.Cos(B * i + C)));
                }
            }
            return ret;
        }
    }
}
