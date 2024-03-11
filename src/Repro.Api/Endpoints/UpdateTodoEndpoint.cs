using Marten;
using Microsoft.AspNetCore.Mvc;
using Repro.Api.Domain;
using Repro.Api.Domain.Events;
using Repro.Api.Endpoints.Models;
using System.ComponentModel.DataAnnotations;
using Wolverine.Http;

namespace Repro.Api.Endpoints;

public static class UpdateTodoEndpoint
{
    public static Task<Todo?> Load(Guid todoId, IDocumentSession session)
    {
        return session.LoadAsync<Todo>(todoId);
    }

    [Tags("Todo")]
    [WolverinePost("/todos/{todoId}"), EmptyResponse]
    public static void Update([FromRoute] Guid todoId, UpdateTodoRequest request, [Required] Todo todo, IDocumentSession session)
    {
        session.Events.Append(todo.Id, new TodoUpdated(todo.Id, request.Description));
    }
}
