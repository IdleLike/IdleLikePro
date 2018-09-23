using System;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Net;
using NetData.OpCode;
using NetData.Message;

namespace Service
{   
    /// <summary>
    /// 网络服务类
    /// </summary>
    public class NetService : BaseService<NetData.OpCode.OpCodeNetOperation>
    {
        private IClient client;                                                          //客户端
        private Dictionary<byte, object> netMsgDataDic = new Dictionary<byte, object>();    //网络消息参数
        private Dictionary<OpCodeModule, Action<Dictionary<byte, object>>> responsers = new Dictionary<OpCodeModule, Action<Dictionary<byte, object>>>();

        protected override OpCodeModule ServiceOpCode
        {
            get
            {
                return OpCodeModule.Net;
            }
        }

        public override void Init()
        {
            //初始化客户端
            client = GameClient.instance.StartClient();
        }

        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="opCodeModule">Op code module.</param>
        /// <param name="opCodeOperation">Op code operation.</param>
        /// <param name="msg">Message.</param>
        public void SendNetMsg(OpCodeModule opCodeModule, byte opCodeOperation, BaseMsgData msg)
        {
            if (client == null)
            {
                Debug.LogError(GetType() + "/SendNetMsg()/ 客户端对象为NULL");
            }
            else
            {
                netMsgDataDic.Clear();
                netMsgDataDic.Add(opCodeOperation, msg);
                client.SendMessage(opCodeModule, netMsgDataDic);
            }
        }

        /// <summary>
        /// 注册网络消息
        /// </summary>
        /// <param name="opCodeModule">Op code module.</param>
        /// <param name="responser">Responser.</param>
        public void RegisterNetMsg(OpCodeModule opCodeModule, Action<Dictionary<byte, object>> responser)
        {
            if (responsers == null) responsers = new Dictionary<OpCodeModule, Action<Dictionary<byte, object>>>();

            if (responsers.ContainsKey(opCodeModule)) return;

            responsers.Add(opCodeModule, responser);
        }

        /// <summary>
        /// 移除网络消息
        /// </summary>
        /// <param name="opCodeModule">Op code module.</param>
        public void RemoveNetMsg(OpCodeModule opCodeModule)
        {
            if (responsers == null) return;
            if(responsers.ContainsKey(opCodeModule))
            {
                responsers.Remove(opCodeModule);
            }
        }


        private void OnOperationResponse(OpCodeModule opCodeModule, Dictionary<byte, object> parameter)
        {
            Action<Dictionary<byte, object>> responser = null;
            if(responsers.TryGetValue(opCodeModule, out responser))
            {
                responser(parameter);
            }
            else
            {
                Debug.LogError(GetType() + "/OnOperationResponse()/ 不能处理的消息类型： " + opCodeModule);
            }
        }

    }
}

