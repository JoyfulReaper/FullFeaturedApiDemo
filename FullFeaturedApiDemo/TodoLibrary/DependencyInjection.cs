using Microsoft.Extensions.DependencyInjection;
using TodoLibrary.DataAccess;

namespace TodoLibrary;
public static class DependencyInjection
{
    public static IServiceCollection AddTodoServices(this IServiceCollection services,
        Action<TodoOptions>? setupAction = null)
    {
        services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        services.AddSingleton<ITodoData, TodoData>();

        if (setupAction is not null)
        {
            services.Configure(setupAction);
        }

        return services;
    }
}
