﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.OpCode
{
    /// <summary>
    /// 二级协议：用户模块
    /// </summary>
    public enum OpCodeUserOperation : byte
    {
        None,
        Register,
        Login,
        Create
    }

    public enum OpCodeUserEvent : byte
    {
        None,
        Register,
        Login
    }
}
