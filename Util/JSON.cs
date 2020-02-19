using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Util
{
    /// <summary>
    /// 解析JSON，仿Javascript风格
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// json反序列化
        /// </summary>
        /// <typeparam name="T">需要反序列化的对象，可以是list/array/class</typeparam>
        /// <param name="jsonString">json数据字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T Parse<T>(string jsonString)
        {
            //jsonString = UnFormatTime(jsonString);
            try
            {
                JavaScriptSerializer s = new JavaScriptSerializer();
                return (T)s.Deserialize(jsonString, typeof(T));
            }
            catch
            {
                try
                {
                    var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                    var d = new DataContractJsonSerializer(typeof(T));
                    return (T)d.ReadObject(ms);
                }
                catch (Exception ex)
                {
                    Util.Log.WriteLog(ex);
                    return default(T);
                }
            }
        }

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="jsonObject">需要序列化的对象</param>
        /// <returns>序列化后的json字符串</returns>
        public static string String(object jsonObject)
        {
            try
            {
                JavaScriptSerializer s = new JavaScriptSerializer();
                return s.Serialize(jsonObject);
            }
            catch (Exception ee)
            {
                try
                {
                    var ms = new MemoryStream();
                    var d = new DataContractJsonSerializer(jsonObject.GetType());
                    d.WriteObject(ms, jsonObject);
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch (Exception ex)
                {
                    Util.Log.WriteLog(ex);
                    return "";
                }
            }
        }

        /// <summary>
        /// 将对象json字符串转为字典
        /// </summary>
        /// <param name="jsonObjStr">对象json字符串</param>
        /// <returns></returns>
        public static Dictionary<string, object> ToDictionary(string jsonObjStr)
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            return s.Deserialize<Dictionary<string, object>>(jsonObjStr);
        }

        /// <summary>
        /// 反序列化前，格式化 "2016-08-05T10: 22: 53+08: 00"时间格式为C#格式"\/Date(1294499956278+0800)\/"
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// <returns>处理后的json字符串</returns>
        public static string UnFormatTime(string jsonString)
        {
            //替换Json的Date字符串  
            string p = @"\d{4}-\d{2}-\d{2}.*\d{2}:.*\d{2}:.*\d{2}:.*\d{2}";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertDateStringToJsonDate);
            Regex reg = new Regex(p);
            var res = reg.Replace(jsonString, matchEvaluator);
            return res;
        }

        /// <summary>  
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串  
        /// </summary>  
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        /// <summary>  
        /// 将时间字符串转为Json时间  
        /// </summary>  
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
    }
}
