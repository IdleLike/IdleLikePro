using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using NetData.Message;
using NetData.OpCode;

namespace Net
{
    public class GameClient : MonoSingleton<GameClient>, IPhotonPeerListener
    {
        private ClientState m_ClientState = ClientState.DisConnect;
        private PhotonPeer m_PhotonPeer = null;
        private Action<OpCodeModule, Dictionary<byte, object>> m_Handler;
        //private static 

        /// <summary>
        /// Creates the client.
        /// </summary>
        /// <param name="localhost">Localhost.</param>
        /// <param name="seerverName">Seerver name.</param>
        public void StartClient(string localhost, string seerverName)
        {
            if(instance != null)
            {
                Debug.Log("客户端已经创建");
                return;
            }

            //参数检查
            this.m_ClientState = ClientState.DisConnect;
            m_PhotonPeer = new PhotonPeer(this, ConnectionProtocol.Tcp);
            m_PhotonPeer.Connect(localhost, seerverName);     //链接服务器
            Debug.Log("连接服务器中......");


            //启动异步客户端服务
            StartCoroutine(StartReceive());

        }


        public void DebugReturn(DebugLevel level, string message)
        {

        }

        public void OnEvent(EventData eventData)
        {

        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            if(m_Handler != null)
            {
                m_Handler((OpCodeModule)operationResponse.OperationCode, operationResponse.Parameters);
            }
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    m_ClientState = ClientState.Connect;
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