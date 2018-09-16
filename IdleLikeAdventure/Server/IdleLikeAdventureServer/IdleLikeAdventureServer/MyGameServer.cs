using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using Photon.SocketServer;
using System.IO;

namespace IdleLikeAdventureServer
{
    //服务器框架主类 框架入口
    public class MyGameServer : Photon.SocketServer.ApplicationBase
    {

        //单例模式
        public static readonly ILogger LOG = LogManager.GetCurrentClassLogger();

        //当有客户端接入时候调用
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            LOG.Info("客户端连接");
            return new ClientPeer(initRequest);
        }


        //当框架启动时候调用
        protected override void Setup()
        {
            //ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            //设置配置文件属性
            log4net.GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(
                                                                Path.Combine(this.ApplicationRootPath, "bin_Win64"), "log");//设置日志文件存储目录
            //File.Create(log4net.GlobalContext.Properties[@"Photon:ApplicationLogPath"] + "/text.txt");
            //日志配置文件
            FileInfo logConfigFileInfo = new FileInfo(Path.Combine(this.BinaryPath, "log4net.config"));
            if (logConfigFileInfo.Exists)//配置文件存在
            {
                //设置Photon日志插件为Log4Next
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                //Log4Next这个插件读取配置文件
                XmlConfigurator.ConfigureAndWatch(logConfigFileInfo);
            }
            for (int i = 0; i < 1666; i++)
            {
                LOG.Info("123213");
            }

            LOG.Debug("服务器初始化完成");
        }

        //当框架停止时候调用
        protected override void TearDown()
        {
            LOG.Debug("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
    }
}
