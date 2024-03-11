using Repro.Api.Domain;
using Repro.Api.Domain.Events;

namespace Repro.Api.Tests.Endpoints;

[Collection(ApiTestCollection.Name)]
public sealed class GetTodoEndpointTest(ApiFixture api) : ApiTest(api)
{
    [Fact]
    public async Task Does_update_Todo()
    {
        var todoId = Guid.NewGuid();

        await Host.SeedDataAsync(
            session =>
            {
                var created = new TodoCreated(todoId, "Lorem Ipsum");
                session.Events.StartStream<Todo>(created.TodoId, created);
            });

        var result = await Host.Scenario(x =>
        {
            x.Get
             .Url($"/todos/{todoId}");
        });

        var response = await result.ReadAsJsonAsync<Todo>();

        response.Should().BeEquivalentTo(new Todo
        {
            Id = todoId,
            Description = "Lorem Ipsum",
        });
    }
}
