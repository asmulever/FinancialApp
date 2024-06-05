using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;
using Newtonsoft.Json;
using DllEntityLayer;

namespace DllDalFinancial;

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

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbConnection.GetAsync<T>(id);
    }

    public async Task<int> InsertAsync(T entity)
    {
        return await _dbConnection.InsertAsync(entity);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        return await _dbConnection.UpdateAsync(entity);
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        return await _dbConnection.DeleteAsync(entity);
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
            // Manejar el error de creación de tabla
            Console.WriteLine($"Error al crear la tabla: {ex.Message}");
            return false;
        }
    }

    private string GetSqlType(Type propertyType)
    {
        // Mapea los tipos de propiedades de .NET a tipos de SQL
        if (propertyType == typeof(int))
            return "INT";
        else if (propertyType == typeof(string))
            return "NVARCHAR(MAX)";
        // Añade más mapeos de tipos según sea necesario

        throw new NotSupportedException($"Tipo de propiedad no compatible: {propertyType.Name}");
    }

}


