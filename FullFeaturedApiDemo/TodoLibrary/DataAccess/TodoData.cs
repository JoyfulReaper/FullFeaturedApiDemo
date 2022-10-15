using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoLibrary.DataAccess;

public class TodoData
{
	private readonly ISqlDataAccess _sqlDataAccess;

	public TodoData(ISqlDataAccess sqlDataAccess)
	{
		_sqlDataAccess = sqlDataAccess;
	}

	public Task<IEnumerable<Todo>> GetAll(string ownerId)
	{
		return _sqlDataAccess.Query<Todo, dynamic>("dbo.spTodo_GetAll", new
		{
			ownerId
		}, "TodoApiData");
	}

	public async Task<Todo?> Get(string ownerId, int todoId)
	{
		return (await _sqlDataAccess.Query<Todo, dynamic>("dbo.spTodo_Get", new
		{
			ownerId,
			todoId
		}, "TodoApiData"))
		.FirstOrDefault();
	}

	public async Task<Todo?> Create(string ownerId, string task)
	{
		return (await _sqlDataAccess.Query<Todo, dynamic>("dbo.spTodo_Create", new
		{
			ownerId,
			task
		}, "TodoApiData"))
		.FirstOrDefault();
	}

	public Task UpdateTask(string ownerId, int todoId, string task)
	{
		return _sqlDataAccess.Execute<dynamic>("dbo.spTodo_UpdateTask", new
		{
			ownerId,
			todoId,
			task
		}, "TodoApiData");
	}

	public Task Complete(string ownerId, int todoId)
	{
		return _sqlDataAccess.Execute<dynamic>("dbo.spTodo_Complete", new
		{
			ownerId,
			todoId
		}, "TodoApiData");
	}

	public Task Delete(string ownerId, int todoId)
	{
		return _sqlDataAccess.Execute<dynamic>("dbo.spTodo_Delete", new
		{
			ownerId,
			todoId
		}, "TodoApiData");
	}
}
