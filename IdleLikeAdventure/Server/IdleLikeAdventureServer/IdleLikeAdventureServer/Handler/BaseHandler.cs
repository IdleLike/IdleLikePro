using IdleLikeAdventureServer;
using NetData.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.Handler
{
    public abstract class BaseHandler
    {
        public OpCodeModule OpCode;

        public abstract void OnOperationRequest
            (Photon.SocketServer.OperationRequest operationRequest, 
            Photon.SocketServer.SendParameters sendParameters, 
            ClientPeer peer);
    }
}
