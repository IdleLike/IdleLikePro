using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdleLikeAdventureServer.Data.Entity;

namespace IdleLikeAdventureServer.Domain.Dal
{
    public class PlayerDal : BaseDal
    {
        private List<Player> players;       //在线玩家账号

        public IList<Player> GetAll()
        {
            return session.CreateQuery("from Player").List<Player>();
        }

        public Player Get(string name)
        {
            Player player = null;
            if (players != null)
            {
                player = players.Find(p => p.Name == name);
            }

            if (player == null)
            {
                IList<Player> tempAccounts = session.CreateQuery("from Player a where a.Name=:Name").SetString("Name", name).List<Player>();
                if (tempAccounts != null && tempAccounts.Count > 0)
                {
                    player = tempAccounts[0];
                }
            }

            return player;
        }

        public Player Get(int accountID)
        {
            Player player = null;
            if (players != null)
            {
                player = players.Find(p => p.AccountID == accountID);
            }

            if (player == null)
            {
                IList<Player> tempAccounts = session.CreateQuery("from Player a where a.AccountID=:AccountID").SetInt32("AccountID", accountID).List<Player>();
                if (tempAccounts != null && tempAccounts.Count > 0)
                {
                    player = tempAccounts[0];
                }
            }

            return player;
        }

        public int Insert(Player player)
        {
            int id = (int)session.Save(player);
            session.Flush();
            return id;
        }
    }
}
