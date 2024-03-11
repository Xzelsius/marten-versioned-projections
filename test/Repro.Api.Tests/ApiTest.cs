using Alba;

namespace Repro.Api.Tests;

[Collection(ApiTestCollection.Name)]
public abstract class ApiTest : IAsyncLifetime
{
    private readonly ApiFixture api;

    protected ApiTest(ApiFixture api)
    {
        this.api = api;
    }

    protected IAlbaHost Host => api.Host;

    public async Task InitializeAsync()
    {
        await api.Host.ResetDatabaseAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
