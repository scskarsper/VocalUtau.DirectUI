using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VocalUtau.DirectUI.Utils.AbilityUtils;
using VocalUtau.DirectUI.Utils.ParamUtils;
using VocalUtau.DirectUI.Utils.PianoUtils;
using VocalUtau.Formats.Model.VocalObject;

namespace VocalUtau.DirectUI.Forms
{
    public partial class SingerWindow : Form
    {
        public enum ParamViewType
        {
            Dynamic,
            Pitch
        }
        public enum RollActionType
        {
            Note,
            Pitch
        }
        public class ViewController
        {
            PianoRollWindow PianoWindow;
            ParamCurveWindow ParamWindow;
            public ViewController(ref PianoRollWindow PianoWin,ref ParamCurveWindow ParamWin)
            {
                this.PianoWindow = PianoWin;
                this.ParamWindow = ParamWin;
            }
            public void AllocView(IntPtr ObjectPtr)
            {
                Track_NoteView = new NoteView(ObjectPtr, this.PianoWindow);
                Track_PitchView = new PitchView(ObjectPtr, this.PianoWindow);
                Param_PitchView = new PITParamView(ObjectPtr, this.ParamWindow);
                Param_DynamicView = new DYNParamView(ObjectPtr, this.ParamWindow);
                Global_ActionView = new ActionView(ObjectPtr, this.PianoWindow, this.ParamWindow);

                CopyPasteController = new CopyPaste(ref Track_NoteView, ref Track_PitchView);

                Track_NoteView.NoteActionBegin += Track_NoteView_NoteActionBegin;
                Track_NoteView.NoteActionEnd += Track_NoteView_NoteActionEnd;
                Track_PitchView.PitchActionBegin += Track_PitchView_PitchActionBegin;
                Track_PitchView.PitchActionEnd += Track_PitchView_PitchActionEnd;
                Param_DynamicView.DynActionBegin += Param_DynamicView_DynActionBegin;
                Param_DynamicView.DynActionEnd += Param_DynamicView_DynActionEnd;
                Param_PitchView.PitchActionBegin += Param_PitchView_PitchActionBegin;
                Param_PitchView.PitchActionEnd += Param_PitchView_PitchActionEnd;
            }

            #region
            void Param_PitchView_PitchActionEnd(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (PitchActionEnd != null) PitchActionEnd(eventType);
            }
            void Param_PitchView_PitchActionBegin(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (PitchActionBegin != null) PitchActionBegin(eventType);
            }
            void Param_DynamicView_DynActionEnd(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (DynActionEnd != null) DynActionEnd(eventType);
            }
            void Param_DynamicView_DynActionBegin(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (DynActionBegin != null) DynActionBegin(eventType);
            }
            void Track_PitchView_PitchActionEnd(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (PitchActionEnd != null) PitchActionEnd(eventType);
            }
            void Track_PitchView_PitchActionBegin(PitchView.PitchDragingType eventType)
            {
                if (eventType == PitchView.PitchDragingType.None) return;
                if (PitchActionBegin != null) PitchActionBegin(eventType);
            }
            void Track_NoteView_NoteActionEnd(NoteView.NoteDragingType eventType, bool Callback = false)
            {
                if (eventType == NoteView.NoteDragingType.None) return;
                if (eventType == NoteView.NoteDragingType.NoteMove)
                {
                    if (!Callback)
                    {
                        if (NoteActionEnd != null) NoteActionEnd(eventType);
                    }
                }
                else
                {
                    if (NoteActionEnd != null) NoteActionEnd(eventType);
                }
            }
            void Track_NoteView_NoteActionBegin(NoteView.NoteDragingType eventType, bool Callback = false)
            {
                if (eventType == NoteView.NoteDragingType.None) return;
                if (NoteActionBegin != null) NoteActionBegin(eventType);
            }
            public delegate void OnNoteEventHandler(VocalUtau.DirectUI.Utils.PianoUtils.NoteView.NoteDragingType eventType);
            public event OnNoteEventHandler NoteActionEnd;
            public event OnNoteEventHandler NoteActionBegin;
            public event VocalUtau.DirectUI.Utils.ParamUtils.DYNParamView.OnPitchEventHandler DynActionEnd;
            public event VocalUtau.DirectUI.Utils.ParamUtils.DYNParamView.OnPitchEventHandler DynActionBegin;
            public event VocalUtau.DirectUI.Utils.PianoUtils.PitchView.OnPitchEventHandler PitchActionEnd;
            public event VocalUtau.DirectUI.Utils.PianoUtils.PitchView.OnPitchEventHandler PitchActionBegin;
            #endregion

