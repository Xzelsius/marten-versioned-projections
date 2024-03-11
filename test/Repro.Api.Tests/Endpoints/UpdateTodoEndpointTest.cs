using Repro.Api.Domain;
using Repro.Api.Domain.Events;
using Repro.Api.Endpoints.Models;
using System.Net;

namespace Repro.Api.Tests.Endpoints;

[Collection(ApiTestCollection.Name)]
public sealed class UpdateTodoEndpointTest(ApiFixture api) : ApiTest(api)
{
    [Fact]
    public async Task Does_update_Todo()
    {
        var todoId = Guid.NewGuid();

        await Host.SeedDataAsync(
            session =>
            {
                var created = new TodoCreated(todoId, "Lorem");
                session.Events.StartStream<Todo>(created.TodoId, created);
            });

        await Host.Scenario(x =>
        {
            x.Post
             .Json(new UpdateTodoRequest("Lorem Ipsum"))
             .ToUrl($"/todos/{todoId}");

            x.StatusCodeShouldBe(HttpStatusCode.NoContent);
        });

        await using var session = Host.DocumentSession();

        var todo = await session.LoadAsync<Todo>(todoId);

        todo.Should().NotBeNull();
        todo!.Description.Should().Be("Lorem Ipsum");
    }
}
