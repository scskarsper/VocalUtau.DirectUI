using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Calculators;
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
            _TrackPlayers = new Dictionary<int, AppWrapper>();
            _TrackFiles = new Dictionary<int, FileStream>();
            _TrackRPC = new Dictionary<int, Pipe_Server>();
            _TrackPreBufferTime = new Dictionary<int, double>();
            _TrackDelayBufferTime = new Dictionary<int, double>();
            _ReverseDic = new Dictionary<string, int>();
            _CommandList = new Dictionary<int, List<NoteListCalculator.NotePreRender>>();
        }

        Dictionary<int, AppWrapper> _TrackPlayers;
        Dictionary<int, FileStream> _TrackFiles;
        Dictionary<int, Pipe_Server> _TrackRPC;
        Dictionary<int, double> _TrackPreBufferTime;
        Dictionary<int, double> _TrackDelayBufferTime;
        Dictionary<string, int> _ReverseDic;
        Dictionary<int, List<NoteListCalculator.NotePreRender>> _CommandList;

        private void InitPlayer(int Index)
        {
            string DomainName="VolB."+Process.GetCurrentProcess().Id.ToString()+"."+Index.ToString();
            _ReverseDic.Add(DomainName, Index);
            string temp = System.Environment.GetEnvironmentVariable("TEMP");
            DirectoryInfo info = new DirectoryInfo(temp);
            string Folder = info.FullName;
            try
            {
                Folder = info.CreateSubdirectory("Chorista\\Instance." + Process.GetCurrentProcess().Id.ToString() + "\\" + DomainName).FullName;
            }
            catch { ;}


            FileStream Fs = new FileStream(Folder+"\\"+DomainName+".wav", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            byte[] head = IOHelper.GenerateHead();
            Fs.Write(head, 0, head.Length);
            Fs.Seek(head.Length, SeekOrigin.Begin);
            long headSize = head.Length;
            _TrackFiles.Add(Index, Fs);
            
            FormatHelper fh = new FormatHelper(IOHelper.NormalPcmMono16_Format);
            AppWrapper bplayer = new AppWrapper(DomainName, AppWrapper.WrapperType.Buffer_Player);
            bplayer.CreateObject(new object[] { Fs, headSize + (long)fh.Ms2Bytes(delaybufftime) });
            bplayer.getInstanceObject<VocalUtau.WavTools.Model.Player.BufferedPlayer>().InitPlayer();
            _TrackPreBufferTime.Add(Index, prebuffertime);
            _TrackDelayBufferTime.Add(Index, delaybufftime);
           // bplayer.getInstanceObject<VocalUtau.WavTools.Model.Player.BufferedPlayer>().Buffer_Play();
            _TrackPlayers.Add(Index, bplayer);

            Pipe_Server rpc = new Pipe_Server("WavToolPipe_" + DomainName, Fs, (int)headSize);
            rpc.RecieveEndSignal += rpc_RecieveEndSignal;
            _TrackRPC.Add(Index, rpc);
        }

        void rpc_RecieveEndSignal(long SignalData, string PipeName)
        {
            try
            {
                string s = PipeName.Replace("WavToolPipe_", "");
                int Index = _ReverseDic[s];
                _TrackPreBufferTime[Index] = 0;
            }
            catch { ;}
        }

        public void AddPlayTrack(int idx, List<NoteListCalculator.NotePreRender> lst)
        {
            InitPlayer(idx);
            _CommandList.Add(idx, lst);
        }

        public void ListenAll()
        {
            foreach (KeyValuePair<int, AppWrapper> ap in _TrackPlayers)
            {
                _TrackPreBufferTime[ap.Key] = prebuffertime;
                _TrackRPC[ap.Key].StartServer();
                _TrackPlayers[ap.Key].getInstanceObject<VocalUtau.WavTools.Model.Player.BufferedPlayer>().Buffer_Play();
            }
            BufferFillTimer.Enabled = true;
       }

        private void PlayerWindow_Load(object sender, EventArgs e)
        {
            
        }
        public void Pause()
        {
            BufferFillTimer.Enabled = false;
        }
        public void Resume()
        {
            BufferFillTimer.Enabled = true;
        }
        public void ToDispose()
        {
            try
            {
                BufferFillTimer.Enabled = false;
                foreach (KeyValuePair<int, AppWrapper> ap in _TrackPlayers)
                {
                    _TrackPlayers[ap.Key].getInstanceObject<VocalUtau.WavTools.Model.Player.BufferedPlayer>().DisposePlayer();
                    _TrackPlayers[ap.Key].Unload();
                    _TrackFiles[ap.Key].Close();
                    _TrackRPC[ap.Key].ExitServer();
                }
            }
            catch { ;}
        }
        private void PlayerWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ToDispose();
        }

        private void BufferFillTimer_Tick(object sender, EventArgs e)
        {
            int bfb = 100;
            foreach (KeyValuePair<int, AppWrapper> ap in _TrackPlayers)
            {
                FormatHelper fh = new FormatHelper(IOHelper.NormalPcmMono16_Format);
                _TrackPlayers[ap.Key].getInstanceObject<VocalUtau.WavTools.Model.Player.BufferedPlayer>().FillBuffer(fh.Ms2Bytes(_TrackPreBufferTime[ap.Key]));
                int bf2 = (int)((_TrackPlayers[ap.Key].getInstanceObject<VocalUtau.WavTools.Model.Player.BufferedPlayer>().UntallPercent / 0.9) * 100);
                bfb = Math.Min(bfb, bf2);
            }
            BufferBfb.Value = bfb;
        }

        public void RunAll()
        {
        }

        private void BufferBfb_Click(object sender, EventArgs e)
        {

        }
    }
}
