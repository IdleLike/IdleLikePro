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
            using (conn = new SQLiteConnection("Data Source=Database.sqlite;Version=3;"))
            {
                conn.Open();
                string query = "select * from table1";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Clone();
            }
        }

        public static void CreateDataBase()
        {
            SQLiteConnection.CreateFile("Database.sqlite");
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
