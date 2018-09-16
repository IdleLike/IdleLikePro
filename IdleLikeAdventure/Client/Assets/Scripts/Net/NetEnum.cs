using System;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;

namespace Net.NetEnum
{
    /// <summary>
    /// 用来区分消息对应的模块
    /// </summary>
    public enum OpCodeModule : byte
    {
        User,
        Actor,
        Battle
    }

    public enum OpCodeUserOperation : byte
    {
        Login,
        Create
    }


    public enum ClientState : byte
    {
        DisConnect,
        Connect,
        LoginSuccess,
        LoginFailed
    }
}

