using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using DllEntityLayer;

namespace DllDalFinancial
{
    public abstract class SqlAbstractGenericRepository<T> where T : class, IEntityBase
    {
        private readonly IDbConnection _dbConnection;
        public SqlAbstractGenericRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbConnection.GetListAsync<T>();
        }

        public async Task<T> GetByIdAsync(long? id)
        {
            if (id == null)
            {
                throw new ArgumentException("The id cannot be null.");
            }

            try
            {
                var tableName = typeof(T).Name;
                var sql = $"SELECT * FROM {tableName} WHERE Id = @Id";
                var result = await _dbConnection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id });

                if (result == null)
                {
                    throw new InvalidOperationException($"No entity found with id {id}");
                }

                return result;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("An error occurred while retrieving the entity.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the entity.", ex);
            }
        }

        public async Task<T> InsertAsync<T>(T entity)
        {
            await CreateTableIfNotExists();

            var entityType = typeof(T);
            var properties = entityType.GetProperties()
                .Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var propertyNames = properties.Select(p => p.Name).ToList();
            var propertyValues = properties.ToDictionary(p => p.Name, p => p.GetValue(entity));

            var columnNames = string.Join(", ", propertyNames);
            var parameterNames = string.Join(", ", propertyNames.Select(name => "@" + name));
            var sql = $"INSERT INTO {entityType.Name} ({columnNames}) OUTPUT INSERTED.Id VALUES ({parameterNames});";

            var insertedId = await _dbConnection.QuerySingleAsync<long>(sql, propertyValues);

            var idProperty = entityType.GetProperty("Id");
            if (idProperty != null)
            {
                idProperty.SetValue(entity, insertedId);
            }

            return entity;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            var entityType = typeof(T);
            var idProperty = entityType.GetProperty("Id");

            if (idProperty == null)
            {
                throw new ArgumentException("The entity does not have an Id property.");
            }

            var idValue = idProperty.GetValue(entity);
            if (idValue == null)
            {
                throw new ArgumentException("The Id property cannot be null.");
            }

            var properties = entityType.GetProperties()
                .Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                .ToList();

            var propertyNames = properties.Select(p => p.Name).ToList();
            var propertyValues = properties.ToDictionary(p => p.Name, p => p.GetValue(entity));

            var setClause = string.Join(", ", propertyNames.Select(name => $"{name} = @{name}"));
            var sql = $"UPDATE {entityType.Name} SET {setClause} WHERE Id = @Id";

            propertyValues.Add("Id", idValue);

            var rowsAffected = await _dbConnection.ExecuteAsync(sql, propertyValues);

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(long? id)
        {
            var tableName = typeof(T).Name;
            var sql = $"DELETE FROM {tableName} WHERE Id = @Id";
            var rowsAffected = await _dbConnection.ExecuteAsync(sql, new { Id = id });

            return rowsAffected > 0;
        }

        public async Task<bool> CreateTableIfNotExists()
        {
            var tableName = typeof(T).Name;
            var properties = typeof(T).GetProperties();

            var columns = properties.Select(property =>
            {
                var columnName = property.Name;
                var sqlType = GetSqlType(property.PropertyType);
                return $"{columnName} {sqlType}";
            });

            var columnsString = string.Join(", ", columns);

            var sql = $"IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName) " +
                      $"BEGIN " +
                      $"CREATE TABLE {tableName} ({columnsString}) " +
                      $"END";

            try
            {
                await _dbConnection.ExecuteAsync(sql, new { TableName = tableName });
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear la tabla: {ex.Message}");
                return false;
            }
        }

        private string GetSqlType(Type propertyType)
        {
            if (propertyType == typeof(int) || propertyType == typeof(long))
                return "BIGINT";
            if (propertyType == typeof(string))
                return "NVARCHAR(MAX)";
            if (propertyType == typeof(DateTime))
                return "DATETIME";
            if (propertyType == typeof(bool))
                return "BIT";
            if (propertyType == typeof(decimal))
                return "DECIMAL(18, 2)";
            // Añadir más tipos según sea necesario

            throw new NotSupportedException($"Tipo de propiedad no compatible: {propertyType.Name}");
        }
    }
}
