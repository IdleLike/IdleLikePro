using System;
using SUIFW;
using NetData.OpCode;
using NetData.Message;
using System.Collections.Generic;
using UnityEngine;
using Log;

namespace Service
{
    public abstract class BaseService<OpCodeRequest, OpCodeEvent> where OpCodeRequest : struct where OpCodeEvent : struct
    {
    
        private Dictionary<byte, Action<BaseMsgData>> handlers = new Dictionary<byte, Action<BaseMsgData>>();

        private Dictionary<byte, Action<BaseMsgData>> receivers = new Dictionary<byte, Action<BaseMsgData>>();
        protected abstract OpCodeModule ServiceOpCode { get;}
        public abstract void Init();
        public virtual void AddNetListener()
        {
            GameService.Instance.RegisterNetMsgCenter(ServiceOpCode, OnOperationResponse);
        }

        #region Message
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
        protected BaseUIForm OpenUIForm(string uiFormName, object args)
        {
            //if (args != null || args.Length > 0) UIParams.SetParams(args);
            //打开界面
            BaseUIForm baseUIForms = UIManager.GetInstance().ShowUIForms(uiFormName);
            //刷新界面数据
            baseUIForms.UpdatePanel(args);

            return baseUIForms;
           
        }
        #endregion

        #region Net

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        /// <param name="msg">Message.</param>
        protected void SendNetMsg(OpCodeRequest opcode, BaseMsgData  msg)
        {
            GameService.Instance.SendNetMsg(ServiceOpCode,  Convert.ToByte(opcode), msg);
        }



        /// <summary>
        /// 注册本模块网络消息
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        /// <param name="handler">Handler.</param>
        protected void RegisterNetMsg(OpCodeRequest opcode, Action<BaseMsgData> handler)
        {
            if (handlers == null) handlers = new Dictionary<byte, Action<BaseMsgData>>();
            byte tempOpCode = Convert.ToByte(opcode);
            if(handlers.ContainsKey(tempOpCode))
            {
                LogWarning(GetType() + "/RegisterNetMsg()/ 已经注册本消息：" + opcode.ToString());
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
        protected void RemoveNetMsg(OpCodeRequest opcode)
        {
            byte tempOpCode = Convert.ToByte(opcode);
            if (handlers.ContainsKey(tempOpCode))
            {
                handlers.Remove(tempOpCode);
            }
        }

        /// <summary>
        /// 注册本模块网络推送消息回调
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        /// <param name="handler">Handler.</param>
        protected void RegisterNetEventMsg(OpCodeEvent opcode, Action<BaseMsgData> handler)
        {
            if (handlers == null) handlers = new Dictionary<byte, Action<BaseMsgData>>();
            byte tempOpCode = Convert.ToByte(opcode);
            if (receivers.ContainsKey(tempOpCode))
            {
                LogWarning(GetType() + "/RegisterNetMsg()/ 已经注册本消息：" + opcode.ToString());
            }
            else
            {
                handlers.Add(tempOpCode, handler);
            }
        }

        /// <summary>
        /// 移除本模块网络推送消息回调
        /// </summary>
        /// <param name="opcode">Opcode.</param>
        protected void RemoveNetEventMsg(OpCodeEvent opcode)
        {
            byte tempOpCode = Convert.ToByte(opcode);
            if (receivers.ContainsKey(tempOpCode))
            {
                handlers.Remove(tempOpCode);
            }
        }

        #endregion

        #region Log

        /// <summary>
        /// 普通消息
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="isAlwaysInput">If set to <c>true</c> 总会输出，即使设置禁用Log.</param>
        protected void Log(string msg,bool isAlwaysInput = false)
        {

            TLog.LogInput(msg, Level.Low, isAlwaysInput);
        }

        /// <summary>
        /// 警告消息
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="isAlwaysInput">If set to <c>true</c> 总会输出，即使设置禁用Log</param>
        protected void LogWarning(string msg, bool isAlwaysInput = false)
        {
            TLog.LogInput(msg, Level.Special, isAlwaysInput);
        }

        /// <summary>
        /// 严重错误消息
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="isAlwaysInput">If set to <c>true</c> 总会输出，即使设置禁用Log</param>
        protected void LogError(string msg, bool isAlwaysInput = false)
        {
            TLog.LogInput(msg, Level.High, isAlwaysInput);
        }

        #endregion

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