using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdleLikeAdventureServer.Domain
{
    public abstract class BaseDal
    {
        protected ISession session;

        protected BaseDal()
        {
            session = SessionManager.GetSession();
        }
    }
}
