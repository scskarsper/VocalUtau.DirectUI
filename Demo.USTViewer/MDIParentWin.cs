using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Forms;
using VocalUtau.Formats.Model.USTs.Original;
using VocalUtau.Formats.Model.VocalObject;

namespace Demo.USTViewer
{
    public partial class MDIParentWin : Form
    {
        private int childFormNumber = 0;

        public MDIParentWin()
        {
            InitializeComponent();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "窗口 " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
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
        SingerWindow SW = new SingerWindow();
        private void AddSW()
        {
            try
            {
                SW.Close();
            }
            catch { ;}
            SW = new SingerWindow();
            SW.MdiParent = this;
            SW.Show();
        }
        private void MDIParentWin_Load(object sender, EventArgs e)
        {
            /*ProjectObject poj = LoadUST(@"D:\VocalUtau\VocalUtau.DebugExampleFiles\DemoUSTS\Sakurane2.Tracks\Track-4b158252-eb7f-4223-b7b0-d78f32e044ec.ust");
            PartsObject PO = poj.TrackerList[1].PartList[0];
            AddSW();
            SW.LoadParts(ref PO);*/
        }
    }
}
