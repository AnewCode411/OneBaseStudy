#if !NOT_UNITY
using System;
using System.Globalization;

namespace ET
{
    public class UnityLogger: ILog
    {
        public void Trace(string msg)
        {
            UnityEngine.Debug.Log(GetCurTime() + msg);
        }

        public void Debug(string msg)
        {
            UnityEngine.Debug.Log(GetCurTime() + msg);
        }

        public void Info(string msg)
        {
            UnityEngine.Debug.Log(GetCurTime() + msg);
        }

        public void Warning(string msg)
        {
            UnityEngine.Debug.LogWarning(GetCurTime() + msg);
        }

        public void Error(string msg)
        {
            UnityEngine.Debug.LogError(GetCurTime() + msg);
        }

        public void Error(Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }

        public void Fatal(string message)
        {
            UnityEngine.Debug.LogError(GetCurTime() + message);
        }

        public void Proto(string message)
        {
            UnityEngine.Debug.Log(GetCurTime() + message);
        }

        public void Vital(string message)
        {
            UnityEngine.Debug.Log(GetCurTime() + message);
        }

        public void Trace(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(GetCurTime() + message, args);
        }

        public void Warning(string message, params object[] args)
        {
            UnityEngine.Debug.LogWarningFormat(GetCurTime() + message, args);
        }

        public void Info(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(GetCurTime() + message, args);
        }

        public void Debug(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(GetCurTime() + message, args);
        }

        public void Error(string message, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(GetCurTime() + message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            UnityEngine.Debug.LogErrorFormat(GetCurTime() + message, args);
        }

         public void Proto(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(GetCurTime() + message, args);
        }

         public void Vital(string message, params object[] args)
        {
            UnityEngine.Debug.LogFormat(GetCurTime() + message, args);
        }

        public static string GetCurTime(bool all = false)
        {
            DateTime now = DateTime.Now;
            if (all) return now.ToString("[MM-dd HH:mm:ss fff]", DateTimeFormatInfo.InvariantInfo);
            else return now.ToString("[HH:mm:ss fff]", DateTimeFormatInfo.InvariantInfo);
        }

        private static ulong GetTimestamp()
        {
            return (ulong)(DateTime.Now - new DateTime(190, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        }
    }
}
#endif