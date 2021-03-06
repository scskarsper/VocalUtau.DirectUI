﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.SingerUtils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.AttributeUtils.CategoryForms
{
    public partial class PhonemeAtomCategoryWindow : Form
    {
        PhonemeEditorModel pmodel = null;
        public PhonemeAtomCategoryWindow(object ListObject)
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            if (ListObject.GetType() == typeof(PhonemeEditorModel))
            {
                pmodel = ((PhonemeEditorModel)ListObject);
                this.ListValue = pmodel.Plist;
            }
            else
            {
                this.ListValue = new List<NoteAtomObject>();
            }
        }
        private List<NoteAtomObject> _ListValue;
        public List<NoteAtomObject> ListValue
        {
            get { return _ListValue; }
            set { _ListValue = value; }
        }

        int CurrentIndex = 0;
        private void PhonemeAtomCategoryWindow_Load(object sender, EventArgs e)
        {
            if (pmodel != null)
            {
                this.Text = "\""+pmodel.Lyric+"\" 发音部件设置";
                this.lbl_Length.Text = pmodel.Length.ToString();
                this.lbl_NoteView.Text = pmodel.Lyric;
                ctl_pa_Start.Maximum = (int)pmodel.Length;
                ReloadListValue();
                ResizeShown();
                SetCurrentObject(0);
            }
        }
        void ResizeShown()
        {
            if (this.pnl_Phoneme.Tag == null || this.pnl_Phoneme.Tag.GetType() != typeof(List<long>))
            {
                return;
            }
            List<long> TickArray = (List<long>)this.pnl_Phoneme.Tag;

            if (TickArray.Count > 0) this.pnl_Phoneme.BackColor = this.pnl_Phoneme.Controls[TickArray.Count - 1].BackColor;
            int totalwidth = this.pnl_Phoneme.ClientRectangle.Width;
            long totaltick = pmodel.Length;
            double w2t = (double)totalwidth / (double)totaltick;

            int LastLeft = 0;
            for (int i = 0; i < TickArray.Count; i++)
            {
                this.pnl_Phoneme.Controls[i].Left = LastLeft;
                this.pnl_Phoneme.Controls[i].Width = (int)(TickArray[i] * w2t);
                this.pnl_Phoneme.Controls[i].Text = ListValue[i].PhonemeAtom;
                LastLeft = LastLeft + (int)(TickArray[i] * w2t);
            }
            if (TickArray.Count > 0)
            {
                int la = TickArray.Count - 1;
                int et = this.pnl_Phoneme.Controls[la].Left + this.pnl_Phoneme.Controls[la].Width;
                if (et < totalwidth)
                {
                    this.pnl_Phoneme.Controls[la].Width = this.pnl_Phoneme.Controls[la].Width + totalwidth - et;
                }
            }
        }
        bool isSingleAuto
        {
            get
            {
                int At0C = 0;
                for (int i = 0; i < ListValue.Count; i++)
                {
                    if (ListValue[i].AtomLength == 0)
                    {
                        At0C++;
                        if (At0C > 1)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        private long getStartTick(int Index)
        {
            long ret = 0;
            try
            {
                List<long> TickArray = (List<long>)this.pnl_Phoneme.Tag;
                for (int i = 0; i < Index; i++)
                {
                    ret += TickArray[i];
                }
            }
            catch { ;}
            return ret;
        }
        private long currentStartTick
        {
            get
            {
                return getStartTick(CurrentIndex);   
            }
        }
        void SetCurrentObject(int ObjectIndex)
        {
            if (ObjectIndex >= ListValue.Count)
            {
                ObjectIndex = 0;
            };
            CurrentIndex=ObjectIndex;
            ctl_pa_Start.Enabled = ObjectIndex > 0;
            ctl_pa_Start.Value = (int)currentStartTick;
            for (int i = 0; i < ListValue.Count; i++)
            {
                if (ListValue[i].AtomLength == 0)
                {
                    this.pnl_Phoneme.Controls[i].BackColor = i == ObjectIndex ? Color.Olive : Color.FromArgb(192, 192, 0);
                }else
                {
                    this.pnl_Phoneme.Controls[i].BackColor = i == ObjectIndex ? Color.Teal : Color.FromArgb(0, 192, 192);
                }
            }
            chk_Bfb.Checked = ListValue[CurrentIndex].LengthIsPercent;
            chk_Bfb.Tag = CurrentIndex;
            chk_ZyB.Checked = ListValue[CurrentIndex].AtomLength == 0;
            chk_ZyB.Tag = CurrentIndex;

            chk_Bfb.Enabled = !chk_ZyB.Checked;

            setupAtomPropertyView(ObjectIndex);

            if (chk_ZyB.Checked)
            {
                chk_ZyB.Enabled = !isSingleAuto;
            }
            else
            {
                chk_ZyB.Enabled = true;
            }

            btn_ForwardPhonObj.Enabled = CurrentIndex < ListValue.Count - 1;
            btn_BackPhonObj.Enabled = CurrentIndex > 0;
            btn_DelPhonObj.Enabled = ListValue.Count > 1;
        }
        void ReloadListValue()
        {
            //初始化
            ReCalcListValue();
            if (this.pnl_Phoneme.Tag == null || this.pnl_Phoneme.Tag.GetType() != typeof(List<long>))
            {
                return;
            }
            List<long> TickArray = (List<long>)this.pnl_Phoneme.Tag;
            //添加等数量Lbl
            for (int i = TickArray.Count; i < this.pnl_Phoneme.Controls.Count; i++)
            {
                this.pnl_Phoneme.Controls[i].Visible = false;
            }
            for (int i = 0; i < TickArray.Count; i++)
            {
                Label lb;
                if (i >= this.pnl_Phoneme.Controls.Count)
                    lb = new Label();
                else
                    lb = (Label)this.pnl_Phoneme.Controls[i];
                lb.Visible = true;
                //if(i!=TickArray.Count-1)lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                lb.Height = pnl_Phoneme.ClientRectangle.Height+2;
                lb.Top = -1;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Tag = i;
                lb.Click += Atom_Click;
                lb.Paint += Atom_Paint;
                this.pnl_Phoneme.Controls.Add(lb);
            }
            btn_DelPhonObj.Enabled = ListValue.Count > 1;
        }

        void Atom_Paint(object sender, PaintEventArgs e)
        {
            if (!sender.Equals(pnl_Phoneme.Controls[0]))
            {
                e.Graphics.DrawLine(Pens.Black, 0, 0, 0, e.ClipRectangle.Height);
            }
        }
        void ReCalcListValue()
        {
            //初始化
            if (this.pnl_Phoneme.Tag == null || this.pnl_Phoneme.Tag.GetType() != typeof(List<long>))
            {
                this.pnl_Phoneme.Tag = new List<long>();
            }
            if (ListValue.Count == 0) return;
            //检查并确认存在延长部
            bool flagHaveAuto = false;
            for (int i = 0; i < ListValue.Count; i++)
            {
                if (ListValue[i].AtomLength == 0)
                {
                    flagHaveAuto = true;
                    break;
                }
            }
            if (!flagHaveAuto)
            {
                ListValue[ListValue.Count - 1].AtomLength = 0;
            }
            //计算每单元长度
            List<long> TickArray = PhonemeSplitTools.SplitedTickList(pmodel.Length, ListValue);
            this.pnl_Phoneme.Tag = TickArray;
        }
        void Atom_Click(object sender, EventArgs e)
        {
            Label CurLbl = (Label)sender;
            int Index=pnl_Phoneme.Controls.IndexOf(CurLbl);
            SetCurrentObject(Index);
        }
        private void btn_OK_Click(object sender, EventArgs e)
        {
            
        }

        private void pnl_Phoneme_Resize(object sender, EventArgs e)
        {
            ResizeShown();
        }

        private void ctl_pa_Start_Scroll(object sender, EventArgs e)
        {
            if (this.pnl_Phoneme.Tag == null || this.pnl_Phoneme.Tag.GetType() != typeof(List<long>))
            {
                return;
            }
            List<long> TickArray = (List<long>)this.pnl_Phoneme.Tag;
            long st1 = getStartTick(CurrentIndex - 1);
            long Tl1 = ctl_pa_Start.Value - st1;
            long tEnd = pmodel.Length;
            long st2 = getStartTick(CurrentIndex + 1);
            long MaxNoteLen = pmodel.Length / TickArray.Count;
            if (CurrentIndex + 1 < ListValue.Count) tEnd = st2;
            long Tl2 = tEnd - ctl_pa_Start.Value;
            long MinTickNote=10;
            if (MinTickNote > MaxNoteLen) MinTickNote = MaxNoteLen;
            try
            {
                if (ListValue[CurrentIndex - 1].AtomLength > 0 && Tl1 < MinTickNote)
                {
                    //前置音符过短
                    long MinValue = MinTickNote + st1;
                    ctl_pa_Start.Value = (int)MinValue;
                    ctl_pa_Start_Scroll(null, null);
                    return;
                }
                if (ListValue[CurrentIndex].AtomLength > 0 || (!isSingleAuto && ListValue[CurrentIndex - 1].AtomLength == 0))
                {
                    if (Tl2 < MinTickNote)
                    {
                        //后置音符过短
                        long MinValue = tEnd - MinTickNote;
                        ctl_pa_Start.Value = (int)MinValue;
                        ctl_pa_Start_Scroll(null, null);
                        return;
                    }
                }
            }
            catch { ;}

            if (ListValue[CurrentIndex - 1].AtomLength > 0)
            {
                if (ListValue[CurrentIndex - 1].LengthIsPercent)
                {
                    double bfb = (double)Tl1 / (double)pmodel.Length;
                    bfb = bfb * 100;
                    ListValue[CurrentIndex - 1].AtomLength = (long)Math.Round(bfb);
                }
                else
                {
                    ListValue[CurrentIndex - 1].AtomLength = Tl1;
                }
                this.pnl_Phoneme.Controls[CurrentIndex - 1].BackColor = Color.FromArgb(0, 192, 192);
            }
            if (ListValue[CurrentIndex].AtomLength > 0 || (!isSingleAuto && ListValue[CurrentIndex-1].AtomLength == 0))
            {
                if (ListValue[CurrentIndex].LengthIsPercent)
                {
                    double bfb = (double)Tl2 / (double)pmodel.Length;
                    bfb = bfb * 100;
                    ListValue[CurrentIndex].AtomLength = (long)Math.Round(bfb);
                }
                else
                {
                    ListValue[CurrentIndex].AtomLength = Tl2;
                }
                this.pnl_Phoneme.Controls[CurrentIndex].BackColor = Color.Teal;
            }
            ReCalcListValue();
            ResizeShown();
            AtomPropertyGrid.Refresh();
        }

        private void setupAtomPropertyView(int ObjectIndex)
        {
            if (ObjectIndex > ListValue.Count)
            {
                ObjectIndex = 0;
            };
            if (CurrentIndex == ObjectIndex)
            {
                if (ListValue.Count > 1)
                {
                    if (CurrentIndex == 0)
                    {
                        //第一个
                        NoteAtomObject curObj = _ListValue[CurrentIndex];
                        FirstPhonemeAttrModels bpam = new FirstPhonemeAttrModels(ref curObj);
                        AtomPropertyGrid.SelectedObject = bpam;
                    }
                    else if (CurrentIndex == ListValue.Count - 1)
                    {
                        //最后一个
                        NoteAtomObject curObj = _ListValue[CurrentIndex];
                        LastPhonemeAttrModels bpam = new LastPhonemeAttrModels(ref curObj);
                        AtomPropertyGrid.SelectedObject = bpam;
                    }
                    else
                    {
                        //其他
                        NoteAtomObject curObj = _ListValue[CurrentIndex];
                        BasicPhonemeAttrModels bpam = new BasicPhonemeAttrModels(ref curObj);
                        AtomPropertyGrid.SelectedObject = bpam;
                    }
                }
                else if(ListValue.Count==1)
                {
                    NoteAtomObject curObj = _ListValue[0];
                    SinglePhonemeAttrModels bpam = new SinglePhonemeAttrModels(ref curObj);
                    AtomPropertyGrid.SelectedObject = bpam;
                }
            }
        }

        private void chk_Bfb_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pnl_Phoneme.Tag == null || this.pnl_Phoneme.Tag.GetType() != typeof(List<long>))
                {
                    return;
                }
                if ((int)chk_Bfb.Tag != CurrentIndex) return;
                if (ListValue[CurrentIndex].AtomLength == 0)
                {
                    chk_Bfb.Checked = false;
                    return;
                }
                List<long> TickArray = (List<long>)this.pnl_Phoneme.Tag;
                if (ListValue[CurrentIndex].LengthIsPercent != chk_Bfb.Checked)
                {
                    if (chk_Bfb.Checked)
                    {
                        try
                        {
                            long tot = 0;
                            for (int i = 0; i < TickArray.Count; i++)
                            {
                                tot = tot + TickArray[CurrentIndex];
                            }
                            ListValue[CurrentIndex].AtomLength = (long)Math.Round(((double)TickArray[CurrentIndex] / (double)tot) * 100);
                            ListValue[CurrentIndex].LengthIsPercent = true;
                        }
                        catch { ;}
                    }
                    else
                    {
                        try
                        {
                            ListValue[CurrentIndex].AtomLength = TickArray[CurrentIndex];
                            ListValue[CurrentIndex].LengthIsPercent = false;
                        }
                        catch { ;}
                    }
                }
                AtomPropertyGrid.Refresh();
            }
            catch { ;}
        }

        private void chk_ZyB_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pnl_Phoneme.Tag == null || this.pnl_Phoneme.Tag.GetType() != typeof(List<long>))
                {
                    return;
                }
                if ((int)chk_ZyB.Tag != CurrentIndex) return;
                List<long> TickArray = (List<long>)this.pnl_Phoneme.Tag;
                if (chk_ZyB.Checked)
                {
                    bool ava = false;
                    for (int i = 0; i < ListValue.Count; i++)
                    {
                        if (ListValue[i].AtomLength == 0)
                        {
                            ava = true;
                        }
                    }
                    if (ava)
                    {
                        ListValue[CurrentIndex].AtomLength = 0;
                    }
                }
                else
                {
                    try
                    {
                        ListValue[CurrentIndex].AtomLength = TickArray[CurrentIndex];
                    }
                    catch { ;}
                }
                ReCalcListValue();
                ResizeShown();
                if (ListValue[CurrentIndex].AtomLength == 0)
                {
                    this.pnl_Phoneme.Controls[CurrentIndex].BackColor = Color.Olive;
                }
                else
                {
                    this.pnl_Phoneme.Controls[CurrentIndex].BackColor = Color.Teal;
                }
                chk_Bfb.Enabled = !chk_ZyB.Checked;
            }
            catch { ;}
        }

        private void AtomPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            try
            {
                for (int i = 0; i < ListValue.Count; i++)
                {
                    this.pnl_Phoneme.Controls[i].Text = ListValue[i].PhonemeAtom;
                }
            }
            catch { ;}
        }

        private void btn_AddPhonObj_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("请输入要插入的部件发音符号", "添加部件", "a");
            if (input.Trim().Length > 0)
            {
                NoteAtomObject nao = new NoteAtomObject(input);
                nao.InitNoteAtom();
                try
                {
                    nao.FadeOutLengthMs = ListValue[ListValue.Count - 1].FadeOutLengthMs;
                    nao.Flags = ListValue[ListValue.Count - 1].Flags;
                }
                catch { ;}
                nao.AtomLength = 0;
                nao.VolumePercentInt = 100;
                ListValue.Add(nao);
                ReloadListValue();
                ResizeShown();
                SetCurrentObject(ListValue.Count-1);
            }
        }

        private void btn_DelPhonObj_Click(object sender, EventArgs e)
        {
            if (ListValue.Count > 1)
            {
                ListValue.RemoveAt(CurrentIndex); 
                ReloadListValue();
                ResizeShown();
                SetCurrentObject(0);
            }
        }

        private void btn_ForwardPhonObj_Click(object sender, EventArgs e)
        {
            try
            {
                NoteAtomObject NAO1 = ListValue[CurrentIndex];
                ListValue.RemoveAt(CurrentIndex);
                ListValue.Insert(CurrentIndex + 1, NAO1);
                ReloadListValue();
                SetCurrentObject(CurrentIndex + 1);
                ResizeShown();
            }
            catch { ;}
        }

        private void btn_BackPhonObj_Click(object sender, EventArgs e)
        {
            try
            {
                NoteAtomObject NAO1 = ListValue[CurrentIndex];
                ListValue.RemoveAt(CurrentIndex);
                ListValue.Insert(CurrentIndex - 1, NAO1);
                ReloadListValue();
                SetCurrentObject(CurrentIndex - 1);
                ResizeShown();
            }
            catch { ;}
        }

    }
}
