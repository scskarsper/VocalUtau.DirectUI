using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Utils.TrackerUtils
{
    public class TrackerView
    {
        public delegate void OnPartsEventHandler(PartsDragingType eventType);
        public event OnPartsEventHandler TrackerActionEnd;
        public event OnPartsEventHandler TrackerActionBegin;
        public enum PartsDragingType
        {
            None,
            PartsMove
        }
        PartsDragingType _TrackerDragingStatus = PartsDragingType.None;

        IntPtr ProjectObjectPtr = IntPtr.Zero;
        TrackerRollWindow TrackerWindow;

        bool _HandleEvents = false;
        public bool HandleEvents
        {
            get { return _HandleEvents; }
            set { _HandleEvents = value; }
        }

        public TrackerView(IntPtr ProjectObjectPtr, TrackerRollWindow TrackerWindow)
        {
            this.TrackerWindow = TrackerWindow;
            this.ProjectObjectPtr = ProjectObjectPtr;
            hookTrackerWindow();
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
        private List<TrackerObject> TrackerList
        {
            get
            {
                if (ProjectObject == null) return new List<TrackerObject>();
                return ProjectObject.TrackerList;
            }
        }
        private List<BackerObject> BackerList
        {
            get
            {
                if (ProjectObject == null) return new List<BackerObject>();
                return ProjectObject.BackerList;
            }
        }

        void ResetShowingParts()
        {
            for (int i = 0; i < ProjectObject.TrackerList.Count; i++)
            {
                for (int j = 0; i < ProjectObject.TrackerList[i].PartList.Count; j++)
                {
                    ShowingPartLocation = new PartLocation(new TrackLocation((uint)i, ProjectObject), (uint)j, null);
                    break;
                }
                if (ShowingPartLocation != null) break;
            }
        }
        public void hookTrackerWindow()
        {
            ResetShowingParts();
            TrackerWindow.TrackerProps.Tempo = ProjectObject.BaseTempo;
            TrackerWindow.TPartsPaint += TrackerWindow_TPartsPaint;
            TrackerWindow.TGridePaint += TrackerWindow_TGridePaint;
            TrackerWindow.PartsMouseDown += TrackerWindow_PartsMouseDown;
            TrackerWindow.PartsMouseUp += TrackerWindow_PartsMouseUp;
            TrackerWindow.PartsMouseMove += TrackerWindow_PartsMouseMove;
            TrackerWindow.PartsMouseClick += TrackerWindow_PartsMouseClick;
            TrackerWindow.PartsMouseDoubleClick += TrackerWindow_PartsMouseDoubleClick;
        }

        void TrackerWindow_PartsMouseDoubleClick(object sender, Models.TrackerMouseEventArgs e)
        {
            ResetMouse();
        }

        void TrackerWindow_PartsMouseClick(object sender, Models.TrackerMouseEventArgs e)
        {
        }


        public class TrackLocation
        {
            public TrackLocation(uint AbsoluteTrackID,ProjectObject ProjectObject)
            {
                uint atid = AbsoluteTrackID;
                if (atid >= ProjectObject.TrackerList.Count)
                {
                    this.TrackID = (uint)(atid - ProjectObject.TrackerList.Count);
                    this.Type = TrackType.Barker;
                    this.AbsoluteTrackID = AbsoluteTrackID;
                    if (TrackID >= ProjectObject.BackerList.Count)
                    {
                        this.TrackID =(uint)(this.TrackID - ProjectObject.BackerList.Count);
                        this.Type = TrackType.Outside;
                    }
                    //Barker
                }
                else
                {
                    this.TrackID = atid;
                    this.Type = TrackType.Tracker;
                    this.AbsoluteTrackID = AbsoluteTrackID;
                }
            }
            public TrackLocation(uint TrackID, uint AbsoluteTrackID, TrackType Type)
            {
                this.TrackID = TrackID;
                this.Type = Type;
                this.AbsoluteTrackID = AbsoluteTrackID;
            }
            public uint TrackID;
            public uint AbsoluteTrackID;
            public enum TrackType
            {
                Barker,
                Tracker,
                Outside
            }
            public TrackType Type;

            public override bool Equals(object obj)
            {
                if (obj == null)//步骤1
                    return false;
                if (this.GetType() != obj.GetType())//步骤3
                    return false;
                if (obj.GetType() == typeof(TrackLocation))
                {
                    TrackLocation TB = (TrackLocation)obj;
                    if (TB.Type != this.Type) return false;
                    if (TB.TrackID != this.TrackID) return false;
                    if (TB.AbsoluteTrackID != this.AbsoluteTrackID) return false;
                    return true;
                }
                return base.Equals(obj);
            }
        }
        public class PartLocation
        {
            public PartLocation(TrackLocation TrackLocation, uint PartID , Models.TrackerMouseEventArgs e)
            {
                this.TrackLocation = TrackLocation;
                this.e=e;
                this.PartID = PartID;
            }
            public TrackLocation TrackLocation;
            public Models.TrackerMouseEventArgs e;
            public uint PartID;
            public override bool Equals(object obj)
            {
                if (obj == null)//步骤1
                    return false;
                if (this.GetType() != obj.GetType())//步骤3
                    return false;
                if (obj.GetType() == typeof(PartLocation))
                {
                    PartLocation TB = (PartLocation)obj;
                    if (this.PartID != TB.PartID) return false;
                    if (this.e.Tick != TB.e.Tick) return false;
                    if (!this.e.MouseEventArgs.Equals(TB.e.MouseEventArgs)) return false;
                    return this.TrackLocation.Equals(TB.TrackLocation);
                }
                return base.Equals(obj);
            }
        }
        PartLocation GetLocation(Models.TrackerMouseEventArgs e)
        {
            PartLocation pLocate = null;
            TrackLocation tLocate = new TrackLocation((uint)e.AbsoluteTrackID, this.ProjectObject);
            if (tLocate.Type== TrackLocation.TrackType.Barker)
            {
                //Barker
                BackerObject Track = BackerList[(int)tLocate.TrackID];
                int curIdx = -1;
                for (int i = 0; i < Track.WavPartList.Count; i++)
                {
                    WavePartsObject pobj = Track.WavPartList[i];
                    long st = pobj.getAbsoluteStartTick(TrackerWindow.TrackerProps.Tempo);
                    long et = pobj.getAbsoluteEndTick(TrackerWindow.TrackerProps.Tempo);
                    if (e.Tick < st)
                    {
                        break;
                    }
                    else if (e.Tick <= et)
                    {
                        curIdx = i;
                        break;
                    }
                }
                if (curIdx != -1)
                {
                    pLocate = new PartLocation(tLocate, (uint)curIdx, e);
                }
            }
            else
            {
                //Tracker
                TrackerObject Track = TrackerList[(int)tLocate.TrackID];
                int curIdx = -1;
                for (int i = 0; i < Track.PartList.Count; i++)
                {
                    PartsObject pobj = Track.PartList[i];
                    long st = pobj.getAbsoluteStartTick(TrackerWindow.TrackerProps.Tempo);
                    long et = pobj.getAbsoluteEndTick(TrackerWindow.TrackerProps.Tempo);
                    if (e.Tick < st)
                    {
                        break;
                    }
                    else if (e.Tick <= et)
                    {
                        curIdx = i;
                        break;
                    }
                }
                if (curIdx != -1)
                {
                    pLocate = new PartLocation(tLocate, (uint)curIdx, e);
                }
            }
            return pLocate;
        }

        PartLocation SelectPartLocation = null;
        PartLocation CurrentPartLocation = null;
        PartLocation ShowingPartLocation = null;
        TrackLocation HoldingLocation = null;
        long HoldingTick = -1;

        void TrackerWindow_PartsMouseDown(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button != MouseButtons.Left) return;
            if (_TrackerDragingStatus == PartsDragingType.None)
            {
                CurrentPartLocation = GetLocation(e);
                if (CurrentPartLocation != null)
                {
                    _TrackerDragingStatus = PartsDragingType.PartsMove;
                    SelectPartLocation = CurrentPartLocation;
                }
            }
        }
        void TrackerWindow_PartsMouseUp(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button != MouseButtons.Left) return;
            try
            {
                if (_TrackerDragingStatus == PartsDragingType.PartsMove)
                {
                    long EndTick = e.Tick;
                    if (CurrentPartLocation != null)
                    {
                        if (EndTick == CurrentPartLocation.e.Tick && e.AbsoluteTrackID == CurrentPartLocation.e.AbsoluteTrackID) return;
                        long Dert = EndTick - CurrentPartLocation.e.Tick;//小于0前移
                        TrackLocation TrackLocation = new TrackLocation((uint)e.AbsoluteTrackID, this.ProjectObject);
                        if (CurrentPartLocation.TrackLocation.Type == TrackLocation.TrackType.Tracker)
                        {
                            string SelectGUID = "";
                            if (ShowingPartLocation.TrackLocation.TrackID == CurrentPartLocation.TrackLocation.TrackID)
                            {
                                 SelectGUID=TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[(int)ShowingPartLocation.PartID].getGuid();
                            }
                            //Traker X
                            TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[(int)CurrentPartLocation.PartID].StartTime += TrackerWindow.TrackerProps.Tick2Time(Dert);
                            if (TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[(int)CurrentPartLocation.PartID].StartTime < 0) TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[(int)CurrentPartLocation.PartID].StartTime = 0;
                            //Traker Y
                            if (!TrackLocation.Equals(CurrentPartLocation.TrackLocation) && TrackLocation.Type == CurrentPartLocation.TrackLocation.Type)
                            {
                                TrackerList[(int)TrackLocation.TrackID].PartList.Add(TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[(int)CurrentPartLocation.PartID]);
                                TrackerList[(int)TrackLocation.TrackID].PartList.Sort();
                                TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList.RemoveAt((int)CurrentPartLocation.PartID);
                                TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList.Sort();
                                if (SelectGUID != "")
                                {
                                    for (int i = 0; i < TrackerList[(int)TrackLocation.TrackID].PartList.Count; i++)
                                    {
                                        if (TrackerList[(int)TrackLocation.TrackID].PartList[i].getGuid()==SelectGUID)
                                        {
                                            ShowingPartLocation.PartID = (uint)i;
                                            ShowingPartLocation.TrackLocation.TrackID = TrackLocation.TrackID;
                                            SelectGUID = "";
                                            break;
                                        }
                                    }
                                    if (SelectGUID != "")
                                    {
                                        for (int i = 0; i < TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList.Count; i++)
                                        {
                                            if (TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[i].getGuid() == SelectGUID)
                                            {
                                                ShowingPartLocation.PartID = (uint)i;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList.Sort();
                                if (SelectGUID != "")
                                {
                                    for (int i = 0; i < TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList.Count; i++)
                                    {
                                        if (TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[i].getGuid()==SelectGUID)
                                        {
                                            ShowingPartLocation.PartID = (uint)i;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (CurrentPartLocation.TrackLocation.Type == TrackLocation.TrackType.Barker)
                        {
                            //Barker X
                            BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList[(int)CurrentPartLocation.PartID].StartTime += TrackerWindow.TrackerProps.Tick2Time(Dert);
                            if (BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList[(int)CurrentPartLocation.PartID].StartTime < 0) BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList[(int)CurrentPartLocation.PartID].StartTime = 0;
                            //Barker Y
                            if (!TrackLocation.Equals(CurrentPartLocation.TrackLocation) && TrackLocation.Type == CurrentPartLocation.TrackLocation.Type)
                            {
                                BackerList[(int)TrackLocation.TrackID].WavPartList.Add(BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList[(int)CurrentPartLocation.PartID]);
                                BackerList[(int)TrackLocation.TrackID].WavPartList.Sort();
                                BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList.RemoveAt((int)CurrentPartLocation.PartID);
                                BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList.Sort();
                            }
                            else
                            {
                                BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList.Sort();
                            }
                        }
                        //END
                        ResetMouse();
                    }
                }
            }
            catch { ;}
        }
        void ResetMouse()
        {
            HoldingLocation = null;
            HoldingTick = -1;
            CurrentPartLocation = null;
            _TrackerDragingStatus = PartsDragingType.None;
        }
        void TrackerWindow_PartsMouseMove(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button != MouseButtons.Left)
            {
                ResetMouse();
                return;
            }
            if (_TrackerDragingStatus == PartsDragingType.PartsMove)
            {
                TrackLocation curLocate = new TrackLocation((uint)e.AbsoluteTrackID, this.ProjectObject);
                if (CurrentPartLocation != null)
                {
                    if (curLocate.Type == CurrentPartLocation.TrackLocation.Type)
                    {
                        HoldingLocation = curLocate;
                        HoldingTick = e.Tick;
                    }
                    else
                    {
                        HoldingLocation = null;
                        HoldingTick = -1;
                    }
                }
                else
                {
                    HoldingLocation = null;
                    HoldingTick = -1;
                }
            }
            else
            {
                HoldingLocation = null;
                HoldingTick = -1;
            }
        }

        void SingleTrackPaint(VocalUtau.DirectUI.DrawUtils.TrackerPartsDrawUtils.TrackPainterArgs Args, VocalUtau.DirectUI.DrawUtils.TrackerPartsDrawUtils Utils)
        {
            //加亮选择的窗口
            if(SelectPartLocation!=null)
            {
                if (Args.TrackIndex == SelectPartLocation.TrackLocation.TrackID)
                {
                    if (SelectPartLocation.TrackLocation.Type == TrackLocation.TrackType.Tracker && Args.TrackObject.GetType() == typeof(TrackerObject))
                    {
                        Utils.FillParts(Args.TrackArea, this.TrackerList[Args.TrackIndex].PartList, Color.White ,(int)SelectPartLocation.PartID);
                    }
                    else if (SelectPartLocation.TrackLocation.Type == TrackLocation.TrackType.Barker && Args.TrackObject.GetType() == typeof(BackerObject))
                    {
                        Utils.FillParts(Args.TrackArea, this.BackerList[Args.TrackIndex].WavPartList, Color.White, (int)SelectPartLocation.PartID);
                    }
                }
            }
            //加亮当前编辑的窗口
            if (ShowingPartLocation != null)
            {
                if (Args.TrackIndex == ShowingPartLocation.TrackLocation.TrackID)
                {
                    if (Args.TrackObject.GetType() == typeof(TrackerObject))
                    {
                        Utils.FillParts(Args.TrackArea, this.TrackerList[Args.TrackIndex].PartList, Color.Red, (int)ShowingPartLocation.PartID);
                    }
                }
            }
            //绘制条
            if (Args.TrackObject.GetType() == typeof(TrackerObject))
            {
                Utils.FillParts(Args.TrackArea, this.TrackerList[Args.TrackIndex].PartList, Color.BlueViolet);
            }
            else if (Args.TrackObject.GetType() == typeof(BackerObject))
            {
                Utils.FillParts(Args.TrackArea, this.BackerList[Args.TrackIndex].WavPartList, Color.LawnGreen);
            }
            //绘制holding
            if (HoldingLocation != null && HoldingTick >= 0)
            {
                if (HoldingLocation.AbsoluteTrackID == Args.AbsoluteIndex)
                {
                    long Dert = HoldingTick - CurrentPartLocation.e.Tick;//小于0前移
                    double startT = 0;
                    double durT = 0;
                    if (CurrentPartLocation.TrackLocation.Type == TrackLocation.TrackType.Tracker)
                    {
                        startT=TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[(int)CurrentPartLocation.PartID].StartTime + TrackerWindow.TrackerProps.Tick2Time(Dert);
                        durT = TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[(int)CurrentPartLocation.PartID].DuringTime;
                    }
                    else if (CurrentPartLocation.TrackLocation.Type == TrackLocation.TrackType.Barker)
                    {
                        startT = BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList[(int)CurrentPartLocation.PartID].StartTime + TrackerWindow.TrackerProps.Tick2Time(Dert);
                        durT = BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList[(int)CurrentPartLocation.PartID].DuringTime;
                    }
                    if (startT < 0) startT = 0;
                    Utils.DrawParts(Args.TrackArea, startT, durT, Color.Red);
                }
            }
        }
        void TrackerWindow_TPartsPaint(object sender, DrawUtils.TrackerPartsDrawUtils utils)
        {
            utils.DrawTracks(this.TrackerList, this.BackerList, new VocalUtau.DirectUI.DrawUtils.TrackerPartsDrawUtils.OneTrackPaintHandler(SingleTrackPaint));
        }

        void SingleGridePaint(VocalUtau.DirectUI.DrawUtils.TrackerGridesDrawUtils.GridePainterArgs Args, VocalUtau.DirectUI.DrawUtils.TrackerGridesDrawUtils Utils)
        {
            Utils.DrawString(Args.TrackArea.Location, Color.Red, "TEST");
        }
        void TrackerWindow_TGridePaint(object sender, DrawUtils.TrackerGridesDrawUtils utils)
        {
            utils.DrawTracks(this.TrackerList,this.BackerList,new DrawUtils.TrackerGridesDrawUtils.OneGridePaintHandler(SingleGridePaint));
        }
    }
}
