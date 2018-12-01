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
                //玩家信息
                Player player = new Player();
                player.AccountID = createUserRequestMsgData.AccountID;
                player.CreateDate = DateTime.Now;
                player.Name = createUserRequestMsgData.PlayerName;
                player.ServerID = createUserRequestMsgData.ServerID;
                player.UpdateDate = DateTime.Now;

                player.ID = serverDataCenter.PlayerDal.Insert(player);

                createUserRespondeMsgData.AccountID = player.AccountID;
                createUserRespondeMsgData.PlayerName = player.Name;
                createUserRespondeMsgData.ServerID = player.ServerID;
                

                Team team = new Team();
                team.Name = createUserRequestMsgData.TeamName;
                team.PlayerID = player.ID;
                team.CreateDate = DateTime.Now;
                team.UpdateDate = DateTime.Now;
                team.ActorIDs = string.Empty;

                //角色数据
                createUserRespondeMsgData.Actors = createUserRequestMsgData.Actors;
                for (int i = 0; i < createUserRequestMsgData.Actors.Count; i++)
                {
                    ActorMsgData msgData = createUserRequestMsgData.Actors[i];
                    Actor actor = new Actor();
                    actor.CareerID = GameConst.CAREER_DEFAULT_ID;
                    actor.CareerLevel = 1;
                    actor.CareerPoint = 0;
                    actor.CreateDate = DateTime.Now;
                    actor.RaceID = msgData.RaceID;
                    actor.TotalExp = 0;
                    actor.Name = msgData.Name;
                    actor.UpdateDate = DateTime.Now;

                    actor.ID = serverDataCenter.ActorDal.Insert(actor);
                    msgData.DataBaseID = actor.ID;
                    msgData.CareerID = actor.CareerID;
                    msgData.CareerLevel = actor.CareerLevel;
                    msgData.CareerPoint = actor.CareerPoint;
                    msgData.TotalExp = actor.TotalExp;
                    msgData.CreateTime = actor.CreateDate;
                    msgData.UpdateTime = actor.UpdateDate;
                    

                    if (i != 0) team.ActorIDs += "|";
                    team.ActorIDs += actor.ID;
                }

                createUserRespondeMsgData.TeamID = serverDataCenter.TeamDal.Insert(team);
                createUserRespondeMsgData.TeamName = team.Name;
            }


            //发送消息
            SendResponse(peer, sendParameters, createUserRespondeMsgData);
        }
    }
}
