using System;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Net.NetEnum;

namespace Net
{
    public class GameClient : BaseClient
    {
        private ClientState clientState = ClientState.DisConnect;
        private static PhotonPeer photonPeer = null;
        private static GameClient instance = null;
        //private static 

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="localhost">Localhost.</param>
        /// <param name="seerverName">Seerver name.</param>
        public static void CreateClient(string localhost, string seerverName)
        {
            if(instance != null)
            {
                Debug.Log("客户端已经创建");
                return;
            }

            localhost = "127.0.0.1:4530";
            seerverName = "ChatServer";
            //参数检查


            GameClient listener = new GameClient();
            listener.clientState = ClientState.DisConnect;
            photonPeer = new PhotonPeer(listener, ConnectionProtocol.Tcp);
            photonPeer.Connect(localhost, seerverName);     //链接服务器
            Debug.Log("连接服务器中......");


            //启动异步客户端服务

            while (listener.clientState == ClientState.DisConnect)
            {
                photonPeer.Service();
            }
            Dictionary<byte, object> dict = new Dictionary<byte, object>();
            dict.Add(1, "username");
            dict.Add(2, "password");
            //向服务端发送请求
            photonPeer.OpCustom(1, dict, true);
            while (true)
            {
                photonPeer.Service();
            }

            
        }


        public override void DebugReturn(DebugLevel level, string message)
        {

        }

        public override void OnEvent(EventData eventData)
        {

        }

        public override void OnOperationResponse(OperationResponse operationResponse)
        {

        }

        public override void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    clientState = ClientState.Connect;
                    break;
                case StatusCode.Disconnect:
                    clientState = ClientState.DisConnect;
                    break;
                default:
                    break;
            }
        }


    }
}

