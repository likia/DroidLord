using DroidLord.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DroidLord.Network
{    
    public delegate void RequestFinishedHandler(object sender);
    
    /// <summary>
    /// Web会话类
    /// 封装一次会话具有的属性（主要是 Cookie）
    /// </summary>
    public class WebSession
    {
        // 数据处理器，对象和字符串直接进行转换的处理器
        // 子类有 JsonHandler： dynamic对象
        //        RawHandler : byte[]
        //        PlainTextHandler : string    
        private ReflectionHandler DataHandler;
        // 除了默认header还需要添加的header
        private NameValueCollection Headers;
        // 会话积累的Cookie
        public CookieCollection Cookies;
        // 是否允许30x跳转
        public bool AllowRedirect = false;
        public int Timeout = 10000;
        // 最多重试次数， 
        public int MaximumRetry = 5;
        // http代理
        public string Proxy = "";

        public WebSession(ReflectionHandler handler)
        {
            DataHandler = handler;
            Headers = new NameValueCollection();
        }   
        public WebSession(NameValueCollection customHeader, ReflectionHandler handler)
        {
            DataHandler = handler;
            Headers = customHeader;    
        }    
        /// <summary>
        /// 创建WebConnection对象
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        WebConnection buildRequest(string uri)
        {
            if (Cookies == null)
            {
                Cookies = new CookieCollection();
            }
            var req = new WebConnection(uri, DataHandler);
            foreach (var header in Headers.Keys)
            {
                req.SetHeader(header.ToString(), Headers[header.ToString()]);
            }
            req.SetCookie(Cookies);
            req.SetTimeout(Timeout);
            req.SetAllowAutoRedirect(AllowRedirect);
            req.Retry = MaximumRetry;
            if (!string.IsNullOrEmpty(Proxy))
            {
                req.SetProxy(Proxy);
            }            
            return req;
        }

        public Object Get(string uri)
        {
            var conn = buildRequest(uri);
            conn.SetMethod(RequestMethod.GET);
            var resp = conn.SendRequest();
            // 请求成功, 累加cookie
            if (resp != null)
            {
                var respCookies = conn.ResponseCookie;
                Cookies.Add(respCookies);
            }
            return resp;
        }

        public Object Post(string uri, Object data)
        {
            var conn = buildRequest(uri);
            if (string.IsNullOrEmpty(Headers["Content-Type"]))
            {
                // 未设置默认表单类型
                conn.SetHeader(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            }
            var resp = conn.SendRequest(RequestMethod.POST, data);
            // 请求成功, 累加cookie
            if (resp != null)
            {
                var respCookies = conn.ResponseCookie;
                Cookies.Add(respCookies);
            }
            return resp;
        }
    }
}