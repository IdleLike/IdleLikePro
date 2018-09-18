using System;
using SUIFW;
using NetData.OpCode;
using NetData.Message;
using System.Collections.Generic;
using UnityEngine;

namespace Service
{
    public abstract class BaseService<OpcodeT> where OpcodeT : struct
    {
    
        private Dictionary<byte, Action<BaseMsgData>> handlers = new Dictionary<byte, Action<BaseMsgData>>();
        protected abstract OpCodeModule ServiceOpCode { get;}
        public abstract void Init();
        public virtual void AddNetListener(){}

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType">消息的类型</param>
        /// <param name="msgName">消息名称</param>
        /// <param name="msgContent">消息内容</param>
        protected void SendMessage(string msgType, string msgName, object msgContent)
        {
            KeyValuesUpdate kvs = new KeyValuesUpdate(msgName, msgContent);
            MessageCenter.SendMessage(msgType, kvs);
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="messagType">消息分类</param>
        /// <param name="handler">消息委托</param>
        protected void ReceiveMessage(string messagType, MessageCenter.DelMessageDelivery handler)
        {
            MessageCenter.AddMsgListener(messagType, handler);
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="messagType">消息分类</param>
        /// <param name="handler">消息委托</param>
        protected void RemoveMessage(string messagType, MessageCenter.DelMessageDelivery handler)
        {
            MessageCenter.RemoveMsgListener(messagType, handler);
        }

        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormName"></param>
        protected void OpenUIForm(string uiFormName, object args)
        {
            //if (args != null || args.Length > 0) UIParams.SetParams(args);
            //打开界面
            BaseUIForm baseUIForms = UIManager.GetInstance().ShowUIForms(uiFormName);
            //刷新界面数据
            baseUIForms.UpdatePanel(args);
           
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        /// <param name="parameter">Parameter.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected void SendNetMsg(OpcodeT opcode, BaseMsgData  msg)
        {
            GameService.Instance.SendNetMsg(ServiceOpCode,  Convert.ToByte(opcode), msg);
        }



        /// <summary>
        /// 注册本模块网络消息
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        /// <param name="handler">Handler.</param>
        protected void RegisterNetMsg(OpcodeT opcode, Action<BaseMsgData> handler)
        {
            if (handlers == null) handlers = new Dictionary<byte, Action<BaseMsgData>>();
            byte tempOpCode = Convert.ToByte(opcode);
            if(handlers.ContainsKey(tempOpCode))
            {
                Debug.Log(GetType() + "/RegisterNetMsg()/ 已经注册本消息：" + opcode.ToString());
            }
            else
            {
                handlers.Add(tempOpCode, handler);
            }
        }

        /// <summary>
        /// 移除本模块网络消息
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        protected void RemoveNetMsg(OpcodeT opcode)
        {
            byte tempOpCode = Convert.ToByte(opcode);
            if (handlers.ContainsKey(tempOpCode))
            {
                handlers.Remove(tempOpCode);
            }
        }

        /// <summary>
        /// 处理本模块网络消息
        /// </summary>
        /// <param name="parameter">Parameter.</param>
        private void OnOperationResponse(Dictionary<byte, object> parameter)
        {
            Action<BaseMsgData> handler;
            foreach (var item in parameter)
            {
                if (handlers.TryGetValue(item.Key, out handler))
                {
                    handler(item.Value as BaseMsgData);
                }
            }
        }

    }
}