using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using VocalUtau.Formats.Model.Utils;

namespace VocalUtau.DirectUI.Utils.AbilityUtils
{
    public class UndoAbleUtils<T>
    {
        IntPtr ObjectPtr = IntPtr.Zero;
        public UndoAbleUtils(IntPtr ObjectPtr)
        {
            this.ObjectPtr = ObjectPtr;
        }
        public void UpdatePtr(IntPtr ObjectPtr)
        {
            this.ObjectPtr = ObjectPtr;
        }
        private T Object
        {
            get
            {
                T ret = default(T);
                try
                {
                    GCHandle handle = GCHandle.FromIntPtr(ObjectPtr);
                    ret = (T)handle.Target;
                }
                catch { ;}
                return ret;
            }
        }
        List<T> UndoList = new List<T>();
        List<T> RepeatList = new List<T>();

        public int UndoCount
        {
            get
            {
                return UndoList.Count;
            }
        }
        public int RepeatCount
        {
            get
            {
                return RepeatList.Count;
            }
        }

        private object Clone(object Obj)
        {
            BinaryFormatter Formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            MemoryStream stream = new MemoryStream();
            Formatter.Serialize(stream, Obj);
            stream.Position = 0;
            object clonedObj = Formatter.Deserialize(stream);
            stream.Close();
            return clonedObj;
        }
        public bool AddUndoPoint(bool ClearRepeat=true)
        {
            try
            {
                object O = Clone(Object);
                if(ClearRepeat)RepeatList.Clear();
                UndoList.Add((T)O);
                return true;
            }
            catch { return false; }
        }
        public void RemoveUndoPoint()
        {
            try
            {
                UndoList.RemoveAt(UndoList.Count - 1);
            }catch{;}
        }
        public bool AddRepeatPoint(bool ClearUndo = false)
        {
            try
            {
                object O = Clone(Object);
                if (ClearUndo) UndoList.Clear();
                RepeatList.Add((T)O);
                return true;
            }
            catch { return false; }
        }
        public void RemoveRepeatPoint()
        {
            try
            {
                RepeatList.RemoveAt(RepeatList.Count - 1);
            }
            catch { ;}
        }
        public void ClearRepeat()
        {
            try
            {
                RepeatList.Clear();
            }
            catch { ;}
        }
        public void ClearUndo()
        {
            try
            {
                UndoList.Clear();
            }
            catch { ;}
        }
        public object PeekUndo()
        {
            if (UndoList.Count <= 0) return null;
            try
            {
                T Obj = UndoList[UndoList.Count - 1];
                UndoList.RemoveAt(UndoList.Count - 1);
                return Obj;
            }
            catch { return null; }
        }
        public object PeekRepeat()
        {
            if (RepeatList.Count <= 0) return null;
            try
            {
                T Obj = RepeatList[RepeatList.Count - 1];
                RepeatList.RemoveAt(RepeatList.Count - 1);
                return Obj;
            }
            catch { return null; }
        }
    }
}
