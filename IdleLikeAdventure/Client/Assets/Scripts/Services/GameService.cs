using System;
using System.Collections;
using System.Collections.Generic;
using Net;
using NetData.OpCode;
using NetData.Message;
using UnityEngine;
using Log;

namespace Service
{
    public class GameService : ClassSingleton<GameService>
    {

        private UserService userService;                //用户服务
        private ActorService actorService;              //角色服务
        private BattleService battleService;            //战斗服务
        private NetService NetService;                  //网络服务类

        public void Initialize()
        {
            //初始化所有服务类
            NetService = new NetService();
            userService = new UserService();
            actorService = new ActorService();
            battleService = new BattleService();

            NetService.Init();
        }

        /// <summary>
        /// 登陆逻辑
        /// </summary>
        public void Login()
        {
            userService.Login();
        }

        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="opCodeModule">Op code module.</param>
        /// <param name="opCodeOperation">Op code operation.</param>
        /// <param name="msg">Message.</param>
        public void SendNetMsg(OpCodeModule opCodeModule, byte opCodeOperation, BaseMsgData msg)
        {
            NetService.SendNetMsg(opCodeModule, opCodeOperation, msg);
        }
    }
}

