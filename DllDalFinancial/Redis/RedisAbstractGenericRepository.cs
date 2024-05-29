using StackExchange.Redis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using DllEntityLayer;

namespace DllDalFinancial;

public abstract class RedisAbstractGenericRepository<T> where T : class, IEntityBase
{

    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    protected RedisAbstractGenericRepository(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _database = _redis.GetDatabase();
    }
 protected virtual string GetKey(int id) => $"{typeof(T).Name.ToLower()}:{id}";

    protected virtual string GetSetKey() => $"{typeof(T).Name.ToLower()}_ids";

    protected virtual string GetCounterKey() => $"{typeof(T).Name.ToLower()}_id_counter";

    public async Task<int> CreateAsync(T entity)
    {
        // Increment the counter to get a new ID
        int newId = (int)await _database.StringIncrementAsync(GetCounterKey());
        entity.Id = newId;

        string entityJson = JsonConvert.SerializeObject(entity);
        var key = GetKey(newId);
        await _database.HashSetAsync(key, new HashEntry[] { new HashEntry("data", entityJson) });
        await _database.SetAddAsync(GetSetKey(), newId);

        return newId;
    }

    public virtual async Task<T> GetAsync(int id)
    {
        var key = GetKey(id);
        var entityJson = await _database.HashGetAsync(key, "data");
        return entityJson.IsNull ? null : JsonConvert.DeserializeObject<T>(entityJson);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        var ids = await _database.SetMembersAsync(GetSetKey());
        List<T> entities = new List<T>();

        foreach (var id in ids)
        {
            var key = GetKey((int)id);
            var entityJson = await _database.HashGetAsync(key, "data");
            if (!entityJson.IsNull)
            {
                entities.Add(JsonConvert.DeserializeObject<T>(entityJson));
            }
        }

        return entities;
    }

    public virtual async Task UpdateAsync(int id, T entity)
    {
        string entityJson = JsonConvert.SerializeObject(entity);
        var key = GetKey(id);
        await _database.HashSetAsync(key, new HashEntry[] { new HashEntry("data", entityJson) });
    }

    public virtual async Task DeleteAsync(int id)
    {
        var key = GetKey(id);
        await _database.KeyDeleteAsync(key);
        await _database.SetRemoveAsync(GetSetKey(), id);
    }
}
