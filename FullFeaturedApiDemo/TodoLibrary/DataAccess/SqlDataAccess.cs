using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace TodoLibrary.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly TodoOptions _options;

    public SqlDataAccess(IOptions<TodoOptions> options)
    {
        _options = options.Value;
    }

    public async Task<IEnumerable<T>> Query<T, U>(string storedProcedure, U parameters)
    {
        using IDbConnection connection = new SqlConnection(_options.ConnectionString);
        var rows = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);

        return rows.ToList();
    }

    public async Task Execute<T>(string storedProcedure, T Parameters)
    {
        using IDbConnection connection = new SqlConnection(_options.ConnectionString);
        await connection.ExecuteAsync(storedProcedure, Parameters, commandType: CommandType.StoredProcedure);
    }
}