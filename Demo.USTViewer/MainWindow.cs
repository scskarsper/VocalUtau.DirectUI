using Demo.USTViewer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.USTs.Original;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Forms
{
    public partial class MainWindow : Form
    {
        SingerWindow sw = new SingerWindow();
        AttributesWindow aw = new AttributesWindow();
        TrackerWindow tw = new TrackerWindow();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            Demo.USTViewer.BarkUST bu = new Demo.USTViewer.BarkUST();
           /* ProjectObject poj = new ProjectObject();
            poj.InitEmpty();*/
            
            ProjectObject poj = bu.GetTest(false);
            PartsObject PO = poj.TrackerList[0].PartList[0];

            sw.ShowOnDock(this.MainDock);
            aw.ShowOnDock(this.MainDock);
            tw.ShowOnDock(this.MainDock);
            tw.BindAttributeWindow(aw);
            sw.BindAttributeWindow(aw);
            tw.ShowingEditorChanged += tw_ShowingEditorChanged;
            tw.SelectingPartChanged += tw_SelectingPartChanged;
            tw.TotalTimePosChange += tw_TotalTimePosChange;
            sw.TotalTimePosChange += sw_TotalTimePosChange;

            LoadProject(ref poj);
        }

        void LoadProject(ref ProjectObject poj)
        {
            sw.LoadProjectObject(ref poj);
            aw.LoadProjectObject(ref poj);
            tw.LoadProjectObject(ref poj);
        }

        void tw_SelectingPartChanged(PartsObject PartObject, bool isEditing)
        {
            aw.LoadPartsPtr(ref PartObject,isEditing);
        }

        void tw_TotalTimePosChange(double Time)
        {
            sw.setCurrentTimePos(Time);
        }

        void sw_TotalTimePosChange(double Time)
        {
            tw.setCurrentTimePos(Time);
        }
        
        void tw_ShowingEditorChanged(PartsObject PartObject)
        {
            sw.LoadParts(ref PartObject);
            tw.RealarmTickPosition();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Demo.USTViewer.BarkUST bu = new Demo.USTViewer.BarkUST();
            ObjectDeserializer<ProjectObject> DPO = new ObjectDeserializer<ProjectObject>();
            BasicFileInformation bfi = DPO.ReadBasicInformation(bu.GetCacheFile());
            if (bfi.SavePassword.Length > 0 || bfi.IntroduceText.Trim().Length > 0)
            {
                IntroduceSwap ISW = new IntroduceSwap(bfi);
                if (ISW.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
            }
            try
            {
                ProjectObject OOP = DPO.DeserializeFromZipFile(bu.GetCacheFile(), bfi);
                LoadProject(ref OOP);
            }
            catch (Exception ew)
            {
                if (ew.Message == "Password Error or File Broken")
                {
                    MessageBox.Show("密码错误!");
                }else
                {
                    MessageBox.Show("未知错误!");
                }
            }
        }

    }
}
