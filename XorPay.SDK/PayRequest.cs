using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace XorPay.SDK
{
    /// <summary>
    /// 请求类
    /// </summary>
    public static class PayRequest
    {
        #region 获取Get请求string类型值 +  static string GetQueryString(string strName)
        /// <summary>
        /// 获取Get请求string类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetQueryString(string strName)
        {
            return HttpContext.Current.Request.QueryString[strName] == null ? "" : HttpContext.Current.Request.QueryString[strName];
        }
        #endregion

        #region 获取Get请求int类型值 + static int GetQueryInt(string strName, int defaultvalue = 0)
        /// <summary>
        /// 获取Get请求int类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static int GetQueryInt(string strName, int defaultvalue = 0)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\d+");
                Match objmach = obj.Match(HttpContext.Current.Request.QueryString[strName].ToString());
                if (objmach.Success)
                    return Convert.ToInt32(objmach.Value);
                else
                    return defaultvalue;
            }
        }
        #endregion

        #region 获取Get请求float类型值 + static float GetQueryFloat(string strName, float defaultvalue = 0)
        /// <summary>
        /// 获取Get请求float类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static float GetQueryFloat(string strName, float defaultvalue = 0)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null || HttpContext.Current.Request.QueryString[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                float.TryParse(HttpContext.Current.Request.QueryString[strName], out defaultvalue);
                return defaultvalue;
            }
        } 
        #endregion

        #region 获取表单string类型值 + static string GetFormString(string strName)
        /// <summary>
        /// 获取表单string类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetFormString(string strName)
        {
            return HttpContext.Current.Request.Form[strName] == null ? "" : HttpContext.Current.Request.Form[strName];
        } 
        #endregion

        #region 获取表单int类型值 + static int GetFormInt(string strName, int defaultvalue = 0)
        /// <summary>
        /// 获取表单int类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static int GetFormInt(string strName, int defaultvalue = 0)
        {
            if (HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                Regex obj = new Regex("\\d+");
                Match objmach = obj.Match(HttpContext.Current.Request.Form[strName].ToString());
                if (objmach.Success)
                    return Convert.ToInt32(objmach.Value);
                else
                    return defaultvalue;
            }
        } 
        #endregion

        #region  获取表单float类型值 + static float GetFormFloat(string strName, float defaultvalue = 0)
        /// <summary>
        /// 获取表单float类型值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static float GetFormFloat(string strName, float defaultvalue = 0)
        {
            if (HttpContext.Current.Request.Form[strName] == null || HttpContext.Current.Request.Form[strName].ToString() == string.Empty)
                return defaultvalue;
            else
            {
                float.TryParse(HttpContext.Current.Request.Form[strName], out defaultvalue);
                return defaultvalue;
            }
        }
        #endregion

        #region 发送请求 + static string SendRequest(string url, string para, string method = "GET", string coding = "UTF-8")
        /// <summary>
        /// 通讯函数
        /// </summary>
        /// <returns></returns>
        public static string SendRequest(string url, string para, string method = "GET", string coding = "UTF-8")
        {
            string strResult = "";
            if (url == null || url == "")
                return null;
            if (method == null || method == "")
                method = "GET";
            // GET方式
            if (method.ToUpper() == "GET")
            {
                StreamReader sr = null;
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    WebRequest wrq = WebRequest.Create(url + para);
                    wrq.Method = "GET";
                    WebResponse wrp = wrq.GetResponse();
                    sr = new StreamReader(wrp.GetResponseStream(), Encoding.GetEncoding(coding));
                    strResult = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Dispose(); sr.Close();
                    }
                }
            }
            // POST方式
            if (method.ToUpper() == "POST")
            {
                StreamReader sr = null;
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = "POST";
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.Timeout = 20000;//超时20秒
                    byte[] body = Encoding.GetEncoding(coding).GetBytes(para);
                    req.ContentLength = body.Length;
                    Stream sm = req.GetRequestStream();
                    sm.Write(body, 0, body.Length);
                    using (WebResponse wrp = req.GetResponse())
                    {
                        sr = new StreamReader(wrp.GetResponseStream(), Encoding.GetEncoding(coding));
                        strResult = sr.ReadToEnd();
                    }
                    sm.Dispose(); sm.Close();
                    req.Abort();
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message, ex);
                    return strResult;
                }
                finally
                {
                    if (sr != null)
                    {
                        sr.Dispose(); sr.Close();
                    }
                }
            }
            return strResult;
        }
        #endregion
    }
}
