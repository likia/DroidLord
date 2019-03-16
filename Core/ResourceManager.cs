using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.ComponentModel;
using System.Collections.Specialized;

namespace DroidLord.Core
{
    public delegate void ResourceItemChangedHandler(object sender, string Key);

    public enum ResourceState
    {
        NA = 128,
        PENDING = 1,
        ALIVE = 2,
        DIED = 4,
        FAIL = 8
    }

    public enum ResourceAttribute
    {
        OK = 128,
        BLOCKED = 1,
        EXPIRED = 2,
        UNSTABLE = 4
    }

    [Serializable()]
    public abstract class BaseResource : ICloneable, INotifyPropertyChanged
    {
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; NotifyPropertyChanged("Key"); }
        }
        private ResourceState state = ResourceState.NA;

        public ResourceState State
        {
            get { return state; }
            set { state = value; NotifyPropertyChanged("State"); }
        }
        private ResourceAttribute attribute = ResourceAttribute.OK;

        public ResourceAttribute Attribute
        {
            get { return attribute; }
            set { attribute = value; NotifyPropertyChanged("Attribute"); }
        }
        private Hashtable value = new Hashtable();

        public Hashtable Value
        {
            get { return value; }
            set { this.value = value; NotifyPropertyChanged("Value"); }
        }
        private string groupName = null;

        public BaseResource()
        {

        }
        private void NotifyPropertyChanged(string pName)
        {
            OnPropertyChanged(pName);
        }


        public object Clone()
        {
            object Copy = MemberwiseClone();
            lock (Value.SyncRoot)
            {
                ((BaseResource)Copy).Value = (Hashtable)Value.Clone();
            }
            return Copy;
        }

        public object GetValue(string Key)
        {
            lock (Value.SyncRoot)
            {
                return Value[Key];
            }
        }

        public void SetValue(string Key, object Val)
        {
            lock (Value.SyncRoot)
            {
                Value[Key] = Val;
                NotifyPropertyChanged("Value");
            }
        }


        public void OnPropertyChanged(string PName)
        {
            if (hd == null) return;
            foreach (System.ComponentModel.PropertyChangedEventHandler hdl in hd)
            {
                hdl.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(PName));
            }
        }

        private List<System.ComponentModel.PropertyChangedEventHandler> hd = new List<System.ComponentModel.PropertyChangedEventHandler>();

        event System.ComponentModel.PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                hd.Add(value);
            }
            remove
            {
                hd.Remove(value);
            }
        }
    }


    [System.Runtime.Remoting.Contexts.Synchronization, Serializable]
    public class ResourceManager : Savable
    {
        public event ResourceItemChangedHandler ResourceItemChanged;


        private OrderedDictionary Resources;
        public int Count { get { return Resources.Count; } }
        private object ResLock = new object();

        private Picker<BaseResource> Picker;

        private bool bu = false;
        public bool BlockUpdate
        {
            get
            { return bu; }
            set
            {
                bu = value;
                if (!bu)
                    if (ResourceItemChanged != null)
                        ResourceItemChanged(null, null);
            }
        }
        int pickConstant = 1;
        public int PickConstant
        {
            get { return pickConstant; }
            set { pickConstant = value; }
        }
        object TTbLock = new object();
        Hashtable Timetable = new Hashtable();

        public ResourceManager()
        {
            Resources = new OrderedDictionary();
            Picker = new LoopPicker<BaseResource>();
            ResourceItemChanged += new ResourceItemChangedHandler(ChangedInternalHandler);
            ChangedInternalHandler(null, null);
        }

        private void ChangedInternalHandler(object sender, string Key)
        {

        }

        public void Clear()
        {
            lock (ResLock)
            {
                Resources.Clear();
            }
        }

        public object Save()
        {
            ArrayList al = new ArrayList();
            al.Add(Resources);
            return al;
        }

        public void SetPicker(Picker<BaseResource> Instance)
        {
            Picker = Instance;
            ChangedInternalHandler(null, null);
        }
        
        public bool Add(string Key, BaseResource _Item)
        {
            BaseResource Item = (BaseResource)_Item.Clone();
            Item.Key = Key;
            lock (ResLock)
            {
                if (Resources.Contains(Key) == true)
                {
                    Resources[Key] = Item;
                    return true;
                }
                Resources.Add(Key, Item);
            }

            if (ResourceItemChanged != null && !BlockUpdate) ResourceItemChanged(this, Key);
            return true;
        }
        public void Add(BaseResource item)
        {
            this.Add(item.Key, item);
        }
        public BaseResource Get(string Key)
        {
            BaseResource res = null;
            lock (ResLock)
            {
                if (Resources.Contains(Key) == false) return null;

                res = Resources[Key] as BaseResource;
            }
            return res;
        }

        public bool Set(string Key, BaseResource _Item)
        {
            BaseResource Item = (BaseResource)_Item; lock (ResLock)
            {
                if (Resources.Contains(Key) == false)
                {
                    return Add(Key, Item);
                }
                Resources[Key] = Item;
            }

            if (ResourceItemChanged != null && !BlockUpdate) ResourceItemChanged(this, Key);
            return true;
        }

        public void Remove(string Key)
        {
            lock (ResLock)
            {
                if (Resources.Contains(Key))
                {

                    Resources.Remove(Key);
                }

                if (ResourceItemChanged != null && !BlockUpdate) ResourceItemChanged(this, Key);
            }
        }

        public BaseResource Pick(int StateMask = 255, int AttrMask = 255)
        {
            if (Count == 0) return null;
            Picker.Update(ListAlive());

            var proxy = (BaseResource)Picker.Pick();
            do
            {
                System.Threading.Thread.Sleep(20);
                proxy = Picker.Pick();

                lock (TTbLock)
                {
                    if (Timetable[proxy.Key] == null)
                    {
                        Timetable[proxy.Key] = DateTime.Now;
                        break;
                    }
                }

                if (proxy == null)
                    continue;
                else
                {
                    lock (TTbLock)
                    {
                        if (DateTime.Now.Subtract(((DateTime)Timetable[proxy.Key])).TotalSeconds >= pickConstant)
                        {
                            Timetable[proxy.Key] = DateTime.Now;
                            break;
                        }
                    }
                }
            }
            while (true);

            return proxy;
        }

        public BaseResource[] PickMultiple(int Count, int StateMask = 255, int AttrMask = 255)
        {
            if (Count == 0) return null;
            Picker.Update(ListAlive());
            return Picker.PickMultiple(Count);
        }
        public ICollection Source
        {
            get
            {
                var rt = new List<BaseResource>();
                lock (ResLock)
                {
                    var arr = new DictionaryEntry[Count];
                    Resources.CopyTo(arr, 0);

                    foreach (DictionaryEntry e in arr)
                    {
                        rt.Add(e.Value as BaseResource);
                    }
                }
                return rt;
            }
        }

        public List<BaseResource> ListAlive()
        {
            List<BaseResource> List = new List<BaseResource>(60000);
            for (int i = 0; i < Count; i++)
            {
                //DictionaryEntry Entry =
                BaseResource Item = Resources[i] as BaseResource;
                if (Item.Attribute == ResourceAttribute.OK && Item.State == ResourceState.ALIVE)
                    List.Add((BaseResource)Item);
            }
            return List;
        }
        public List<BaseResource> ListAll(int StateMask = 255, int AttrMask = 255)
        {
            List<BaseResource> List = new List<BaseResource>(60000);
            for (int i = 0; i < Count; i++)
            {
                BaseResource Item = Resources[i] as BaseResource;
                if ((((int)(Item.State)) & StateMask) > 0 && (((int)(Item.Attribute)) & AttrMask) > 0)
                    List.Add((BaseResource)Item);
            }
            return List;
        }

        public void Load(object Obj)
        {

            Resources.Clear();
            ArrayList r = (ArrayList)Obj;
            Resources = r[0] as OrderedDictionary;
        }
    }
}
