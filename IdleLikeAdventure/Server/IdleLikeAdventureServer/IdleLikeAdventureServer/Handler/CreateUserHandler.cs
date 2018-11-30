using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdleLikeAdventureServer.Data.Entity;
using NetData.Message;
using NetData.OpCode;
using Photon.SocketServer;

namespace IdleLikeAdventureServer.Handler
{
    public class CreateUserHandler : BaseHandler
    {
        public CreateUserHandler()
        {
            OpCode = OpCodeModule.Actor;
            OpCodeOperation = (byte)OpCodeActorOperation.CreateUser;
        }

        public override void OnOperationRequest(BaseMsgData baseMsgData, SendParameters sendParameters, ClientPeer peer)
        {
            //转型消息
            CreateUserRequestAndRespondeMsgData createUserRequestMsgData = baseMsgData as CreateUserRequestAndRespondeMsgData;
            if (createUserRequestMsgData == null)
            {
                MyGameServer.log.Error("没有获得登陆消息！");
                return;
            }

            CreateUserRequestAndRespondeMsgData createUserRespondeMsgData = new CreateUserRequestAndRespondeMsgData();
            //参数检测
            if (createUserRequestMsgData.PlayerName == null || createUserRequestMsgData.PlayerName == string.Empty)
            {//用户名称为空
                createUserRespondeMsgData.IsError = true;
                createUserRespondeMsgData.Error = ErrorCode.CreatePlayerError;
            }
            else if (createUserRequestMsgData.TeamName == null || createUserRequestMsgData.TeamName == string.Empty)
            {//队伍名称为空
                createUserRespondeMsgData.IsError = true;
                createUserRespondeMsgData.Error = ErrorCode.CreatePlayerError;
            }
            else if (createUserRequestMsgData.Actors == null || createUserRequestMsgData.Actors.Count != 3)
            {//角色数据错误
                createUserRespondeMsgData.IsError = true;
                createUserRespondeMsgData.Error = ErrorCode.CreatePlayerError;
            }
            else
            {              
                if (serverDataCenter.PlayerDal.Get(createUserRequestMsgData.PlayerName) != null)
                {//判断玩家重名
                    createUserRespondeMsgData.IsError = true;
                    createUserRespondeMsgData.Error = ErrorCode.CreatePlayerNameExit;
                    SendResponse(peer, sendParameters, createUserRespondeMsgData);
                    return;
                }

                
                for (int i = 0; i < createUserRequestMsgData.Actors.Count; i++)
                {
                    if(!serverDataCenter.StaticDataMgr.mRaceDataMap.ContainsKey((uint)createUserRequestMsgData.Actors[i].RaceID))
                    {//判断种族是否存在
                        createUserRespondeMsgData.IsError = true;
                        createUserRespondeMsgData.Error = ErrorCode.CreateActorRaceIDNonExit;
                        SendResponse(peer, sendParameters, createUserRespondeMsgData);
                        return;
                    }
                    if (!serverDataCenter.StaticDataMgr.mCareerDataMap.ContainsKey((uint)createUserRequestMsgData.Actors[i].CareerID))
                    {//判断职业是否存在
                        createUserRespondeMsgData.IsError = true;
                        createUserRespondeMsgData.Error = ErrorCode.CreateActorCareerNonExit;
                        SendResponse(peer, sendParameters, createUserRespondeMsgData);
                        return;
                    }
                }
                Player player = new Player();
                player.AccountID = createUserRequestMsgData.AccountID;
                player.CreateDate = DateTime.Now;
                player.Name = createUserRequestMsgData.PlayerName;
                

                //创建数据

                //玩家数据

                //角色数据
                //队伍数据

                //存储消息

                //初始化网络消息

            }


            //发送消息
            SendResponse(peer, sendParameters, createUserRespondeMsgData);
        }
    }
}
