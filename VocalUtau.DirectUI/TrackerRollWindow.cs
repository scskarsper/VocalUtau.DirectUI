using System;
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
    public partial class TrackerRollWindow : UserControl
    {

        /// <summary>
        /// 私有属性
        /// </summary>
        #region
        TrackerConfigures tconf = new TrackerConfigures();
        TrackerProperties tprops;
        double ShownTimeCount;
        int ShownTrackCount;
        #endregion

        /// <summary>
        /// 公共属性
        /// </summary>
        #region

        public TrackerProperties TrackerProps
        {
            get
            {
                return tprops;
            }
        }
        public void setTrackerStartTime(double starttime)
        {
            tprops.Starttime = starttime;
            d2DPainterBox1.Refresh();
        }

        public double MaxShownTick { get { return tprops.Starttime + ShownTimeCount; } }
        public double MinShownTick { get { return tprops.Starttime; } }
        public double MinTrackId { get { return tprops.TopTrackId; } }
        public double MaxTrackId { get { return tprops.TopTrackId+ShownTrackCount;} }
        public bool IsInitalized { get { return (tprops!=null && tconf!=null);} }
        #endregion

        /// <summary>
        /// 界面冒泡事件声明
        /// </summary>
        #region
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event EventHandler TrackerRollBeforeResize;
        public event EventHandler TrackerRollAfterResize;
        #endregion
        /// <summary>
        /// 基础逻辑-私有
        /// </summary>
        #region
        void genShownArea()
        {
            ShownTrackCount = (int)Math.Ceiling((double)(d2DPainterBox1.ClientRectangle.Height - tconf.Const_TitleHeight) / tconf.Const_TrachHeight);
            ShownTimeCount = tprops.Pixel2Ms(d2DPainterBox1.ClientRectangle.Width - tconf.Const_GridWidth);
        }
       /* void SetScrollMax()
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
        }*/
        void InitGUI()
        {
            //SetScrollMax();
            noteScrollBar1.Height = this.ClientRectangle.Height - tconf.Const_TitleHeight;
            noteScrollBar1.Top = tconf.Const_TitleHeight;
            noteScrollBar1.Width = tconf.Const_VScrollBarWidth;
            noteScrollBar1.Left = this.ClientRectangle.Width - tconf.Const_VScrollBarWidth;
            d2DPainterBox1.Top = 0;
            d2DPainterBox1.Left = 0;
            d2DPainterBox1.Width = this.ClientRectangle.Width;
            d2DPainterBox1.Height = this.ClientRectangle.Height;
            if(tprops!=null)genShownArea();
        }
        private void TrackerRollWindow_Resize(object sender, EventArgs e)
        {
            if (TrackerRollBeforeResize != null) TrackerRollBeforeResize(sender, e);
            InitGUI();
            if (TrackerRollAfterResize != null) TrackerRollAfterResize(sender, e);
        }
        private void noteScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int TopNote = noteScrollBar1.Value;
            tprops.TopTrackId = (uint)(TopNote > 0 ? TopNote : 0);
            d2DPainterBox1.Refresh();
        }
        #endregion

        /// <summary>
        /// 基础逻辑-公有
        /// </summary>
        #region
        public TrackerRollWindow()
        {
            InitializeComponent();
            tprops = new TrackerProperties(tconf);
            InitGUI();
        }
        public void RedrawPiano()
        {
            d2DPainterBox1.Refresh();
        }
        #endregion


        /// <summary>
        /// 绘图冒泡事件声明
        /// </summary>
        #region
        // 创建一个委托，返回类型为void，两个参数
        public delegate void OnTrackerPartsDrawHandler(object sender, DrawUtils.TrackerPartsDrawUtils utils);
        public delegate void OnTrackerTitleDrawHandler(object sender, DrawUtils.TitleDrawUtils utils);
        public delegate void OnTrackerGridDrawHandler(object sender, DrawUtils.TrackerGridesDrawUtils utils);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event OnTrackerPartsDrawHandler PartsPaint;
        public event OnTrackerTitleDrawHandler TitlePaint;
        public event OnTrackerGridDrawHandler GridPaint;
        #endregion

        /// <summary>
        /// 绘图逻辑
        /// </summary>
        #region
        bool isDrawing = false;
        private void d2DPainterBox1_D2DPaint(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e)
        {
            if (isDrawing) return;
            isDrawing = true;
            try
            {
              //  PianoRollPoint sp = pprops.getPianoStartPoint();
              //  DrawPianoTrackArea(sender, e, sp);
                DrawTrackerGridArea(sender, e);
                DrawTrackerTitleArea(sender, e);
            }
            catch { ;}
            isDrawing = false;
        }
        private void DrawPianoTrackArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e, PianoRollPoint startPoint)
        {
         /*   D2DGraphics g = e.D2DGraphics;

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
            if (PartsPaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                    );
                PartsPaint(sender, new VocalUtau.DirectUI.DrawUtils.TrackDrawUtils(d2de, rconf, pprops));
            }*/
        }
        private void DrawTrackerTitleArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e)
        {
            D2DGraphics g = e.D2DGraphics;
            Rectangle BlackRect = new Rectangle(
                0,
                0,
                e.ClipRectangle.Width,
                tconf.Const_TitleHeight
            );
            g.FillRectangle(BlackRect, Color.Black);
            Rectangle TitleSpliterRect = new Rectangle(
                0, 
                tconf.Const_TitleHeight-tconf.Const_TitleHeightSpliter-1, 
                e.ClipRectangle.Width, 
                tconf.Const_TitleHeightSpliter
            );
            g.DrawLine(new Point(0, tconf.Const_TitleLineTop), new Point(e.ClipRectangle.Width, tconf.Const_TitleLineTop), tconf.TitleColor_Line,2);
            g.DrawLine(new Point(0, tconf.Const_TitleRulerTop), new Point(e.ClipRectangle.Width, tconf.Const_TitleRulerTop), tconf.TitleColor_Ruler,2);
            
            //绘制分节符
            /*
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
            */
            //绘制分割线
            g.DrawLine(new Point(tconf.Const_GridWidth, 0), new Point(tconf.Const_GridWidth, tconf.Const_TitleHeight), Color.White, 2);
            g.FillRectangle(TitleSpliterRect, Color.White);

            Rectangle CurrentRect = new Rectangle(
                0,
                0,
                e.ClipRectangle.Width,
                tconf.Const_TitleHeight
            );//可绘制区域
            if (TitlePaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                );
               // TitlePaint(sender, new VocalUtau.DirectUI.DrawUtils.TitleDrawUtils(d2de, rconf, pprops));
            }
        }
        private void DrawTrackerGridArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e)
        {
            D2DGraphics g = e.D2DGraphics;

            Rectangle CurrentRect = new Rectangle(
                0,
                tconf.Const_TitleHeight,
                tconf.Const_GridWidth,
                e.ClipRectangle.Height - tconf.Const_TitleHeight);//可绘制区域

            g.DrawLine(new Point(tconf.Const_GridWidth, tconf.Const_TitleHeight), new Point(tconf.Const_GridWidth, e.ClipRectangle.Height), Color.White, 2);

            if (GridPaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                    );
            //    GridPaint(sender, new VocalUtau.DirectUI.DrawUtils.TrackerGridesDrawUtils(d2de,rconf, pprops));
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
          /*  PianoMouseEventArgs pme = new PianoMouseEventArgs(e);
            pme.CalcAxis(pprops, rconf, pme_cache);
            OnMouseEventHandler Handle = null;
            switch (pme.Area)
            {
                case PianoMouseEventArgs.AreaType.Roll: Handle = Roll; break;
                case PianoMouseEventArgs.AreaType.Title: Handle = Title; break;
                case PianoMouseEventArgs.AreaType.Track: Handle = Track; break;
            }
            if (Handle != null) Handle(sender, pme);
            return pme;*/
            return null;
        }
        private bool pme_sendEnterEvent = false;

        bool isMMoving = false;
        private void d2DPainterBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMMoving) return;
            isMMoving = true;
            d2DPainterBox1.Refresh();

            /* PianoMouseEventArgs pme = new PianoMouseEventArgs(e);
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
             pme_sendEnterEvent = false;*/
            this.OnMouseMove(e);
            isMMoving = false;
        }
        bool isMDown = false;
        private void d2DPainterBox1_MouseDown(object sender, MouseEventArgs e)
        {
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
            pme_cache = RiseMouseHandle(sender, e,
                RollMouseClick,
                TitleMouseClick,
                TrackMouseClick);
            this.OnMouseClick(e);
        }
        private void d2DPainterBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pme_cache = RiseMouseHandle(sender, e,
                RollMouseDoubleClick,
                TitleMouseDoubleClick,
                TrackMouseDoubleClick);
            this.OnMouseDoubleClick(e);
        }
        private void d2DPainterBox1_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
            pme_sendEnterEvent = true;
        }
        private void d2DPainterBox1_MouseLeave(object sender, EventArgs e)
        {
            return;//DEBUG
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
