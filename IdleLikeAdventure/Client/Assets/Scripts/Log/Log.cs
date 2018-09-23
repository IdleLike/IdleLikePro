/*
 
*  Title:"地下守护神"项目开发
*  
*      核心层：日志调试系统(Log日志)
*      
*  Description:
*      
*      作用：更方便于软件(游戏)开发人员，调试系统程序
*      基本实现原理：
*            1：把开发人员在代码中定义的调试语句，写入本日志的缓存
*            2：当缓存中数量超过定义的最大写入文件数值，则把缓存内容调试语句一次性写入文本文件
*      
*  Date:2018
*  
*  Version:2.12
*  
*  Modify Recoder: 
* 
*/
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;
using System.Threading;  //多线程

namespace Log
{
    public static class TLog
    {
        public static Logtype DefaultType = Logtype.debug;
        public static bool LogEnable = true; 

        /// <summary>
        /// 使用默认类型输出Log
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="level">Level.</param>
        static public void LogInput(string message, Level level, bool isAlwaysInput)
        {
            LogInput(message, level, DefaultType, isAlwaysInput);
        }

        /// <summary>
        /// 输出LOG
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="level">Level.</param>
        /// <param name="logtype">Logtype.</param>
        static public void LogInput(string message,Level level,Logtype logtype, bool isAlwaysInput)
        {
            if(!isAlwaysInput && !LogEnable) return;

            switch (logtype)
            {
                case Logtype.debug:
                    TDebug.DebugLog(message, level);
                    break;
                case Logtype.writeFile:
                    Log.Write(message, level);
                    break;
                case Logtype.console:
                    TConsole.ConsoleLog(message, level);
                    break;
                default:
                    break;
            }
        }
    }
    public interface IConfigManager
    {
        /// <summary>
        /// 属性：应用设置
        /// </summary>
        Dictionary<string, string> AppSetting { get; }

        /// <summary>
        /// 得到AppSetting的最大数量
        /// </summary>
        int GetAppSettingMaxNumber();


    }
    public static class Log
    {
        #region 日志核心字段

        /// <summary>
        /// Log日志的缓存数组
        /// </summary>
        private static List<string>_LiLogArray;
        /// <summary>
        /// Log日志文件路径
        /// </summary>
        private static string _LogPath = null;
        /// <summary>
        /// Log日志状态(部署模式)
        /// </summary>
        private static State _LogState;
        /// <summary>
        /// Log日志最大容量
        /// </summary>
        private static int _LogMaxCapacity;
        /// <summary>
        /// Log日志缓存最大容量
        /// </summary>
        private static int _LogBufferMaxNumber;


        #endregion

        #region 日志文件常量定义

        /// <summary>
        /// 日志文件路径
        /// </summary>
        private const string XML_CONFIG_LOG_PATH = "LogPath";
        /// <summary>
        /// 日志状态(部署模式)
        /// </summary>
        private const string XML_CONFIG_LOG_STATE = "LogState";
        /// <summary>
        /// 日志最大容量
        /// </summary>
        private const string XML_CONFIG_LOG_MAX_CAPACITY = "LogMaxCapacity";
        /// <summary>
        /// 日志缓存最大容量
        /// </summary>
        private const string XML_CONFIG_LOG_BUFFER_NUMBER = "LogBufferNumber";
        
        /// <summary>
        /// 日志状态开发模式
        /// </summary>
        private const string XML_CONFIG_LOG_STATE_DEVELOP = "Develop";
        /// <summary>
        /// 日志状态指定代码输出模式
        /// </summary>
        private const string XML_CONFIG_LOG_STATE_SPEACIAL = "Speacial";
        /// <summary>
        /// 日志状态部署模式
        /// </summary>
        private const string XML_CONFIG_LOG_STATE_DEPLOY = "Deploy";
        /// <summary>
        /// 日志状态停止模式
        /// </summary>
        private const string XML_CONFIG_LOG_STATE_STOP = "Stop";

        /// <summary>
        /// 日志默认路径
        /// </summary>
        private const string XML_CONFIG_LOG_DEFAULT_PATH = "Log.txt";

