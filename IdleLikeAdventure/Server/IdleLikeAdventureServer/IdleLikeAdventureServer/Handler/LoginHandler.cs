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
                    UserMsgData userMsgData = new UserMsgData();
                    userMsgData.Name = account.Name;
                    userMsgData.CreateTime = account.CreateDate;
                    userMsgData.DatabaseID = account.ID;
                    loginRespondeMsgData.userData = userMsgData;

                    //TODO 追加用户游戏数据

                    MyGameServer.log.Info("登陆成功，ID：" + account.ID);
                }
            }

            //发送消息
            SendResponse(peer, sendParameters, loginRespondeMsgData);
        }
    }
}
