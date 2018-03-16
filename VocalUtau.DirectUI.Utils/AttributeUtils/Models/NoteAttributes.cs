using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.Models
{
    [DefaultProperty("Note_Lyric")]
    public class NoteAttributes:PartAttributes
    {
        IntPtr notePtr = IntPtr.Zero;
        public NoteAttributes(IntPtr PartsObjectPtr,IntPtr NotesObjectPtr):base(PartsObjectPtr)
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
                NoteObject.Lyric=value;
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

        public class PhonemeAtomListConverter : ExpandableObjectConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context,
                                   System.Type destinationType)
            {
                if (destinationType == typeof(List<NoteAtomObject>))
                    return true;
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
            {
                if (destinationType == typeof(System.String) &&
                     value is List<NoteAtomObject>)
                {
                    List<NoteAtomObject> so = (List<NoteAtomObject>)value;
                    List<string> ovo=new List<string>();
                    string Phoneme = "";
                    foreach (NoteAtomObject NAO in so)
                    {
                        ovo.Add(NAO.PhonemeAtom);
                    }
                    return String.Join("|", ovo);
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
        //http://blog.csdn.net/luyifeiniu/article/details/5426960
        [CategoryAttribute("音符信息"), DisplayName("发音部件")]
        [TypeConverterAttribute(typeof(PhonemeAtomListConverter)),  
        DescriptionAttribute("展开以查看应用程序的拼写选项。")]  
        public List<NoteAtomObject> Note_Phonemes
        {
            get
            {
              //  List<PhonemeAttributes> ret=new List<PhonemeAttributes>();
              //  ret.Add(new PhonemeAttributes());
                return NoteObject.PhonemeAtoms;
            }
            set
            {
                ;// NoteObject.PhonemeAtoms = value;
            }
        }
    }
}
