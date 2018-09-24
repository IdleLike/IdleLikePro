using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace IdleLikeAdventureServer.SQLite
{
    public class SQLiteConnect
    {
        static SQLiteConnection conn;
        public static void SQLite()
        {
            ////创建一个数据库
            //SQLiteConnection.CreateFile("Idle.sqlite");
            ////创建连接字符串
            //conn = new SQLiteConnection("Data Source = Idle.sqlite;Version = 3;");
            ////数据库密码
            //conn.SetPassword("root");
            Console.WriteLine("进入1");

            using (conn = new SQLiteConnection(@"Data Source=test.db;Pooling=true;FailIfMissing=false"))
            {
                Console.WriteLine("进入");
                conn.SetPassword("root");
                conn.Open();
                string query = "select * from table1";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Clone();
            }

            //string sql = "SELECT * FROM userInfo";
            ////string conStr = "D:/sqlliteDb/document.db";
            //string connStr = @"Data Source=" + @"D:\sqlliteDb\document.db;Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10";
            //using (SQLiteConnection conn = new SQLiteConnection(connStr))
            //{
            //    //conn.Open();
            //    using (SQLiteDataAdapter ap = new SQLiteDataAdapter(sql, conn))
            //    {
            //        DataSet ds = new DataSet();
            //        ap.Fill(ds);

            //        DataTable dt = ds.Tables[0];
            //    }
            //}


        }

        public void Create()
        {
            //打开数据库
            conn.Open();
            //创建表语句
            string query = "create table table1 (id INTEGER,name VARCHAR)";
            //创建命令
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            //执行命令
            cmd.ExecuteNonQuery();
            //释放资源
            conn.Close();
        }
    }
}
