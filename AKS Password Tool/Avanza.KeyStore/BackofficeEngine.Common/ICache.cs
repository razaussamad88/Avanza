#region Modification History
/************************** MODIFICATION HISTORY ****************************
Module: ICache.cs
Created By:
Created On: 09/03/2016
=============================================================================
Date        Updated By                    DESCRIPTION

*****************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avanza.Core.Caching
{
    public interface ICache
    {
        void Set(string key, string objectToCache,int minutes);
        void Set<T>(string key, T objectToCache, int minutes) where T : class;
         void SetQueryResult(string query, Dictionary<String, object> entities, int minutes);
        List<T> GetQueryResult<T>(string query);
        string Get(string key);
        T Get<T>(string key);
        bool ContainsKey(String key);
        bool ContainsValue(string tableName, int id);
        void Delete(string key);
        void Delete(string tableName,int id);
        void FlushAll();
        List<String> GetAllKeys();
        List<String> GetAllKeysOfTable(String tableName);
        Dictionary<String, T> GetAll<T>();
        Dictionary<String, T> GetAllOfTable<T>(String tableName);
        void DeleteAllOfTable<T>(String tableName);
        CacheStatus GetStatus();
   }
}
