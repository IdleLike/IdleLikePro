using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;
using PhotonHostRuntimeInterfaces;

namespace IdleLikeAdventureServer
{
    public class ClientPeer : Photon.SocketServer.ClientPeer
    {
        public ClientPeer(InitRequest ir) : base(ir) { }

        //该客户端断开连接
        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {

        }

        //该客户端出操作请求
        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {

        }
    }
}
