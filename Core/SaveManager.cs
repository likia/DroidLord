using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace DroidLord.Core
{
    public class UBinder : System.Runtime.Serialization.SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
            return ass.GetType(typeName);
        }
    }

    public interface Savable
    {
        object Save();
        void Load(object State);
    }

    public class SaveManager
    {
        private Hashtable SavedObject;
        private Hashtable ObjectHandler;

        public SaveManager()
        {
            SavedObject = new Hashtable();
            ObjectHandler = new Hashtable();
        }

        public SaveManager(string StorageFile)
        {
            SavedObject = new Hashtable();
            ObjectHandler = new Hashtable();
            LoadStorage(StorageFile);
        }

        public bool LoadStorage(string FileName)
        {
            if (!File.Exists(FileName))
            {
                return false;
            }
            FileStream FileHandle = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                BinaryFormatter Formatter = new BinaryFormatter();
                Formatter.Binder = new UBinder();
                SavedObject = (Hashtable)Formatter.Deserialize(FileHandle);
                FileHandle.Close();
                FileHandle.Dispose();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("加载配置文件出现异常!可能引起配置文件丢失!\r\n 错误记录在目录下 : 序列化异常.txt");
                System.IO.File.WriteAllText("序列化异常.txt", ex.Message + "\r\n\r\n" + ex.StackTrace);
                System.IO.File.Copy(FileName, "restore.bak", true);
                FileHandle.Close();
                FileHandle.Dispose();
                return false;
            }
            LoadFlush();
            return true;
        }

        public bool SaveStorage(string FileName)
        {
            FileStream FileHandle = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
            BinaryFormatter Formatter = new BinaryFormatter();
            SaveFlush();
            Formatter.Serialize(FileHandle, SavedObject);
            FileHandle.Close();
            FileHandle.Dispose();
            return true;
        }

        private void LoadFlush()
        {
            foreach (DictionaryEntry Entry in SavedObject)
            {
                string Name = (string)Entry.Key;
                if (ObjectHandler.Contains(Name))
                {
                    ((Savable)ObjectHandler[Name]).Load(Entry.Value);
                }
            }
        }

        private void SaveFlush()
        {
            SavedObject.Clear();
            foreach (DictionaryEntry Entry in ObjectHandler)
            {
                string Name = (string)Entry.Key;
                Savable Handler = (Savable)Entry.Value;
                SavedObject.Add(Name, Handler.Save());
            }
        }

        public bool Handle(string Name, Savable Handler)
        {
            if (ObjectHandler.Contains(Name))
            {
                return false;
            }
            ObjectHandler.Add(Name, Handler);
            if (SavedObject.Contains(Name))
            {
                Handler.Load(SavedObject[Name]);
            }
            return true;
        }
    }
}
