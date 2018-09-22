using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Log
{


    /// <summary>
    ///  封装可以开关的Debug类
    /// </summary>
    public class TDebug
    {
        /// <summary>
        /// 是否启用Log
        /// </summary>
        static public bool enableLog = true;

        /// <summary>
        /// DebugLog
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="context"></param>
        static public void DebugLog(object message, Level level, UnityEngine.Object context = null)
        {
            switch (level)
            {
                case Level.High:
                    LogError(message, level, null);
                    break;
                case Level.Special:
                    LogWarning(message, level, null);
                    break;
                case Level.Low:
                    Log(message, level, null);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 输出信息到控制台
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        static private void Log(object message, Level level, UnityEngine.Object context = null)
        {
            if (enableLog)
                Debug.Log(Now() + " " + message);
        }
        /// <summary>
        /// 输出错误信息到控制台
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        static private void LogError(object message, Level level, UnityEngine.Object context = null)
        {
            if (enableLog)
                Debug.LogError(Now() + " " + message);

        }
        /// <summary>
        /// 输出警告信息到控制台
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        static private void LogWarning(object message, Level level, UnityEngine.Object context = null)
        {
            if (enableLog)
                Debug.LogWarning(Now() + " " + message);

        }

        static private string Now()
        {
            return System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff");
        }
    }
}