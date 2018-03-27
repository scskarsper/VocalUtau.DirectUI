using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.Database.VocalDatabase;
using VocalUtau.Formats.Model.Utils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.SingerTools
{
    public partial class SingerAtomCategoryWindow : Form
    {
        IntPtr ProjectObjectPtr;
        string SystemSingerTag="(系统)";
        public SingerAtomCategoryWindow(IntPtr ProjectObjectPtr)
        {
            InitializeComponent();
            this.ProjectObjectPtr = ProjectObjectPtr;
            this.Text = "歌手编辑器";
        }

        private ProjectObject ProjectObject
        {
            get
            {
                ProjectObject ret = null;
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(ProjectObjectPtr);
                    ret = (ProjectObject)handle.Target;
                }
                catch { ;}
                return ret;
            }
        }
        private void SingerAtomCategoryWindow_Load(object sender, EventArgs e)
        {
            LoadList();
        }
        private void LoadList(string setupSelect="")
        {
            list_Singer.Items.Clear();
            int defidx = 0;
            for (int i = 0; i < ProjectObject.SingerList.Count; i++)
            {
                if (defidx==0 && setupSelect != "" && setupSelect == ProjectObject.SingerList[i].VocalName)
                {
                    defidx = i;
                }
                list_Singer.Items.Add(ProjectObject.SingerList[i].VocalName + (ProjectObject.SingerList[i].IsSystemSinger?SystemSingerTag:""));
            }
            if (ProjectObject.SingerList.Count <= defidx && ProjectObject.SingerList.Count > 0) defidx = 0;
            if (ProjectObject.SingerList.Count > defidx)
            {
                list_Singer.SelectedIndex = defidx;
                list_Singer_SelectedIndexChanged(null, null);
            }
            else
            {
                btn_CreateNew_Click(null, null);
            }
        }
        private bool bool_chkIsEnableSave()
        {
            string NewName = txt_Name.Text;
            string NewGUID = txt_GUID.Text;
            if (NewName == "") return false;
            if (txt_Resampler.Text.Trim() == "") return false;
            for (int i = 0; i < ProjectObject.SingerList.Count; i++)
            {
                if (ProjectObject.SingerList[i].VocalName.Trim() == NewName)
                {
                    if (ProjectObject.SingerList[i].getGuid().Trim() != NewGUID)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (bool_chkIsEnableSave())
            {
                if (txt_GUID.Text == "")
                {
                    //添加歌姬
                    SingerObject so = new SingerObject();
                    so.VocalName = txt_Name.Text;
                    so.SingerFolder = txt_Dir.Text;
                    so.Flags = txt_Flags.Text;
                    so.PartResampler = txt_Resampler.Text;
                    ProjectObject.SingerList.Add(so);
                    LoadList(txt_Name.Text);
                }
                else
                {
                    //更新歌姬
                    for (int i = 0; i < ProjectObject.SingerList.Count; i++)
                    {
                        if (ProjectObject.SingerList[i].getGuid().Trim() == txt_GUID.Text.Trim())
                        {
                            ProjectObject.SingerList[i].VocalName = txt_Name.Text;
                            ProjectObject.SingerList[i].SingerFolder = txt_Dir.Text;
                            ProjectObject.SingerList[i].Flags = txt_Flags.Text;
                            ProjectObject.SingerList[i].PartResampler = txt_Resampler.Text;
                        }
                    }
                    LoadList(txt_Name.Text);
                }
            }
            else
            {
                MessageBox.Show("歌姬属性错误！");
            }
        }

        private void btn_DelSinger_Click(object sender, EventArgs e)
        {
            if (ProjectObject.SingerList.Count>0 && list_Singer.SelectedIndex >=0)
            {
                try
                {
                    string singerName = list_Singer.Items[list_Singer.SelectedIndex].ToString();
                    if (MessageBox.Show("您确认要删除歌姬" + singerName + "么？","删除确认",MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        for (int i = 0; i < ProjectObject.SingerList.Count; i++)
                        {
                            if (ProjectObject.SingerList[i].VocalName.Trim() == singerName.Trim())
                            {
                                if (ProjectObject.SingerList[i].getGuid().Trim() == txt_GUID.Text.Trim())
                                {
                                    ProjectObject.SingerList.RemoveAt(i);
                                }
                            }
                        }
                        LoadList();
                    }
                }
                catch { ;}
            }
        }

        private void list_Singer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProjectObject.SingerList.Count>0 && list_Singer.SelectedIndex >=0)
            {
                try
                {
                    string singerName = list_Singer.Items[list_Singer.SelectedIndex].ToString();
                    int lst = singerName.LastIndexOf(SystemSingerTag);
                    if (lst != -1)
                    {
                        singerName = singerName.Substring(0, lst);
                    }
                    for (int i = 0; i < ProjectObject.SingerList.Count; i++)
                    {
                        if (ProjectObject.SingerList[i].VocalName.Trim() == singerName.Trim())
                        {
                            txt_Dir.Text=ProjectObject.SingerList[i].SingerFolder;
                            txt_Name.Text = ProjectObject.SingerList[i].VocalName;
                            txt_Flags.Text = ProjectObject.SingerList[i].Flags;
                            txt_Resampler.Text = ProjectObject.SingerList[i].PartResampler;
                            txt_GUID.Text = ProjectObject.SingerList[i].getGuid();
                            btn_Save.Text = "更新";
                            btn_DelSinger.Enabled = true;
                            try
                            {
                                UtauPic.Image = Image.FromFile(PathUtils.AbsolutePath(ProjectObject.SingerList[i].getRealSingerFolder(), ProjectObject.SingerList[i].Avatar));
                            }
                            catch { ;}
                        }
                    }
                }
                catch
                {
                    btn_CreateNew_Click(null, null);
                }
            }
        }

        private void btn_CreateNew_Click(object sender, EventArgs e)
        {
            txt_GUID.Text = "";
            txt_Flags.Text = "";
            txt_Name.Text = "";
            txt_Dir.Text = "";
            txt_Resampler.Text = "resampler.exe";
            btn_Save.Text = "添加";
            btn_DelSinger.Enabled = false;
        }

        private void btn_BrowseVoiceDir_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.DefaultExt = ".txt";
            ofd.Filter = "UTAU角色描述文件|character.txt;voicedbi.dat";
            ofd.FileName = "voicedbi.dat";
            ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\VoiceDir";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (System.IO.File.Exists(ofd.FileName))
                {
                    txt_Dir.Text = PathUtils.RelativePath(AppDomain.CurrentDomain.BaseDirectory, (new System.IO.FileInfo(ofd.FileName)).Directory.FullName);
                }
            }
        }

        private void btn_BrowseResampler_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.DefaultExt = ".exe";
            ofd.Filter = "Resampler引擎|*.exe";
            ofd.FileName = "resampler.exe";
            ofd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Engines";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (System.IO.File.Exists(ofd.FileName))
                {
                    txt_Resampler.Text = PathUtils.RelativePath(AppDomain.CurrentDomain.BaseDirectory, ofd.FileName);
                }
            }
        }

        private void txt_Dir_TextChanged(object sender, EventArgs e)
        {
            if (System.IO.File.Exists(txt_Dir.Text + "\\character.txt"))
            {
                try
                {
                    CharacterAtom charatom = VocalUtau.Formats.Model.USTs.Otos.CharacterSerializer.DeSerialize(txt_Dir.Text + "\\character.txt");
                    if (this.txt_Name.Text.Trim() == "")
                    {
                        this.txt_Name.Text = charatom.Dbname;
                    }
                    UtauPic.Image = Image.FromFile(PathUtils.AbsolutePath(PathUtils.AbsolutePath(txt_Dir.Text),charatom.Avatar));
                }
                catch { ;}
            }
            else
            {
                UtauPic.Image = null;
            }
        }
    }
}
