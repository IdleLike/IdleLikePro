using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdleLikeAdventureServer.SQLite;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SQLiteConnect.SQLite();
            }
            catch(Exception e)
            {

                Console.WriteLine("出错了。。。。。" + e.Message);
            }


            Console.ReadLine();
        }
    }
}
