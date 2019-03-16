using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Specialized;
using System.Collections;
using System.Threading;

namespace DroidLord.Network
{
    public enum RequestMethod
    {
        GET = 1,
        POST = 2
    }

    public class WebConnection
    {
        public event RequestFinishedHandler RequestFinished;
        public NameValueCollection ResponseHeaders;
        public string LastError;
        public int Retry = 3;
        public static bool EnableMultiServerAccelerate { get; set; }
        private ReflectionHandler Processor;
        private HttpWebRequest BaseRequest;
        public object ResponseObject;
        private RequestMethod ReqMethod;
        private object ReqObject;
        private bool AllowRD = false;
        public HttpWebResponse LastResponse;

        public WebConnection(string Url, ReflectionHandler Handle)
        {
            Processor = Handle;
            InitRequest(Url);
        }

        public WebConnection(string Url)
        {
            Processor = new PlainTextHandler();
            InitRequest(Url);
        }

        ~WebConnection()
        {
            try
            {
                BaseRequest.Abort();
            }
            catch
            {
            }
            BaseRequest = null;
        }

        private void InitRequest(string Url)
        {
            BaseRequest = (HttpWebRequest)HttpWebRequest.Create(Url);
            BaseRequest.Timeout = 10000;
            BaseRequest.ServicePoint.Expect100Continue = false;
            BaseRequest.ServicePoint.ConnectionLimit = 30000;

        }
        public void SetProcessor(ReflectionHandler handler)
        {
            Processor = handler;
        }
        public string GetDomain()
        {
            return BaseRequest.RequestUri.Host;
        }

        public bool SetTimeout(int Timeout)
        {
            BaseRequest.Timeout = Timeout;
            return true;
        }

        public bool SetProxy(string ProxyAddr)
        {
            if (string.IsNullOrEmpty(ProxyAddr))
            {
                BaseRequest.Proxy = null;
                return false;
            }
            BaseRequest.Proxy = new WebProxy(ProxyAddr);
            BaseRequest.Timeout = 12500;
            return true;
        }
        public bool SetHeader(HttpRequestHeader Name, string val)
        {
            return SetHeader(Name.ToString(), val);
        }
        public CookieCollection ResponseCookie
        {
            get;
            set;
        }
        public bool SetHeader(string Name, string Value)
        {
            switch (Name)
            {
                case "Accept":
                    BaseRequest.Accept = Value;
                    break;
                case "Content-Type":
                    BaseRequest.ContentType = Value;
                    break;
                case "User-Agent":
                    BaseRequest.UserAgent = Value;
                    break;
                case "Connection":
                    BaseRequest.KeepAlive = false;//.Connection = Value;
                    break;
                case "Referer":
                    BaseRequest.Referer = Value;
                    break;
                default:
                    BaseRequest.Headers[Name] = Value;
                    break;
            }
            return true;
        }

        public bool SetCookie(CookieCollection Cookies)
        {
            CookieContainer BaseCookie = (BaseRequest.CookieContainer == null) ? new CookieContainer() : BaseRequest.CookieContainer;
            foreach (Cookie Cookie in Cookies)
            {
                BaseCookie.Add(new Cookie((string)Cookie.Name, (string)Cookie.Value, "/", GetDomain()));
            }
            BaseRequest.CookieContainer = BaseCookie;            
            return true;
        }

        public bool SetCookie(string Name, string Value)
        {
            CookieContainer BaseCookie = (BaseRequest.CookieContainer == null) ? new CookieContainer() : BaseRequest.CookieContainer;
            BaseCookie.Add(new Cookie(Name, Value, "/", GetDomain()));
            BaseRequest.CookieContainer = BaseCookie;
            return true;
        }

        public bool SetMethod(RequestMethod Method)
        {
            ReqMethod = Method;
            return true;
        }

        public bool SetData(object Data)
        {
            ReqObject = Data;
            return true;
        }

