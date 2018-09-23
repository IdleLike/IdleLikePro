using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using NetData.Message;
using NetData.OpCode;

namespace Net
{
    public class GameClient : MonoSingleton<GameClient>, IClient
    {
        [SerializeField] private string m_Serverhost = "192.168.199.1";
        [SerializeField] private string m_ServerName = "IdleLikeAdventureServer";

        private ClientState m_ClientState = ClientState.DisConnect;
        private PhotonPeer m_PhotonPeer = null;
        private Action<OpCodeModule, Dictionary<byte, object>> m_ResponseHandler;
        private Action<OpCodeModule, Dictionary<byte, object>> m_EventHandler;
        private Coroutine m_ReceiverCoroutine;
        //private static 

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="localhost">Localhost.</param>
        /// <param name="seerverName">Seerver name.</param>
        public IClient StartClient()
        {

            //参数检查
            this.m_ClientState = ClientState.DisConnect;
            m_PhotonPeer = new PhotonPeer(this, ConnectionProtocol.Tcp);
            m_PhotonPeer.Connect(m_Serverhost, m_ServerName);     //链接服务器
            Debug.Log("连接服务器中......");


            //启动异步客户端服务
            m_ReceiverCoroutine = StartCoroutine(StartReceive());


            return this;
        }


        public void DebugReturn(DebugLevel level, string message)
        {

        }

        public void OnEvent(EventData eventData)
        {

        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            Debug.Log("++++++++++有消息");

            if (m_ResponseHandler != null)
            {
                m_ResponseHandler((OpCodeModule)operationResponse.OperationCode, operationResponse.Parameters);
            }
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    m_ClientState = ClientState.Connect;
                    Debug.Log("++++++++++连接上");
                    break;
                case StatusCode.Disconnect:
                    m_ClientState = ClientState.DisConnect;
                    break;
                default:
                    break;
            }
        }

        public bool SendMessage(OpCodeModule opCode, Dictionary<byte, object> parameters)
        {
            return m_PhotonPeer.OpCustom((byte)opCode, parameters, true);
        }

        private IEnumerator StartReceive()
        {
            while(true)
            {
                m_PhotonPeer.Service();
                yield return 0;
            }

        }
    }
}