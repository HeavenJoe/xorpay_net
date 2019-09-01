using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XorPay.SDK
{
    // <summary>
    /// JSON帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary> 
        /// 对象转JSON 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>JSON格式的字符串</returns> 
        public static string ObjectToJSON(object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }


        /// <summary> 
        /// JSON文本转对象,泛型方法 
        /// </summary> 
        /// <typeparam name="T">类型</typeparam> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>指定类型的对象</returns> 
        public static T JSONToObject<T>(string jsonText)
        {
            return jsonText == null ? default(T) : JsonConvert.DeserializeObject<T>(jsonText);
        }

        /// <summary> 
        /// 将JSON文本转换为数据表数据 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据表字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> TablesDataFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, List<Dictionary<string, object>>>>(jsonText);
        }

        /// <summary> 
        /// 将JSON文本转换成数据行 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据行的字典</returns>
        public static Dictionary<string, object> DataRowFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, object>>(jsonText);
        }

        /// <summary>
        /// 获取Json关键词
        /// </summary>
        /// <param name="result"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetString(string result, string key)
        {
            JObject Items = JObject.Parse(result);
            if (Items[key] != null)
            {
                return Items[key].ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 输出Json列表
        /// </summary>
        /// <param name="response"></param>
        public static void WriteJson(HttpContext context, object response)
        {

            string jsonpCallback = context.Request["callback"],
                   json = ObjectToJSON(response);
            context.Response.Clear();
            if (String.IsNullOrWhiteSpace(jsonpCallback))
            {
                context.Response.AddHeader("Content-Type", "text/plain");
                context.Response.Write(json);
            }
            else
            {
                context.Response.AddHeader("Content-Type", "application/javascript");
                context.Response.Write(String.Format("{0}({1});", jsonpCallback, json));
            }
            context.Response.End();
        }
    }

    /// <summary>
    /// 回传Json数据
    /// </summary>
    public class JsonData<T>
    {
        /// <summary>
        /// 状态，1正常，0失败
        /// </summary>
        public int status { get; set; } = 0;

        /// <summary>
        /// 消息
        /// </summary>
        public string msg { get; set; } = string.Empty;

        /// <summary>
        /// 数据
        /// </summary>
        public T data { get; set; }
    }
}
