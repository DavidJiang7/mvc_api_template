using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Collections.Generic;

namespace Util
{
    /// <summary>
    /// 写日志委托
    /// </summary>
    /// <param name="message">日志信息</param>
    public delegate void DelegateWriteLog(string message);

    /// <summary>
    /// 日志工具
    /// </summary>
    public class Log
    {
        private static string ErrorFolder = AppDomain.CurrentDomain.BaseDirectory + "Log";

        private static string ErrorFile;

        private static Dictionary<long, long> lockDic = new Dictionary<long, long>();

        private static DelegateWriteLog wlog;

        static Log()
        {
            ErrorFile = ErrorFolder + @"\" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".Log";
            if (!Directory.Exists(ErrorFolder))
            {
                Directory.CreateDirectory(ErrorFolder);
            }
            if (!File.Exists(ErrorFile))
            {
                //异常日志不存在创建
                StreamWriter write = File.CreateText(ErrorFile);
                write.WriteLine("----------LOG----------");
                write.Close();
            }
            wlog += new DelegateWriteLog(Write);
        }

        private static void Write(string message)
        {
            try
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(ErrorFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite, 8, System.IO.FileOptions.Asynchronous))
                {

                    Byte[] dataArray = System.Text.Encoding.UTF8.GetBytes("--------start--------" + System.Environment.NewLine + "[" + DateTime.Now.ToString() + "]" + System.Environment.NewLine + message + "---------end---------" + System.Environment.NewLine);
                    bool flag = true;
                    long slen = dataArray.Length;
                    long len = 0;
                    while (flag)
                    {
                        try
                        {
                            if (len >= fs.Length)
                            {
                                fs.Lock(len, slen);
                                lockDic[len] = slen;
                                flag = false;
                            }
                            else
                            {
                                len = fs.Length;
                            }
                        }
                        catch (Exception)
                        {
                            while (!lockDic.ContainsKey(len))
                            {
                                len += lockDic[len];
                            }
                        }
                    }
                    fs.Seek(len, System.IO.SeekOrigin.Begin);
                    fs.Write(dataArray, 0, dataArray.Length);
                    fs.Close();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 写入异常信息
        /// </summary>
        /// <param name="message">信息</param>
        public static void WriteLog(string message)
        {
            wlog.BeginInvoke(message, it => wlog.EndInvoke(it), null);//异步写日志，第二个参数是回调函数，回调结束异步，EndInvoke还是必须调用，否则可能会造成内存泄漏
        }

        /// <summary>
        /// 写入异常信息
        /// </summary>
        /// <param name="ex">异常Exception</param>
        public static void WriteLog(Exception ex)
        {
            string s = "";
            s += "StackTrace: " + ex.StackTrace + System.Environment.NewLine;
            s += "Message: " + ex.Message + System.Environment.NewLine;
            if (ex.InnerException != null)
            {
                s += "InnerException: " + ex.InnerException.Message + System.Environment.NewLine;
            }
            WriteLog(s);
        }
    }
}