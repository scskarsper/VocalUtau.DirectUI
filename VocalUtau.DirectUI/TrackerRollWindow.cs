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
        TrackerConfigures rconf = new TrackerConfigures();
        TrackerProperties pprops;
        double ShownTrackCount;
        double ShownTickCount;
        #endregion

        /// <summary>
        /// 公共属性
        /// </summary>
        #region

        public TrackerProperties TrackerProps
        {
            get
            {
                return pprops;
            }
        }
        public void setCrotchetSize(uint Size = 66)
        {
            if (Size < 32) return;
            pprops.CrotchetLengthPixel = Size;
            if (pprops != null) genShownArea();
            d2DPainterBox1.Refresh();
        }
        public void setTrackHeight(uint Size = 18)
        {
            if (Size < 18) return;
            rconf.setTrackHeight(Size);
            SetScrollMax();
            d2DPainterBox1.Refresh();
        }
       
        public void setPianoStartTick(long Tick)
        {
            TrackerProps.PianoStartTick = Tick;
            d2DPainterBox1.Refresh();
        }

        public uint MinShownTrackerNumber { get { return pprops.TopTrackId; } }
        public uint MaxShownTrackerNumber { get { return MinShownTrackerNumber + (uint)(ShownTrackCount>1?ShownTrackCount-1:1); } }
        public long MaxShownTick { get { return MinShownTick + (long)Math.Round(ShownTickCount, 0); } }
        public long MinShownTick { get { return pprops.PianoStartTick; } }
        public bool IsInitalized { get { return (pprops != null && rconf != null); } }
        public uint ShownTrackerCount { get { 
                int TotalHeight=d2DPainterBox1.ClientRectangle.Height - rconf.Const_TitleHeight;
                if (TotalHeight < 0) return 0;
                int Count = TotalHeight / rconf.Const_TrackHeight;
                return (uint)Count;        
        } }
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
            ShownTrackCount = (double)(d2DPainterBox1.ClientRectangle.Height - rconf.Const_TitleHeight) / rconf.Const_TrackHeight;
            ShownTickCount = pprops.dertPixel2dertTick(d2DPainterBox1.ClientRectangle.Width - rconf.Const_GridWidth);
        }
        void SetScrollMax()
        {
            //TODO
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
            if (pprops != null) genShownArea();
        }
        private void TrackerRollWindow_Resize(object sender, EventArgs e)
        {
            if (TrackerRollBeforeResize != null) TrackerRollBeforeResize(sender, e);
            InitGUI();
            if (TrackerRollAfterResize != null) TrackerRollAfterResize(sender, e);
        }
        private void noteScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            pprops.TopTrackId = (uint)(noteScrollBar1.Value < 0 ? 0 : noteScrollBar1.Value);
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
            pprops = new TrackerProperties(rconf);
            pprops.TopTrackId = 0;
            InitGUI();
        }
        public void RedrawPiano()
        {

            d2DPainterBox1.Refresh();
        }
        public void setScrollBarMax(uint Value)
        {
            if (noteScrollBar1.Value < Value)
            {
                noteScrollBar1.Value = 0;
                noteScrollBar1_Scroll(null,null);
            }
            noteScrollBar1.Maximum = (int)Value;
        }
        #endregion


        /// <summary>
        /// 绘图冒泡事件声明
        /// </summary>
        #region
        // 创建一个委托，返回类型为void，两个参数
        public delegate void OnTrackerPartsDrawHandler(object sender, DrawUtils.TrackerPartsDrawUtils utils);
        public delegate void OnTrackerTitleDrawHandler(object sender, DrawUtils.TrackerTitlesDrawUtils utils);
        public delegate void OnTrackerGrideDrawHandler(object sender,  DrawUtils.TrackerGridesDrawUtils utils);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event OnTrackerPartsDrawHandler TPartsPaint;
        public event OnTrackerGrideDrawHandler TGridePaint;
        public event OnTrackerTitleDrawHandler TTitlePaint;
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
                PianoRollPoint sp = pprops.getPianoStartPoint();
                DrawTrackPartsArea(sender, e, sp);
                DrawTrackGrideArea(sender, e);
                DrawTrackTitleArea(sender, e, sp);
            }
            catch { ;}
            isDrawing = false;
        }
        private void DrawTrackPartsArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e, PianoRollPoint startPoint)
        {
            D2DGraphics g = e.D2DGraphics;

             int y = rconf.Const_TitleHeight;//绘制点纵坐标
             int w = e.ClipRectangle.Width - rconf.Const_VScrollBarWidth;
             //绘制分节符
             //初始化
             double x = rconf.Const_GridWidth;//起点画线
             long BeatNumber = startPoint.BeatNumber;//获取起点拍号
             double BeatPixelLength = pprops.dertTick2dertPixel(pprops.BeatLength);//一拍长度
             if (startPoint.DenominatolTicksBefore != 0)//非完整Beats
             {
                 //起点不在小节线
                 BeatNumber = startPoint.NextWholeBeatNumber;
                 x = x + pprops.dertTick2dertPixel(startPoint.NextWholeBeatDistance);
             }
            while (x <= w)
            {
                g.DrawLine(
                new Point((int)Math.Round(x, 0), rconf.Const_TitleHeight),
                new Point((int)Math.Round(x, 0), e.ClipRectangle.Height),
                    BeatNumber % pprops.BeatsCountPerSummery == 0 ? Color.White : rconf.TitleColor_Marker
                );
                BeatNumber = BeatNumber + 1;
                x = x + BeatPixelLength;
            }


            //Rise 绘制Note等//传递事件
            Rectangle CurrentRect = new Rectangle(
                rconf.Const_GridWidth,
                rconf.Const_TitleHeight,
                w - rconf.Const_GridWidth,
                e.ClipRectangle.Height - rconf.Const_TitleHeight);//可绘制区域
            if (TPartsPaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                    );
                TPartsPaint(sender, new DrawUtils.TrackerPartsDrawUtils(d2de, rconf,pprops));
            }
        }
        private void DrawTrackTitleArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e, PianoRollPoint startPoint)
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
                rconf.Const_TitleHeight - rconf.Const_TitleHeightSpliter - 1,
                e.ClipRectangle.Width,
                rconf.Const_TitleHeightSpliter
            );
            g.DrawLine(new Point(0, rconf.Const_TitleLineTop), new Point(e.ClipRectangle.Width, rconf.Const_TitleLineTop), rconf.TitleColor_Line, 2);
            g.DrawLine(new Point(0, rconf.Const_TitleRulerTop), new Point(e.ClipRectangle.Width, rconf.Const_TitleRulerTop), rconf.TitleColor_Ruler, 2);

            //绘制分节符

            //初始化
            double x = rconf.Const_GridWidth;//起点画线
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
                    long SummeryId = BeatNumber / pprops.BeatsCountPerSummery;
                    g.DrawText(" " + SummeryId.ToString(),
                        new Rectangle(new Point((int)Math.Round(x, 0), rconf.Const_TitleLineTop), new Size((int)pprops.BeatLength, rconf.Const_TitleRulerTop - rconf.Const_TitleLineTop)),
                        Color.White,
                        new Font("宋体", 12));
                }
                else
                {
                    g.DrawLine(
                    new Point((int)Math.Round(x, 0), My1),
                    new Point((int)Math.Round(x, 0), My2),
                    rconf.TitleColor_Marker);
                }
                BeatNumber = BeatNumber + 1;
                x = x + BeatPixelLength;
            }

            //绘制分割线
            g.DrawLine(new Point(rconf.Const_GridWidth, 0), new Point(rconf.Const_GridWidth, rconf.Const_TitleHeight), Color.White, 2);
            g.FillRectangle(TitleSpliterRect, rconf.PianoColor_WhiteKey);

            g.DrawText("Tempo: "+String.Format("{0:0.00}",pprops.Tempo),
                new Rectangle(new Point(0, rconf.Const_TitleLineTop), new Size((int)rconf.Const_GridWidth, rconf.Const_TitleRulerTop - rconf.Const_TitleLineTop)),
                Color.White,
                new Font("Tahoma", 10, FontStyle.Bold));

            Rectangle CurrentRect = new Rectangle(
                0,
                0,
                e.ClipRectangle.Width,
                rconf.Const_TitleHeight
            );//可绘制区域
            if (TTitlePaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                );
                TTitlePaint(sender, new VocalUtau.DirectUI.DrawUtils.TrackerTitlesDrawUtils(d2de, rconf));
            }
        }
        private void DrawTrackGrideArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e)
        {
            D2DGraphics g = e.D2DGraphics;

            Rectangle CurrentRect = new Rectangle(
                0,
                rconf.Const_TitleHeight,
                rconf.Const_GridWidth,
                e.ClipRectangle.Height - rconf.Const_TitleHeight);//可绘制区域

            g.FillRectangle(CurrentRect, Color.Black);
            g.DrawLine(new Point(rconf.Const_GridWidth, rconf.Const_TitleHeight), new Point(rconf.Const_GridWidth, e.ClipRectangle.Height), Color.White, 2);

            if (TGridePaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                    );
                TGridePaint(sender, new DrawUtils.TrackerGridesDrawUtils(d2de, rconf, pprops));
            }
        }
        #endregion


        /// <summary>
        /// 鼠标冒泡事件声明
        /// </summary>
        #region
        // 创建一个委托，返回类型为void，两个参数
        public delegate void OnMouseEventHandler(object sender, TrackerMouseEventArgs e);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event OnMouseEventHandler PartsMouseDown;
        public event OnMouseEventHandler GridsMouseDown;
        public event OnMouseEventHandler TitleMouseDown;
        public event OnMouseEventHandler PartsMouseUp;
        public event OnMouseEventHandler GridsMouseUp;
        public event OnMouseEventHandler TitleMouseUp;
        public event OnMouseEventHandler PartsMouseMove;
        public event OnMouseEventHandler GridsMouseMove;
        public event OnMouseEventHandler TitleMouseMove;
        public event OnMouseEventHandler PartsMouseClick;
        public event OnMouseEventHandler GridsMouseClick;
        public event OnMouseEventHandler TitleMouseClick;
        public event OnMouseEventHandler PartsMouseDoubleClick;
        public event OnMouseEventHandler GridsMouseDoubleClick;
        public event OnMouseEventHandler TitleMouseDoubleClick;
        public event EventHandler PartsMouseEnter;
        public event EventHandler GridsMouseEnter;
        public event EventHandler TitleMouseEnter;
        public event EventHandler PartsMouseLeave;
        public event EventHandler GridsMouseLeave;
        public event EventHandler TitleMouseLeave;
        #endregion

        /// <summary>
        /// 鼠标事件逻辑
        /// </summary>
        #region
        private TrackerMouseEventArgs pme_cache;
        private GridesMouseEventArgs pge_cache;
        private TrackerMouseEventArgs RiseMouseHandle(object sender, MouseEventArgs e, OnMouseEventHandler Roll, OnMouseEventHandler Title, OnMouseEventHandler Track)
        {
            TrackerMouseEventArgs pme = new TrackerMouseEventArgs(e);
            pme.CalcAxis(pprops, rconf, pme_cache);
            pme.Tag=null;
            OnMouseEventHandler Handle = null;
            switch (pme.Area)
            {
                case TrackerMouseEventArgs.AreaType.Roll: Handle = Roll;break;
                case TrackerMouseEventArgs.AreaType.Title: Handle = Title; break;
                case TrackerMouseEventArgs.AreaType.Track: Handle = Track; break;
            }
            if (pme.Area == TrackerMouseEventArgs.AreaType.Roll)
            {
                GridesMouseEventArgs pge = new GridesMouseEventArgs(e);
                pge.CalcArea(pprops, rconf, pge_cache);
                pme.Tag = pge;
            }
            if (Handle != null) Handle(sender, pme);
            return pme;
        }
        private bool pme_sendEnterEvent = false;

        bool isMMoving = false;
        private void d2DPainterBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMMoving) return;
            isMMoving = true;
            d2DPainterBox1.Refresh();

            TrackerMouseEventArgs pme = new TrackerMouseEventArgs(e);
            pme.CalcAxis(pprops, rconf, pme_cache);
            pme.Tag = null;
            OnMouseEventHandler Handle = null;//Move事件
            EventHandler HandleEnter = null;//Enter事件
            EventHandler HandleLeave = null;//Leave事件
            switch (pme.Area)
            {
                case TrackerMouseEventArgs.AreaType.Roll: Handle = GridsMouseMove; HandleEnter = GridsMouseEnter; break;
                case TrackerMouseEventArgs.AreaType.Title: Handle = TitleMouseMove; HandleEnter = TitleMouseEnter; break;
                case TrackerMouseEventArgs.AreaType.Track: Handle = PartsMouseMove; HandleEnter = PartsMouseEnter; break;
            }
            if (pme_sendEnterEvent)
            {
                if (HandleEnter != null) HandleEnter(sender, e);
            }
            else if (pme_cache.Area != pme.Area)
            {
                switch (pme_cache.Area)
                {
                    case TrackerMouseEventArgs.AreaType.Roll: HandleLeave = GridsMouseLeave; break;
                    case TrackerMouseEventArgs.AreaType.Title: HandleLeave = TitleMouseLeave; break;
                    case TrackerMouseEventArgs.AreaType.Track: HandleLeave = PartsMouseLeave; break;
                }
                if (HandleEnter != null) HandleEnter(sender, e);
                if (HandleLeave != null) HandleLeave(sender, e);
            }
            if (pme.Area == TrackerMouseEventArgs.AreaType.Roll)
            {
                GridesMouseEventArgs pge = new GridesMouseEventArgs(e);
                pge.CalcArea(pprops, rconf, pge_cache);
                pme.Tag = pge;
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
            if (isMDown) return;
            isMDown = true;
            pme_cache = RiseMouseHandle(sender, e,
                GridsMouseDown,
                TitleMouseDown,
                PartsMouseDown);
            this.OnMouseDown(e);
            isMDown = false;
        }
        bool isMUp = false;
        private void d2DPainterBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMUp) return;
            isMUp = true;
            pme_cache = RiseMouseHandle(sender, e,
                GridsMouseUp,
                TitleMouseUp,
                PartsMouseUp);
            this.OnMouseUp(e);
            isMUp = false;
        }
        private void d2DPainterBox1_MouseClick(object sender, MouseEventArgs e)
        {
            pme_cache = RiseMouseHandle(sender, e,
                GridsMouseClick,
                TitleMouseClick,
                PartsMouseClick);
            this.OnMouseClick(e);
        }
        private void d2DPainterBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pme_cache = RiseMouseHandle(sender, e,
                GridsMouseDoubleClick,
                TitleMouseDoubleClick,
                PartsMouseDoubleClick);
            this.OnMouseDoubleClick(e);
        }
        private void d2DPainterBox1_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
            pme_sendEnterEvent = true;
        }
        private void d2DPainterBox1_MouseLeave(object sender, EventArgs e)
        {
            EventHandler Handle = null;
            switch (pme_cache.Area)
            {
                case TrackerMouseEventArgs.AreaType.Roll: Handle = GridsMouseLeave; break;
                case TrackerMouseEventArgs.AreaType.Title: Handle = GridsMouseLeave; break;
                case TrackerMouseEventArgs.AreaType.Track: Handle = GridsMouseLeave; break;
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
