using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoLibrary.DataAccess;

namespace TodoApi.EndPoints;

public static class TodoEndpoints
{
    public static void AddTodoEndpoints(this WebApplication app)
    {
        app.MapGet("/api/Todos", GetAllTodos);
        app.MapPost("/api/Todos", CreateTodo).RequireAuthorization();
        app.MapDelete("/api/Todos/{id}", DeleteTodo);
        app.MapPut("/api/Todo/{id}/Complete", CompleteTodo);
    }

    private async static Task<IResult> GetAllTodos(ITodoData data, HttpContext httpContext)
    {
        var output = await data.GetAll(GetUserId(httpContext));
        return Results.Ok(output);
    }

    private async static Task<IResult> CreateTodo(ITodoData data, [FromBody] string task, HttpContext httpContext)
    {
        var output = await data.Create(GetUserId(httpContext), task);
        return Results.Ok(output);
    }

    private async static Task<IResult> DeleteTodo(ITodoData data, int id, HttpContext httpContext)
    {
        await data.Delete(GetUserId(httpContext), id);
        return Results.NoContent();
    }

    private async static Task<IResult> CompleteTodo(ITodoData data, int id, HttpContext httpContext)
    {
        await data.Complete(GetUserId(httpContext), id);
        return Results.NoContent();
    }

    private static string? GetUserId(HttpContext httpContext)
    {
        var userId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        return userId;
    }
}