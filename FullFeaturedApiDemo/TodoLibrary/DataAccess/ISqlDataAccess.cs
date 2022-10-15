namespace TodoLibrary.DataAccess;

public interface ISqlDataAccess
{
    Task<IEnumerable<T>> Query<T, U>(string storedProcedure, U parameters, string connectionStringName);
    Task Execute<T>(string storedProcedure, T Parameters, string connectionStringName);
}