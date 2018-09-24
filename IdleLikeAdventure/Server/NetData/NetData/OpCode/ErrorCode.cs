using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetData.OpCode
{
    /// <summary>
    /// 错误码
    /// </summary>
    public enum ErrorCode : uint
    {
        //  登陆注册
        LoginAccountError,              //登陆账号错误
        LoginPasswordError,             //登陆密码错误
        RegisterAccountError,           //注册账号错误
        RegisterAccountExist,           //注册账号已存在
        RegisterPasswordError,          //注册密码错误

    }
}
