using System;
using SUIFW;

namespace Service
{
    public abstract class BaseService
    {

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

    }
}