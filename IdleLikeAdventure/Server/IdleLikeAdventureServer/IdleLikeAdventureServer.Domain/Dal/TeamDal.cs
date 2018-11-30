using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdleLikeAdventureServer.Data.Entity;

namespace IdleLikeAdventureServer.Domain.Dal
{
    public class TeamDal : BaseDal
    {
        private List<Team> teams;       //在线玩家账号

        public IList<Team> GetAll()
        {
            return session.CreateQuery("from Team").List<Team>();
        }

        public Team Get(string name, int playerId)
        {
            Team team = null;
            if (teams != null)
            {
                team = teams.Find(p => p.Name == name);
            }

            if (team == null)
            {
                IList<Team> tempAccounts = session.CreateQuery("from Team a where a.Name=:Name, a.PlayerID=:playerId").SetString("Name", name).List<Team>();
                if (tempAccounts != null && tempAccounts.Count > 0)
                {
                    team = tempAccounts[0];
                }
            }

            return team;
        }

        public Team Get(int teamId)
        {
            Team team = null;
            if (teams != null)
            {
                team = teams.Find(p => p.ID == teamId);
            }

            if (team == null)
            {
                team = session.Get<Team>(teamId);
            }

            return team;
        }

        public int Insert(Team team)
        {
            int id = (int)session.Save(team);
            session.Flush();
            return id;
        }
    }
}