        /// <summary>
        /// 日志默认最大容量
        /// </summary>
        private const int LOG_DEFAULT_MAX_CACITY_NUMBER = 2000;
        /// <summary>
        /// 日志缓存默认最大容量
        /// </summary>
        private const int LOG_DEFAULT_MAX_LOG_BUFFER_NUMBER = 1;
        /// <summary>
        /// 日志提示信息
        /// </summary>
        private const string LOG_TIPS = "@@@  Important  !!!  ";

        #endregion

        #region 临时变量

        /// <summary>
        /// Log日志状态(部署模式)
        /// </summary>
        private static string strLogState = null;
        /// <summary>
        /// Log日志最大容量
        /// </summary>
        private static string strLogMaxCapacity = null;
        /// <summary>
        /// Log日志缓存最大容量
        /// </summary>
        private static string strLogBufferNumber = null;

        #endregion

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Log()
        {
            //日志缓存数据
            _LiLogArray = new List<string>();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            //日志文件路径
            IConfigManager configMgr = new ConfigManager(KernalParameter.GetLogPath(), KernalParameter.GetLogRootNodeName());
            _LogPath =  configMgr.AppSetting[XML_CONFIG_LOG_PATH];

            //日志状态(部署模式)
            strLogState = configMgr.AppSetting[XML_CONFIG_LOG_STATE];

            //日志最大容量
            strLogMaxCapacity = configMgr.AppSetting[XML_CONFIG_LOG_MAX_CAPACITY];

            //日志缓存最大容量
            strLogBufferNumber = configMgr.AppSetting[XML_CONFIG_LOG_BUFFER_NUMBER];
            //日志文件路径
            if (string.IsNullOrEmpty(_LogPath))
            {
                _LogPath = KernalParameter.GetDungesonFighterLogPath() + XML_CONFIG_LOG_DEFAULT_PATH;
            TDebug.DebugLog(_LogPath,Level.Low);
            }
            //创建文件
            if (!File.Exists(_LogPath))//不存在指定路径的文件
            {
                //没有则创建
                File.Create(_LogPath);
                //关闭当前线程
                Thread.CurrentThread.Abort();
            }
#endif


            //日志状态(部署模式)
            if (!string.IsNullOrEmpty(strLogState))
            {
                switch (strLogState)
                {
                    case XML_CONFIG_LOG_STATE_DEVELOP:
                        _LogState = State.Develop;
                        break;
                    case XML_CONFIG_LOG_STATE_SPEACIAL:
                        _LogState = State.Speacial;
                        break;
                    case XML_CONFIG_LOG_STATE_DEPLOY:
                        _LogState = State.Deploy;
                        break;
                    case XML_CONFIG_LOG_STATE_STOP:
                        _LogState = State.Stop;
                        break;
                    default:
                        _LogState = State.Stop;
                        break;
                }
            }
            else
            {
                _LogState = State.Stop;
            }

            //日志最大容量            
            if (!string.IsNullOrEmpty(strLogMaxCapacity))
            {
                _LogMaxCapacity = Convert.ToInt32(strLogMaxCapacity);
            }
            else
            {
                _LogMaxCapacity = LOG_DEFAULT_MAX_CACITY_NUMBER;
            }

            //日志缓存最大容量
            if (!string.IsNullOrEmpty(strLogBufferNumber))
            {
                _LogBufferMaxNumber = Convert.ToInt32(strLogBufferNumber);
            }
            else
            {
                _LogBufferMaxNumber = LOG_DEFAULT_MAX_LOG_BUFFER_NUMBER;
            }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR

            ////创建文件
            //if (!File.Exists(_LogPath))//不存在指定路径的文件
            //{
            //    //没有则创建
            //    File.Create(_LogPath);
            //    //关闭当前线程
            //    Thread.CurrentThread.Abort();
            //}

#endif
           
            //把日志文件中的数据同步到日志缓存中
            SyncFileDataToLogArray();
        }

        /// <summary>
        /// 把日志文件中的数据同步到日志缓存中
        /// </summary>
        private static void SyncFileDataToLogArray()
        {
            TDebug.DebugLog("把日志文件中的数据同步到日志缓存中", Level.Low);
            if (!string.IsNullOrEmpty(_LogPath))
            {
                //读文件
                StreamReader sr = new StreamReader(_LogPath);
                TDebug.DebugLog("读取文件", Level.Low);
                while (sr.Peek() >= 0)
                {
                    TDebug.DebugLog("添加数据", Level.Low);
                    _LiLogArray.Add(sr.ReadLine());
                }
                sr.Close();
            }
        }

