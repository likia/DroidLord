using DroidLord.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Resource
{
    public class Script : BaseResource
    {
        public string FileName
        {
            get
            {
                return GetValue("filename") as string;           
            }
            set
            {
                SetValue("filename", value);
            }
        }
        public string FullPath
        {
            get
            {
                return GetValue("path") as string;
            }
            set
            {
                SetValue("path", value);
            }
        }
        public string Content
        {
            get
            {
                return this.GetValue("content") as string;
            }
            set
            {
                this.SetValue("content", value);
            }
        }
        public string DisplayName
        {
            get
            {
                return this.GetValue("display") as string;
            }
            set
            {
                this.SetValue("display", value);
            }
        }
        public string Require
        {
            get
            {
                return this.GetValue("require") as string;
            }
            set
            {
                this.SetValue("require", value);
            }
        }
        public string Configuration
        {
            get
            {
                return this.GetValue("configuration") as string;
            }
            set
            {
                this.SetValue("configuration", value);
            }
        }
        public string ConfigName
        {
            get
            {
                return GetValue("config") as string;
            }
            set
            {
                SetValue("config", value);
            }
        }
        public void ParseConfig(string path)
        {
            try
            {
                var conf = File.ReadAllText(path);
                var root = JObject.Parse(conf);
                DisplayName = root["display"].ToString();
                Require = root["require"].ToString();
                Configuration = root["configuration"].ToString();
                ConfigName = root["config"].ToString();
            }
            catch (Exception ex)
            {
                Program.Logs.WriteLog("脚本配置解析失败", $"文件:{path}", LogLevel.Exception);
            }
        }
        
        public string ForgeConfig()
        {
            var confRoot = JObject.Parse(Configuration);
            var views = (JArray)confRoot["views"];
            foreach(JToken vCtl in views)
            {
                foreach (JProperty p in vCtl)
                {
                    if (p.Value.ToString().Contains("$"))
                    {
                        var varName = p.Value.ToString();
                        p.Value = GetValue(varName).ToString();
                    }
                }
            }
            return confRoot.ToString();
        }
    }
}
