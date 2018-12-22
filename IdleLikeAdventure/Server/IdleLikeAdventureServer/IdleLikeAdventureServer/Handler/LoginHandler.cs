using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdleLikeAdventureServer.Data.Entity;
using NetData.Message;
using NetData.OpCode;
using Photon.SocketServer;

namespace IdleLikeAdventureServer.Handler
{
    public class LoginHandler : BaseHandler
    {
        public LoginHandler()
        {
            OpCode = OpCodeModule.User;
            OpCodeOperation = (byte)OpCodeUserOperation.Login;         
        }

        public override void OnOperationRequest(BaseMsgData baseMsgData, SendParameters sendParameters, ClientPeer peer)
        {
            //转换消息类
            LoginRequstMsgData loginRequstMsgData = baseMsgData as LoginRequstMsgData;
            if (loginRequstMsgData == null)
            {
                MyGameServer.log.Error("没有注册消息数据，客户端：" + peer.LocalIP);
                return;
            }
            //参数检查
            LoginRespondeMsgData loginRespondeMsgData = new LoginRespondeMsgData();
            loginRespondeMsgData.IsError = true;
            if (loginRequstMsgData.Account == null || loginRequstMsgData.Account == string.Empty)
            {//账户为空
                loginRespondeMsgData.Error = ErrorCode.LoginAccountError;
                MyGameServer.log.Info("登陆失败，账户名称为空");
            }
            else if (loginRequstMsgData.Password == null || loginRequstMsgData.Password == string.Empty)
            {//密码为空
                loginRespondeMsgData.Error = ErrorCode.LoginPasswordError;
                MyGameServer.log.Info("登陆失败，密码为空");
            }
            else
            {
                Account account = null;
                account = serverDataCenter.AccountDal.Get(loginRequstMsgData.Account);
                if (account == null)
                {//没有该账号
                    loginRespondeMsgData.Error = ErrorCode.LoginAccountError;
                    MyGameServer.log.Info("账号没有注册：" + loginRequstMsgData.Account);
                }
                else if (!account.Password.Equals(loginRequstMsgData.Password))
                {//密码错误
                    loginRespondeMsgData.Error = ErrorCode.LoginPasswordError;
                    MyGameServer.log.Info("登陆密码错误，账号：" + loginRequstMsgData.Account);
                }
                else
                {//登陆成功
                    loginRespondeMsgData.IsError = false;
                    loginRespondeMsgData.AccountID = account.ID;
                    //TODO 追加用户游戏数据

                    Player player = serverDataCenter.PlayerDal.Get(account.ID);
                    if (player == null)
                    {//还没创建过角色
                        loginRespondeMsgData.IsNewPlayer = true;
                    }
                    else
                    {//获取其它信息
                        loginRespondeMsgData.IsNewPlayer = false;
                        //获取玩家信息
                        PlayerMsgData playerMsgData = new PlayerMsgData();
                        playerMsgData.DatabaseID = player.ID;
                        playerMsgData.Name = player.Name;
                        playerMsgData.ServerID = player.ServerID;
                        loginRespondeMsgData.Player = playerMsgData;
                        //获取角色信息
                        IList<Actor> actors = serverDataCenter.ActorDal.GetAllPlayer(player.ID);
                        if (actors != null)
                        {
                            List<ActorMsgData> actorMsgs = new List<ActorMsgData>(actors.Count);
                            for (int i = 0; i < actors.Count; i++)
                            {
                                ActorMsgData actorMsgData = new ActorMsgData();
                                actorMsgData.CareerID = actors[i].CareerID;
                                actorMsgData.CareerLevel = actors[i].CareerLevel;
                                actorMsgData.CareerPoint = actors[i].CareerPoint;
                                actorMsgData.CreateTime = actors[i].CreateDate;
                                actorMsgData.DataBaseID = actors[i].ID;
                                actorMsgData.Name = actors[i].Name;
                                actorMsgData.RaceID = actors[i].RaceID;
                                actorMsgData.TotalExp = actors[i].TotalExp;

                                actorMsgs.Add(actorMsgData);
                            }
                            loginRespondeMsgData.Actors = actorMsgs;

                            IList<Team> teams = serverDataCenter.TeamDal.GetAllPlayer(player.ID);
                            if (teams != null)
                            {
                                List<TeamMsgData> teamMsgDatas = new List<TeamMsgData>();
                                for (int i = 0; i < teams.Count; i++)
                                {
                                    TeamMsgData teamMsgData = new TeamMsgData();
                                    
                                    string[] actorIds = teams[i].ActorIDs.Split('|');
                                    teamMsgData.ActorIDs = new List<int>(actorIds.Length);
                                    for (int j = 0; j < actorIds.Length; j++)
                                    {
                                        teamMsgData.ActorIDs.Add(Convert.ToInt32(actorIds[j]));                             
                                    }
                                    teamMsgData.DatabaseID = teams[i].ID;
                                    teamMsgData.Name = teams[i].Name;

                                    teamMsgDatas.Add(teamMsgData);
                                }
                                loginRespondeMsgData.Teams = teamMsgDatas;
                            }
                            else
                            {
                                MyGameServer.log.Info("玩家没有队伍， ID：" + player.ID + " 名称：" + player.Name);
                            }                         
                        }
                        else
                        {
                            MyGameServer.log.Info("玩家没有英雄， 玩家名称：" + player.Name);
                        }
                        //获取队伍信息
                    }

                    MyGameServer.log.Info("登陆成功，ID：" + account.ID);
                }
            }

            //发送消息
            SendResponse(peer, sendParameters, loginRespondeMsgData);
        }
    }
}
