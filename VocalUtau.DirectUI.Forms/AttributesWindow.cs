using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.AttributeUtils.Models;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;
using WeifenLuo.WinFormsUI.Docking;

namespace VocalUtau.DirectUI.Forms
{
    public partial class AttributesWindow : DockContent
    {
        ObjectAlloc<NoteObject> NoteBinder = new ObjectAlloc<NoteObject>();

        public AttributesWindow()
        {
            InitializeComponent();
        }
        public void ShowOnDock(DockPanel DockPanel)
        {
            this.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
        }
        public void LoadPartsPtr(IntPtr PartsPtr)
        {
            this.PropertyViewer.Tag = new PartAttributes(PartsPtr);

            this.PropertyViewer.SelectedObject = this.PropertyViewer.Tag;
        }
        public void LoadNotesPtr(IntPtr PartsPtr,ref NoteObject CurrentNote)
        {
            NoteBinder.ReAlloc(CurrentNote);

            this.PropertyViewer.Tag = new NoteAttributes(PartsPtr, NoteBinder.IntPtr);

            this.PropertyViewer.SelectedObject = this.PropertyViewer.Tag;
        }
    }
}
