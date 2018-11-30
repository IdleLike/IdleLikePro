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
        /// <summary>
        /// 没有错误
        /// </summary>
        None,
        /// <summary>
        /// 登陆账号错误
        /// </summary>
        LoginAccountError,
        /// <summary>
        /// 登陆密码错误
        /// </summary>
        LoginPasswordError,
        /// <summary>
        /// 注册账号错误
        /// </summary>
        RegisterAccountError,
        /// <summary>
        /// 注册账号已存在
        /// </summary>
        RegisterAccountExist,
        /// <summary>
        /// 注册密码错误
        /// </summary>
        RegisterPasswordError,
        /// <summary>
        /// 创建玩家错误
        /// </summary>
        CreateUserError,
        /// <summary>
        /// 创建玩家名称存在
        /// </summary>
        CreateUserNameExit,
        /// <summary>
        /// 创建队伍名称存在
        /// </summary>
        CreateTeamNameExit,
        /// <summary>
        /// 创建角色名称存在
        /// </summary>
        CreateActorNameExit,
    }
}
