using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VocalUtau.DirectUI.Forms.UserControles
{
    public partial class VolumeMixer : UserControl
    {
        public delegate void VolumeChangedHandler(VolumeMixer Sender,int TotalVolume);
        public event VolumeChangedHandler OnVolumeChanged;

        bool _Mute = false;
        public bool Mute
        {
            get { return _Mute;}
            set { _Mute = value ;
            if (value)
            {
                btnMuteUnmute.ImageKey = "Mute.png";
            }
            else
            {
                btnMuteUnmute.ImageKey = "UnMute.png";
            }
            }
        }

        int _Volume = 100;

        public int Volume
        {
            get { return _Volume; }
            set { _Volume = value;
            tbVolume.Value = value;
            num_Volume.Value = value;
            CheckMute();
            }
        }

        string _TrackName = "TrackName";

        public string TrackName
        {
            get { return _TrackName; }
            set { _TrackName = value;
            txt_tName.Text = value;
            }
        }

        int _Magnification = 0;

        public int Magnification
        {
            get { return _Magnification; }
            set {
                if (value <= 9 && value >= 0)
                {
                    _Magnification = value;
                }
                else if (value > 9)
                {
                    _Magnification = 9;
                }
                else if (value < 0)
                {
                    _Magnification = 0;
                }
                num_Magnification.Value = _Magnification*100;
                CheckMute();
            }
        }

        void CheckMute()
        {
            if (_Volume > 0 || Magnification > 0)
            {
                Mute = false;
            }
            else
            {
                Mute = true;
            }
        }

        public VolumeMixer()
        {
            InitializeComponent();
        }


        private void btnMuteUnmute_Click(object sender, EventArgs e)
        {
            if (Volume > 0 || Magnification > 0)
            {
                Magnification = 0;
                Volume = 0;
                if (OnVolumeChanged != null) OnVolumeChanged(this,Volume + Magnification * 100);
            }
        }

        private void tbVolume_Scroll(object sender, EventArgs e)
        {
            Volume = tbVolume.Value;
            if (OnVolumeChanged != null) OnVolumeChanged(this, Volume + Magnification * 100);
        }

        private void num_Magnification_ValueChanged(object sender, EventArgs e)
        {
            Magnification = (int)(num_Magnification.Value / 100);
            if (OnVolumeChanged != null) OnVolumeChanged(this, Volume + Magnification * 100);
        }

        private void num_Volume_ValueChanged(object sender, EventArgs e)
        {
            Volume = (int)num_Volume.Value;
            if (OnVolumeChanged != null) OnVolumeChanged(this, Volume + Magnification * 100);
        }
    }
}
