using System.Collections.Generic;
using Photon.SocketServer;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using System.IO;
using log4net.Config;
using IdleLikeAdventureServer.Handler;
using NetData.OpCode;

namespace IdleLikeAdventureServer
{
    //所有的server端 主类都要集成自applicationbase
    public class MyGameServer : ApplicationBase
    {
        public static readonly ILogger log = LogManager.GetCurrentClassLogger();

        //public static MyGameServer Instance
        //{
        //    get;
        //    private set;
        //}
        public OpCodeModule OpCode;
        public Dictionary<OpCodeModule, Dictionary<byte, BaseHandler>> handlers = new Dictionary<OpCodeModule, Dictionary<byte, BaseHandler>>();
        public Dictionary<byte, BaseHandler> handlerDict = new Dictionary<byte, BaseHandler>();

        public List<ClientPeer> peerList = new List<ClientPeer>();//通过这个集合可以访问到所有客户端的peer，从而向任何一个客户端发送数据 

        //当一个客户端请求链接的
        //我们使用peerbase，表示和一个客户端的链接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            log.Info("一个客户端连接过来了。。。。");
            ClientPeer peer = new ClientPeer(initRequest);
            peerList.Add(peer);
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


            InitHandler();
        }

        public void InitHandler()
        {
            LoginHandler loginHandler = new LoginHandler();
            handlerDict.Add(loginHandler.OpCodeOperation, loginHandler);
            RegisterHandler registerHandler = new RegisterHandler();
            handlerDict.Add(registerHandler.OpCodeOperation, registerHandler);

            handlers.Add(OpCodeModule.User, handlerDict);
        }
        //server端关闭的时候
        protected override void TearDown()
        {
            log.Info("服务器应用关闭了");
        }
    }
}
