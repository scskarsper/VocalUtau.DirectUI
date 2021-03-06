﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Models
{
    public class ParamValuePair
    {
        private bool _isBindedNote = false;

        public bool IsBindedNote
        {
            get { return _isBindedNote; }
        }
        public long Tick;
        public long Value;
        public NoteObject PNote;
        public ParamValuePair(long Tick, long Value)
        {
            this.Tick = Tick;
            this.Value = Value;
            _isBindedNote = false;
        }
        public ParamValuePair(NoteObject Note, long Value)
        {
            this.PNote = Note;
            this.Tick = PNote.Tick;
            this.Value = Value;
            _isBindedNote = true;
        }

    }
}