            public PitchView Track_PitchView;
            public NoteView Track_NoteView;
            public ActionView Global_ActionView;
            public DYNParamView Param_DynamicView;
            public PITParamView Param_PitchView;
            public void SwitchParamView(ParamViewType type)
            {
                if (type == ParamViewType.Pitch)
                {
                    if (!Param_PitchView.HandleEvents)
                    {
                        Param_PitchView.HandleEvents = true;
                        Param_DynamicView.HandleEvents = false;
                        ParamWindow.Refresh();
                    }
                }
                else
                {
                    if (!Param_DynamicView.HandleEvents)
                    {
                        Param_PitchView.HandleEvents =  false;
                        Param_DynamicView.HandleEvents = true;
                        ParamWindow.Refresh();
                    }
                }
            }
            public void SwitchRollAction(RollActionType type)
            {
                if (type == RollActionType.Pitch)
                {
                    if (!Track_PitchView.HandleEvents)
                    {
                        Track_NoteView.HandleEvents = false; 
                        Track_PitchView.HandleEvents = true;
                        PianoWindow.Refresh();
                    }
                }
                else
                {
                    if (!Track_NoteView.HandleEvents)
                    {
                        Track_NoteView.HandleEvents = true;
                        Track_PitchView.HandleEvents = false;
                        PianoWindow.Refresh();
                    }
                }
            }
            public ParamViewType GetParamType()
            {
                if (Param_PitchView.HandleEvents)return ParamViewType.Pitch;
                return ParamViewType.Dynamic;
            }
            public RollActionType GetRollActionType()
            {
                if (Track_PitchView.HandleEvents) return RollActionType.Pitch;
                return RollActionType.Note;
            }
            public uint ParamZoom
            {
                get
                {
                    if (GetParamType() == ParamViewType.Pitch)
                    {
                        return Param_PitchView.Zoom;
                    }
                    else
                    {
                        return Param_DynamicView.Zoom;
                    }
                }
                set
                {
                    if (GetParamType() == ParamViewType.Pitch)
                    {
                        if(value>0)Param_PitchView.Zoom=value;
                    }
                    else
                    {
                        if (value > 0) Param_DynamicView.Zoom = value;
                    }
                }
            }
            public void SetupPartsObjectPtr(IntPtr Ptr)
            {
                Track_PitchView.setPartsObjectPtr(Ptr);
                Track_NoteView.setPartsObjectPtr(Ptr);
                Global_ActionView.setPartsObjectPtr(Ptr);
                Param_PitchView.setPartsObjectPtr(Ptr);
                Param_DynamicView.setPartsObjectPtr(Ptr);
                ParamWindow.RedrawPiano();
                PianoWindow.RedrawPiano();
            }

            public void SetNoteViewTool(NoteView.NoteToolsType Tool)
            {
                SwitchRollAction(RollActionType.Note);
                Track_NoteView.NoteToolsStatus = Tool;
            }
            public void SetPitchViewTool(PitchView.PitchDragingType Tool)
            {
                SwitchRollAction(RollActionType.Pitch);
                Track_PitchView.PitchToolsStatus = Tool;
            }
            public void SetParamGraphicTool(PitchView.PitchDragingType Tool)
            {
                Param_PitchView.PitchToolsStatus = Tool;
                Param_DynamicView.DynToolsStatus = Tool;
            }

            public CopyPaste CopyPasteController;
            public class CopyPaste
            {
                NoteView NV;
                PitchView PV;
                public CopyPaste(ref NoteView nNV, ref PitchView pPV)
                {
                    this.NV = nNV;
                    this.PV = pPV;
                }

                List<NoteObject> PNV = new List<NoteObject>();
                List<PitchObject> PPV = new List<PitchObject>();
                public bool CopySelectsNote()
                {
                    PNV = NV.getSelectNotes(true);
                    if (PNV.Count <= 0) return false;
                    PPV = NV.getSelectPitchs(true);
                    return true;
                }
                public bool isCopyed
                {
                    get
                    {
                        return PNV.Count > 0;
                    }
                }
                public bool PasteNotes(long StartTick)
                {
                    bool R = NV.AddNotes(StartTick, PNV);
                    PNV.Clear();
                    if (!R) return false;
                    else
                    {
                        PV.AddPitchs(StartTick, PPV);
                    }
                    return true;
                }
            }

        }

        ViewController Controller;

        public SingerWindow()
        {
            InitializeComponent();
            Controller = new ViewController(ref this.pianoRollWindow1, ref this.paramCurveWindow1);
        }

        private void ctl_Scroll_LeftPos_Scroll(object sender, ScrollEventArgs e)
        {
            pianoRollWindow1.setPianoStartTick(ctl_Scroll_LeftPos.Value);
            paramCurveWindow1.setPianoStartTick(ctl_Scroll_LeftPos.Value);
        }

        private void ctl_Track_PianoWidth_Scroll(object sender, EventArgs e)
        {
            pianoRollWindow1.setCrotchetSize((uint)ctl_Track_PianoWidth.Value);
            paramCurveWindow1.setCrotchetSize((uint)ctl_Track_PianoWidth.Value);
        }

        private void ctl_Track_NoteHeight_Scroll(object sender, EventArgs e)
        {
            pianoRollWindow1.setNoteHeight((uint)ctl_Track_NoteHeight.Value);
        }
        
        private void ctl_Param_LZoom_Scroll(object sender, EventArgs e)
        {
            ctl_Param_RZoom.Value = ctl_Param_LZoom.Value;
            Controller.ParamZoom= (uint)ctl_Param_LZoom.Value;
        }

        private void ctl_Param_RZoom_Scroll(object sender, EventArgs e)
        {
            ctl_Param_LZoom.Value = ctl_Param_RZoom.Value;
            ctl_Param_LZoom_Scroll(null, null);
        }

        private void PlayNote(int NoteNumber)
        {
            System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart((note) =>
            {
                try
                {
                    using (MidiOut midiOut = new MidiOut(0))
                    {
                        midiOut.Volume = 65535;
                        midiOut.Send(MidiMessage.StartNote(NoteNumber, 127, 1).RawData);
                        System.Threading.Thread.Sleep(500);
                        midiOut.Send(MidiMessage.StopNote(NoteNumber, 127, 1).RawData);
                    }
                }
                catch { ;}
            }));
            th.Start(NoteNumber);
        }

        private void pianoRollWindow1_RollMouseDown(object sender, PianoMouseEventArgs e)
        {
            PlayNote((int)e.PitchValue.NoteNumber);
        }

    }
}
