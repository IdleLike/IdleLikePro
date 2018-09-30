using IdleLikeAdventureServer;
using NetData.Message;
using NetData.OpCode;
using Photon.SocketServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace IdleLikeAdventureServer.Handler
{
    public abstract class BaseHandler
    {
        public OpCodeModule OpCode;
        public byte OpCodeOperation;
        private static MemoryStream memoryStream = new MemoryStream();
        private static BinaryFormatter binaryFormatter = new BinaryFormatter();


        public abstract void OnOperationRequest
            (BaseMsgData baseMsgData,
            Photon.SocketServer.SendParameters sendParameters, 
            ClientPeer peer);

        protected void SendResponse(ClientPeer peer,
            Photon.SocketServer.SendParameters sendParameters, 
            BaseMsgData baseMsgData)
        {
            
            binaryFormatter.Serialize(memoryStream, baseMsgData);
            byte[] arr = memoryStream.ToArray();

            OperationResponse operationResponse = new OperationResponse();
            operationResponse.OperationCode = (byte)OpCode;
            operationResponse.Parameters = new Dictionary<byte, object>();
            operationResponse.Parameters.Add(OpCodeOperation, arr);
            peer.SendOperationResponse(operationResponse, sendParameters);
        }
    }
}
