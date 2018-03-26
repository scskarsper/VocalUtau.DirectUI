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
        public UndoAbleUtils()
        {
        }
        private T Object = default(T);
        List<T> UndoList = new List<T>();
        List<string> UndoRepoList = new List<string>();
        List<T> RepeatList = new List<T>();
        List<string> RepeatRepoList = new List<string>();

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
            return Force.DeepCloner.DeepClonerExtensions.DeepClone<object>(Obj);
        }
        public void RegisterPoint(T Object)
        {
            this.Object = (T)Clone(Object);
        }
        public bool AddUndoPoint(string RepoIntroduce)
        {
            try
            {
                UndoList.Add(Object);
                UndoRepoList.Add(RepoIntroduce);
                return true;
            }
            catch { return false; }
        }
        public void RemoveUndoPoint()
        {
            try
            {
                UndoList.RemoveAt(UndoList.Count - 1);
                UndoRepoList.RemoveAt(UndoRepoList.Count - 1);
            }catch{;}
        }
        public bool AddRepeatPoint(string RepoIntroduce)
        {
            try
            {
                RepeatList.Add(Object);
                RepeatRepoList.Add(RepoIntroduce);
                return true;
            }
            catch { return false; }
        }
        public void RemoveRepeatPoint()
        {
            try
            {
                RepeatList.RemoveAt(RepeatList.Count - 1);
                RepeatRepoList.RemoveAt(RepeatRepoList.Count - 1);
            }
            catch { ;}
        }
        public void ClearRepeat()
        {
            try
            {
                RepeatList.Clear();
                RepeatRepoList.Clear();
            }
            catch { ;}
        }
        public void ClearUndo()
        {
            try
            {
                UndoList.Clear();
                UndoRepoList.Clear();
            }
            catch { ;}
        }
        public string LastRepeatRepo
        {
            get
            {
                if (RepeatRepoList.Count == 0) return "";
                return RepeatRepoList[RepeatRepoList.Count - 1];
            }
        }
        public string LastUndoRepo
        {
            get
            {
                if (UndoRepoList.Count == 0) return "";
                return UndoRepoList[UndoRepoList.Count - 1];
            }
        }
        public KeyValuePair<T, string> PeekUndo(bool KeepData=false)
        {
            if (UndoList.Count <= 0) return new KeyValuePair<T, string>(default(T), "");
            try
            {
                T Obj = UndoList[UndoList.Count - 1];
                String Repo = UndoRepoList[UndoRepoList.Count - 1];
                if (!KeepData)
                {
                    UndoList.RemoveAt(UndoList.Count - 1);
                    UndoRepoList.RemoveAt(UndoRepoList.Count - 1);
                }
                return new KeyValuePair<T, string>(Obj, Repo);
            }
            catch { return new KeyValuePair<T, string>(default(T), ""); }
        }
        public KeyValuePair<T, string> PeekRepeat(bool KeepData = false)
        {
            if (RepeatList.Count <= 0) return new KeyValuePair<T, string>(default(T), "");
            try
            {
                T Obj = RepeatList[RepeatList.Count - 1];
                String Repo = RepeatRepoList[RepeatRepoList.Count - 1];
                if (!KeepData)
                {
                    RepeatList.RemoveAt(RepeatList.Count - 1);
                    RepeatRepoList.RemoveAt(RepeatRepoList.Count - 1);
                }
                return new KeyValuePair<T,string>(Obj,Repo);
            }
            catch { return new KeyValuePair<T, string>(default(T), ""); }
        }
    }
}
