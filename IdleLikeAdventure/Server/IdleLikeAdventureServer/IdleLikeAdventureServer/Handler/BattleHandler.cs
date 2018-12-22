using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetData.Enumeration;
using NetData.Message;
using NetData.OpCode;
using Photon.SocketServer;

namespace IdleLikeAdventureServer.Handler
{
    public class BattleHandler : BaseHandler
    {
        public BattleHandler()
        {
            OpCode = OpCodeModule.Battle;
            OpCodeOperation = (byte)OpCodeBattleOperation.BattleRequest;
        }

        public override void OnOperationRequest(BaseMsgData baseMsgData, SendParameters sendParameters, ClientPeer peer)
        {
            MyGameServer.log.Info("开始处理战斗请求");
            BattleRequestMsgData battleRequestMsgData = baseMsgData as BattleRequestMsgData;


            BattleRespondeMsgData battleRespondeMsgData = new BattleRespondeMsgData();

            battleRespondeMsgData.IsError = false;
            battleRespondeMsgData.FindEnemyTime = 15;
            BattleMsgData battleMsgData = new BattleMsgData();
            battleMsgData.TeamID = battleRequestMsgData.TeamID;
            battleMsgData.EnemyID = new List<uint>() {1001,1002,1003};
            battleMsgData.Exps = new List<int>() { 100, 200, 500 };
            battleMsgData.GoldRewards = 1000;
            battleMsgData.IsWin = true;
            battleMsgData.ItemRewards = new List<uint>() { 1001, 1002 };
            battleMsgData.RestTime = 10;
            battleMsgData.Rounds = new List<RoundInfoMsgData>();
            for (int i = 0; i < 10; i++)
            {
                RoundInfoMsgData roundInfoMsgData = new RoundInfoMsgData();

                roundInfoMsgData.AttackInfos = new List<AttackInfoMsgData>();

                for (byte j = 0; j < 6; j++)
                {
                    AttackInfoMsgData attackInfoMsgData = new AttackInfoMsgData();

                    attackInfoMsgData.AttakPos = j;
                    attackInfoMsgData.DefendsPos = j % 2 == 0 ? (byte)(j + 1) : (byte)(j - 1);
                    attackInfoMsgData.EffectType = EnumAbilityEffect.Damage;
                    attackInfoMsgData.AbilityID = 0;
                    attackInfoMsgData.AbiilityEffectValue = 10;
                }

                battleMsgData.Rounds.Add(roundInfoMsgData);
            }
            battleRespondeMsgData.BattleInfo = battleMsgData;
            SendResponse(peer, sendParameters, battleRespondeMsgData);
            MyGameServer.log.Info("成功发送战斗请求");
        }
    }
}
