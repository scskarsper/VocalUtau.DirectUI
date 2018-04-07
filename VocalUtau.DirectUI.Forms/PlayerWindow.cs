using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VocalUtau.Calculators;
using VocalUtau.Wavtools.Render;
using VocalUtau.WavTools;
using VocalUtau.WavTools.Model.Pipe;
using VocalUtau.WavTools.Model.Wave.NAudio.Extra;
using WeifenLuo.WinFormsUI.Docking;

namespace VocalUtau.DirectUI.Forms
{
    public partial class PlayerWindow : DockContent
    {
        double prebuffertime = 1000;
        double delaybufftime = 0;//3000;

        public PlayerWindow()
        {
            InitializeComponent();
            _RendPlayer = new Dictionary<int, CachePlayer>();
            _CommandList = new Dictionary<int, List<NoteListCalculator.NotePreRender>>();
        }

        Dictionary<int, CachePlayer> _RendPlayer;
        Dictionary<int, List<NoteListCalculator.NotePreRender>> _CommandList;

        public void AddPlayTrack(int idx, List<NoteListCalculator.NotePreRender> lst)
        {
            _CommandList.Add(idx, lst);
            _RendPlayer.Add(idx, new CachePlayer());
        }
        public void RendingTrack(int idx)
        {
            Task.Factory.StartNew((object idxobj) =>
            {
                int Index = (int)idxobj;
                List<NoteListCalculator.NotePreRender> lst = _CommandList[Index];
                CachePlayer Player = _RendPlayer[Index];

                string temp = System.Environment.GetEnvironmentVariable("TEMP");
                DirectoryInfo info = new DirectoryInfo(temp);
                DirectoryInfo baseDir = info.CreateSubdirectory("hymn1");
                Player.StartRending(baseDir, lst);
            },idx);
        }

        private void PlayerWindow_Load(object sender, EventArgs e)
        {
            
        }
        public void Pause()
        {
        }
        public void Resume()
        {
        }
        public void ToDispose()
        {
        }
        private void PlayerWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ToDispose();
        }
        public void PlayAll()
        {
            for (int i = 0; i < _RendPlayer.Count; i++)
            {
                _RendPlayer[i].Play();
            }
        }

        private void BufferBfb_Click(object sender, EventArgs e)
        {

        }
    }
}
