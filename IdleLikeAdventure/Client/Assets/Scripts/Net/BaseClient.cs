using System;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using NetData.OpCode;
using NetData.Message;

namespace Net
{
    public abstract class BaseClient : IPhotonPeerListener
    {
        public abstract void DebugReturn(DebugLevel level, string message);
        public abstract void OnEvent(EventData eventData);
        public abstract void OnOperationResponse(OperationResponse operationResponse);
        public abstract void OnStatusChanged(StatusCode statusCode);

        public abstract bool SendMessage(OpCodeModule opCode, Dictionary<byte, object> parameters);
    }
}

