using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI
{
    public class TrackerProperties
    {
        TrackerConfigures rconf;
        /// <summary>
        /// Numerator of Beats
        /// How much Beats in one Summery
        /// </summary>
        public uint BeatsCountPerSummery { get; set; }
        /// <summary>
        /// Denominator of Beats
        //  Use one type of Note as a Beat
        /// </summary>
        public NoteType BeatType { get; set; }
        public enum NoteType
        {
            Semibreve = 1,//全音符
            Minim = 2,//二分音符
            Crotchet = 4,//四分音符
            Quaver = 8,//八分音符
            Demiquaver = 16,//十六分音符
            Demisemiquaver = 32//三十二分音符
        }
        private const uint SemibreveLength = 1920;
        internal uint BeatLength
        {
            get
            {
                return (uint)(SemibreveLength / (uint)BeatType);
            }
        }

        internal TrackerProperties(TrackerConfigures tconf)
        {
            BeatsCountPerSummery = 4;
            BeatType = NoteType.Crotchet;
            this.rconf = rconf;
        }
        
        uint _TopTrackId = 0;

        public uint TopTrackId
        {
            get { return _TopTrackId; }
            set { _TopTrackId = value; }
        }
        
        public static double Tick2Time(long Tick, double Tempo)
        {
            double TickPerSecond = 8 * Tempo;
            return Tick / TickPerSecond;
        }
        public static long Time2Tick(double Time, double Tempo)
        {
            double TickPerSecond = 8 * Tempo;
            return (long)Math.Round(Time * TickPerSecond);
        }
        
        private uint _crotchetLengthPixel = 66;
        internal uint CrotchetLengthPixel
        {
            get { return _crotchetLengthPixel; }
            set
            {
                if (value < 3)
                {
                    _crotchetLengthPixel = 3;
                }
                else
                {
                    _crotchetLengthPixel = value;
                }
            }
        }
        double baseTempo = 120.0;

        public double Tempo
        {
            get { return baseTempo; }
            set
            {
                baseTempo = value;
                if (baseTempo < 30) baseTempo = 30;
                if (baseTempo > 300) baseTempo = 500;
            }
        }
        public double Tick2Time(long Tick)
        {
            double TickPerSecond = 8 * baseTempo;
            return Tick / TickPerSecond;
        }
        public long Time2Tick(double Time)
        {
            double TickPerSecond = 8 * baseTempo;
            return (long)Math.Round(Time * TickPerSecond);
        }

        public double dertTick2dertPixel(long dertTick)
        {
            double PixelPerTick = (double)(_crotchetLengthPixel * 4) / SemibreveLength;
            double ret = dertTick * PixelPerTick;
            return ret;
        }
        public double dertPixel2dertTick(long dertPixel)
        {
            double PixelPerTick = (double)(_crotchetLengthPixel * 4) / SemibreveLength;
            double ret = dertPixel / PixelPerTick;
            return ret;
        }
        private long _pianoStartTick = 0;
        internal long PianoStartTick
        {
            get
            {
                return _pianoStartTick;
            }
            set
            {
                if (value != _pianoStartTick)
                {
                    _pianoStartTick = value;
                }
            }
        }

        internal PianoRollPoint getPianoStartPoint()
        {
            //获取当前信息
            long BeatCountBefore = PianoStartTick / BeatLength;//获取之前有几个整拍子
            long BeatDenominatolBefore = PianoStartTick % BeatLength;//获取余数拍子

            PianoRollPoint ret = new PianoRollPoint();
            ret.Tick = PianoStartTick;
            ret.BeatNumber = BeatCountBefore;
            ret.DenominatolTicksBefore = BeatDenominatolBefore;
            ret.NextWholeBeatNumber = ret.BeatNumber + 1;
            ret.NextWholeBeatDistance = BeatLength - BeatDenominatolBefore;
            return ret;
        }
    }
}
