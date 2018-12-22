using System;
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
            OpCode = OpCodeModule.User;
            OpCodeOperation = (byte)OpCodeUserOperation.Create;
        }

        public override void OnOperationRequest(BaseMsgData baseMsgData, SendParameters sendParameters, ClientPeer peer)
        {
            MyGameServer.log.Info("开始创建玩家信息。");
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
                MyGameServer.log.Info("玩家名称为空");
            }
            else if (createUserRequestMsgData.TeamName == null || createUserRequestMsgData.TeamName == string.Empty)
            {//队伍名称为空
                createUserRespondeMsgData.IsError = true;
                createUserRespondeMsgData.Error = ErrorCode.CreatePlayerError;
                MyGameServer.log.Info("队伍名称为空");
            }
            else if (createUserRequestMsgData.Actors == null || createUserRequestMsgData.Actors.Count != 3)
            {//角色数据错误
                createUserRespondeMsgData.IsError = true;
                createUserRespondeMsgData.Error = ErrorCode.CreatePlayerError;
                MyGameServer.log.Info("角色数据为空或是数量不为3");
            }
            else
            {              
                if (serverDataCenter.PlayerDal.Get(createUserRequestMsgData.PlayerName) != null)
                {//判断玩家重名
                    createUserRespondeMsgData.IsError = true;
                    createUserRespondeMsgData.Error = ErrorCode.CreatePlayerNameExit;
                    SendResponse(peer, sendParameters, createUserRespondeMsgData);
                    MyGameServer.log.Info("存在同名玩家");
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
                    //if (!serverDataCenter.StaticDataMgr.mCareerDataMap.ContainsKey((uint)createUserRequestMsgData.Actors[i].CareerID))
                    //{//判断职业是否存在
                    //    createUserRespondeMsgData.IsError = true;
                    //    createUserRespondeMsgData.Error = ErrorCode.CreateActorCareerNonExit;
                    //    SendResponse(peer, sendParameters, createUserRespondeMsgData);
                    //    return;
                    //}
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
                    actor.PlayerID = player.ID;
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
                createUserRespondeMsgData.IsError = false;
            }


            //发送消息
            SendResponse(peer, sendParameters, createUserRespondeMsgData);

            

            MyGameServer.log.Info("创建玩家信息完成。" + createUserRespondeMsgData.Error);
        }
    }
}
