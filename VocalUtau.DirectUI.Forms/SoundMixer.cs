using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Forms.UserControles;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Forms
{
    public partial class SoundMixer : Form
    {
        VolumeMixer MainMixer;
        List<VolumeMixer> TrackMixer = new List<VolumeMixer>();

        public event VocalUtau.DirectUI.Forms.AttributesWindow.OnGlobalVolumeChange GlobalVolumeChanged;
        public event VocalUtau.DirectUI.Forms.AttributesWindow.OnChannelVolumeChange ChannelVolumeChanged;

        ObjectAlloc<ProjectObject> ProjectBinder;
        public SoundMixer(ObjectAlloc<ProjectObject> ProjectBinder)
        {
            InitializeComponent();
            this.ProjectBinder = ProjectBinder;
            this.TopMost = true;
            this.topMost.Checked = true;
            UpdateWin();
        }

        public void SetBinder(ObjectAlloc<ProjectObject> ProjectBinder)
        {
            this.ProjectBinder = ProjectBinder;
            UpdateWin();
        }

        void InitVolume(ref VolumeMixer VMix,int Volume)
        {
            VMix.Volume = Volume % 100;
            VMix.Magnification = Volume / 100;
            if (VMix.Volume == 0)
            {
                if (VMix.Magnification > 0)
                {
                    VMix.Magnification = VMix.Magnification - 1;
                    VMix.Volume = 100;
                }
                else
                {
                    VMix.Mute = true;
                }
            }
        }

        public void UpdateWin()
        {
            //Update全局
            MainMixer = new VolumeMixer();
            TrackMixer.Clear();
            
            MainMixer.TrackName = "全局";
            InitVolume(ref MainMixer,ProjectBinder.AllocedSource.GlobalVolume);
            MainMixer.OnVolumeChanged += MainMixer_OnVolumeChanged;

            for (int i = 0; i < ProjectBinder.AllocedSource.TrackerList.Count; i++)
            {
                VolumeMixer cmixer = new VolumeMixer();
                cmixer.Volume=(int)(ProjectBinder.AllocedSource.TrackerList[i].getVolume()*100.0);
                cmixer.TrackName = "[T]"+ProjectBinder.AllocedSource.TrackerList[i].getName();
                cmixer.OnVolumeChanged += Mixer_OnVolumeChanged;
                if (cmixer.TrackName == "[T]")
                {
                    cmixer.TrackName = "[T]#" + TrackMixer.Count;
                }
                TrackMixer.Add(cmixer);
            }

            for (int i = 0; i < ProjectBinder.AllocedSource.BackerList.Count; i++)
            {
                VolumeMixer cmixer = new VolumeMixer();
                cmixer.Volume = (int)(ProjectBinder.AllocedSource.BackerList[i].getVolume() * 100.0);
                cmixer.TrackName = "[B]" + ProjectBinder.AllocedSource.BackerList[i].getName();
                cmixer.OnVolumeChanged += Mixer_OnVolumeChanged;
                if (cmixer.TrackName == "[B]")
                {
                    cmixer.TrackName = "[B]#" + TrackMixer.Count;
                }
                TrackMixer.Add(cmixer);
            }
            
            flowLayoutPanelMain.Controls.Clear();
            flowLayoutPanelMain.Controls.Add(MainMixer);

            flowLayoutPanelTrack.Controls.Clear();
            flowLayoutPanelTrack.Controls.AddRange(TrackMixer.ToArray());

        }

        void Mixer_OnVolumeChanged(VolumeMixer Sender, int TotalVolume)
        {
            if (ChannelVolumeChanged != null)
            {
                int idx = TrackMixer.IndexOf(Sender);
                ChannelVolumeChanged(idx,TotalVolume);
            }
        }

        void MainMixer_OnVolumeChanged(VolumeMixer Sender, int TotalVolume)
        {
            if (GlobalVolumeChanged != null)
            {
                GlobalVolumeChanged(TotalVolume);
            }
        }

        private void topMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = this.topMost.Checked;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateWin();
        }
    }
}
