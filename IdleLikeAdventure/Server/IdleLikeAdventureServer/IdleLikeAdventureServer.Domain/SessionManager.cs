using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Cfg;

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
            Configuration configuration = new Configuration().Configure();
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
