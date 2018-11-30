using IdleLikeAdventureServer.Data.Entity;
using IdleLikeAdventureServer.Domain;
using IdleLikeAdventureServer.Domain.Dal;
using StaticData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdleLikeAdventureServer
{
    public class ServerDataCenter
    {
        private StaticDataMgr staticDataMgr;

        private AccountDal accountDal;            //账号数据
        private ActorDal actorDal;                //角色数据
        private PlayerDal playerDal;              //玩家数据
        private TeamDal teamDal;                  //队伍数据

        public AccountDal AccountDal { get => accountDal; private set => accountDal = value; }
        public ActorDal ActorDal { get => actorDal; private set => actorDal = value; }
        public PlayerDal PlayerDal { get => playerDal; private set => playerDal = value; }
        public TeamDal TeamDal { get => teamDal; private set => teamDal = value; }
        public StaticDataMgr StaticDataMgr { get => staticDataMgr; set => staticDataMgr = value; }


        //初始化数据
        public void InitDatas()
        {
            //静态数据
            MyGameServer.log.Info("开始加载静态配置数据。");

            StaticDataMgr = StaticDataMgr.mInstance;
            StaticDataMgr.LoadData();

            MyGameServer.log.Info("加载静态配置数据完成。");

            //动态数据
            MyGameServer.log.Info("开始初始化动态数据。");

            AccountDal = new AccountDal();
            ActorDal = new ActorDal();
            PlayerDal = new PlayerDal();
            TeamDal = new TeamDal(); 

            MyGameServer.log.Info("初始化动态数据完成。");

            //测试数据库连接
            //TestConnectDataBase();
        }

        private void TestConnectDataBase()
        {
            //测试连接数据库代码
            MyGameServer.log.Info("++++++++++++++++++++++++++++++++++++++++");
            AccountDal accountDal = DataAccessManager.Get<AccountDal>();

            foreach (var item in accountDal.GetAll())
            {
                MyGameServer.log.Info("大大大");
                MyGameServer.log.Info(item.ID + ":" + item.Name + ":" + item.Password + ":" + item.CreateDate + ":" + item.UpdateDate);
            }
        }
    }
}
