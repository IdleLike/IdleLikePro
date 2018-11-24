﻿using NetData.OpCode;
using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegisterPanel : BaseUIForm
{
    public class RegisterViewModel
    {
        public Action<string, string, ushort> RegisterCallBack;
    }

    public InputField m_Input_Email;
    public InputField m_Input_SetPassword;
    public InputField m_Input_ConfirmPassword;
    public Dropdown m_Dropdown_SelectServer;
    public Button m_Btn_Register;
    public Text m_Text_EmailErrorOrRegisterOrNull;
    public Text m_Text_PasswordNull;
    public Text m_Text_PasswordNullOrDissimilarity;

    //邮箱格式集合
    private List<string> m_EmailFormatList = new List<string>();
    private RegisterViewModel m_RegisterViewModel = null;
    private bool m_IsRegisterSuccess;

    private void Start()
    {
        ReceiveMessage("Register", ReceiveRegisterMessage);
        
        m_EmailFormatList.Add("qq.com");
        m_EmailFormatList.Add("163.com");
        m_EmailFormatList.Add("sina.com");
        m_EmailFormatList.Add("sina.cn");

        m_Btn_Register.onClick.AddListener(OnRegisterCallBack);
    }
    
    private void ReceiveRegisterMessage(KeyValuesUpdate kv)
    {
        Log("m_ErrorMessage");
        string m_ErrorMessage = kv.Values.ToString();
        switch (kv.Key)
        {
            case "RegisterAccountError":
                m_Text_EmailErrorOrRegisterOrNull.text = m_ErrorMessage;
                StartCoroutine(DisableAfterTwoSeconds(m_Text_EmailErrorOrRegisterOrNull));
                m_IsRegisterSuccess = false;
                break;
            case "RegisterAccountExist":
                m_Text_EmailErrorOrRegisterOrNull.text = m_ErrorMessage;
                StartCoroutine(DisableAfterTwoSeconds(m_Text_EmailErrorOrRegisterOrNull));
                m_IsRegisterSuccess = false;
                break;
            case "RegisterPasswordError":
                m_Text_PasswordNull.text = m_ErrorMessage;
                StartCoroutine(DisableAfterTwoSeconds(m_Text_PasswordNull));
                m_IsRegisterSuccess = false;
                break;
            default:
                break;
        }
        m_IsRegisterSuccess = true;
    }

    /// <summary>
    /// 注册按钮回调
    /// </summary>
    public void OnRegisterCallBack()
    {
        if (!EmailNullChecked(m_Text_EmailErrorOrRegisterOrNull, m_Input_Email)
            || !PasswordChecked(m_Text_PasswordNull, m_Input_SetPassword)
            || !PasswordChecked(m_Text_PasswordNullOrDissimilarity, m_Input_ConfirmPassword, m_Input_SetPassword))
        {
            Log("注册失败");
            return;
        }



        m_RegisterViewModel.RegisterCallBack(m_Input_Email.text, m_Input_SetPassword.text, (ushort)m_Dropdown_SelectServer.value);

        if (!m_IsRegisterSuccess)
        {
            Log("注册失败111");
            return;
        }


        //Destroy(gameObject);
        Log("注册成功");
       
      
    }
    /// <summary>
    /// 邮箱格式检测
    /// </summary>
    /// <returns></returns>
    private bool EmailFormatChecked()
    {
        string m_Email = m_Input_Email.text;
        int m_Pos = m_Email.IndexOf("@");
        if (m_Pos > 0)
        {
            string m_EmailFormat = m_Email.Substring(m_Pos + 1, m_Email.Length - 1 - m_Pos) ;
      
            for (int i = 0; i < m_EmailFormatList.Count; i++)
            {
                if (m_EmailFormat == m_EmailFormatList[i])
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// 邮箱检测
    /// </summary>
    /// <param name="go"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    private bool EmailNullChecked(Text go, InputField email)
    {
        if (email.text == "" || email.text == String.Empty)
        {
            go.gameObject.SetActive(true);
            go.text = "账号不能为空！";
        }
        else if (!EmailFormatChecked())
        {
            go.text = "邮箱格式错误！";
            go.gameObject.SetActive(true);
        }
        else
        {
            go.gameObject.SetActive(false);
            return true;
        }
        //yield return new WaitForSeconds(2);
        //go.gameObject.SetActive(false);
        StartCoroutine(DisableAfterTwoSeconds(go));
        return false;
    }
    /// <summary>
    /// 密码检测
    /// </summary>
    /// <param name="go"></param>
    /// <param name="passWord1"></param>
    /// <param name="passWord2"></param>
    /// <returns></returns>
    private bool PasswordChecked(Text go, InputField passWord1, InputField passWord2 = null)
    {     
        if (passWord1.text == "" || passWord1.text == String.Empty)
        {
            go.gameObject.SetActive(true);
            go.text = "密码不能为空！";
        }
        else if(passWord1.text.Length < 6)
        {
            go.gameObject.SetActive(true);
            go.text = "密码不能少于6位数！";
        }
        else if (passWord1 == m_Input_ConfirmPassword && passWord1.text != passWord2.text)
        {
            go.gameObject.SetActive(true);
            go.text = "两次密码输入不一样！";
        }
        else
        {
            go.gameObject.SetActive(false);
            return true;
        }
        StartCoroutine(DisableAfterTwoSeconds(go));
        return false;
        //yield return new WaitForSeconds(2);
        //go.gameObject.SetActive(false);
    }
    /// <summary>
    /// 两秒后禁用
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    private IEnumerator DisableAfterTwoSeconds(Text go)
    {
        go.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        go.gameObject.SetActive(false);
    }


    public override void UpdatePanel(object viewModel)
    {
        m_RegisterViewModel = (RegisterViewModel)viewModel;
    }
}
