using System.Collections;
using System.Collections.Generic;
using Service;
using UnityEngine;

public class GameStart : MonoBehaviour {

	// Use this for initialization
	private void Start () {
        //获取全局数据
        GameGlobal.LocalData = TestLocalData.Instance.LocalData;
        //初始化游戏服务
        GameService.Instance.Initialize();
        //登陆逻辑
        GameService.Instance.Login();
	}	
}
