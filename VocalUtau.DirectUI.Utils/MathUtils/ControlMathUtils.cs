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
        public static List<ControlObject> CalcLineSilk(ControlObject S1, ControlObject S2,long MinTickDert=1)
        {
            if (S1 == null) return new List<ControlObject>();
            if (S2 == null) return new List<ControlObject>();
            if (MinTickDert < 1) MinTickDert = 1;

            ControlObject M1 = S1;
            ControlObject M2 = S2;
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
            List<ControlObject> ret = new List<ControlObject>();
            for (long i = M1.Tick; i <= M2.Tick; i += MinTickDert)
            {
                ret.Add(new ControlObject(i, pK*i+pB));
            }
            return ret;
        }
        public static List<ControlObject> CalcGraphR(ControlObject S1, ControlObject S2, long MinTickDert = 1)
        {
            return CalcGraphJ(S2, S1, MinTickDert);
        }
        public static List<ControlObject> CalcGraphJ(ControlObject S1, ControlObject S2, long MinTickDert = 1)
        {
            if (S1 == null) return new List<ControlObject>();
            if (S2 == null) return new List<ControlObject>();
            if (MinTickDert < 1) MinTickDert = 1;
            //系数计算
            //0点坐标
            double B = pi/(2*((double)S2.Tick - (double)S1.Tick));
            double C = -B * S1.Tick;
            double A = Math.Abs(S1.Value - S2.Value);

            List<ControlObject> ret = new List<ControlObject>();
            for (long i = Math.Min(S1.Tick, S2.Tick); i <= Math.Max(S1.Tick, S2.Tick); i += MinTickDert)
            {
                if (S2.Value <= S1.Value)
                {
                    ret.Add(new ControlObject(i, S2.Value + A * Math.Cos(B * i + C)));
                }
                else
                {
                    ret.Add(new ControlObject(i, S1.Value + A-A * Math.Cos(B * i + C)));
                }
            }
            return ret;
        }
        public static List<ControlObject> CalcGraphS(ControlObject S1, ControlObject S2, long MinTickDert = 1)
        {
            if (S1 == null) return new List<ControlObject>();
            if (S2 == null) return new List<ControlObject>();
            if (MinTickDert < 1) MinTickDert = 1;
            //系数计算
            //0点坐标
            double B = pi / (((double)S2.Tick - (double)S1.Tick));
            double C = -B * S1.Tick;
            double A = Math.Abs(S1.Value - S2.Value)/2;

            List<ControlObject> ret = new List<ControlObject>();
            for (long i = Math.Min(S1.Tick, S2.Tick); i <= Math.Max(S1.Tick, S2.Tick); i += MinTickDert)
            {
                if (S2.Value <= S1.Value)
                {
                    ret.Add(new ControlObject(i, S2.Value + A + A * Math.Cos(B * i + C)));
                }
                else
                {
                    ret.Add(new ControlObject(i, S1.Value + A - A * Math.Cos(B * i + C)));
                }
            }
            return ret;
        }
    }
}
