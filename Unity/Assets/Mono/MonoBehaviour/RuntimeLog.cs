
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Diagnostics;
using System.Threading;

namespace ET
{

    ///     增加 保存本地文件数据  
    ///
    public class RuntimeLog : MonoSingleton<RuntimeLog>
    {
        public int logCount = 0;
        private static bool initFileSuccess = false;
        public static int errorCount = 0;
        public static int exceptCount = 0;
        private static int MAX_ERROR_LOG = 100;
        private string outpath;
        private StreamWriter writer;
        

        private static List<string> writeList = new List<string>();
        public static List<string> errorList = new List<string>();

#if UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod]
        static void RunOnStart()
        {
            Application.logMessageReceived -= HandleLog;
            ETTask.ExceptionHandler -= Error;

            writeList.Clear();
            initFileSuccess = false;
        }
#endif

        private void Awake()
        {

        }

        private void Start()
        {
            InitLogFile();
            Application.logMessageReceived += HandleLog;
            ETTask.ExceptionHandler += Error;
        }

        private void OnDestroy()
        {
            writer?.Close();
        }

        private void InitLogFile()
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                outpath = Application.persistentDataPath + "/Clent.outlog.txt";
            }
            else
            {
                if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                    outpath = Application.dataPath + "/../Clent.outlog.txt";
                else
                    outpath = Application.dataPath + "/Clent.outlog.txt";
            }
            try
            {
                if (File.Exists(outpath))
                {
                    File.Delete(outpath);
                }
                writer = new StreamWriter(outpath, false, Encoding.UTF8);
                writer.WriteLine("\tinit file success!!" + Application.platform);
                writer.WriteLine("\tdataPath:" + Application.dataPath);
                writer.WriteLine("\tpersistentDataPath:" + Application.persistentDataPath);
                writer.WriteLine("\tstreamingAssetsPath:" + Application.streamingAssetsPath);
                Log.Debug("log file:" + outpath);
                writer.Flush();
                initFileSuccess = true;
            }
            catch (Exception e)
            {
                initFileSuccess = false;
                Application.logMessageReceived -= HandleLog;
                Log.Error("write outlog.txt error:" + e.Message);
            }
        }


        static public void Error(Exception e)
        {
            string logString;
            string stackTrace;

            if (e.Data.Contains("StackTrace"))
            {
                logString = "[ERROR EXCEPTION]";
                stackTrace = e.Data["StackTrace"] + "";

            } else {
                logString = "[ERROR]";
                stackTrace = e.ToString();
            }

            HandleLog(logString, stackTrace, LogType.Exception);
        }


        static private void HandleLog(string logString, string stackTrace, LogType type)
        {
            var needOutLog = initFileSuccess;
            int index = logString.IndexOf("stack traceback");
            if (index > 0 && needOutLog)
            {
                string log = logString.Substring(0, index);
                string trace = logString.Substring(index, logString.Length - index);
                logString = log;
                stackTrace = trace;
            }

            if (type == LogType.Log)
            {
                if (needOutLog)
                {
                    writeList.Add(logString);
                }
            }

            else if (type == LogType.Error)
            {
                if (logString.IndexOf("LUA ERROR") > 0 || logString.IndexOf("stack traceback") > 0) exceptCount++;
                else errorCount++;

                if (needOutLog)
                {
                    writeList.Add(logString);
                    writeList.Add(stackTrace + "\n");

                    if (errorList.Count >= MAX_ERROR_LOG)
                    {
                        errorList.RemoveAt(1);
                    }

                    if (errorList.Count < MAX_ERROR_LOG)
                    {
                        errorList.Add(logString);
                        errorList.Add(stackTrace + "\n");
                    }
                }
            }
            else if (type == LogType.Exception)
            {
                exceptCount++;
                if (needOutLog)
                {
                    writeList.Add(logString);
                    writeList.Add(stackTrace + "\n");

                    if (errorList.Count >= MAX_ERROR_LOG)
                    {
                        errorList.RemoveAt(1);
                    }
                    if (errorList.Count < MAX_ERROR_LOG)
                    {
                        errorList.Add(logString);
                        errorList.Add(stackTrace + "\n");
                    }
                }
            }
        }

    }


}
