using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.OpCode
{
    /// <summary>
    /// 二级协议：战斗模块
    /// </summary>
    public enum OpCodeBattleOperation : byte
    {
        None,
        BattleRequest
    }

    public enum OpCodeBattleEvent : byte
    {

    }
}
