using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdleLikeAdventureServer.Data.Entity;

namespace IdleLikeAdventureServer.Domain
{
    public class AccountDal : BaseDal
    {
        public IList<Account> Get()
        {
            return session.CreateQuery("from Account").List<Account>();
        }
    }
}
