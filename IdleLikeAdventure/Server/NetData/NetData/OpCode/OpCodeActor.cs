using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.OpCode
{
    /// <summary>
    /// 二级协议：角色模块
    /// </summary>
    public enum OpCodeActorOperation : byte
    {
        None,               //无
        CreateUser,         //登陆第一次创建
        Create,             //创建角色
    }

    public enum OpCodeActorEvent : byte
    {
    }
}
