﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VocalUtau.DirectUI.Models
{
    public class PitchNode : IComparable, IComparer<PitchNode> 
    {
        public long Tick { get; set; }
        public long Length { get; set; }
        public VocalUtau.DirectUI.PitchValuePair.OctaveTypeEnum OctaveType { get { return this.pvp.OctaveType; } set { this.pvp.OctaveType = value; } }
        public PitchNode(long Tick, double PitchValue)
        {
            this.pvp = new PitchValuePair(PitchValue);
            this.Tick = Tick;
        }
        public PitchNode(long Tick, uint NoteNumber, int PitchWheel)
        {
            this.pvp = new PitchValuePair(NoteNumber, PitchWheel);
            this.Tick = Tick;
        }
        public PitchNode(long Tick, PitchValuePair PitchValue)
        {
            this.pvp = PitchValue;
            this.Tick = Tick;
        }
        PitchValuePair pvp = new PitchValuePair(60);
        public PitchValuePair PitchValue
        {
            get
            {
                return pvp;
            }
            set
            {
                pvp = value;
            }
        }

        public int CompareTo(Object o)
        {
            if (this.Tick > ((PitchNode)o).Tick)
                return 1;
            else if (this.Tick == ((PitchNode)o).Tick)
                return 0;
            else
                return -1;
        }

        public int Compare(PitchNode x, PitchNode y)
        {
            if (x.Tick < y.Tick)
                return -1;
            else if (x.Tick == y.Tick)
                return 0;
            else
                return 1;
        }  
    }
}
