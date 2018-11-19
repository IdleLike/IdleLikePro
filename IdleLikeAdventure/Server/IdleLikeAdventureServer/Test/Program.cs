using System;
using IdleLikeAdventureServer.Domain;
using IdleLikeAdventureServer.Domain.Dal;

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

            

            foreach (var item in accountDal.GetAll())
            {
                Console.WriteLine("大大大");
                Console.WriteLine(item.ID + ":" + item.Name + ":" + item.Password + ":" + item.CreateDate + ":" + item.UpdateDate) ;
            }
            NetData.Tools.SerializeTool.RegisterMessage(registerTest);
            Console.ReadLine();
        }

        private static bool registerTest(Type t, byte code, Func<object, byte[]> se, Func<byte[], object> de)
        {
            Console.WriteLine(t.Name + ":" + code);
            return true;
        }
    }
}