        public bool SendRequestAsync(RequestMethod Method, object Data)
        {
            System.Threading.Thread RequestThread = new System.Threading.Thread(new System.Threading.ThreadStart(SendRequestProxy));
            SetMethod(Method);
            SetData(Data);
            RequestThread.Start();
            return true;
        }

        public object SendRequest()
        {
            return SendRequest(ReqMethod, ReqObject);
        }

        private void SendRequestProxy()
        {
            SendRequest();
        }
        public void SetAllowAutoRedirect(bool isAllow)
        {
            BaseRequest.AllowAutoRedirect = isAllow;
            AllowRD = isAllow;
        }
        public object SendRequest(RequestMethod Method, object Data)
        {
            int retry_count = 1;
            do
            {
                try
                {
                    byte[] Body = null;
                    if (Data != null)
                    {
                        Body = Processor.ObjectToData(Data);
                    }
                    BaseRequest.Method = Method == RequestMethod.POST ? "POST" : "GET";
                    BaseRequest.AllowAutoRedirect = AllowRD;
                    BaseRequest.Expect = null;
                    if (BaseRequest.CookieContainer == null)
                    {
                        // 初始化Cookies容器以获取返回的Cookie
                        BaseRequest.CookieContainer = new CookieContainer();
                    }
                    BaseRequest.ServicePoint.Expect100Continue = false;
                    BaseRequest.CachePolicy = new System.Net.Cache.HttpRequestCachePolicy(System.Net.Cache.HttpRequestCacheLevel.NoCacheNoStore);
                    if (Body != null)
                    {
                        //BaseRequest.ContentLength = Body.Length;
                        Stream RequestStream = BaseRequest.GetRequestStream();
                        RequestStream.Write(Body, 0, Body.Length);
                        RequestStream.Close();
                    }
                    HttpWebResponse Response = (HttpWebResponse)BaseRequest.GetResponse();
                    LastResponse = Response;
                    ResponseCookie = Response.Cookies;
                    ResponseHeaders = new NameValueCollection();
                    for (int i = 0; i < Response.Headers.Count; i++)
                    {
                        ResponseHeaders.Add(Response.Headers.Keys[i], Response.Headers[i]);
                    }
                    Stream ResponseStream = Response.GetResponseStream();
                    MemoryStream Buffer = new MemoryStream();
                    int ReadLen = 0;
                    byte[] Part = new byte[409600];
                    while (true)
                    {
                        ReadLen = ResponseStream.Read(Part, 0, 409600);
                        if (ReadLen == 0) break;
                        Buffer.Write(Part, 0, ReadLen);
                    }
                    ResponseStream.Close();
                    ResponseStream.Dispose();
                    byte[] ResponseData = Buffer.ToArray();

                    Buffer.Close();
                    Buffer.Dispose();
                    ResponseObject = Processor.DataToObject(ResponseData);
                    LastError = null;
                    if (RequestFinished != null) RequestFinished(this);
                    return ResponseObject;
                }
                catch (Exception ex)
                {
                    LastError = "网络错误";
                    ResponseHeaders = null;
                    ResponseObject = null;
                    Thread.Sleep(500);
                }
            } while (retry_count++ <= Retry);
            return
                null;
        }
      
        void SetCookies(CookieCollection cc, ref HttpWebRequest req)
        {
            req.CookieContainer = req.CookieContainer == null ? new CookieContainer() : req.CookieContainer;
            foreach (Cookie c in cc)
            {
                Cookie t = new Cookie(c.Name, c.Value, "/", req.RequestUri.Host);
                t.Expires = new System.DateTime(9999, 12, 1);
                req.CookieContainer.Add(t);
            }
        }
    }


    /// <summary>
    /// https 的解决方案：
    ///    参考地址：http://social.microsoft.com/Forums/zh-CN/wcfzhchs/thread/1591a00d-d431-4ad8-bbd5-34950c39d563
    /// </summary>
    public static class HttpUtil
    {
        /// <summary>
        /// Sets the cert policy.
        /// </summary>
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }

        /// <summary>
        /// Remotes the certificate validate.
        /// </summary>
        private static bool RemoteCertificateValidate(
           object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!           
            System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }
    }
}