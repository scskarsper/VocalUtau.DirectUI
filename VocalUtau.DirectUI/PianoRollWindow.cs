﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BalthasarLib.D2DPainter;
using VocalUtau.DirectUI.Models;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI
{
    public partial class PianoRollWindow : UserControl
    {

        public bool DisableMouse { get; set; }
        /// <summary>
        /// 私有属性
        /// </summary>
        #region
        RollConfigures rconf = new RollConfigures();
        PianoProperties pprops;
        double ShownNoteCount;
        double ShownTickCount;
        #endregion

        /// <summary>
        /// 公共属性
        /// </summary>
        #region
        public PitchAtomObject.OctaveTypeEnum OctaveType 
        {
            get { return pprops.OctaveType; }
            set
            {
                pprops.OctaveType = value;
                d2DPainterBox1.Refresh();
            } 
        }

        public PianoProperties PianoProps
        {
            get
            {
                return pprops;
            }
        }
        public void setCrotchetSize(uint Size=66)
        {
            if (Size < 32) return;
            pprops.CrotchetLengthPixel = Size;
            if (pprops != null) genShownArea();
            d2DPainterBox1.Refresh();
        }
        public void setNoteHeight(uint Size=13)
        {
            if (Size < 13) return;
            rconf.setNoteHeight(Size); 
            SetScrollMax();
            d2DPainterBox1.Refresh();
        }
        public void setPianoStartTick(long Tick)
        {
            PianoProps.PianoStartTick = Tick;
            d2DPainterBox1.Refresh();
        }
        public void setScrollToNote(uint NoteNumber)
        {
            uint halfNote=(uint)ShownNoteCount/2;//获取一半的Note号
            
            uint TopNote = NoteNumber + halfNote;//获取在屏幕中间时的Note号;
            uint ZeroTop = (uint)(ShownNoteCount > (int)ShownNoteCount ? ShownNoteCount + 1 : ShownNoteCount);//获取极限值.

            uint TargetTop = ZeroTop;
            if (TopNote > ZeroTop)
            {
                //没超界
                TargetTop = TopNote;
            }
            if (TopNote > rconf.MaxNoteNumber) TargetTop = (uint)rconf.MaxNoteNumber;
           // if (NoteNumber == 0) NoteNumber = 1;
           // int TopNote = rconf.MaxNoteNumber - (int)NoteNumber;
            pprops.PianoTopNote = TargetTop;// (uint)(TopNote > 0 ? TopNote : 0);
            noteScrollBar1.Value = rconf.MaxNoteNumber-(int)TargetTop;
            d2DPainterBox1.Refresh();
        }

        public uint MaxShownNoteNumber { get { return pprops.PianoTopNote; } }
        public uint MinShownNoteNumber { get { return MaxShownNoteNumber - (uint)(ShownNoteCount > (int)ShownNoteCount ? ShownNoteCount + 1 : ShownNoteCount); } }
        public long MaxShownTick { get { return MinShownTick+(long)Math.Round(ShownTickCount,0); } }
        public long MinShownTick { get { return pprops.PianoStartTick; } }
        public bool IsInitalized { get { return (pprops!=null && rconf!=null);} }
        #endregion

        /// <summary>
        /// 界面冒泡事件声明
        /// </summary>
        #region
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event EventHandler PianoRollBeforeResize;
        public event EventHandler PianoRollAfterResize;
        #endregion
        /// <summary>
        /// 基础逻辑-私有
        /// </summary>
        #region
        void genShownArea()
        {
            ShownNoteCount = (double)(d2DPainterBox1.ClientRectangle.Height - rconf.Const_TitleHeight) / rconf.Const_RollNoteHeight;
            ShownTickCount = pprops.dertPixel2dertTick(d2DPainterBox1.ClientRectangle.Width-rconf.Const_RollWidth);
        }
        void SetScrollMax()
        {
            int noteArea = this.ClientRectangle.Height - rconf.Const_TitleHeight;
            int noteCount = noteArea / rconf.Const_RollNoteHeight;
            int ScrollMax = rconf.MaxNoteNumber - noteCount;
            if (noteScrollBar1.Value > ScrollMax)
            {
                noteScrollBar1.Value = ScrollMax;
                noteScrollBar1_Scroll(null, null);
            }
            noteScrollBar1.Maximum = ScrollMax;
        }
        void InitGUI()
        {
            SetScrollMax();
            noteScrollBar1.Height = this.ClientRectangle.Height - rconf.Const_TitleHeight;
            noteScrollBar1.Top = rconf.Const_TitleHeight;
            noteScrollBar1.Width = rconf.Const_VScrollBarWidth;
            noteScrollBar1.Left = this.ClientRectangle.Width - rconf.Const_VScrollBarWidth;
            d2DPainterBox1.Top = 0;
            d2DPainterBox1.Left = 0;
            d2DPainterBox1.Width = this.ClientRectangle.Width;
            d2DPainterBox1.Height = this.ClientRectangle.Height;
            d2DPainterBox1.MouseWheel += d2DPainterBox1_MouseWheel;
            if(pprops!=null)genShownArea();
        }

        void d2DPainterBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                noteScrollBar1.Value -= (int)(e.Delta / 120);
                noteScrollBar1.Refresh();
                noteScrollBar1.Invalidate();
                noteScrollBar1.Update();
                noteScrollBar1_Scroll(null, null);
            }
            catch { ;}
        }
        private void PianoRollWindow_Resize(object sender, EventArgs e)
        {
            if (PianoRollBeforeResize != null) PianoRollBeforeResize(sender, e);
            InitGUI();
            if (PianoRollAfterResize != null) PianoRollAfterResize(sender, e);
        }
        private void noteScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int TopNote = rconf.MaxNoteNumber - noteScrollBar1.Value;
            pprops.PianoTopNote = (uint)(TopNote > 0 ? TopNote : 0);
            d2DPainterBox1.Refresh();
        }
        #endregion

        /// <summary>
        /// 基础逻辑-公有
        /// </summary>
        #region
        public PianoRollWindow()
        {
            InitializeComponent();
            pprops = new PianoProperties(rconf);
            FpsCreater.Interval = 1000 / GlobalStatic.StaticFps;
            FpsCreater.Tick += FpsCreater_Tick;
            FpsCreater.Enabled = true;
            int TopNote=(rconf.MaxNoteNumber - noteScrollBar1.Value);
            pprops.PianoTopNote = (uint)(TopNote>0?TopNote:0);
            InitGUI();
        }

        void FpsCreater_Tick(object sender, EventArgs e)
        {
            PaintFPS();
        }
        public void RedrawPiano(bool force = false)
        {
            updateFps = true;
            if (force)
            {
                FpsCreater.Enabled = false;
                FpsCreater.Enabled = true;
                PaintFPS();
            }
            //            d2DPainterBox1.Refresh();
        }
        #endregion


        /// <summary>
        /// 绘图冒泡事件声明
        /// </summary>
        #region
        // 创建一个委托，返回类型为void，两个参数
        public delegate void OnPianoTrackDrawHandler(object sender, DrawUtils.TrackDrawUtils utils);
        public delegate void OnPianoTitleDrawHandler(object sender, DrawUtils.TitleDrawUtils utils);
        public delegate void OnPianoRollDrawHandler(object sender, DrawUtils.RollDrawUtils utils);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event OnPianoTrackDrawHandler TrackPaint;
        public event OnPianoTitleDrawHandler TitlePaint;
        public event OnPianoRollDrawHandler RollPaint;
        #endregion

        /// <summary>
        /// 绘图逻辑
        /// </summary>
        #region
        bool isDrawing = false;
        bool updateFps = false;
        Timer FpsCreater = new Timer();
        private void PaintFPS()
        {
            if (updateFps)
            {
                d2DPainterBox1.Refresh();
                updateFps = false;
            }
        }
        public override void Refresh()
        {
            updateFps = true;
        }
        private void d2DPainterBox1_D2DPaint(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e)
        {
            if (isDrawing) return;
            isDrawing = true;
            try
            {
                PianoRollPoint sp = pprops.getPianoStartPoint();
                DrawPianoTrackArea(sender, e, sp);
                DrawPianoRollArea(sender, e);
                DrawPianoTitleArea(sender, e, sp);
            }
            catch { ;}
            isDrawing = false;
        }
        private void DrawPianoTrackArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e, PianoRollPoint startPoint)
        {
            D2DGraphics g = e.D2DGraphics;

            int y = rconf.Const_TitleHeight;//绘制点纵坐标
            int w = e.ClipRectangle.Width - rconf.Const_VScrollBarWidth;
            //绘制钢琴窗
            int cNote = (int)pprops.PianoTopNote;//绘制区域第一个阶的音符
            while (y < e.ClipRectangle.Height)//若未画超界
            {
                //计算绘制区域
                Point LT = new Point(rconf.Const_RollWidth, y);//矩形左上角
                Point LB = new Point(rconf.Const_RollWidth, y + rconf.Const_RollNoteHeight);//矩形左下角
                Point RT = new Point(w, y);//矩形右上角
                Point RB = new Point(w, y + rconf.Const_RollNoteHeight);//矩形右下角
                Rectangle Rect = new Rectangle(LT, new Size(w - rconf.Const_RollWidth, rconf.Const_RollNoteHeight));//矩形区域
                //计算色域
                PitchAtomObject NoteValue = new PitchAtomObject((uint)cNote, 0);
                NoteValue.OctaveType = pprops.OctaveType;
                int Octave = NoteValue.OctaveType == PitchAtomObject.OctaveTypeEnum.Voice ? NoteValue.Octave : NoteValue.Octave-1;
                int Key = NoteValue.Key;
                bool isBlackKey = NoteValue.IsBlackKey;
                Color KeyColor = isBlackKey ? rconf.RollColor_BlackKey_NormalSound : rconf.RollColor_WhiteKey_NormalSound;
                Color LineColor = rconf.RollColor_LineKey_NormalSound;
                Color OLineColor = rconf.RollColor_LineOctive_NormalSound;
                switch (rconf.getVoiceArea(cNote))
                {
                    case RollConfigures.VoiceKeyArea.OverSound: KeyColor = isBlackKey ? rconf.RollColor_BlackKey_OverSound : rconf.RollColor_WhiteKey_OverSound; LineColor = rconf.RollColor_LineKey_OverSound; OLineColor = rconf.RollColor_LineOctive_OverSound; break;
                    case RollConfigures.VoiceKeyArea.NoSound: KeyColor = isBlackKey ? rconf.RollColor_BlackKey_NoSound : rconf.RollColor_WhiteKey_NoSound; LineColor = rconf.RollColor_LineKey_NoSound; OLineColor = rconf.RollColor_LineOctive_OverSound; break;
                }
                //绘制矩形
                g.FillRectangle(Rect, KeyColor);
                //绘制边线
                g.DrawLine(LB, RB, (Key == 5 || Key == 0) ? OLineColor : LineColor,2);//isB ? LineL : LineB);
                //递归
                y = y + rconf.Const_RollNoteHeight;
                cNote = cNote - 1;
            }
            //绘制分节符
            //初始化
            double x = rconf.Const_RollWidth;//起点画线
            long BeatNumber = startPoint.BeatNumber;//获取起点拍号
            double BeatPixelLength = pprops.dertTick2dertPixel(pprops.BeatLength);//一拍长度
            if (startPoint.DenominatolTicksBefore!=0)//非完整Beats
            {
                //起点不在小节线
                BeatNumber=startPoint.NextWholeBeatNumber;
                x = x+pprops.dertTick2dertPixel(startPoint.NextWholeBeatDistance);
            }
            while (x <= w)
            {
                g.DrawLine(
                new Point((int)Math.Round(x, 0), rconf.Const_TitleHeight),
                new Point((int)Math.Round(x, 0), e.ClipRectangle.Height),
                    BeatNumber % pprops.BeatsCountPerSummery == 0 ? Color.Black : rconf.RollColor_LineOctive_NormalSound
                );
                BeatNumber=BeatNumber+1;
                x = x + BeatPixelLength;
            }
            //Rise 绘制Note等//传递事件
            Rectangle CurrentRect = new Rectangle(
                rconf.Const_RollWidth,
                rconf.Const_TitleHeight,
                w-rconf.Const_RollWidth,
                e.ClipRectangle.Height-rconf.Const_TitleHeight);//可绘制区域
            if (TrackPaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                    );
                TrackPaint(sender, new VocalUtau.DirectUI.DrawUtils.TrackDrawUtils(d2de, rconf, pprops));
            }
        }
        private void DrawPianoTitleArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e, PianoRollPoint startPoint)
        {
            D2DGraphics g = e.D2DGraphics;
            Rectangle BlackRect = new Rectangle(
                0,
                0,
                e.ClipRectangle.Width,
                rconf.Const_TitleHeight
            );
            g.FillRectangle(BlackRect, Color.Black);
            Rectangle TitleSpliterRect = new Rectangle(
                0, 
                rconf.Const_TitleHeight-rconf.Const_TitleHeightSpliter-1, 
                e.ClipRectangle.Width, 
                rconf.Const_TitleHeightSpliter
            );
            g.DrawLine(new Point(0, rconf.Const_TitleLineTop), new Point(e.ClipRectangle.Width, rconf.Const_TitleLineTop), rconf.TitleColor_Line,2);
            g.DrawLine(new Point(0, rconf.Const_TitleRulerTop), new Point(e.ClipRectangle.Width, rconf.Const_TitleRulerTop), rconf.TitleColor_Ruler,2);
            
            //绘制分节符

            //初始化
            double x = rconf.Const_RollWidth;//起点画线
            long BeatNumber = startPoint.BeatNumber;//获取起点拍号
            double BeatPixelLength = pprops.dertTick2dertPixel(pprops.BeatLength);//一拍长度
            if (startPoint.DenominatolTicksBefore != 0)//非完整Beats
            {
                //起点不在小节线
                BeatNumber = startPoint.NextWholeBeatNumber;
                x = x + pprops.dertTick2dertPixel(startPoint.NextWholeBeatDistance);
            }
            int My1 = (rconf.Const_TitleRulerTop * 2 / 3);
            int My2 = rconf.Const_TitleRulerTop + 1;
            while (x <= e.ClipRectangle.Width)
            {
                if (BeatNumber % pprops.BeatsCountPerSummery == 0)
                {
                    g.DrawLine(
                    new Point((int)Math.Round(x, 0), 0),
                    new Point((int)Math.Round(x, 0), rconf.Const_TitleHeight),
                    Color.White);
                    long SummeryId=BeatNumber/pprops.BeatsCountPerSummery;
                    g.DrawText(" "+SummeryId.ToString(),
                        new Rectangle(new Point((int)Math.Round(x, 0), rconf.Const_TitleLineTop), new Size((int)pprops.BeatLength, rconf.Const_TitleRulerTop - rconf.Const_TitleLineTop)),
                        Color.White,
                        new Font("宋体", 12));
                }
                else
                {
                    g.DrawLine(
                    new Point((int)Math.Round(x,0), My1),
                    new Point((int)Math.Round(x, 0), My2),
                    rconf.TitleColor_Marker);
                }
                BeatNumber = BeatNumber + 1;
                x = x + BeatPixelLength;
            }

            //绘制分割线
            g.DrawLine(new Point(rconf.Const_RollWidth, 0), new Point(rconf.Const_RollWidth, rconf.Const_TitleHeight), Color.White, 2);
            g.FillRectangle(TitleSpliterRect, rconf.PianoColor_WhiteKey);

            Rectangle CurrentRect = new Rectangle(
                0,
                0,
                e.ClipRectangle.Width,
                rconf.Const_TitleHeight
            );//可绘制区域
            if (TitlePaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                );
                TitlePaint(sender, new VocalUtau.DirectUI.DrawUtils.TitleDrawUtils(d2de, rconf, pprops));
            }
        }
        private void DrawPianoRollArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e)
        {
            D2DGraphics g = e.D2DGraphics;

            int y = rconf.Const_TitleHeight;//绘制点纵坐标
            int cNote = (int)pprops.PianoTopNote;//绘制区域第一个阶的音符
            int MyNotePx = (int)((e.MousePoint.Y - rconf.Const_TitleHeight) / rconf.Const_RollNoteHeight);//获取当前琴键
            int MyNote = (int)pprops.PianoTopNote - MyNotePx;//获取坐标
            while (y < e.ClipRectangle.Height)//若未画超界
            {
                //计算绘制区域
                Point LT = new Point(0, y);//矩形左上角
                Point LB = new Point(0, y + rconf.Const_RollNoteHeight);//矩形左下角
                Point RT = new Point(rconf.Const_RollWidth, y);//矩形右上角
                Point RB = new Point(rconf.Const_RollWidth, y + rconf.Const_RollNoteHeight);//矩形右下角
                Rectangle WhiteRect = new Rectangle(LT, new Size(rconf.Const_RollWidth, rconf.Const_RollNoteHeight));//矩形区域
                Rectangle BlackRect = new Rectangle(LT, new Size(rconf.Const_RollBlackWidth, rconf.Const_RollNoteHeight));//矩形区域
                Rectangle WordRect = new Rectangle(new Point(LT.X + rconf.Const_RollBlackWidth, LT.Y), new Size(rconf.Const_RollWidth - rconf.Const_RollBlackWidth, rconf.Const_RollNoteHeight));//矩形区域
                //绘制基础矩形
                g.FillRectangle(WhiteRect, rconf.PianoColor_WhiteKey);
                g.DrawRectangle(WhiteRect, rconf.PianoColor_Line);
                //绘制黑键
                PitchAtomObject NoteValue = new PitchAtomObject((uint)cNote, 0);
                NoteValue.OctaveType = pprops.OctaveType;
                int Octave = NoteValue.Octave;
                int Key = NoteValue.Key;
                bool isBlackKey = NoteValue.IsBlackKey;
                if (isBlackKey)
                {
                    g.FillRectangle(BlackRect, rconf.PianoColor_BlackKey);
                }
                //获取鼠标位置琴键
                //绘制符号
                if (MyNote == cNote)
                {
                    g.FillRectangle(WordRect, rconf.PianoColor_MouseKey);
                    g.DrawText("  " + NoteValue.NoteChar + Octave.ToString(), WordRect, rconf.PianoColor_BlackKey, new System.Drawing.Font("Tahoma", 10));
                }
                else if (Key == 0)
                {
                    g.DrawText("  " + NoteValue.NoteChar + Octave.ToString(), WordRect, rconf.PianoColor_BlackKey, new System.Drawing.Font("Tahoma", 10));
                }

                //递归
                y = y + rconf.Const_RollNoteHeight;
                cNote = cNote - 1;
            }
            //Rise 绘制Note等//传递事件
            //            e.ClipRectangle.Width - rconf.Const_RollWidth, rconf.Const_RollNoteHeight

            Rectangle CurrentRect = new Rectangle(
                0,
                rconf.Const_TitleHeight,
                rconf.Const_RollWidth,
                e.ClipRectangle.Height - rconf.Const_TitleHeight);//可绘制区域

            if (RollPaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                    );
                RollPaint(sender, new VocalUtau.DirectUI.DrawUtils.RollDrawUtils(d2de,rconf, pprops));
            }
        }
        #endregion


        /// <summary>
        /// 鼠标冒泡事件声明
        /// </summary>
        #region
        // 创建一个委托，返回类型为void，两个参数
        public delegate void OnMouseEventHandler(object sender, PianoMouseEventArgs e);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event OnMouseEventHandler TrackMouseDown;
        public event OnMouseEventHandler RollMouseDown;
        public event OnMouseEventHandler TitleMouseDown;
        public event OnMouseEventHandler TrackMouseUp;
        public event OnMouseEventHandler RollMouseUp;
        public event OnMouseEventHandler TitleMouseUp;
        public event OnMouseEventHandler TrackMouseMove;
        public event OnMouseEventHandler RollMouseMove;
        public event OnMouseEventHandler TitleMouseMove;
        public event OnMouseEventHandler TrackMouseClick;
        public event OnMouseEventHandler RollMouseClick;
        public event OnMouseEventHandler TitleMouseClick;
        public event OnMouseEventHandler TrackMouseDoubleClick;
        public event OnMouseEventHandler RollMouseDoubleClick;
        public event OnMouseEventHandler TitleMouseDoubleClick;
        public event EventHandler TrackMouseEnter;
        public event EventHandler RollMouseEnter;
        public event EventHandler TitleMouseEnter;
        public event EventHandler TrackMouseLeave;
        public event EventHandler RollMouseLeave;
        public event EventHandler TitleMouseLeave;
        #endregion

        /// <summary>
        /// 鼠标事件逻辑
        /// </summary>
        #region
        private PianoMouseEventArgs pme_cache;
        private PianoMouseEventArgs RiseMouseHandle(object sender, MouseEventArgs e, OnMouseEventHandler Roll, OnMouseEventHandler Title, OnMouseEventHandler Track)
        {
            PianoMouseEventArgs pme = new PianoMouseEventArgs(e);
            pme.CalcAxis(pprops, rconf, pme_cache);
            OnMouseEventHandler Handle = null;
            switch (pme.Area)
            {
                case PianoMouseEventArgs.AreaType.Roll: Handle = Roll; break;
                case PianoMouseEventArgs.AreaType.Title: Handle = Title; break;
                case PianoMouseEventArgs.AreaType.Track: Handle = Track; break;
            }
            if (Handle != null) Handle(sender, pme);
            return pme;
        }
        private bool pme_sendEnterEvent = false;

        bool isMMoving = false;
        private void d2DPainterBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (DisableMouse) return;
            if (isMMoving) return;
            isMMoving = true;
            d2DPainterBox1.Refresh();

            PianoMouseEventArgs pme = new PianoMouseEventArgs(e);
            pme.CalcAxis(pprops, rconf, pme_cache);
            OnMouseEventHandler Handle = null;//Move事件
            EventHandler HandleEnter = null;//Enter事件
            EventHandler HandleLeave = null;//Leave事件
            switch (pme.Area)
            {
                case PianoMouseEventArgs.AreaType.Roll: Handle = RollMouseMove; HandleEnter = RollMouseEnter; break;
                case PianoMouseEventArgs.AreaType.Title: Handle = TitleMouseMove; HandleEnter = TitleMouseEnter; break;
                case PianoMouseEventArgs.AreaType.Track: Handle = TrackMouseMove; HandleEnter = TrackMouseEnter; break;
            }
            if (pme_sendEnterEvent)
            {
                if (HandleEnter != null) HandleEnter(sender, e);
            }else if (pme_cache.Area != pme.Area)
            {
                switch (pme_cache.Area)
                {
                    case PianoMouseEventArgs.AreaType.Roll: HandleLeave = RollMouseLeave; break;
                    case PianoMouseEventArgs.AreaType.Title: HandleLeave = TitleMouseLeave; break;
                    case PianoMouseEventArgs.AreaType.Track: HandleLeave = TrackMouseLeave; break;
                }
                if (HandleEnter != null) HandleEnter(sender, e);
                if (HandleLeave != null) HandleLeave(sender, e);
            }
            if (Handle != null) Handle(sender, pme);//发送Move
            pme_cache = pme;
            pme_sendEnterEvent = false;
            this.OnMouseMove(e);
            isMMoving = false;
        }
        bool isMDown = false;
        private void d2DPainterBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (DisableMouse) return;
            if (isMDown) return;
            isMDown = true;
            pme_cache = RiseMouseHandle(sender, e,
                RollMouseDown,
                TitleMouseDown,
                TrackMouseDown);
            this.OnMouseDown(e);
            isMDown = false;
        }
        bool isMUp = false;
        private void d2DPainterBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (DisableMouse) return;
            if (isMUp) return;
            isMUp = true;
            pme_cache = RiseMouseHandle(sender, e,
                RollMouseUp,
                TitleMouseUp,
                TrackMouseUp);
            this.OnMouseUp(e);
            isMUp = false;
        }
        private void d2DPainterBox1_MouseClick(object sender, MouseEventArgs e)
        {
         //   if (DisableMouse) return;
            pme_cache = RiseMouseHandle(sender, e,
                RollMouseClick,
                TitleMouseClick,
                TrackMouseClick);
            this.OnMouseClick(e);
        }
        private void d2DPainterBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           // if (DisableMouse) return;
            pme_cache = RiseMouseHandle(sender, e,
                RollMouseDoubleClick,
                TitleMouseDoubleClick,
                TrackMouseDoubleClick);
            this.OnMouseDoubleClick(e);
        }
        private void d2DPainterBox1_MouseEnter(object sender, EventArgs e)
        {
            if (DisableMouse) return;
            this.OnMouseEnter(e);
            pme_sendEnterEvent = true;
        }
        private void d2DPainterBox1_MouseLeave(object sender, EventArgs e)
        {
            if (DisableMouse) return;
            EventHandler Handle = null;
            switch (pme_cache.Area)
            {
                case PianoMouseEventArgs.AreaType.Roll: Handle = RollMouseLeave; break;
                case PianoMouseEventArgs.AreaType.Title: Handle = RollMouseLeave; break;
                case PianoMouseEventArgs.AreaType.Track: Handle = RollMouseLeave; break;
            }
            if (Handle != null) Handle(sender, e);
            pme_sendEnterEvent = false;
            this.OnMouseLeave(e);
        }
        #endregion

        /// <summary>
        /// 键盘事件逻辑
        /// </summary>
        #region
        private void d2DPainterBox1_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void d2DPainterBox1_KeyUp(object sender, KeyEventArgs e)
        {
            OnKeyUp(e);
        }

        private void d2DPainterBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnKeyPress(e);
        }
        #endregion

        private void d2DPainterBox1_Load(object sender, EventArgs e)
        {

        }

    }
}
