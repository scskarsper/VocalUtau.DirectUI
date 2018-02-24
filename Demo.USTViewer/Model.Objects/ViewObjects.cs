using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.DirectUI.Models;

namespace Demo.USTViewer.Model.Objects
{
    public class ViewObjects
    {
        public ViewObjects()
        {
            _NoteList = new List<PianoNote>();
            _PitchList = new List<PitchNode>();
            notelisthandle = GCHandle.Alloc(_NoteList);
            pitchlisthandle = GCHandle.Alloc(_PitchList);
        }
        ~ViewObjects()
        {
            notelisthandle.Free();
            pitchlisthandle.Free();
        }
        List<PianoNote> _NoteList;
        GCHandle notelisthandle;

        public IntPtr NotelistPtr
        {
            get
            {
                return GCHandle.ToIntPtr(notelisthandle);
            }
        }

        public List<PianoNote> NoteList
        {
            get { return _NoteList; }
        }
        List<PitchNode> _PitchList;
        GCHandle pitchlisthandle;

        public IntPtr PitchlistPtr
        {
            get
            {
                return GCHandle.ToIntPtr(pitchlisthandle);
            }
        }

        public List<PitchNode> PitchList
        {
            get { return _PitchList; }
        }
    }
}
