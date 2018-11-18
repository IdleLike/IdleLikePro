﻿using System.Collections.Generic;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using System.IO;
using log4net.Config;
using IdleLikeAdventureServer.Handler;
using NetData.OpCode;
using System;

namespace IdleLikeAdventureServer
{
    //所有的server端 主类都要集成自applicationbase
    public class MyGameServer : ApplicationBase
    {
        public static readonly ILogger log = LogManager.GetCurrentClassLogger();

        private OpCodeModule OpCode;
        private Dictionary<OpCodeModule, Dictionary<byte, BaseHandler>> handlers = new Dictionary<OpCodeModule, Dictionary<byte, BaseHandler>>();
        private Dictionary<byte, BaseHandler> handlerDict = new Dictionary<byte, BaseHandler>();
        private List<ClientPeer> peerList = new List<ClientPeer>();//通过这个集合可以访问到所有客户端的peer，从而向任何一个客户端发送数据 
        private ServerDataCenter serverDataCenter;

        public Dictionary<OpCodeModule, Dictionary<byte, BaseHandler>> Handlers { get => handlers; set => handlers = value; }
        public Dictionary<byte, BaseHandler> HandlerDict { get => handlerDict; set => handlerDict = value; }
        public List<ClientPeer> PeerList { get => peerList; set => peerList = value; }
        public ServerDataCenter ServerDataCenter { get => serverDataCenter; set => serverDataCenter = value; }
        public OpCodeModule OpCode1 { get => OpCode; set => OpCode = value; }

        //当一个客户端请求链接的
        //我们使用peerbase，表示和一个客户端的链接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("一个客户端连接过来了。。。。");
            ClientPeer peer = new ClientPeer(initRequest);
            PeerList.Add(peer);
            return peer;
        }

        //初始化
        protected override void Setup()
        {
            //Instance = this;
            // 日志的初始化
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(
                                                                    Path.Combine(this.ApplicationRootPath, "bin_Win64"), "log");
            FileInfo configFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (configFileInfo.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);//让photon知道
                XmlConfigurator.ConfigureAndWatch(configFileInfo);//让log4net这个插件读取配置文件
            }

            log.Info("Setup Completed！");

            InitData();
            InitHandler();          
        }

        private void InitData()
        {
            log.Info("InitData Start");

            ServerDataCenter = new ServerDataCenter();
            ServerDataCenter.InitDatas();

            log.Info("InitData End");
        }

        public void InitHandler()
        {
            log.Info("InitHandler Start");

            LoginHandler loginHandler = new LoginHandler();
            HandlerDict.Add(loginHandler.OpCodeOperation, loginHandler);
            RegisterHandler registerHandler = new RegisterHandler();
            HandlerDict.Add(registerHandler.OpCodeOperation, registerHandler);

            Handlers.Add(OpCodeModule.User, HandlerDict);

            log.Info("InitHandler End");
        }
        //server端关闭的时候
        protected override void TearDown()
        {
            log.Info("服务器应用关闭了");
        }
    }
}
