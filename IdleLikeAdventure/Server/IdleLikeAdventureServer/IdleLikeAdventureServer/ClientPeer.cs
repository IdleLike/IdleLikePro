using IdleLikeAdventureServer.Handler;
using NetData.Message;
using NetData.OpCode;
using NetData.Tools;
using Photon.SocketServer;
using System.Collections.Generic;

namespace IdleLikeAdventureServer
{
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public float x, y, z;
        public string username;


        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {

        }

        //处理客户端断开链接的后续工作
        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            MyGameServer.Instance.peerList.Remove(this);
        }

        //处理客户端的请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseHandler handler = null;
            Dictionary<byte, BaseHandler> handlerDic;
            MyGameServer.Instance.handlers.TryGetValue((OpCodeModule)operationRequest.OperationCode, out handlerDic);
            foreach (var item in operationRequest.Parameters)
            {
                bool isContains = handlerDic.TryGetValue(item.Key, out handler);
                if (isContains)
                {
                    handler.OnOperationRequest((BaseMsgData)item.Value, sendParameters, this);
                }
            }
        }
    }
}
