﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.AttributeUtils.Models;
using VocalUtau.DirectUI.Utils.SingerUtils;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;
using WeifenLuo.WinFormsUI.Docking;

namespace VocalUtau.DirectUI.Forms
{
    public partial class AttributesWindow : DockContent
    {
        SingerDataFinder SingerDataFinder;
        ObjectAlloc<ProjectObject> ProjectBinder = new ObjectAlloc<ProjectObject>();
        ObjectAlloc<NoteObject> NoteBinder = new ObjectAlloc<NoteObject>();
        ObjectAlloc<PartsObject> PartsBinder = new ObjectAlloc<PartsObject>();
        ObjectAlloc<WavePartsObject> WavePartsBinder = new ObjectAlloc<WavePartsObject>();
        public delegate void OnAttributeChangeHandler(PropertyValueChangedEventArgs e, ProjectObject oldObj);
        public event OnAttributeChangeHandler AttributeChange;
        
        public AttributesWindow()
        {
            InitializeComponent();
        }
        public void ShowOnDock(DockPanel DockPanel)
        {
            this.Show(DockPanel, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
        }

        public void SetupSingerDataFinder(ref SingerDataFinder sls)
        {
            SingerDataFinder = sls;
        }
        public object Clone(object source)
        {
            return source;
            BinaryFormatter Formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            //The internal array cannot expand to greater than Int32.MaxValue elements.
            Formatter.Serialize(stream, source);
            stream.Position = 0;
            object clonedObj = Formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }

        public void LoadProjectObject(ref ProjectObject projects)
        {
            ProjectBinder.ReAlloc(projects);
            if (projects.GlobalVolume < 100)
            {
                tbVolume.Value = projects.GlobalVolume;
            }
            else
            {
                tbVolume.Value = 100;
            }
        }
        public void LoadPartsPtr(ref PartsObject PartsObj,bool isCurrentEditing=true)
        {
            PartsBinder.ReAlloc(PartsObj);

            PartAttributes pa = new PartAttributes(PartsBinder.IntPtr,ProjectBinder.IntPtr);

            pa.setIsCurrent(isCurrentEditing);

            pa.setSingerDataFinder(SingerDataFinder);

            this.PropertyViewer.Tag = pa;

            this.PropertyViewer.SelectedObject = this.PropertyViewer.Tag;
        }
        public void LoadWavPartsPtr(ref WavePartsObject PartsObj)
        {
            WavePartsBinder.ReAlloc(PartsObj);

            WavePartAttributes pa = new WavePartAttributes(WavePartsBinder.IntPtr, ProjectBinder.IntPtr);
            
            this.PropertyViewer.Tag = pa;

            this.PropertyViewer.SelectedObject = this.PropertyViewer.Tag;
        }
        public void LoadNotesPtr(ref PartsObject PartsObj, ref NoteObject CurrentNote)
        {
            PartsBinder.ReAlloc(PartsObj);

            NoteBinder.ReAlloc(CurrentNote);

            NoteAttributes NA = new NoteAttributes(PartsBinder.IntPtr, NoteBinder.IntPtr, ProjectBinder.IntPtr);

            NA.setSingerDataFinder(SingerDataFinder);

            NA.PhonemesChanged += NA_PhonemesChanged;

            this.PropertyViewer.Tag = NA;

            this.PropertyViewer.SelectedObject = this.PropertyViewer.Tag;
        }


        ProjectObject Cache = null;
        void AddCache()
        {
            if (ProjectBinder.AllocedObject != null && ProjectBinder.AllocedObject is ProjectObject)
            {
                Cache = (ProjectObject)Clone(ProjectBinder.AllocedObject);
            }
        }
        void NA_PhonemesChanged()
        {
            if (AttributeChange != null) AttributeChange(null, Cache);
        }

        public void GuiRefresh()
        {
            PropertyViewer.Refresh();
        }

        private void PropertyViewer_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (AttributeChange != null) AttributeChange(e,Cache);
            PropertyViewer.Refresh();
            AddCache();
        }

        private void PropertyViewer_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
        {
            AddCache();
        }

        private void MemoryCleaner_Tick(object sender, EventArgs e)
        {
            GC.Collect(0, GCCollectionMode.Forced);
        }

