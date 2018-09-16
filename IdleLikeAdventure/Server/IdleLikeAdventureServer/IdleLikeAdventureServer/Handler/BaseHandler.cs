using IdleLikeAdventureServer;
using NetData.Message;
using NetData.OpCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdleLikeAdventureServer.Handler
{
    public abstract class BaseHandler
    {
        public OpCodeModule OpCode;
        public byte OpCodeOperation;


        public abstract void OnOperationRequest
            (BaseMsgData baseMsgData,
            Photon.SocketServer.SendParameters sendParameters, 
            ClientPeer peer);
    }
}
