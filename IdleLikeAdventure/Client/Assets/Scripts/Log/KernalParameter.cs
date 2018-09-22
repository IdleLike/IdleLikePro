/*
 
*  Title:"地下守护神"项目开发
*  
*      核心层：核心层的参数列表
*      
*  Description:
*      
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
using UnityEngine;

namespace Log
{
    public class KernalParameter 
    {

        //#if UNITY_STANDALONE_WIN

        //        /// <summary>
        //        /// 系统配置信息_日志路径
        //        /// </summary>
        //        internal static readonly string SystemConfigInfo_LogPath = "file://" + Application.dataPath + "/StreamingAssets/SystemConfigInfo.xml";
        //        /// <summary>
        //        /// 系统配置信息_日志根节点名称
        //        /// </summary>
        //        internal static readonly string SystemConfigInfo_LogRootNodeName = "SystemConfigInfo";

        //        /// <summary>
        //        /// 系统配置信息_XML路径
        //        /// </summary>
        //        internal static readonly string DialogsXMLConfigInfo_XmlPath = "file://" + Application.dataPath + "/StreamingAssets/SystemDialogsInfo.xml";
        //        /// <summary>
        //        /// 系统配置信息_XML根节点名称
        //        /// </summary>
        //        internal static readonly string DialogsXMLConfigInfo_XmlRootNodeName = "Dialogs_CN";

        //#elif UNITY_ANDROID

        //        /// <summary>
        //        /// 系统配置信息_日志路径
        //        /// </summary>
        //        internal static readonly string SystemConfigInfo_LogPath = Application.dataPath + "!/Assets/SystemConfigInfo.xml";
        //        /// <summary>
        //        /// 系统配置信息_日志根节点名称
        //        /// </summary>
        //        internal static readonly string SystemConfigInfo_LogRootNodeName = "SystemConfigInfo";

        //        /// <summary>
        //        /// 系统配置信息_XML路径
        //        /// </summary>
        //        internal static readonly string DialogsXMLConfigInfo_XmlPath = Application.dataPath + "!/Assets/SystemDialogsInfo.xml";
        //        /// <summary>
        //        /// 系统配置信息_XML根节点名称
        //        /// </summary>
        //        internal static readonly string DialogsXMLConfigInfo_XmlRootNodeName = "Dialogs_CN";

        //#elif UNITY_IPHONE

        //        /// <summary>
        //        /// 系统配置信息_日志路径
        //        /// </summary>
        //        internal static readonly string SystemConfigInfo_LogPath = Application.dataPath + "/Raw/SystemConfigInfo.xml";
        //        /// <summary>
        //        /// 系统配置信息_日志根节点名称
        //        /// </summary>
        //        internal static readonly string SystemConfigInfo_LogRootNodeName = "SystemConfigInfo";

        //        /// <summary>
        //        /// 系统配置信息_XML路径
        //        /// </summary>
        //        internal static readonly string DialogsXMLConfigInfo_XmlPath = Application.dataPath + "/Raw/SystemDialogsInfo.xml";
        //        /// <summary>
        //        /// 系统配置信息_XML根节点名称
        //        /// </summary>
        //        internal static readonly string DialogsXMLConfigInfo_XmlRootNodeName = "Dialogs_CN";

        //#endif



        /// <summary>
        /// 得到日志路径
        /// </summary>
        /// <returns></returns>
        public static string GetLogPath()
        {
            string logPath = null;

            //Android 或者 Iphone 环境
            if (Application.platform == RuntimePlatform.Android)
            {
                logPath = "jar:file://" + Application.dataPath + "!/assets/SystemConfigInfo.xml";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                logPath = Application.dataPath + "/Raw/SystemConfigInfo.xml";
            }
            //Win环境
            else
            {
                logPath = "file://" + Application.streamingAssetsPath + "/SystemConfigInfo.xml";
            }
            return logPath;
        }

        /// <summary>
        /// 得到默认日志路径
        /// </summary>
        /// <returns></returns>
        public static string GetDungesonFighterLogPath()
        {
            string logPath = null;

            //Android 或者 Iphone 环境
            if (Application.platform == RuntimePlatform.Android)
            {
                logPath = "jar:file://" + Application.dataPath + "!/assets/Resources/Txt/";
            }
            else if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                logPath = Application.dataPath + "/Raw/Resources/Txt/";
            }
            //Win环境
            else
            {
                logPath = /*"file://" +*/ Application.dataPath + "/Resources/Txt/";
                Debug.Log("Win");
            }
            return logPath;
        }

        /// <summary>
        /// 得到日志根节点名称
        /// </summary>
        /// <returns></returns>
        public static string GetLogRootNodeName()
        {
            string strReturnXMLRootNodeName = null;

            strReturnXMLRootNodeName = "SystemConfigInfo";
            return strReturnXMLRootNodeName;
        }

        /// <summary>
        /// 得到对话配置XML路径
        /// </summary>
        /// <returns></returns>
        public static string GetDialogConfigXMLPath()
        {
            string dialogConfigXMLPath = null;

            //Android 或者 Iphone 环境
            if (Application.platform == RuntimePlatform.Android)
            {
                dialogConfigXMLPath = "jar:file://" + Application.dataPath + "!/assets/SystemDialogsInfo.xml";
            }
            else if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                dialogConfigXMLPath = Application.dataPath + "/Raw/SystemDialogsInfo.xml";
            }
            //Win环境
            else
            {
                dialogConfigXMLPath = "file://" + Application.streamingAssetsPath + "/SystemDialogsInfo.xml";
            }
            return dialogConfigXMLPath;
        }

        /// <summary>
        /// 得到对话XML根节点名称
        /// </summary>
        /// <returns></returns>
        public static string GetDialogConfigXMLRootNodeName()
        {
            string strReturnXMLRootNodeName = null;

            strReturnXMLRootNodeName = "Dialogs_CN";
            return strReturnXMLRootNodeName;
        }


    }

}
