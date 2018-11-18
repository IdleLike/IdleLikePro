using IdleLikeAdventureServer.Data.Entity;
using IdleLikeAdventureServer.Domain;
using IdleLikeAdventureServer.Domain.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdleLikeAdventureServer
{
    public class ServerDataCenter
    {
        private AccountDal accountDal;            //账号数据

        public AccountDal AccountDal { get => accountDal; set => accountDal = value; }


        //初始化数据
        public void InitDatas()
        {
            AccountDal = new AccountDal();

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
