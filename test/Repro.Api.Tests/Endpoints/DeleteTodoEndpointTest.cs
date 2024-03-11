using Repro.Api.Domain;
using Repro.Api.Domain.Events;
using System.Net;

namespace Repro.Api.Tests.Endpoints;

[Collection(ApiTestCollection.Name)]
public sealed class DeleteTodoEndpointTest(ApiFixture api) : ApiTest(api)
{
    [Fact]
    public async Task Does_delete_Todo()
    {
        var todoId = Guid.NewGuid();

        await Host.SeedDataAsync(
            session =>
            {
                var created = new TodoCreated(todoId, "Lorem Ipsum");
                session.Events.StartStream<Todo>(created.TodoId, created);
            });

        await Host.Scenario(x =>
        {
            x.Delete
             .Url($"/todos/{todoId}");

            x.StatusCodeShouldBe(HttpStatusCode.NoContent);
        });

        await using var session = Host.DocumentSession();

        var todo = await session.LoadAsync<Todo>(todoId);

        todo.Should().BeNull();
    }
}
