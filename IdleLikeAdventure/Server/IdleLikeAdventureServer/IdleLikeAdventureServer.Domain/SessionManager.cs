using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace IdleLikeAdventureServer.Domain
{
    internal static class SessionManager
    {
        private static ISessionFactory sessionFactory;

        static SessionManager()
        {
            sessionFactory = GetSessionFactory();
        }

        private static ISessionFactory GetSessionFactory()
        {
            //// 读取配置
            //var configuration = new Configuration().Configure();

            //// 创建表结构
            //SchemaMetadataUpdater.QuoteTableAndColumns(configuration);
            //new SchemaExport(configuration).Create(false, true);

            //// 打开Session
            //var sessionFactory = configuration.BuildSessionFactory();

            //Console.WriteLine("完成");
            //Console.ReadKey();
       
        Configuration configuration = new Configuration().Configure("hibernate.cfg.xml");
        return configuration.BuildSessionFactory();
        //try
        //{

        //}
        //catch(Exception e)
        //{
        //    throw new Exception("创建SessionFactory失败");
        //}

    }

        internal static ISession GetSession()
        {
            return sessionFactory.OpenSession();
        }
    }
}
