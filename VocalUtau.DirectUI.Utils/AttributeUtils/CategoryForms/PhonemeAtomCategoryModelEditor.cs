using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.CategoryForms
{
    public class PhonemeAtomListConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context,
                               System.Type destinationType)
        {
            if (destinationType == typeof(PhonemeEditorModel))
                return true;
            return base.CanConvertTo(context, destinationType);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            if (destinationType == typeof(System.String) &&
                 value is PhonemeEditorModel)
            {
                List<NoteAtomObject> so = ((PhonemeEditorModel)value).Plist;
                List<string> ovo = new List<string>();
                foreach (NoteAtomObject NAO in so)
                {
                    ovo.Add(NAO.PhonemeAtom);
                }
                return String.Join("|", ovo);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    public class PhonemeEditorModel
    {
        List<NoteAtomObject> _Plist = null;

        public List<NoteAtomObject> Plist
        {
            get { return _Plist; }
            set { _Plist = value; }
        }

        string _Lyric = "";

        public string Lyric
        {
            get { return _Lyric; }
            set { _Lyric = value; }
        }
        long _Length = 0;

        public long Length
        {
            get { return _Length; }
            set { _Length = value; }
        }

        public PhonemeEditorModel(NoteObject note)
        {
            _Lyric = note.Lyric;
            _Length = note.Length;
            _Plist = note.PhonemeAtoms;
        }
    }
    public class PhonemeAtomCategoryModelEditor : System.Drawing.Design.UITypeEditor
    {
        public PhonemeAtomCategoryModelEditor()
        {
        }
        public override System.Drawing.Design.UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService service = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

            if (service == null)
            {
                return null;
            }

            PhonemeAtomCategoryWindow form = new PhonemeAtomCategoryWindow(value);
            if (service.ShowDialog(form) == DialogResult.OK)
            {
                PhonemeEditorModel pmodel = (PhonemeEditorModel)value;
                pmodel.Plist = form.ListValue;
                return pmodel;
            }

            return value;
        }
    }
}
