using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++");

            RegisterRequestMsgData registerRequestMsgData = baseMsgData as RegisterRequestMsgData;

            UserMsgData userMsgData = new UserMsgData();

            userMsgData.CreateTime = DateTime.Now;
            userMsgData.Name = "测试";
            userMsgData.DatabaseID = 100;
            RegisterRespondeMsgData registerRespondeMsgData = new RegisterRespondeMsgData();
            registerRespondeMsgData.userData = userMsgData;



            peer.SendMessage(userMsgData, sendParameters);
        }
    }
}
