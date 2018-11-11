using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NetData.Message;
using IdleLikeAdventureServer.Domain;
using IdleLikeAdventureServer.Data.Entity;

namespace Test
{
    class Program
    {
        //static void Main(string[] args)
        //{
        //    RegisterRequestMsgData registerRequestMsgData = new RegisterRequestMsgData();
        //    MemoryStream stream = new MemoryStream();

        //}

        ////测试数据库连接
        //static void Main(string[] args)
        //{
        //    try
        //    {
        //        string inputKey = Console.ReadLine();
        //        if (inputKey.Contains("a"))
        //        {
        //            SQLiteConnect.CreateDataBase();
        //            Console.WriteLine("创建数据库成功");
        //        }
        //        else
        //        {
        //            SQLiteConnect.SQLite();
        //        }

        //    }
        //    catch(Exception e)
        //    {

        //        Console.WriteLine("出错了。。。。。" + e.Message);
        //    }


        //    Console.ReadLine();
        //}

        //NHibernet连接测试
        static void Main(string[] args)
        {
            Console.WriteLine("++++++++++++++++++++++++++++++++++++++++");
            AccountDal accountDal = DataAccessManager.Get<AccountDal>();

            foreach (var item in accountDal.Get())
            {
                Console.WriteLine(item.ID + ":" + item.Name + ":" + item.Password + ":" + item.CreateDate + ":" + item.UpdateDate) ;
            }
        }
    }
}
