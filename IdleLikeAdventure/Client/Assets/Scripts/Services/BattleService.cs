using UnityEngine;
using System.Collections;
using NetData.OpCode;

namespace Service
{
    public class BattleService : BaseService<NetData.OpCode.OpCodeBattleOperation, NetData.OpCode.OpCodeBattleEvent>
    {
        protected override OpCodeModule ServiceOpCode
        {
            get
            {
                return OpCodeModule.Battle;
            }
        }

        public override void Init()
        {

        }
    }
}

