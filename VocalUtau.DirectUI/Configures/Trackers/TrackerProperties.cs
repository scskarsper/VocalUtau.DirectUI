using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI
{
    public class TrackerProperties
    {
        internal TrackerProperties(TrackerConfigures tconf)
        {
        }

        double _pixelPerMs = 0.001;

        public double PixelPerMs
        {
            get { return _pixelPerMs; }
            set { _pixelPerMs = value; }
        }

        public double Pixel2Ms(double Pixel)
        {
            return Pixel / _pixelPerMs;
        }
        public double Ms2Pixel(double Millsecond)
        {
            return _pixelPerMs*Millsecond;
        }

        uint _TopTrackId = 0;

        public uint TopTrackId
        {
            get { return _TopTrackId; }
            set { _TopTrackId = value; }
        }

        double _starttime = 0;

        public double Starttime
        {
            get { return _starttime; }
            set { _starttime = value; }
        }
    }
}
