using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Util
{
    /// <summary>
    /// 字符串处理
    /// </summary>
    public static class String
    {
        /// <summary>
        /// 从标题获取商品型号
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetModelSN(string title)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(title))
                return string.Empty;
            title = title.Replace("@", "");
            RegexOptions ro = RegexOptions.Singleline | RegexOptions.IgnoreCase;
            string reg_spkey = @"(?<x>[A-Za-z]+[0-9-/]+[A-Za-z0-9-+/\)\(]{2,}|[0-9]+[A-Za-z]+[A-Za-z0-9]{2,}|[0-9-/]{5,})";
            MatchCollection m = Regex.Matches(title, reg_spkey, ro);
            foreach (Match match in m)
            {
                if (match.Value.Length > 4 && match.Value.Split('/').Length <= 2)
                {
                    list.Add(match.Value);
                }
            }
            string spxh = string.Empty;
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (i == 0)
                        spxh += list[i];
                    else
                        spxh += " " + list[i];
                }
            }
            return spxh;
        }

        /// <summary>
        /// 由 一个字符串集合为种子，生成一个 唯一标识符guid
        /// </summary>
        /// <param name="listObj"></param>
        /// <returns></returns>
        public static string CreateGuid(IEnumerable<string> listObj)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var str = string.Join("\n", listObj);
                var bytes = Encoding.UTF8.GetBytes(str);
                var hash = md5.ComputeHash(bytes);
                return new Guid(hash).ToString("n");
            }
        }

        /// <summary>
        /// 是否正整数
        /// </summary>
        /// <param name="s">需验证的字符串</param>
        /// <returns></returns>
        public static bool IsPositiveInt(string s)
        {
            Regex reg = new Regex(@"^[1-9]\d*$");
            return reg.IsMatch(s);
        }

        /// <summary>
        /// 获取正浮点数
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <returns></returns>
        public static List<string> GetPositiveFloat(string s)
        {
            List<string> result = new List<string>();
            Regex reg = new Regex(@"[+-]?(0|([1-9]\d*))(\.\d+)?");
            MatchCollection m = reg.Matches(s);
            if (m != null && m.Count > 0)
            {
                foreach (Match item in m)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        result.Add(item.Value);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="s">原始字符串</param>
        /// <returns></returns>
        public static List<string> GetNumber(string s)
        {
            List<string> result = new List<string>();
            Regex reg = new Regex(@"\d+");
            MatchCollection m = reg.Matches(s);
            if (m != null && m.Count > 0)
            {
                foreach (Match item in m)
                {
                    if (!string.IsNullOrWhiteSpace(item.Value))
                    {
                        result.Add(item.Value);
                    }
                }
            }
            return result;
        }

        public static int ToInt32(string s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex);
                return 0;
            }
        }


        public static decimal ToDecimal(string s)
        {
            try
            {
                return Convert.ToDecimal(s);
            }
            catch (Exception ex)
            {
                Log.WriteLog(ex);
                return 0;
            }
        }

        /// <summary>
        /// 获取汉字字符串的首字母
        /// </summary>
        /// <param name="strChinese">汉字字符串</param>
        /// <returns></returns>
        public static string GetFirstSpell(this string strChinese)
        {
            try
            {
                if (strChinese.Length > 0)
                {
                    var chr = strChinese[0];
                    var res = GetSpell(chr);
                    if (!string.IsNullOrWhiteSpace(res))
                    {
                        return (res[0]).ToString().ToUpper();
                    }
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }

        /// <summary>
        /// 获取该字符的拼音字符串
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        public static string GetSpell(char chr)
        {
            return NPinyin.Pinyin.GetPinyin(chr);
        }
    }
}
