using TodoLibrary.DataAccess;

namespace TodoApi.ServiceSetup;

public static class TodoServices
{
    public static void AddTodoServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<ITodoData, TodoData>();
    }
}
