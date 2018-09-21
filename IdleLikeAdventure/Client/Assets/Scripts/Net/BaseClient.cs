using System;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using NetData.OpCode;
using NetData.Message;

namespace Net
{
    public interface IClient : IPhotonPeerListener
    {

        bool SendMessage(OpCodeModule opCode, Dictionary<byte, object> parameters);
    }
}

