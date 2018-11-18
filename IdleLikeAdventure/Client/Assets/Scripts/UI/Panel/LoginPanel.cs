using SUIFW;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : BaseUIForm
{
    public class LoginViewModel
    {
        public UnityEngine.Events.UnityAction Btn_Action;
        public Action<string,string> LoginCallBack;
    }

    public InputField m_Input_Email;
    public InputField m_Input_Password;
    public Button m_Btn_Register;
    public Button m_Btn_Login;
    public Text m_Text_EmailNullOrError;
    public Text m_Text_PasswordNullOrError;


    //邮箱格式集合
    private List<string> m_EmailFormatList = new List<string>();
    private LoginViewModel m_LoginViewModel = null;
    private string m_ErrorMessage;

    private void Start()
    {
        ReceiveMessage("Login", ReceiveLoginMessage);

        m_EmailFormatList.Add("qq.com");
        m_EmailFormatList.Add("163.com");
        m_EmailFormatList.Add("sina.com");
        m_EmailFormatList.Add("sina.cn");


        m_Btn_Login.onClick.AddListener(OnLoginCallBack);
    }

    private void ReceiveLoginMessage(KeyValuesUpdate kv)
    {
        m_ErrorMessage = kv.Values.ToString();

        m_Text_EmailNullOrError.text = m_ErrorMessage;
        DisableAfterTwoSeconds(m_Text_EmailNullOrError);
       
    }

    /// <summary>
    /// 登录按钮回调
    /// </summary>
    public void OnLoginCallBack()
    {
        if(!EmailNullChecked(m_Text_EmailNullOrError, m_Input_Email) 
        || !PasswordChecked(m_Text_PasswordNullOrError, m_Input_Password))
        {
            Log("登录失败");
            return;
        }

        if (m_LoginViewModel != null)
        {
            m_LoginViewModel.LoginCallBack(m_Input_Email.text, m_Input_Password.text);
        }

        if (m_ErrorMessage != "" || m_ErrorMessage != string.Empty)
        {
            Log("登录失败");
            return;
        }
        Log("登录成功");

        Destroy(gameObject);
    }
    /// <summary>
    /// 邮箱格式检测
    /// </summary>
    /// <returns></returns>
    private bool EmailFormatChecked()
    {
        string m_Email = m_Input_Email.text;
        Log(1.ToString());
        int m_Pos = m_Email.IndexOf("@");
        Log(m_Pos.ToString());
        if (m_Pos > 0)
        {
            Log((m_Email.Length - 2).ToString());
            string m_EmailFormat = m_Email.Substring(m_Pos + 1, m_Email.Length - 1 - m_Pos);
            Log(m_EmailFormat);

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
    private bool PasswordChecked(Text go, InputField passWord1)
    {
        if (passWord1.text == "" || passWord1.text == String.Empty)
        {
            go.gameObject.SetActive(true);
            go.text = "密码不能为空！";
        }
        else if (passWord1.text.Length < 6)
        {
            go.gameObject.SetActive(true);
            go.text = "密码不能少于6位数！";
        }
        else
        {
            go.gameObject.SetActive(false);
            return true;
        }
        StartCoroutine(DisableAfterTwoSeconds(go));
        return false;
    }



    public override void UpdatePanel(object viewModel)
    {
        m_LoginViewModel = viewModel as LoginViewModel;
    }
    public void OnClickRegister()
    {
        //注册按钮添加事件
        m_LoginViewModel.Btn_Action();
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
}

