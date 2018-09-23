/***
 * 
 *    Title: "SUIFW" UI框架项目
 *           主题: UI窗体的父类
 *    Description: 
 *           功能：定义所有UI窗体的父类。
 *           定义四个生命周期
 *           
 *           1：Display 显示状态。
 *           2：Hiding 隐藏状态
 *           3：ReDisplay 再显示状态。
 *           4：Freeze 冻结状态。
 *           
 *                  
 *    Date: 2017
 *    Version: 0.1版本
 *    Modify Recoder: 
 *    
 *   
 */
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Log;
using UnityEngine;

namespace SUIFW
{
	public abstract class BaseUIForm : MonoBehaviour {
        /*字段*/
        private UIType _CurrentUIType=new UIType();

        /* 属性*/
        //当前UI窗体类型
	    public UIType CurrentUIType
	    {
	        get { return _CurrentUIType; }
	        set { _CurrentUIType = value; }
	    }


        #region  窗体的四种(生命周期)状态

        /// <summary>
        /// 显示状态
        /// </summary>
	    public virtual void Display()
	    {
	        this.gameObject.SetActive(true);
            //设置模态窗体调用(必须是弹出窗体)
            if (_CurrentUIType.UIForms_Type==UIFormType.PopUp)
            {
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject,_CurrentUIType.UIForm_LucencyType);
            }
	    }

        /// <summary>
        /// 隐藏状态
        /// </summary>
	    public virtual void Hiding()
	    {
            this.gameObject.SetActive(false);
            //取消模态窗体调用
            if (_CurrentUIType.UIForms_Type == UIFormType.PopUp)
            {
                UIMaskMgr.GetInstance().CancelMaskWindow();
            }
        }

        /// <summary>
        /// 重新显示状态
        /// </summary>
	    public virtual void Redisplay()
	    {
            this.gameObject.SetActive(true);
            //设置模态窗体调用(必须是弹出窗体)
            if (_CurrentUIType.UIForms_Type == UIFormType.PopUp)
            {
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _CurrentUIType.UIForm_LucencyType);
            }
        }

        /// <summary>
        /// 冻结状态
        /// </summary>
	    public virtual void Freeze()
	    {
            this.gameObject.SetActive(true);
        }


        #endregion

        #region 封装子类常用的方法

        /// <summary>
        /// 注册按钮事件
        /// </summary>
        /// <param name="buttonName">按钮节点名称</param>
        /// <param name="delHandle">委托：需要注册的方法</param>
	    protected void RigisterButtonObjectEvent(string buttonName,EventTriggerListener.VoidDelegate  delHandle)
	    {
            GameObject goButton = UnityHelper.FindTheChildNode(this.gameObject, buttonName).gameObject;
            //给按钮注册事件方法
            if (goButton != null)
            {
                EventTriggerListener.Get(goButton).onClick = delHandle;
            }	    
        }


        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormName"></param>
     //   protected void OpenUIForm(string uiFormName, params object[] args)
	    //{
        //    if (args != null || args.Length > 0) UIParams.SetParams(args);
        //    UIManager.GetInstance().ShowUIForms(uiFormName);
        //}


        /// <summary>
        /// 关闭当前UI窗体
        /// </summary>
	    protected void CloseUIForm()
	    {
	        string strUIFromName = string.Empty;            //处理后的UIFrom 名称
	        int intPosition = -1;

            strUIFromName=GetType().ToString();             //命名空间+类名
            intPosition=strUIFromName.IndexOf('.');
            if (intPosition!=-1)
            {
                //剪切字符串中“.”之间的部分
                strUIFromName = strUIFromName.Substring(intPosition + 1);
            }

            UIManager.GetInstance().CloseUIForms(strUIFromName);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgType">消息的类型</param>
        /// <param name="msgName">消息名称</param>
        /// <param name="msgContent">消息内容</param>
	    protected void SendMessage(string msgType,string msgName,object msgContent)
	    {
            KeyValuesUpdate kvs = new KeyValuesUpdate(msgName,msgContent);
            MessageCenter.SendMessage(msgType, kvs);	    
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="messagType">消息分类</param>
        /// <param name="handler">消息委托</param>
	    protected void ReceiveMessage(string messagType,MessageCenter.DelMessageDelivery handler)
	    {
            MessageCenter.AddMsgListener(messagType, handler);
	    }

        /// <summary>
        /// 显示语言
        /// </summary>
        /// <param name="id"></param>
	    protected string Show(string id)
        {
            string strResult = string.Empty;

            strResult = LauguageMgr.GetInstance().ShowText(id);
            return strResult;
        }

        public abstract void UpdatePanel(object viewModel);

		/// <summary>
		/// 获取界面参数列表
		/// </summary>
		/// <returns>The parameters.</returns>
		protected object[] GetParams() { return UIParams.GetParams(); }
        /// <summary>
        /// 获取范型界面参数列表
        /// </summary>
        /// <returns>The parameters.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T[] GetParams<T>() { return UIParams.GetParams<T>(); }
        /// <summary>
        /// 获取单独的界面参数
        /// </summary>
        /// <returns>The single parameter.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        protected T GetSingleParam<T>(){ return UIParams.GetSingleParam<T>();}



        #endregion

        #region Log

        /// <summary>
        /// 普通消息
        /// </summary>
        /// <param name="msg">Message.</param>
        /// <param name="isAlwaysInput">If set to <c>true</c> 总会输出，即使设置禁用Log.</param>
        protected void Log(string msg, bool isAlwaysInput = false)
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
    }
}