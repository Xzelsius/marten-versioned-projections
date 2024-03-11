using Repro.Api.Domain;
using Repro.Api.Endpoints.Models;

namespace Repro.Api.Tests.Endpoints;

[Collection(ApiTestCollection.Name)]
public sealed class CreateTodoEndpointTest(ApiFixture api) : ApiTest(api)
{
    [Fact]
    public async Task Does_create_Todo()
    {
        var result = await Host.Scenario(x =>
        {
            x.Put
             .Json(new CreateTodoRequest("Lorem Ipsum"))
             .ToUrl("/todos");
        });

        var response = await result.ReadAsJsonAsync<CreateTodoResponse>();

        response.Should().NotBeNull();
        response!.Id.Should().NotBeEmpty();
        response.Description.Should().Be("Lorem Ipsum");

        await using var session = Host.DocumentSession();

        var todo = await session.LoadAsync<Todo>(response.Id);

        todo.Should().BeEquivalentTo(new Todo
        {
            Id = response.Id,
            Description = response.Description,
        });
    }
}
