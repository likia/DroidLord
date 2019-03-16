using Newtonsoft.Json.Linq;
using DroidLord.Network;
using DroidLord.Security;
using DroidLord.Util;
using DroidLord;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using DroidLord.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord
{
    public class UAC
    {
        public static string PUB_KEY = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAlAcaMN+t+KEdBsCvzVNH2hKApJgUZwRLZ0Q2SYvz2hoRD4pWE6zO9H8ogwnKq6nk6gf4/0CxcTkyZE9e0herqGwC8zAVcdNL/p/hOUkNIIXe+AlO6Jxo1pLvxpziLYUcBlh/Fb2JKRfeUaW+Pr3UsDddnjKEjZvXnvQZBk6X+aiCxTcew0m1MpTh1HGbd4ZuYvlYgpfZGZ1n4S6RTlABKuPjCyGe+Kh5xUkoOMzwoCDCG/UWTUlqSyEkElbvYi7HhZvrIUFEZuX6RoXPkuCVB6spRYKnzNFLGUj5gIwMXd1/mM/Y4XCQLbLzQ7hnA5lmILkob3MhoAC8XCAQEHhu5QIDAQAB";
        public static string AES_KEY = "";
        public static int TimeBomb = 0;
        public static DateTime LastTime = DateTime.Now;
        private static WebSession session;
        public static string Privilege = "0";
        public const string UAC_API = "http://qk.wanhongsoft.com/";
        //public const string UAC_API = "http://192.168.1.199/lord/";

        public static string Username = "";
        public static string Password = "";
 
        static UAC()
        {
            Program.RSA.Init(PUB_KEY + ":");
            var headers = new NameValueCollection();
            headers["Content-Type"] = "application/json";
            session = new WebSession(
                headers,
                new JsonHandler());

           
        }
        public static bool Offline()
        {
            var ret = Action("offline", Program.SHA256.Hash(AES_KEY));
            if (ret == null)
            {
                return false;            
            }
            return true;
        }

        /// <summary>
        /// 协商AES密钥
        /// </summary>
        /// <returns></returns>
        public static bool Establish()
        {
            AES_KEY = AES.GenerateKey();
            Program.AES.Init(AES_KEY);
            var json = Action("handshake", AES_KEY, Program.RSA);
            if (json == null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// RPC调用
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object Action(string name, string val, ICipher algorithm = null)
        {
            if (algorithm == null)
                algorithm = Program.AES;

            var encData = algorithm.Encrypt(val);
            var respJson = session.Post(UAC_API, new
            {
                action = name,
                data = encData
            });            
            if (respJson != null)
            {
                var jObj = respJson as JObject;
                if (jObj["data"][0] == null)
                {
                    return null;
                }
                var rtData = algorithm.Decrypt(jObj["data"][0].ToString());
                var json = Program.JsonParse(rtData);
                if (json == null)
                {
                    // 是字符串
                    return rtData;
                }
                else
                {
                    return json;
                }
            }
            return null;
        }

        /// <summary>
        /// 注册账号
        /// </summary>
        /// <param name="u"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public static Object Register(string u, string pw)
        {
            var hmac = Misc.HardwareID();
            var ret = Action("register",  JObject.FromObject(
                new
                {
                    username = u,
                    password = pw,
                    hmac = hmac
                }
            ).ToString());
            if (ret == null)
            {
                return null;
            }
            return ret;
        }
        /// <summary>
        /// 账号登录
        /// </summary>
        /// <param name="u"></param>
        /// <param name="pw"></param>
        /// <returns></returns>
        public static Object Login(string u, string pw)
        {
            var hmac = Misc.HardwareID();
            var ret = Action("login", JObject.FromObject(
                new
                {
                    username = u,
                    password = pw,
                    hmac = hmac
                }
            ).ToString());
            if (ret == null)
            {
                return null;
            }
            return ret;
        }

        public static bool KeepAlive()
        {
            var ret = Action("keepalive", JObject.FromObject(
                new
                {
                    username = Username,
                    password = Password,
                    privilege = Privilege                    
                }).ToString());
            if (ret == null)
            {
                LastTime = DateTime.Now;
                if (DateTime.Now.Subtract(LastTime).TotalMinutes < 5)
                {
                    ++TimeBomb;                    
                }
                else
                {
                    TimeBomb = 0;
                }
                if  (TimeBomb > 5)
                {
                    return false;
                }
            }            
            return true;
        }
    }
}