        private void PlayerPanel_ActiveContentChanged(object sender, EventArgs e)
        {

        }
        public void CurrentPosTime(TimeSpan Ts)
        {
            this.Invoke(new Action(() =>
            {
                lbl_PosCurrent.Text = String.Format("{0:00}:{1:00}.{2:000}", Ts.TotalMinutes, Ts.Seconds, Ts.Milliseconds > 0 ? Ts.Milliseconds : 0);
            }));
        }
        public void CurrentRenderTime(TimeSpan Ts)
        {
            this.Invoke(new Action(() =>
            {
                lbl_RenderCurrent.Text = String.Format("{0:00}:{1:00}.{2:000}", Ts.TotalMinutes, Ts.Seconds, Ts.Milliseconds > 0 ? Ts.Milliseconds : 0);
            }));
        }
        public void TotalRenderTime(TimeSpan Ts)
        {
            this.Invoke(new Action(() =>
            {
                lbl_RenderTotal.Text = String.Format("{0:00}:{1:00}.{2:000}", Ts.TotalMinutes, Ts.Seconds, Ts.Milliseconds > 0 ? Ts.Milliseconds : 0);
            }));
        }
        public enum RenderStatus
        {
            Ready,
            Buffing,
            Playing,
            Paused,
            Stop
        }
        public void SetRenderStatus(RenderStatus Status)
        {
            this.Invoke(new Action(() =>
            {
                switch (Status)
                {
                    case RenderStatus.Ready: lbl_RenderStatus.Text = "准备就绪"; break;
                    case RenderStatus.Buffing: lbl_RenderStatus.Text = "正在缓冲"; break;
                    case RenderStatus.Playing: lbl_RenderStatus.Text = "正在播放"; break;
                    case RenderStatus.Paused: lbl_RenderStatus.Text = "播放暂停"; break;
                    case RenderStatus.Stop: lbl_RenderStatus.Text = "正在停止"; break;
                }
            }));
        }
        
        public delegate void OnGlobalVolumeChange(int NewVolume);
        public event OnGlobalVolumeChange GlobalVolumeChanged;
        public delegate void OnChannelVolumeChange(int ChannelID,int NewVolume);
        public event OnChannelVolumeChange ChannelVolumeChanged;
        private void tbVolume_ValueChanged(object sender, EventArgs e)
        {
            if (ProjectBinder.AllocedObject is ProjectObject)
            {
                ProjectObject po = (ProjectObject)ProjectBinder.AllocedObject;
                if (po.GlobalVolume != tbVolume.Value)
                {
                    po.GlobalVolume = tbVolume.Value;
                    if (GlobalVolumeChanged != null)
                    {
                        GlobalVolumeChanged(po.GlobalVolume);
                    }
                }
            }
        }

        private void btnMixerWin_Click(object sender, EventArgs e)
        {
            SoundMixer sm = new SoundMixer(ProjectBinder);
            sm.ChannelVolumeChanged += sm_ChannelVolumeChanged;
            sm.GlobalVolumeChanged += sm_GlobalVolumeChanged;
            sm.Show();
        }

        void sm_GlobalVolumeChanged(int NewVolume)
        {
            ProjectObject po = (ProjectObject)ProjectBinder.AllocedObject;
            if (po.GlobalVolume != NewVolume)
            {
                po.GlobalVolume = NewVolume;
                if (GlobalVolumeChanged != null)
                {
                    GlobalVolumeChanged(NewVolume);
                }
            }
        }

        void sm_ChannelVolumeChanged(int ChannelID, int NewVolume)
        {
            double floatV = NewVolume / 100.0;
            ProjectObject po = (ProjectObject)ProjectBinder.AllocedObject;
            if (ChannelID < po.TrackerList.Count)
            {
                if (po.TrackerList[ChannelID].getVolume() != floatV)
                {
                    po.TrackerList[ChannelID].setVolume(floatV);
                    if (ChannelVolumeChanged != null)
                    {
                        ChannelVolumeChanged(ChannelID, NewVolume);
                    }
                }
            }
            else
            {
                int Id = ChannelID - po.TrackerList.Count;
                if (po.TrackerList[Id].getVolume() != floatV)
                {
                    po.TrackerList[Id].setVolume(floatV);
                    if (ChannelVolumeChanged != null)
                    {
                        ChannelVolumeChanged(ChannelID, NewVolume);
                    }
                }
            }
        }
    }
}
