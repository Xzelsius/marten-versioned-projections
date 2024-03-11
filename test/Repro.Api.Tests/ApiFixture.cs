using Alba;
using Oakton;

namespace Repro.Api.Tests;

public class ApiFixture : IAsyncLifetime
{
    private IAlbaHost? host;

    public IAlbaHost Host => host ?? throw new InvalidOperationException("Host not initialized");

    public async Task InitializeAsync()
    {
        // Repro.Api uses Oakton CLI
        OaktonEnvironment.AutoStartHost = true;

        host = await AlbaHost.For<Program>();
    }

    public async Task DisposeAsync()
    {
        if (host is not null)
        {
            await host.DisposeAsync();
        }
    }
}
