using System;
using System.Diagnostics;
using System.IO;
using System.Net;

#if NOT_UNITY
using NLog;
#endif

namespace ET
{
    public static class Log
    {
        public static ILog ILog { get; set; }
        
        private const int TraceLevel = 1;
        private const int DebugLevel = 2;       //  调试
        private const int InfoLevel = 3;        //  一般信息
        private const int ProtoLevel = 4;       //  协议
        private const int WarningLevel = 5;     //  警告
        private const int ErrorLevel = 6;       //  错误
        private const int VitalLevel = 7;       //  重要流程
        private const int FatalLevel = 8;       //  致命错误

        private static bool CheckLogLevel(int level)
        {
            return Options.Instance.LogLevel <= level;
        }
        
        public static void Trace(string msg)
        {
            if (!CheckLogLevel(TraceLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            ILog.Trace($"[TRACE]\t{msg}\n{st}");
        }

        public static void Debug(string msg)
        {
            if (!CheckLogLevel(DebugLevel))
            {
                return;
            }
            ILog.Debug($"[DEBUG]\t{msg}");
        }

        public static void Info(string msg)
        {
            if (!CheckLogLevel(InfoLevel))
            {
                return;
            }
            ILog.Info($"[INFO]\t{msg}");
        }

        public static void TraceInfo(string msg)
        {
            if (!CheckLogLevel(InfoLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            ILog.Trace($"[TRACE INFO]\t{msg}\n{st}");
        }

        public static void Warning(string msg)
        {
            if (!CheckLogLevel(WarningLevel))
            {
                return;
            }

            ILog.Warning($"[WARNING]\t{msg}");
        }

        public static void Proto(string msg)
        {
            if (!CheckLogLevel(ProtoLevel))
            {
                return;
            }
            ILog.Proto(string.Format($"[PROTO]\t{msg}"));
        }

        public static void Vital(string msg)
        {
            if (!CheckLogLevel(VitalLevel))
            {
                return;
            }
            ILog.Vital(string.Format($"[VITAL]\t{msg}"));
        }

        public static void Error(string msg)
        {
            if (!CheckLogLevel(ErrorLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            ILog.Error($"[ERROR]\t{msg}\n{st}");
        }

        public static void Fatal(string msg)
        {
            if (!CheckLogLevel(FatalLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            ILog.Fatal($"[FATAL]\t{msg}\n{st}");
        }

        public static void Error(Exception e)
        {
            if (!CheckLogLevel(ErrorLevel))
            {
                return;
            }
            if (e.Data.Contains("StackTrace"))
            {
                ILog.Error($"[ERROR EXCEPTION]\t{e.Data["StackTrace"]}\n{e}");
                return;
            }
            string str = e.ToString();
            ILog.Error($"[ERROR]\t{str}");
        }

        public static void Trace(string message, params object[] args)
        {
            if (!CheckLogLevel(TraceLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            ILog.Trace($"[TRACE]\t{string.Format(message, args)}\n{st}");
        }

        public static void Warning(string message, params object[] args)
        {
            if (!CheckLogLevel(WarningLevel))
            {
                return;
            }
            ILog.Warning($"[WARNING]\t{string.Format(message, args)}");
        }

        public static void Info(string message, params object[] args)
        {
            if (!CheckLogLevel(InfoLevel))
            {
                return;
            }
            ILog.Info($"[INFO]\t{string.Format(message, args)}");
        }

        public static void Debug(string message, params object[] args)
        {
            if (!CheckLogLevel(DebugLevel))
            {
                return;
            }
            ILog.Debug($"[DEBUG]\t{string.Format(message, args)}");

        }

        public static void Error(string message, params object[] args)
        {
            if (!CheckLogLevel(ErrorLevel))
            {
                return;
            }
            StackTrace st = new StackTrace(1, true);
            string s = string.Format(message, args) + '\n' + st;
            ILog.Error($"[ERROR]\t{s}");
        }

        public static void Vital(string message, params object[] args)
        {
            if (!CheckLogLevel(VitalLevel))
            {
                return;
            }
            ILog.Vital($"[Vital]\t{string.Format(message, args)}");
        }
        
        public static void Console(string message)
        {
            if (Options.Instance.Console == 1)
            {
                System.Console.WriteLine(message);
            }
            ILog.Debug($"[CONSOLE DEBUG]\t{message}");
        }
        
        public static void Console(string message, params object[] args)
        {
            string s = string.Format(message, args);
            if (Options.Instance.Console == 1)
            {
                System.Console.WriteLine(s);
            }
            ILog.Debug($"[CONSOLE DEBUG]\t{s}");
        }
    }
}