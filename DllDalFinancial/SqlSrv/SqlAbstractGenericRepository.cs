using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Newtonsoft.Json;
using DllEntityLayer;

namespace DllDalFinancial
{
    public abstract class SqlAbstractGenericRepository<T> where T : class, IEntityBase
    {
        private readonly string _connectionString;

        protected SqlAbstractGenericRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected virtual string TableName => typeof(T).Name;

        public async Task<int> CreateAsync(T entity)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = $@"
                INSERT INTO {TableName} (data) 
                VALUES (@Data);
                SELECT SCOPE_IDENTITY();";

            var entityId = await db.ExecuteScalarAsync<int>(sql, new { Data = JsonConvert.SerializeObject(entity) });
            entity.Id = entityId;
            return entityId;
        }

        public async Task<T> GetAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = $@"
                SELECT data
                FROM {TableName}
                WHERE Id = @Id";

            var entityJson = await db.ExecuteScalarAsync<string>(sql, new { Id = id });
            return entityJson != null ? JsonConvert.DeserializeObject<T>(entityJson) : null;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = $"SELECT data FROM {TableName}";
            var entityJsons = await db.QueryAsync<string>(sql);
            var entities = new List<T>();

            foreach (var entityJson in entityJsons)
            {
                entities.Add(JsonConvert.DeserializeObject<T>(entityJson));
            }

            return entities;
        }

        public async Task UpdateAsync(int id, T entity)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = $@"
                UPDATE {TableName}
                SET data = @Data
                WHERE Id = @Id";

            await db.ExecuteAsync(sql, new { Id = id, Data = JsonConvert.SerializeObject(entity) });
        }

        public async Task DeleteAsync(int id)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            var sql = $@"
                DELETE FROM {TableName}
                WHERE Id = @Id";

            await db.ExecuteAsync(sql, new { Id = id });
        }
    }
}
