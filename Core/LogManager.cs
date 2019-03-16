using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Core
{
    public class LogManager : Savable
    {
        private Hashtable Logs = new Hashtable();
        public event Action<LogItem> LogWritten;

        public LogManager()
        {
            RegisterEntry("default");
        }
        public void RegisterEntry(string Category)
        {
            if (Logs[Category] == null)
            {
                var list = new List<LogItem>();
                Logs[Category] = list;
            }
        }
        public void ClearLog(string Entry = "default")
        {
            Logs[Entry] = new List<LogItem>();
        }
        public void WriteLog(string Message, string Desc = "", LogLevel level = LogLevel.Message, string Entry = "default")
        {
            var logs = Logs[Entry] as List<LogItem>;
            if (logs != null)
            {
                var log = new LogItem() { Category = Entry, Desc = Desc, Level = level, Message = Message };
                logs.Add(log);
                LogWritten?.Invoke(log);
            }
        }
        public List<LogItem> GetLogs(string Entry)
        {
            return Logs[Entry] as List<LogItem> ?? new List<LogItem>();            
        }
        public void Load(object State)
        {
            Logs = State as Hashtable;
        }
        public object Save()
        {
            return Logs;
        }
    }
    [Serializable]
    public class LogItem
    {
        public string Message;
        public string Desc;
        public DateTime CreateTime;
        public LogLevel Level;
        public string Category;

        public LogItem()
        {
            CreateTime = DateTime.Now;
            Level = LogLevel.Message;
        }
    }
    public enum LogLevel
    {
        /// <summary>
        /// 普通消息，无关痛痒
        /// </summary>
        Message,
        /// <summary>
        /// 提醒
        /// </summary>
        Notice,
        /// <summary>
        /// 警告
        /// </summary>
        Warning,
        /// <summary>
        /// 可忽略错误，try-catch捕捉到的 
        /// </summary>
        Exception,
        /// <summary>
        /// 会影响程序逻辑的错误
        /// </summary>
        Error,        
        /// <summary>
        /// 致命错误，程序即将崩溃
        /// </summary>
        Fatal,
    }
}
