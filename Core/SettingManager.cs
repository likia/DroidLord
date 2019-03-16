using DroidLord.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace DroidLord.Core
{
    public delegate void SettingChangedHandler(object sender, string Key, object NewValue);

    public class ConfigRow
    {
        public string Key;
        public List<string> Fields;
        public ConfigRow(string _Key, List<string> _Fields)
        {
            Key = _Key;
            Fields = _Fields;
        }        
    }

    public enum SettingType
    {
        SETTING_INT = 0,
        SETTING_FLOAT = 1,
        SETTING_BOOLEAN = 2,
        SETTING_STRING = 3,
        SETTING_FILEPATH = 4,
        SETTING_RESERVED = -1
    }

    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class SettingItem
    {
        public string Name;
        public string DisplayName;
        public SettingType Type;
        public object Value;
        public object DefaultValue;
        public bool Ignorable;
        public Type ValueType;

        public string GetDisplayType()
        {
            var dispType = "";
            switch (Type)
            {
                case Core.SettingType.SETTING_INT:
                    dispType = "整数";
                    ValueType = Int32.MaxValue.GetType();
                    break;
                case Core.SettingType.SETTING_STRING:
                    ValueType = String.Empty.GetType();
                    dispType = "字符";
                    break;
                case Core.SettingType.SETTING_BOOLEAN:
                    ValueType = Boolean.TrueString.GetType();
                    dispType = "是/否";
                    break;
                case Core.SettingType.SETTING_FLOAT:
                    ValueType = Double.Epsilon.GetType();
                    dispType = "数字";
                    break;
                default:
                    return "";
            }
            return dispType;
        }
    }

    [System.Runtime.Remoting.Contexts.Synchronization]
    public class SettingManager : Savable
    {
        public event SettingChangedHandler SettingChanged;
        private OrderedDictionary SettingTable;

        public SettingManager()
        {
            SettingTable = new OrderedDictionary();
        }

        public object Save()
        {
            Hashtable KeyValue = new Hashtable();
            foreach (DictionaryEntry Entry in SettingTable)
            {
                KeyValue.Add(Entry.Key, ((SettingItem)Entry.Value).Value);
            }
            return KeyValue;
        }

        public void Load(object obj)
        {
            Hashtable KeyValue = (Hashtable)obj;
            foreach (DictionaryEntry Entry in KeyValue)
            {
                SetValue((string)Entry.Key, (object)Entry.Value);
            }
        }

        public void Register(string Name, string DisplayName, SettingType Type, object DefaultValue, bool Ignorable)
        {
            if (SettingTable.Contains(Name)) return;
            SettingItem Item = new SettingItem();
            Item.Name = Name;
            Item.DisplayName = DisplayName;
            Item.Type = Type;
            Item.DefaultValue = DefaultValue;
            Item.Value = Item.DefaultValue;
            Item.Ignorable = Ignorable;
            SettingTable.Add(Name, Item);
            if (SettingChanged != null)
                SettingChanged(this, Name, Item.Value);
        }

        public SettingItem Get(string Name)
        {
            if (SettingTable.Contains(Name))
                return (SettingItem)SettingTable[Name];
            else
                return null;
        }

        public List<SettingItem> List()
        {
            List<SettingItem> List = new List<SettingItem>();
            foreach (DictionaryEntry Entry in SettingTable)
            {
                List.Add((SettingItem)Entry.Value);
            }
            return List;
        }

        public object GetValue(string Name)
        {
            SettingItem Item = Get(Name);
            if (Item == null)
                return null;
            else
                return Item.Value;
        }
        public T GetValue<T>(string Name)
        {
            var val = GetValue(Name);
            if (val != null)
            {
                return (T)val;
            }
            return default(T);
        }
        public void SetValue(string Name, object Value)
        {
            SettingItem Item = Get(Name);
            if (Item != null)
                Item.Value = Value;
            if (SettingChanged != null)
                SettingChanged(this, Name, Value);
        }

        public void ParseConfigRow(List<ConfigRow> Rows)
        {
            for (int i = 0; i < Rows.Count; i++) ParseConfigRow(Rows[i]);
        }

        public void ParseConfigRow(ConfigRow Row)
        {
            if (Row.Fields.Count == 2) Row.Fields.Add("");
            SettingType SType = SettingType.SETTING_RESERVED;
            object SValue = null;
            switch (Row.Fields[1])
            {
                case "INT":
                    SType = SettingType.SETTING_INT;
                    SValue = int.Parse(Row.Fields[2]);
                    break;
                case "FLOAT":
                    SType = SettingType.SETTING_FLOAT;
                    SValue = double.Parse(Row.Fields[2]);
                    break;
                case "BOOLEAN":
                    SType = SettingType.SETTING_BOOLEAN;
                    SValue = bool.Parse(Row.Fields[2].ToLower());
                    break;
                case "STRING":
                    SType = SettingType.SETTING_STRING;
                    SValue = Row.Fields[2];
                    break;
                case "FILEPATH":
                    SType = SettingType.SETTING_FILEPATH;
                    SValue = Row.Fields[2];
                    break;
            }
            Register(Row.Key, Row.Fields[0], SType, SValue, bool.Parse(Row.Fields[3]));
        }
        private static ConfigRow ParseConfigRow(string Raw)
        {
            string Line = Raw.Trim();
            if (Line.Length == 0) return null;
            if (Line.StartsWith("#")) return null;
            string Key = Line.Substring(0, Line.IndexOf("=")).Trim();
            string Value = Line.Substring(Line.IndexOf("=") + 1).Trim();
            List<string> Fields = new List<string>();
            string Buffer = "";
            bool InQuote = false;
            for (int j = 0; j < Value.Length; j++)
            {
                if (Value[j] == '"')
                {
                    InQuote = !InQuote;
                    continue;
                }
                if (InQuote)
                {
                    if (Value[j] == '\\')
                    {
                        char ch = Value[++j];
                        if (ch == 'r')
                            Buffer += "\r";
                        else if (ch == 'n')
                            Buffer += "\n";
                        else if (ch == 't')
                            Buffer += "\t";
                        else
                            Buffer += ch;
                        continue;
                    }
                    Buffer += Value[j];
                    continue;
                }
                if (Value[j] == ':')
                {
                    Fields.Add(Buffer);
                    Buffer = "";
                }
                else
                {
                    Buffer += Value[j];
                }
            }
            if (Buffer.Length > 0)
            {
                Fields.Add(Buffer);
            }
            return new ConfigRow(Key, Fields);
        }

        public static List<ConfigRow> ParseConfig(string FileName)
        {
            string Text = File.ReadAllText(FileName,Encoding.Default);
            if (Text == null) return null;
            List<ConfigRow> Rows = new List<ConfigRow>();
            string[] Lines = StrHelper.Explode("\n", Text);
            for (int i = 0; i < Lines.Length; i++)
            {
                ConfigRow CurrentRow = ParseConfigRow(Lines[i]);
                if (CurrentRow != null)
                {
                    if (CurrentRow.Key == "+include")
                    {
                        string IncludeFile = FileName;
                        IncludeFile = IncludeFile.Substring(0, IncludeFile.LastIndexOf(@"\")) + @"\" + CurrentRow.Fields[0];
                        List<ConfigRow> Include = ParseConfig(IncludeFile);
                        if (Include != null)
                            Rows.AddRange(Include);
                    }
                    else
                        Rows.Add(CurrentRow);
                }
            }
            return Rows;
        }
        public void LoadConfig(string FileName)
        {
            List<ConfigRow> ConfigRows = ParseConfig(FileName);
            for (int i = 0; i < ConfigRows.Count; i++)
            {
                if (ConfigRows[i].Fields.Count >= 2)
                {
                    ParseConfigRow(ConfigRows[i]);
                }
            }
        }
    }
}
