using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.DirectUI.Utils.SingerUtils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.Models
{
    [DefaultProperty("Note_Lyric")]
    public class NoteAttributes:PartAttributes
    {
        public delegate void OnPhonemeChangedHandler();
        public event OnPhonemeChangedHandler PhonemesChanged;

        IntPtr notePtr = IntPtr.Zero;
        public NoteAttributes(IntPtr PartsObjectPtr, IntPtr NotesObjectPtr, IntPtr ProjectObjectPtr)
            : base(PartsObjectPtr,ProjectObjectPtr)
        {
            notePtr = NotesObjectPtr;
        }
        public void setNotesObjectPtr(IntPtr NotesObjectPtr)
        {
            this.notePtr = NotesObjectPtr;
        }
        private NoteObject NoteObject
        {
            get
            {
                NoteObject ret = null;
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(notePtr);
                    ret = (NoteObject)handle.Target;
                }
                catch { ;}
                return ret;
            }
        }
        [CategoryAttribute("音符信息"), DisplayName("音符歌词")]//,DescriptionAttribute("歌词")]
        public string Note_Lyric
        {
            get
            {
                return NoteObject.Lyric;
            }
            set
            {
                if (NoteObject.Lyric != value)
                {
                    NoteObject.Lyric = value;
                    if (base.SingerDataFinder != null)
                    {
                        PartsObject po = PartsObject;
                        base.SingerDataFinder.GetPhonemesDictionary(base.PartsObject).UpdateLyrics(ref po,NoteObject);//.SetupPhonemes(ref po, NoteObject);
                    }
                }
            }
        }
        [CategoryAttribute("音符信息"), DisplayName("音符起点")]
        public long Note_Tick
        {
            get
            {
                return NoteObject.Tick;
            }
            set
            {
                NoteObject.Tick = value;
            }
        }
        [CategoryAttribute("音符信息"), DisplayName("音符长度")]
        public long Note_Length
        {
            get
            {
                return NoteObject.Length;
            }
            set
            {
                NoteObject.Length = value;
            }
        }
        [CategoryAttribute("音符信息"), DisplayName("音符音高")]
        public uint Note_Number
        {
            get
            {
                return NoteObject.PitchValue.NoteNumber;
            }
            set
            {
                NoteObject.PitchValue = new PitchAtomObject(value);
            }
        }
        [CategoryAttribute("音符信息"), DisplayName("颤音比例")]
        public int Note_VerbPrecent
        {
            get
            {
                return (int)(NoteObject.VerbPrecent*100);
            }
            set
            {
                double Rev=(double)value/100;
                if(Rev<=1 && Rev>=0)
                {
                    NoteObject.VerbPrecent = Rev;
                }
            }
        }

        //http://blog.csdn.net/luyifeiniu/article/details/5426960
        [CategoryAttribute("音符信息"), DisplayName("发音部件")]           
        [TypeConverterAttribute(typeof(VocalUtau.DirectUI.Utils.AttributeUtils.CategoryForms.PhonemeAtomListConverter)),  
        DescriptionAttribute("音符的发音部件集合，点击Editor编辑详情")] 
        [Editor(typeof(VocalUtau.DirectUI.Utils.AttributeUtils.CategoryForms.PhonemeAtomCategoryModelEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public VocalUtau.DirectUI.Utils.AttributeUtils.CategoryForms.PhonemeEditorModel Note_Phonemes
        {
            get
            {
                return new VocalUtau.DirectUI.Utils.AttributeUtils.CategoryForms.PhonemeEditorModel(NoteObject);
            }
            set
            {
                NoteObject.PhonemeAtoms = value.Plist;
                if (PhonemesChanged != null) PhonemesChanged();
            }
        }
        [CategoryAttribute("音符信息"), DisplayName("锁定发音部件")]
        public bool NotePhonemeLock
        {
            get
            {
                return NoteObject.LockPhoneme;
            }
            set
            {
                NoteObject.LockPhoneme = value;
            }
        }
    }
}
