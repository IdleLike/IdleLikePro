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
        private MyGameServer gameServer;


        public ClientPeer(InitRequest initRequest) : base(initRequest)
        {
            gameServer = MyGameServer.Instance as MyGameServer;
        }

        //处理客户端断开链接的后续工作
        protected override void OnDisconnect(PhotonHostRuntimeInterfaces.DisconnectReason reasonCode, string reasonDetail)
        {
            gameServer.PeerList.Remove(this);
        }

        //处理客户端的请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            BaseHandler handler = null;
            Dictionary<byte, BaseHandler> handlerDic;
            gameServer.Handlers.TryGetValue((OpCodeModule)operationRequest.OperationCode, out handlerDic);
            MyGameServer.log.Info("operationRequest.Opcode:" + operationRequest.OperationCode);
            IEnumerator<KeyValuePair<byte, object>> parameters = operationRequest.Parameters.GetEnumerator();
            while (parameters.MoveNext())
            {
                bool isContains = handlerDic.TryGetValue(parameters.Current.Key, out handler);
                if (isContains)
                {
                    handler.OnOperationRequest((BaseMsgData)parameters.Current.Value, sendParameters, this);
                }

                //MyGameServer.log.Info("Key:" + parameters.Current.Key);
                //MyGameServer.log.Info("ValueType" + parameters.Current.Value.GetType().Name);
            }

            //foreach (var item in operationRequest.Parameters)
            //{
            //    bool isContains = handlerDic.TryGetValue(item.Key, out handler);
            //    if (isContains)
            //    {
            //        handler.OnOperationRequest((BaseMsgData)item.Value, sendParameters, this);
            //    }
            //}
        }
    }
}
