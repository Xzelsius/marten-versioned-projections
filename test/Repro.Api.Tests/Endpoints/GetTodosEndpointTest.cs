using Repro.Api.Domain;
using Repro.Api.Domain.Events;

namespace Repro.Api.Tests.Endpoints;

[Collection(ApiTestCollection.Name)]
public sealed class GetTodosEndpointTest(ApiFixture api) : ApiTest(api)
{
    [Fact]
    public async Task Does_update_Todo()
    {
        var todoId1 = Guid.NewGuid();
        var todoId2 = Guid.NewGuid();

        await Host.SeedDataAsync(
            session =>
            {
                var created = new TodoCreated(todoId1, "Lorem Ipsum");
                session.Events.StartStream<Todo>(created.TodoId, created);
            },
            session =>
            {
                var created = new TodoCreated(todoId2, "Dolor Sit Amet");
                session.Events.StartStream<Todo>(created.TodoId, created);
            });

        var result = await Host.Scenario(x =>
        {
            x.Get
             .Url("/todos");
        });

        var response = await result.ReadAsJsonAsync<Todo[]>();

        response.Should().BeEquivalentTo(new Todo[]
        {
            new()
            {
                Id = todoId1,
                Description = "Lorem Ipsum",
            },
            new()
            {
                Id = todoId2,
                Description = "Dolor Sit Amet",
            }
        });
    }
}
