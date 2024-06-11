using StackExchange.Redis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using DllEntityLayer;
using DllDalFinancial.Interfaces;

namespace DllDalFinancial;
public abstract class RedisCrudRepository<T> : IGenericCrudRepository<T> where T : class, IEntityBase
{
    #region "Variables"
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    #endregion

    #region "Constructor"
    protected RedisCrudRepository(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _database = _redis.GetDatabase();
    }
    #endregion

    #region "Properties"
    protected virtual string GetKey(int id) => $"{typeof(T).Name.ToLower()}:{id}";

    protected virtual string GetSetKey() => $"{typeof(T).Name.ToLower()}_ids";

    protected virtual string GetCounterKey() => $"{typeof(T).Name.ToLower()}_id_counter";

    #endregion

    #region "PublicsMethods"
    public virtual async Task<T?> GetByID(int id)
    {
        var key = GetKey(id);
        var entityJson = await _database.HashGetAsync(key, "data");

        return entityJson.IsNull ? null : JsonConvert.DeserializeObject<T>(entityJson);
    }

    public virtual async Task<IEnumerable<T>> GetAll()
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

    public virtual async Task<int> Ins(T entity)
    {
        int newId = (int)await _database.StringIncrementAsync(GetCounterKey());
        entity.Id = newId;

        string entityJson = JsonConvert.SerializeObject(entity);
        var key = GetKey(newId);
        await _database.HashSetAsync(key, new HashEntry[] { new HashEntry("data", entityJson) });
        await _database.SetAddAsync(GetSetKey(), newId);
        return int.Parse(key);
    }

   public virtual async Task<bool> Upd(int id, T entity)
{
    try
    {
        entity.Id = id; // Ensure the entity's ID matches the one being updated
        string entityJson = JsonConvert.SerializeObject(entity);
        var key = GetKey(id);

        // Check if the entity exists before updating
        bool exists = await _database.KeyExistsAsync(key);
        if (!exists)
        {
            return false; // Entity doesn't exist
        }

        await _database.HashSetAsync(key, new HashEntry[] { new HashEntry("data", entityJson) });
        return true;
    }
    catch 
    {
        // Log the exception (consider using a logging library)
        //Console.WriteLine($"Error updating entity with ID {id}: {ex.Message}");
        return false;
    }
}

    public virtual async Task<bool> Del(int id)
    {
        var responce = true;
        try
        {
            var key = GetKey(id);
            await _database.KeyDeleteAsync(key);
            await _database.SetRemoveAsync(GetSetKey(), id);
        }
        catch
        {
            responce = false;
        }

        return responce;
    }
    #endregion
}