        /// <summary>
        /// 写数据到文件中
        /// </summary>
        /// <param name="writeFileDate">写入的调试信息</param>
        /// <param name="level">重要等级级别</param>
        public static void Write(string writeFileDate,Level level)
        {
            //AppendDateToFile(writeFileDate);
            TDebug.DebugLog("写数据到文件中", Level.Low);
            //参数检查
            if (_LogState == State.Stop)
            {
                return;
            }

            //如果日志缓存数量超过指定容量，则清空
            if (_LiLogArray.Count >= _LogMaxCapacity)
            {
                //清空缓存的数据
                _LiLogArray.Clear();
            }

            if (!string.IsNullOrEmpty(writeFileDate))
            {
                //增加日期与时间
                writeFileDate = XML_CONFIG_LOG_STATE + "：" + _LogState.ToString() + "/" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff") + "/" + writeFileDate;

                //对于不同的"日志状态"，分特定情形写入文件
                if (level == Level.High)
                {
                    writeFileDate = LOG_TIPS + writeFileDate;
                }
                switch (_LogState)
                {
                    case State.Develop://开发状态
                        //追加调试信息，写入文件
                        AppendDateToFile(writeFileDate);
                        break;
                    case State.Speacial://指定代码输出模式
                        if (level == Level.High || level == Level.Special)
                        {
                            AppendDateToFile(writeFileDate);
                        }
                        break;
                    case State.Deploy://部署模式
                        if (level == Level.High)
                        {
                            AppendDateToFile(writeFileDate);
                        }
                        break;
                    case State.Stop://停止模式
                        break;
                    default:
                        break;
                }
         

            }
           
        }

        /// <summary>
        /// 写数据到文件中(重载：默认重要等级 = 低级)
        /// </summary>
        /// <param name="writeFileDate"></param>
        public static void Write(string writeFileDate)
        {
            Write(writeFileDate, Level.Special);
        }


        /// <summary>
        /// 追加数据到文件
        /// </summary>
        /// <param name="writeFileDate">调试信息</param>
        private static void AppendDateToFile(string writeFileDate)
        {
            TDebug.DebugLog("追加数据到文件", Level.Low);

            //参数检查
            if (!string.IsNullOrEmpty(writeFileDate))
            {
                //调试信息数据追加到缓存集合中
                _LiLogArray.Add(writeFileDate);
            }

            //缓存集合数量超过一定指定数量("_LogBufferMaxNumber")，则同步到实体文件中
            if (_LiLogArray.Count % _LogBufferMaxNumber == 0)
            {
                //同步缓存中的数据信息到实体文件中
                SyncLogArrayToFile();
            }
        }



        #region 重要管理方法

        /// <summary>
        /// 查询日志缓存中所有数据
        /// </summary>
        /// <returns></returns>
        public static List<string>QueryAllDateFromLogBuffer()
        {
            if (_LiLogArray != null)
            {
                return _LiLogArray;
            }
            return null;
        }

        /// <summary>
        /// 清楚实体日志文件与日志缓存中所有数据
        /// </summary>
        public static void ClearLogFileAndBufferAllDate()
        {
            if (_LiLogArray != null)
            {
                //数据全部清空
                _LiLogArray.Clear();
            }
            //同步缓存中的数据信息到实体文件中
            SyncLogArrayToFile();
        }

        /// <summary>
        /// 同步缓存中的数据信息到实体文件中
        /// </summary>
        public static void SyncLogArrayToFile()
        {
            if (!string.IsNullOrEmpty(_LogPath))
            {
                StreamWriter sw = new StreamWriter(_LogPath);
                foreach (string item in _LiLogArray)
                {
                    sw.WriteLine(item);
                }
                sw.Close();
            }
        }

        #endregion


        /// <summary>
        /// 日志状态(部署模式)
        /// </summary>
        public enum State
        {
            Develop,    //开发模式
            Speacial,   //指定代码输出模式
            Deploy,      //部署模式
            Stop          //停止模式
        }


    }
    /// <summary>
    /// 调试信息的等级(表示调试信息本身的重要程度)
    /// </summary>
    public enum Level
    {
        High,
        Special,
        Low
    }
    public enum Logtype
    {
        debug,
        writeFile,
        console
    }
}

