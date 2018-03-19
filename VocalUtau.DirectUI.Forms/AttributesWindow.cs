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
        ObjectAlloc<ProjectObject> ProjectBinder = new ObjectAlloc<ProjectObject>();
        ObjectAlloc<NoteObject> NoteBinder = new ObjectAlloc<NoteObject>();
        ObjectAlloc<PartsObject> PartsBinder = new ObjectAlloc<PartsObject>();
        public delegate void OnAttributeChangeHandler();
        public event OnAttributeChangeHandler AttributeChange;


        public AttributesWindow()
        {
            InitializeComponent();
        }
        public void ShowOnDock(DockPanel DockPanel)
        {
            this.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
        }

        public void LoadProjectObject(ref ProjectObject projects)
        {
            ProjectBinder.ReAlloc(projects);
        }

        public void LoadPartsPtr(ref PartsObject PartsObj,bool isCurrentEditing=true)
        {
            PartsBinder.ReAlloc(PartsObj);

            PartAttributes pa = new PartAttributes(PartsBinder.IntPtr,ProjectBinder.IntPtr);

            pa.setIsCurrent(isCurrentEditing);

            this.PropertyViewer.Tag = pa;

            this.PropertyViewer.SelectedObject = this.PropertyViewer.Tag;
        }
        public void LoadNotesPtr(ref PartsObject PartsObj, ref NoteObject CurrentNote)
        {
            PartsBinder.ReAlloc(PartsObj);

            NoteBinder.ReAlloc(CurrentNote);

            this.PropertyViewer.Tag = new NoteAttributes(PartsBinder.IntPtr, NoteBinder.IntPtr, ProjectBinder.IntPtr);

            this.PropertyViewer.SelectedObject = this.PropertyViewer.Tag;
        }

        private void PropertyViewer_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            /*
             (ProjectObject)ProjectBinder.AllocedObject
             */
            if (AttributeChange != null) AttributeChange();
        }
    }
}
