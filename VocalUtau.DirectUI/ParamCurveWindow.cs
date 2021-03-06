﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BalthasarLib.D2DPainter;

namespace VocalUtau.DirectUI
{
    public partial class ParamCurveWindow : UserControl
    {

        public bool DisableMouse { get; set; }
        /// <summary>
        /// 私有属性
        /// </summary>
        #region
        RollConfigures rconf = new RollConfigures();
        PianoProperties pprops;
        double ShownTickCount;
        #endregion

        /// <summary>
        /// 公共属性
        /// </summary>
        #region
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
        public void setPianoStartTick(long Tick)
        {
            PianoProps.PianoStartTick = Tick;
            d2DPainterBox1.Refresh();
        }

        public long MaxShownTick { get { return MinShownTick+(long)Math.Round(ShownTickCount,0); } }
        public long MinShownTick { get { return pprops.PianoStartTick; } }
        public bool IsInitalized { get { return (pprops!=null && rconf!=null);} }
        #endregion

        /// <summary>
        /// 界面冒泡事件声明
        /// </summary>
        #region
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event EventHandler ParametersWindowBeforeResize;
        public event EventHandler ParametersWindowAfterResize;
        #endregion
        /// <summary>
        /// 基础逻辑-私有
        /// </summary>
        #region
        void genShownArea()
        {
            ShownTickCount = pprops.dertPixel2dertTick(d2DPainterBox1.ClientRectangle.Width-rconf.Const_RollWidth);
        }
        void InitGUI()
        {
            d2DPainterBox1.Top = 0;
            d2DPainterBox1.Left = 0;
            d2DPainterBox1.Width = this.ClientRectangle.Width;
            d2DPainterBox1.Height = this.ClientRectangle.Height;
            if(pprops!=null)genShownArea();
        }
        private void ParametersWindow_Resize(object sender, EventArgs e)
        {
            if (ParametersWindowBeforeResize != null) ParametersWindowBeforeResize(sender, e);
            InitGUI();
            if (ParametersWindowAfterResize != null) ParametersWindowAfterResize(sender, e);
        }
        #endregion

        /// <summary>
        /// 基础逻辑-公有
        /// </summary>
        #region
        
