using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (createUserRequestMsgData.UserName == null || createUserRequestMsgData.UserName == string.Empty)
            {//用户名称为空

            }
            else if (createUserRequestMsgData.TeamName == null || createUserRequestMsgData.TeamName == string.Empty)
            {//队伍名称为空

            }
            else if (createUserRequestMsgData.Actors == null || createUserRequestMsgData.Actors.Count != 3)
            {//角色数据错误

            }
            else
            {
                //创建数据

                //存储消息
                
                //初始化网络消息

            }


            //发送消息
            SendResponse(peer, sendParameters, createUserRespondeMsgData);
        }
    }
}
