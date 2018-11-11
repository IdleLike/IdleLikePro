using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleLikeAdventureServer.Domain
{
    public static class DataAccessManager
    {
        private static Dictionary<Type, object> typeToDal;

        static DataAccessManager()
        {
            typeToDal = new Dictionary<Type, object>();
        }

        internal static void Add<T>() where T : BaseDal , new()
        {   
            if (typeToDal.ContainsKey(typeof(T))) return;

            typeToDal.Add(typeof(T), new T());
        }

        public static T Get<T>() where T : BaseDal, new()
        {
            object result;
            if (typeToDal.TryGetValue(typeof(T), out result))
            {
                return result as T;
            }
            else
            {
                Add<T>();
                return typeToDal[typeof(T)] as T;
            }
        }
    }
}
