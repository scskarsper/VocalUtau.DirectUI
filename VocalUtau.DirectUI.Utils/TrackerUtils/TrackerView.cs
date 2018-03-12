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
        public delegate void OnShowingEditorChangeHandler(PartsObject PartObject);
        public delegate void OnPartsEventHandler(PartsDragingType eventType);
        public event OnPartsEventHandler TrackerActionEnd;
        public event OnPartsEventHandler TrackerActionBegin;
        public event OnShowingEditorChangeHandler ShowingEditorChanged;
        public enum PartsDragingType
        {
            None,
            PartsMove,
            VolumeMove,
            TrackRename,
            TrackSort
        }
        PartsDragingType _TrackerDragingStatus = PartsDragingType.None;

        IntPtr ProjectObjectPtr = IntPtr.Zero;
        TrackerRollWindow TrackerWindow;
        double MovingVolume = 0;
        double MovingAbsID = 0;

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
            ResetShowingParts();
            ResetScrollBar();
        }

        public void setProjectObjectPtr(IntPtr ProjectObjectPtr)
        {
            this.ProjectObjectPtr = ProjectObjectPtr;
            ResetShowingParts();
            ResetScrollBar();
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

        public void ResetShowingParts()
        {
            for (int i = 0; i < ProjectObject.TrackerList.Count; i++)
            {
                if (ProjectObject.TrackerList[i].PartList.Count > 0)
                {
                    _ShowingGUID = ProjectObject.TrackerList[i].PartList[0].getGuid();
                    if (ShowingEditorChanged != null) ShowingEditorChanged(ProjectObject.TrackerList[i].PartList[0]);
                    break;
                }
            }
        }
        public void ResetScrollBar()
        {
            uint MaxValue = (uint)(ProjectObject.BackerList.Count + ProjectObject.TrackerList.Count);
            uint MaxShown = TrackerWindow.ShownTrackerCount;
            if (MaxValue > MaxShown)
            {
                this.TrackerWindow.setScrollBarMax(MaxValue - MaxShown);
            }
            else
            {
                this.TrackerWindow.setScrollBarMax(0);
            }
            //this.TrackerWindow.Height-this.TrackerWindow.TrackerProps.max
            //);
        }
        public PartsObject getShowingPart()
        {
            if (_ShowingGUID == "") return null;
            for (int i = 0; i < ProjectObject.TrackerList.Count; i++)
            {
                for (int j = 0; j < ProjectObject.TrackerList[i].PartList.Count; j++)
                {
                    if (_ShowingGUID == ProjectObject.TrackerList[i].PartList[j].getGuid())
                    {
                        return ProjectObject.TrackerList[i].PartList[j];
                    }
                }
            }
            return null;
        }
        public bool updatePart(ref PartsObject part)
        {
            for (int i = 0; i < ProjectObject.TrackerList.Count; i++)
            {
                for (int j = 0; j < ProjectObject.TrackerList[i].PartList.Count; j++)
                {
                    if (part.getGuid() == ProjectObject.TrackerList[i].PartList[j].getGuid())
                    {
                        ProjectObject.TrackerList[i].PartList[j]=part;
                        return true;
                    }
                }
            }
            return false;
        }

        public void hookTrackerWindow()
        {
            TrackerWindow.TrackerProps.Tempo = ProjectObject.BaseTempo;
            TrackerWindow.TPartsPaint += TrackerWindow_TPartsPaint;
            TrackerWindow.TGridePaint += TrackerWindow_TGridePaint;
            TrackerWindow.PartsMouseDown += TrackerWindow_PartsMouseDown;
            TrackerWindow.PartsMouseUp += TrackerWindow_PartsMouseUp;
            TrackerWindow.PartsMouseMove += TrackerWindow_PartsMouseMove;
            TrackerWindow.PartsMouseClick += TrackerWindow_PartsMouseClick;
            TrackerWindow.PartsMouseDoubleClick += TrackerWindow_PartsMouseDoubleClick;

            TrackerWindow.GridsMouseClick += TrackerWindow_GridsMouseClick;
            TrackerWindow.GridsMouseDoubleClick += TrackerWindow_GridsMouseDoubleClick;
            TrackerWindow.GridsMouseDown += TrackerWindow_GridsMouseDown;
            TrackerWindow.GridsMouseUp += TrackerWindow_GridsMouseUp;
            TrackerWindow.GridsMouseMove += TrackerWindow_GridsMouseMove;

            TrackerWindow.MouseLeave += TrackerWindow_MouseLeave;
            TrackerWindow.TrackerRollAfterResize += TrackerWindow_TrackerRollAfterResize;
        }

        void TrackerWindow_TrackerRollAfterResize(object sender, EventArgs e)
        {
            ResetScrollBar();
        }

        void TrackerWindow_MouseLeave(object sender, EventArgs e)
        {
            ResetMouse();
        }

        void TrackerWindow_GridsMouseMove(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button != MouseButtons.Left)
            {
                ResetMouse();
                return;
            }
            if (_TrackerDragingStatus == PartsDragingType.VolumeMove)
            {
                if (e.AbsoluteTrackID == MovingAbsID)
                {
                    int NX = e.MouseEventArgs.X;
                    int Seg = NX - VolumeCtlRect[(int)e.AbsoluteTrackID].Left;
                    if (Seg < 0) Seg = 0;
                    double bfb = (double)Seg / (double)VolumeCtlRect[(int)e.AbsoluteTrackID].Width;
                    MovingVolume = bfb;
                    if (MovingVolume > 1) MovingVolume = 1;
                }
            }
        }

        void TrackerWindow_GridsMouseUp(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button != MouseButtons.Left) return;

            if (_TrackerDragingStatus == PartsDragingType.VolumeMove)
            {
                if (e.AbsoluteTrackID == MovingAbsID)
                {
                    int NX = e.MouseEventArgs.X;
                    int Seg = NX - VolumeCtlRect[(int)e.AbsoluteTrackID].Left;
                    if (Seg < 0) Seg = 0;
                    double bfb = (double)Seg / (double)VolumeCtlRect[(int)e.AbsoluteTrackID].Width;
                    MovingVolume = bfb;
                    if (MovingVolume > 1) MovingVolume = 1;
                    TrackLocation tLocate = new TrackLocation((uint)e.AbsoluteTrackID,ProjectObject);
                    ITrackerInterface tObject = null;
                    switch (tLocate.Type)
                    {
                        case TrackLocation.TrackType.Barker: tObject=ProjectObject.BackerList[(int)tLocate.TrackID]; break;
                        case TrackLocation.TrackType.Tracker: tObject = ProjectObject.TrackerList[(int)tLocate.TrackID]; break;
                    }
                    if (tObject != null)
                    {
                        tObject.setVolume(MovingVolume);
                        if (TrackerActionEnd != null)
                        {
                            TrackerActionEnd(_TrackerDragingStatus);
                        }
                    }
                }
            }
            ResetMouse();
        }

        void TrackerWindow_GridsMouseDown(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.MouseEventArgs.Button != MouseButtons.Left) return;
            if (e.Tag.GetType() == typeof(Models.GridesMouseEventArgs))
            {
                Models.GridesMouseEventArgs ge = (Models.GridesMouseEventArgs)e.Tag;
                if (ge.Area == Models.GridesMouseEventArgs.GridesAreaType.VolumeArea)
                {
                    if (inVolumeCtl(e.AbsoluteTrackID, e.MouseEventArgs.Location))
                    {
                        TrackLocation tLocate = new TrackLocation((uint)e.AbsoluteTrackID, ProjectObject);
                        MovingAbsID = e.AbsoluteTrackID;
                        if (tLocate.Type == TrackLocation.TrackType.Tracker)
                        {
                            MovingVolume = ProjectObject.TrackerList[(int)tLocate.TrackID].Volume;
                        }
                        else if (tLocate.Type == TrackLocation.TrackType.Barker)
                        {
                            MovingVolume = ProjectObject.BackerList[(int)tLocate.TrackID].Volume;
                        }
                        _TrackerDragingStatus = PartsDragingType.VolumeMove;
                        if (TrackerActionBegin != null)
                        {
                            TrackerActionBegin(_TrackerDragingStatus);
                        }
                    }
                }
            }
        }

        void TrackerWindow_GridsMouseDoubleClick(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.Tag.GetType() == typeof(Models.GridesMouseEventArgs))
            {
                Models.GridesMouseEventArgs ge = (Models.GridesMouseEventArgs)e.Tag;
                if (ge.Area == Models.GridesMouseEventArgs.GridesAreaType.NameArea)
                {
                    TrackLocation tLocate = new TrackLocation((uint)e.AbsoluteTrackID, ProjectObject);
                    switch (tLocate.Type)
                    {
                        case TrackLocation.TrackType.Barker:
                            if (TrackerActionBegin != null)
                            {
                                TrackerActionBegin(PartsDragingType.TrackRename);
                            }
                            string WOldName=ProjectObject.BackerList[(int)tLocate.TrackID].Name;
                            string WNewName = Microsoft.VisualBasic.Interaction.InputBox("Input New Wave Track Name", "Wave Track Name", WOldName);
                            if (WNewName != "")
                            {
                                ProjectObject.BackerList[(int)tLocate.TrackID].Name = WNewName;
                                this.TrackerWindow.RedrawPiano();
                            }
                            if (TrackerActionEnd != null)
                            {
                                TrackerActionEnd(PartsDragingType.TrackRename);
                            }
                            break;
                        case TrackLocation.TrackType.Tracker:
                            if (TrackerActionBegin != null)
                            {
                                TrackerActionBegin(PartsDragingType.TrackRename);
                            }
                            string VOldName = ProjectObject.TrackerList[(int)tLocate.TrackID].Name;
                            string VNewName = Microsoft.VisualBasic.Interaction.InputBox("Input New Vocal Track Name", "Vocal Track Name", VOldName);
                            if (VNewName != "")
                            {
                                ProjectObject.TrackerList[(int)tLocate.TrackID].Name = VNewName;
                                this.TrackerWindow.RedrawPiano();
                            }
                            if (TrackerActionEnd != null)
                            {
                                TrackerActionEnd(PartsDragingType.TrackRename);
                            }
                            break;
                    }
                    
                }
            }
        }
        void TrackerWindow_GridsMouseClick(object sender, Models.TrackerMouseEventArgs e)
        {
            if (e.Tag.GetType() == typeof(Models.GridesMouseEventArgs))
            {
                Models.GridesMouseEventArgs ge = (Models.GridesMouseEventArgs)e.Tag;
                switch (ge.Area)
                {
                    case Models.GridesMouseEventArgs.GridesAreaType.VerticalBtnsAdd:
                        {
                            TrackLocation tLocate = new TrackLocation((uint)e.AbsoluteTrackID, ProjectObject);
                            if (tLocate.TrackID >= 0)
                            {
                                switch (tLocate.Type)
                                {
                                    case TrackLocation.TrackType.Barker:
                                        {
                                            if (tLocate.TrackID+1 < ProjectObject.BackerList.Count)
                                            {
                                                if (TrackerActionBegin != null)
                                                {
                                                    TrackerActionBegin(PartsDragingType.TrackSort);
                                                }
                                                ProjectObject.BackerList[(int)(tLocate.TrackID + 1)].Index--;
                                                ProjectObject.BackerList[(int)tLocate.TrackID].Index++;
                                                ProjectObject.BackerList.Sort();
                                                if (TrackerActionEnd != null)
                                                {
                                                    TrackerActionEnd(PartsDragingType.TrackSort);
                                                }
                                            }
                                        };
                                        break;
                                    case TrackLocation.TrackType.Tracker:
                                        {
                                            if (tLocate.TrackID+1 < ProjectObject.TrackerList.Count)
                                            {
                                                if (TrackerActionBegin != null)
                                                {
                                                    TrackerActionBegin(PartsDragingType.TrackSort);
                                                }
                                                ProjectObject.TrackerList[(int)(tLocate.TrackID + 1)].Index--;
                                                ProjectObject.TrackerList[(int)tLocate.TrackID].Index++;
                                                ProjectObject.TrackerList.Sort();
                                                if (TrackerActionEnd != null)
                                                {
                                                    TrackerActionEnd(PartsDragingType.TrackSort);
                                                }
                                            }
                                        };
                                        break;
                                }
                            }
                        };
                        break;
                    case Models.GridesMouseEventArgs.GridesAreaType.VerticalBtnsDec:
                        {
                            TrackLocation tLocate = new TrackLocation((uint)e.AbsoluteTrackID, ProjectObject);
                            if (tLocate.TrackID > 0)
                            {
                                switch (tLocate.Type)
                                {
                                    case TrackLocation.TrackType.Barker:
                                        {
                                            if (tLocate.TrackID < ProjectObject.BackerList.Count)
                                            {
                                                if (TrackerActionBegin != null)
                                                {
                                                    TrackerActionBegin(PartsDragingType.TrackSort);
                                                }
                                                ProjectObject.BackerList[(int)(tLocate.TrackID - 1)].Index++;
                                                ProjectObject.BackerList[(int)tLocate.TrackID].Index--;
                                                ProjectObject.BackerList.Sort();
                                                if (TrackerActionEnd != null)
                                                {
                                                    TrackerActionEnd(PartsDragingType.TrackSort);
                                                }
                                            }
                                        };
                                        break;
                                    case TrackLocation.TrackType.Tracker:
                                        {
                                            if (tLocate.TrackID < ProjectObject.TrackerList.Count)
                                            {
                                                if (TrackerActionBegin != null)
                                                {
                                                    TrackerActionBegin(PartsDragingType.TrackSort);
                                                }
                                                ProjectObject.TrackerList[(int)(tLocate.TrackID - 1)].Index++;
                                                ProjectObject.TrackerList[(int)tLocate.TrackID].Index--;
                                                ProjectObject.TrackerList.Sort();
                                                if (TrackerActionEnd != null)
                                                {
                                                    TrackerActionEnd(PartsDragingType.TrackSort);
                                                }
                                            }
                                        };
                                        break;
                                }
                            }
                        };
                        break;
                }
                Console.WriteLine(ge.Area.ToString());
            }
        }

        void TrackerWindow_PartsMouseDoubleClick(object sender, Models.TrackerMouseEventArgs e)
        {
            PartLocation pLocate = GetLocation(e);
            if (pLocate.TrackLocation.Type == TrackLocation.TrackType.Tracker)
            {
                if (pLocate.TrackLocation.TrackID < ProjectObject.TrackerList.Count)
                {
                    TrackerObject t = ProjectObject.TrackerList[(int)pLocate.TrackLocation.TrackID];
                    if (pLocate.PartID < t.PartList.Count)
                    {
                        PartsObject po = t.PartList[(int)pLocate.PartID];
                        _ShowingGUID = po.getGuid();
                        if (ShowingEditorChanged != null) ShowingEditorChanged(po);
                    }
                }
            }
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
                if (tLocate.TrackID >= BackerList.Count) return pLocate;
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
                if (tLocate.TrackID >= TrackerList.Count) return pLocate;
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

        //PartLocation SelectPartLocation = null;
        PartLocation CurrentPartLocation = null;
        string _ShowingGUID = "";

        public string ShowingEditorGUID
        {
            get { return _ShowingGUID; }
        }
        string SelectGUID = "";
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
                    SelectGUID = "";
                    switch(CurrentPartLocation.TrackLocation.Type)
                    {
                        case TrackLocation.TrackType.Tracker: SelectGUID = ProjectObject.TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList[(int)CurrentPartLocation.PartID].getGuid(); break;
                        case TrackLocation.TrackType.Barker: SelectGUID = ProjectObject.BackerList[(int)CurrentPartLocation.TrackLocation.TrackID].WavPartList[(int)CurrentPartLocation.PartID].getGuid(); break;
                    }
                    if (TrackerActionBegin != null)
                    {
                        TrackerActionBegin(PartsDragingType.PartsMove);
                    }
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
                            }
                            else
                            {
                                TrackerList[(int)CurrentPartLocation.TrackLocation.TrackID].PartList.Sort();
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
                    if (TrackerActionEnd != null)
                    {
                        TrackerActionEnd(PartsDragingType.PartsMove);
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
            TrackerWindow.RedrawPiano();
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
            //绘制条
            if (Args.TrackObject.GetType() == typeof(TrackerObject))
            {
                int Finded = 0;
                //加亮当前编辑的窗口
                for (int i = 0; i < this.TrackerList[Args.TrackIndex].PartList.Count; i++)
                {
                    if (this.TrackerList[Args.TrackIndex].PartList[i].getGuid() == SelectGUID)
                    {
                        Utils.FillParts(Args.TrackArea, this.TrackerList[Args.TrackIndex].PartList, Color.White, i);
                        Utils.DrawParts(Args.TrackArea, this.TrackerList[Args.TrackIndex].PartList[i].StartTime, this.TrackerList[Args.TrackIndex].PartList[i].DuringTime, Color.White);
                        Finded++;
                    }
                    if (this.TrackerList[Args.TrackIndex].PartList[i].getGuid() == _ShowingGUID)
                    {
                        Utils.FillParts(Args.TrackArea, this.TrackerList[Args.TrackIndex].PartList, Color.Red, i);
                        Finded++;
                    }
                    if (Finded >= 2) break;
                }
                //NORMAL
                Utils.FillParts(Args.TrackArea, this.TrackerList[Args.TrackIndex].PartList, Color.BlueViolet);
            }
            else if (Args.TrackObject.GetType() == typeof(BackerObject))
            {
                for (int i = 0; i < this.BackerList[Args.TrackIndex].WavPartList.Count; i++)
                {
                    if (this.BackerList[Args.TrackIndex].WavPartList[i].getGuid() == SelectGUID)
                    {
                        Utils.FillParts(Args.TrackArea, this.BackerList[Args.TrackIndex].WavPartList, Color.White, i);
                        Utils.DrawParts(Args.TrackArea, this.BackerList[Args.TrackIndex].WavPartList[i].StartTime, this.BackerList[Args.TrackIndex].WavPartList[i].DuringTime, Color.White);
                        break;
                    }
                }
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

        bool inVolumeCtl(int AbsoluteTrackID,Point CurrentPoint)
        {
            Rectangle TapArea = VolumeCtlRect[AbsoluteTrackID];
            //判断体系
            System.Drawing.Drawing2D.GraphicsPath VolumeGraphicsPath = new System.Drawing.Drawing2D.GraphicsPath();
            Region VolumeRegion = new Region();
            VolumeGraphicsPath.Reset();

            //添家多边形点
            Point p1 = new Point(TapArea.Left, TapArea.Bottom);
            Point p2 = new Point(TapArea.Right, TapArea.Top);
            Point p3 = new Point(TapArea.Right, TapArea.Bottom);

            VolumeGraphicsPath.AddPolygon(new Point[3] { p1, p2, p3 });
            VolumeRegion.MakeEmpty();
            VolumeRegion.Union(VolumeGraphicsPath);

            return VolumeRegion.IsVisible(CurrentPoint);
        }
        Dictionary<int, Rectangle> VolumeCtlRect = new Dictionary<int, Rectangle>();
        void DrawVolume(VocalUtau.DirectUI.DrawUtils.TrackerGridesDrawUtils.GridePainterArgs Args, VocalUtau.DirectUI.DrawUtils.TrackerGridesDrawUtils Utils)
        {
            TrackLocation trackLocate = new TrackLocation((uint)Args.AbsoluteIndex, ProjectObject);
            double Volume = 0;
            ITrackerInterface TObject = null;
            switch (trackLocate.Type)
            {
                case TrackLocation.TrackType.Barker: TObject = ProjectObject.BackerList[(int)trackLocate.TrackID]; break;
                case TrackLocation.TrackType.Tracker: TObject = ProjectObject.TrackerList[(int)trackLocate.TrackID]; break;
            }
            if (TObject == null) return;
            Volume = TObject.getVolume();
            if (_TrackerDragingStatus == PartsDragingType.VolumeMove && MovingAbsID == Args.AbsoluteIndex)
            {
                Volume = MovingVolume;
            }
            if (Volume > 1) Volume = 1;
            if (Volume < 0) Volume = 0;

            int ATop = 0;
            Rectangle TapArea = new Rectangle(Args.TrackArea.Location, Args.TrackArea.Size);
            if (TapArea.Height > 18)
            {
                ATop = (TapArea.Height - 18) / 2;
                TapArea.Height = 18;
            }
            TapArea.X += 5;
            TapArea.Width -= 35;
            TapArea.Y += ATop + 2;
            TapArea.Height -= 6;
            Utils.DrawString(new Point(TapArea.X + TapArea.Width + 2, TapArea.Y + 2), Color.White, (Math.Round(Volume * 100).ToString() + "%").PadLeft(4, ' '));
            Utils.DrawTrianglePercent(TapArea, Volume, Color.White);

            if (VolumeCtlRect.ContainsKey(Args.AbsoluteIndex))
            {
                VolumeCtlRect[Args.AbsoluteIndex] = TapArea;
            }
            else
            {
                VolumeCtlRect.Add(Args.AbsoluteIndex, TapArea);
            }
        }
        void SingleGridePaint(VocalUtau.DirectUI.DrawUtils.TrackerGridesDrawUtils.GridePainterArgs Args, VocalUtau.DirectUI.DrawUtils.TrackerGridesDrawUtils Utils)
        {
                DrawVolume(Args, Utils);
        }
        void TrackerWindow_TGridePaint(object sender, DrawUtils.TrackerGridesDrawUtils utils)
        {
            utils.DrawTracks(this.TrackerList, this.BackerList, new DrawUtils.TrackerGridesDrawUtils.OneGridePaintHandler(SingleGridePaint));
            GC.Collect();
        }
    }
}
