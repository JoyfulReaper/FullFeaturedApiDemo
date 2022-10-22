namespace TodoLibrary.DataAccess;

public interface ITodoData
{
    Task Complete(string ownerId, int todoId);
    Task<Todo?> Create(string ownerId, string task);
    Task Delete(string ownerId, int todoId);
    Task<Todo?> Get(string ownerId, int todoId);
    Task<IEnumerable<Todo>> GetAll(string ownerId);
    Task UpdateTask(string ownerId, int todoId, string task);
}