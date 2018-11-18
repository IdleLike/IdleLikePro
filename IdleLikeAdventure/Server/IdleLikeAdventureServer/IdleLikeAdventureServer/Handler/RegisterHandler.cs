using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using IdleLikeAdventureServer.Data.Entity;
using NetData.Message;
using NetData.OpCode;
using Photon.SocketServer;

namespace IdleLikeAdventureServer.Handler
{
    public class RegisterHandler : BaseHandler
    {
        public RegisterHandler()
        {
            OpCode = OpCodeModule.User;
            OpCodeOperation = (byte)OpCodeUserOperation.Register;
        }

        public override void OnOperationRequest(BaseMsgData baseMsgData, SendParameters sendParameters, ClientPeer peer)
        {
            MyGameServer.log.Info("创建新账号");
            RegisterRequestMsgData registerRequestMsgData = baseMsgData as RegisterRequestMsgData;

            if (registerRequestMsgData == null)
            {
                MyGameServer.log.Info("注册信息数据为NULL");
                return;
            }

            Account account = null;
            UserMsgData userMsgData = new UserMsgData();
            //参数合法性检测
            if (!CheckAccountName(registerRequestMsgData.Account))
            {
                //命名不合法
                userMsgData.IsError = true;
                userMsgData.Error = ErrorCode.RegisterAccountError;
                return;
            }
            if (!CheckPassword(registerRequestMsgData.Password))
            {
                //密码不合法
                userMsgData.IsError = true;
                userMsgData.Error = ErrorCode.RegisterPasswordError;
                return;
            }
            //检查用户名是否有重名            
            account = serverDataCenter.AccountDal.Get(registerRequestMsgData.Account);
            if (account != null)
            {
                //有重名
                userMsgData.IsError = true;
                userMsgData.Error = ErrorCode.RegisterAccountExist;
                MyGameServer.log.Info("创建了相同的用户名称： " + registerRequestMsgData.Account);
                return;
            }

            account = new Account();
            account.Name = registerRequestMsgData.Account;
            account.Password = registerRequestMsgData.Password;
            account.CreateDate = DateTime.Now;
            account.UpdateDate = DateTime.Now;
            int userId = serverDataCenter.AccountDal.Insert(account);         

            userMsgData.DatabaseID = userId;
            userMsgData.Name = account.Name;
            userMsgData.CreateTime = account.CreateDate;
            RegisterRespondeMsgData registerRespondeMsgData = new RegisterRespondeMsgData();
            registerRespondeMsgData.userData = userMsgData;

            SendResponse(peer, sendParameters, registerRespondeMsgData);
            MyGameServer.log.Info("创建账户成功，账户ID：" + userId);
        }

        private bool CheckPassword(string password)
        {
            return true;
        }

        private bool CheckAccountName(string account)
        {
            return true;
        }
    }
}
