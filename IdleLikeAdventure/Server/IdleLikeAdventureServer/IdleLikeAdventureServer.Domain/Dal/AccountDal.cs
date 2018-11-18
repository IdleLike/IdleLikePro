using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdleLikeAdventureServer.Data.Entity;

namespace IdleLikeAdventureServer.Domain.Dal
{
    public class AccountDal : BaseDal
    {
        private List<Account> accounts;       //在线玩家账号

        public IList<Account> GetAll()
        {
            return session.CreateQuery("from Account").List<Account>();
        }

        public Account Get(string name)
        {
            Account account = null;
            if (accounts != null)
            {
                account = accounts.Find(p => p.Name == account.Name);
            }

            if (account == null)
            {
                IList<Account> tempAccounts = session.CreateQuery("from Account a where a.Name=:Name").SetString("Name", name).List<Account>();
                if (tempAccounts != null && tempAccounts.Count > 0)
                {
                    account = tempAccounts[0];
                }
            }

            return account;
        }

        public int Insert(Account account)
        {
            int id = (int)session.Save(account);
            session.Flush();
            return id;
        }
    }
}
