namespace TodoLibrary.DataAccess;

public interface ISqlDataAccess
{
    Task Execute<T>(string storedProcedure, T Parameters);
    Task<IEnumerable<T>> Query<T, U>(string storedProcedure, U parameters);
}