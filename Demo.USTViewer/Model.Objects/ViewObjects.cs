using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.DirectUI.Models;
using VocalUtau.Formats.Model.VocalObject;

namespace Demo.USTViewer.Model.Objects
{
    public class ViewObjects
    {
        public ViewObjects()
        {
            _NoteList = new List<NoteObject>();
            _PitchList = new List<PitchObject>();
            notelisthandle = GCHandle.Alloc(_NoteList);
            pitchlisthandle = GCHandle.Alloc(_PitchList);
        }
        ~ViewObjects()
        {
            notelisthandle.Free();
            pitchlisthandle.Free();
        }
        List<NoteObject> _NoteList;
        GCHandle notelisthandle;

        public IntPtr NotelistPtr
        {
            get
            {
                return GCHandle.ToIntPtr(notelisthandle);
            }
        }

        public List<NoteObject> NoteList
        {
            get { return _NoteList; }
        }
        List<PitchObject> _PitchList;
        GCHandle pitchlisthandle;

        public IntPtr PitchlistPtr
        {
            get
            {
                return GCHandle.ToIntPtr(pitchlisthandle);
            }
        }

        public List<PitchObject> PitchList
        {
            get { return _PitchList; }
        }
    }
}
