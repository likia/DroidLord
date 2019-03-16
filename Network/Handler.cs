using DroidLord.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Network
{
    public interface ReflectionHandler
    {
        object DataToObject(byte[] Data);
        byte[] ObjectToData(object Obj);
    }

    public class PlainTextHandler : ReflectionHandler
    {
        private Encoding TextEncoding;

        public PlainTextHandler(Encoding Coding)
        {
            TextEncoding = Coding;
        }

        public PlainTextHandler()
        {
            TextEncoding = Encoding.GetEncoding("utf-8");
        }

        public object DataToObject(byte[] Data)
        {
            return TextEncoding.GetString(Data);
        }

        public byte[] ObjectToData(object Obj)
        {
            return TextEncoding.GetBytes((string)Obj);
        }
    }
    public class JsonHandler : ReflectionHandler
    {
        public object DataToObject(byte[] Data)
        {
            var str = Encoding.Default.GetString(Data);
            dynamic jsonObj =  JObject.Parse(str);
            return jsonObj;
        }

        public byte[] ObjectToData(object Obj)
        {
            return Encoding.Default.GetBytes(
                JObject.FromObject(Obj).ToString());
        }
    }
    public class RawHandler : ReflectionHandler
    {
        public object DataToObject(byte[] Data)
        {
            return Data;
        }

        public byte[] ObjectToData(object Obj)
        {
            return (byte[])Obj;
        }
    }
}
