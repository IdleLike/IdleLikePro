using System;
using System.Collections;
using System.Collections.Generic;
using Log;
using Service;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    /// <summary>
    /// Log默认设置
    /// </summary>
    [Serializable]
    public struct LogSetting
    {
        public bool LogEnable;
        public Logtype LogType;
    }

    [SerializeField] private LogSetting m_LogSetting;

	private void Start () {
        //Log初始化
        TLog.LogEnable = m_LogSetting.LogEnable;
        TLog.DefaultType = m_LogSetting.LogType;

        //获取全局数据
        GameGlobal.LocalData = TestLocalData.Instance.LocalData;
        //初始化游戏服务
        GameService.Instance.Initialize();
        //登陆逻辑
       //GameService.Instance.Login();
	}	
}
