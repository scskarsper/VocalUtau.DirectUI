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
        public MainWindow()
        {
            InitializeComponent();
        }
        ProjectObject LoadUST(string FilePath)
        {
            USTOriginalProject USTPO = USTOriginalSerializer.Deserialize(FilePath);
            PartsObject pro = USTOriginalSerializer.UST2Parts(USTPO);

            ProjectObject poj = new ProjectObject();
            poj.InitEmpty();
            poj.TrackerList[1].PartList[0] = pro;

            foreach (NoteObject po in pro.NoteList)
            {
                byte[] bt = System.Text.Encoding.Default.GetBytes(po.Lyric);
                string Str = System.Text.Encoding.GetEncoding("Shift-JIS").GetString(bt);
                po.Lyric = Str;
            }
            int sg = 1;
            for (long i = 1; i <= pro.TickLength; i += 32)//
            {
                sg = sg * -1;
                pro.PitchBendsList.Add(new PitchObject(i, sg * 0.5));
            }

            string abc = ProjectObject.Serializer.Serialize(poj);

            return poj;
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            ProjectObject poj = LoadUST(@"D:\VocalUtau\VocalUtau.DebugExampleFiles\DemoUSTS\Sakurane2.Tracks\Track-4b158252-eb7f-4223-b7b0-d78f32e044ec.ust");
            PartsObject PO = poj.TrackerList[1].PartList[0];
            SingerWindow sw = new SingerWindow();
            sw.LoadParts(ref PO);
            sw.ShowOnDock(this.MainDock);

            AttributesWindow aw = new AttributesWindow();
            aw.ShowOnDock(this.MainDock);

            TrackerWindow tw = new TrackerWindow();
            tw.ShowOnDock(this.MainDock);
        }

    }
}