        public ParamCurveWindow()
        {
            InitializeComponent();
            FpsCreater.Interval = 1000 / GlobalStatic.StaticFps;
            FpsCreater.Tick += FpsCreater_Tick;
            FpsCreater.Enabled = true;
            pprops = new PianoProperties(rconf);
            InitGUI();
        }
        void FpsCreater_Tick(object sender, EventArgs e)
        {
            PaintFPS();
        }
        public void RedrawPiano(bool force=false)
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
        public delegate void OnParameterButtonsDrawHandler(object sender, DrawUtils.ParamBtnsDrawUtils utils);
        public delegate void OnParameterAreaDrawHandler(object sender, DrawUtils.ParamAreaDrawUtils utils);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event OnParameterButtonsDrawHandler ParamBtnsPaint;
        public event OnParameterAreaDrawHandler ParamAreaPaint;
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
                DrawParameterLinesArea(sender, e, sp);
                DrawParameterButtonsArea(sender, e, sp);
                e.D2DGraphics.DrawLine(new Point(0, 1), new Point(d2DPainterBox1.Width, 1), SystemColors.Control);
            }
            catch { ;}
            isDrawing = false;
        }
        private void DrawParameterButtonsArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e, PianoRollPoint startPoint)
        {
            D2DGraphics g = e.D2DGraphics;
            Rectangle BlackRect = new Rectangle(
                0,
                0,
                rconf.Const_RollWidth,
                e.ClipRectangle.Height
            );
            g.FillRectangle(BlackRect, Color.Black);
            //绘制分割线
            g.DrawLine(new Point(rconf.Const_RollWidth, 0), new Point(rconf.Const_RollWidth, e.ClipRectangle.Height), Color.White, 1);
           
            Rectangle CurrentRect = new Rectangle(
                0,
                0,
                rconf.Const_RollWidth,
                e.ClipRectangle.Height
            );//可绘制区域

            if (ParamBtnsPaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    CurrentRect,
                    e.MousePoint
                );
                ParamBtnsPaint(sender, new VocalUtau.DirectUI.DrawUtils.ParamBtnsDrawUtils(d2de, rconf));
            }
        }
        private void DrawParameterLinesArea(object sender, BalthasarLib.D2DPainter.D2DPaintEventArgs e, PianoRollPoint startPoint)
        {
            D2DGraphics g = e.D2DGraphics;
            Rectangle BlackRect = new Rectangle(
                rconf.Const_RollWidth,
                0,
                e.ClipRectangle.Width - rconf.Const_RollWidth,
                e.ClipRectangle.Height
            );
           // g.FillRectangle(BlackRect, Color.Black);

            if (ParamAreaPaint != null)
            {
                D2DPaintEventArgs d2de = new D2DPaintEventArgs(
                    e.D2DGraphics,
                    BlackRect,
                    e.MousePoint
                    );
                ParamAreaPaint(sender, new VocalUtau.DirectUI.DrawUtils.ParamAreaDrawUtils(d2de, rconf, pprops));
            }

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
            while (x <= e.ClipRectangle.Width)
            {
                if (BeatNumber % pprops.BeatsCountPerSummery == 0)
                {
                    g.DrawLine(
                    new Point((int)Math.Round(x, 0), 0),
                    new Point((int)Math.Round(x, 0), BlackRect.Height),
                    rconf.TitleColor_Marker);
                    long SummeryId = BeatNumber / pprops.BeatsCountPerSummery;
                }
                BeatNumber = BeatNumber + 1;
                x = x + BeatPixelLength;
            }
            g.DrawLine(
                    new Point(BlackRect.Left, BlackRect.Height / 2),
                    new Point(BlackRect.Width+BlackRect.Left, BlackRect.Height/2),
                    rconf.TitleColor_Marker);
        }
        #endregion


        /// <summary>
        /// 鼠标冒泡事件声明
        /// </summary>
        #region
        // 创建一个委托，返回类型为void，两个参数
        public delegate void OnMouseEventHandler(object sender, ParamMouseEventArgs e);
        // 将创建的委托和特定事件关联,在这里特定的事件为KeyDown
        public event OnMouseEventHandler ParamAreaMouseDown;
        public event OnMouseEventHandler ParamBtnsMouseDown;
        public event OnMouseEventHandler ParamAreaMouseUp;
        public event OnMouseEventHandler ParamBtnsMouseUp;
        public event OnMouseEventHandler ParamAreaMouseMove;
        public event OnMouseEventHandler ParamBtnsMouseMove;
        public event OnMouseEventHandler ParamAreaMouseClick;
        public event OnMouseEventHandler ParamBtnsMouseClick;
        public event OnMouseEventHandler ParamAreaMouseDoubleClick;
        public event OnMouseEventHandler ParamBtnsMouseDoubleClick;
        public event EventHandler ParamAreaMouseEnter;
        public event EventHandler ParamBtnsMouseEnter;
        public event EventHandler ParamAreaMouseLeave;
        public event EventHandler ParamBtnsMouseLeave;
        #endregion

        /// <summary>
        /// 鼠标事件逻辑
        /// </summary>
        #region
        private ParamMouseEventArgs pme_cache;
        private ParamMouseEventArgs RiseMouseHandle(object sender, MouseEventArgs e, OnMouseEventHandler Area, OnMouseEventHandler Btns)
        {
            ParamMouseEventArgs pme = new ParamMouseEventArgs(e);
            pme.CalcAxis(pprops, rconf, pme_cache,this.ClientRectangle.Height);
            OnMouseEventHandler Handle = null;
            switch (pme.Area)
            {
                case ParamMouseEventArgs.AreaType.Btns: Handle = Btns; break;
                case ParamMouseEventArgs.AreaType.Area : Handle = Area; break;
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

            ParamMouseEventArgs pme = new ParamMouseEventArgs(e);
            pme.CalcAxis(pprops, rconf, pme_cache, this.ClientRectangle.Height);
            OnMouseEventHandler Handle = null;//Move事件
            EventHandler HandleEnter = null;//Enter事件
            EventHandler HandleLeave = null;//Leave事件
            switch (pme.Area)
            {
                case ParamMouseEventArgs.AreaType.Area: Handle = ParamAreaMouseMove; HandleEnter = ParamAreaMouseEnter; break;
                case ParamMouseEventArgs.AreaType.Btns: Handle = ParamBtnsMouseMove; HandleEnter = ParamBtnsMouseEnter; break;
            }
            if (pme_sendEnterEvent)
            {
                if (HandleEnter != null) HandleEnter(sender, e);
            }else if (pme_cache.Area != pme.Area)
            {
                switch (pme_cache.Area)
                {
                    case ParamMouseEventArgs.AreaType.Area: HandleLeave = ParamAreaMouseLeave; break;
                    case ParamMouseEventArgs.AreaType.Btns: HandleLeave = ParamBtnsMouseLeave; break;
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
                ParamAreaMouseDown,
                ParamBtnsMouseDown);
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
                ParamAreaMouseUp,
                ParamBtnsMouseUp);
            this.OnMouseUp(e);
            isMUp = false;
        }
        private void d2DPainterBox1_MouseClick(object sender, MouseEventArgs e)
        {
          //  if (DisableMouse) return;
            pme_cache = RiseMouseHandle(sender, e,
                ParamAreaMouseClick,
                ParamBtnsMouseClick);
            this.OnMouseClick(e);
        }
        private void d2DPainterBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
         //   if (DisableMouse) return;
            pme_cache = RiseMouseHandle(sender, e,
                ParamAreaMouseDoubleClick,
                ParamBtnsMouseDoubleClick);
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
                case ParamMouseEventArgs.AreaType.Area: Handle = ParamAreaMouseLeave; break;
                case ParamMouseEventArgs.AreaType.Btns: Handle = ParamAreaMouseLeave; break;
            }
            if (Handle != null) Handle(sender, e);
            pme_sendEnterEvent = false;
            this.OnMouseLeave(e);
        }
        #endregion

        private void d2DPainterBox1_Load(object sender, EventArgs e)
        {
        
        }
    }
}
