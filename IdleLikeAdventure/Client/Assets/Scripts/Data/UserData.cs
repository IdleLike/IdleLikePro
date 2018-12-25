using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetData;
using NetData.Message;
using System;

public class UserData
{
    public int TeamID;
    public int ServerID;
    public int PlayerID;
    public int AccountID;
    public string UserName;
    public string TeamName;
    public string PlayerName;

    public DateTime CreateTime;
    public PlayerMsgData PlayerData;
    public List<TeamMsgData> TeamDataList;
    public List<ActorMsgData> ActorDataList;
}
