using System.Collections;
using System.Collections.Generic;

namespace Church.Common.Cache
{
    public interface ICache
    {
        T Get<T>(object id);
        IList<T> GetMany<T>(ICollection ids);
        T Set<T>(T obj);
        void SetMany<T>(IList<T> objs);
        void DeleteAll<T>();
    }
}
