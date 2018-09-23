using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using NetData.Message;
using NetData.OpCode;
using Log;
using HandlerCenterType = System.Action<NetData.OpCode.OpCodeModule,System.Collections.Generic.Dictionary<byte, object>>;

namespace Net
{
    public class GameClient : MonoSingleton<GameClient>, IClient
    {
        [SerializeField] private string m_Serverhost = "192.168.199.1";
        [SerializeField] private string m_ServerName = "IdleLikeAdventureServer";
        [SerializeField] private ConnectionProtocol m_DefaultConnectionProtocol = ConnectionProtocol.Tcp;

        private ClientState m_ClientState = ClientState.DisConnect;
        private PhotonPeer m_PhotonPeer = null;
        private HandlerCenterType m_ResponseHandlerCenter;
        private HandlerCenterType m_EventHandlerCenter;
        private Coroutine m_ReceiverCoroutine;


        //private static 

        /// <summary>
        /// Starts the client.
        /// </summary>
        /// <returns>The client.</returns>
        /// <param name="responseHandlerCenter">Response handler center.</param>
        /// <param name="eventHandlerCenter">Event handler center.</param>
        public IClient StartClient(HandlerCenterType responseHandlerCenter, HandlerCenterType eventHandlerCenter)
        {
            //参数检查
            if(responseHandlerCenter == null)
            {
                TLog.LogInput("服务器消息处理回调为NULL", Level.High, false);
                throw new Exception("服务器消息处理回调为NULL");
            }
            if(eventHandlerCenter == null)
            {
                TLog.LogInput("服务器推送消息处理回调为NULL", Level.High, false);
                throw new Exception("服务器推送消息处理回调为NULL");
            }

            //消息回调方法
            m_ResponseHandlerCenter = responseHandlerCenter;
            m_EventHandlerCenter = eventHandlerCenter;

            //创建客户端，连接服务器
            this.m_ClientState = ClientState.DisConnect;
            m_PhotonPeer = new PhotonPeer(this, m_DefaultConnectionProtocol);
            m_PhotonPeer.Connect(m_Serverhost, m_ServerName);     //链接服务器
            TLog.LogInput("连接服务器中", Level.Low, false);


            //启动异步客户端服务
            m_ReceiverCoroutine = StartCoroutine(StartReceive());


            return this;
        }


        public void DebugReturn(DebugLevel level, string message)
        {

        }

        public void OnEvent(EventData eventData)
        {
            TLog.LogInput("接收到服务器推送的消息", Level.Low, false);

            m_EventHandlerCenter((OpCodeModule)eventData.Code, eventData.Parameters);

        }

        public void OnOperationResponse(OperationResponse operationResponse)
        {
            TLog.LogInput("接收到服务器处理的返回消息", Level.Low, false);

            m_ResponseHandlerCenter((OpCodeModule)operationResponse.OperationCode, operationResponse.Parameters);
        }

        public void OnStatusChanged(StatusCode statusCode)
        {
            switch (statusCode)
            {
                case StatusCode.Connect:
                    m_ClientState = ClientState.Connect;
                    TLog.LogInput("连接上服务器", Level.Low, false);
                    break;
                case StatusCode.Disconnect:
                    TLog.LogInput("与服务器断开连接", Level.High, false);
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