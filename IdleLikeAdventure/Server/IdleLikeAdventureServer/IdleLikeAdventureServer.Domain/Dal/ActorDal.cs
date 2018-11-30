using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdleLikeAdventureServer.Data.Entity;

namespace IdleLikeAdventureServer.Domain.Dal
{
    public class ActorDal : BaseDal
    {
        private List<Actor> actors;       //在线玩家账号

        public IList<Actor> GetAll()
        {
            return session.CreateQuery("from Actor").List<Actor>();
        }

        public Actor[] Get(params int[] actorIds)
        {
            if (actorIds.Length == 0) return null;

            List<Actor> tempActors = new List<Actor>();
            List<int> tempIds = new List<int>();
            Actor temp;
            for (int i = 0; i < actorIds.Length; i++)
            {
                temp = actors.Find(p => p.ID == actorIds[i]);
                if (temp != null) tempActors.Add(temp);
                else tempIds.Add(actorIds[i]);
            }

            if (tempIds.Count > 0)
            {
                for (int i = 0; i < tempIds.Count; i++)
                {
                    temp = session.Get<Actor>(tempIds[i]);
                    if (temp != null) tempActors.Add(temp);
                }
            }

            return tempActors.ToArray();
        }

        public int Insert(Account account)
        {
            int id = (int)session.Save(account);
            session.Flush();
            return id;
        }
    }
}
