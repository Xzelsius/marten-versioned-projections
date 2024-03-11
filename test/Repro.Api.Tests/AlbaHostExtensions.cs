using Alba;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace Repro.Api.Tests;

internal static class AlbaHostExtensions
{
    public static async Task ResetDatabaseAsync(this IAlbaHost host)
    {
        var store = host.Services.GetRequiredService<IDocumentStore>();
        var db = store.Storage.Database;

        await db.ApplyAllConfiguredChangesToDatabaseAsync();

        await db.DeleteAllEventDataAsync();
        await db.DeleteAllDocumentsAsync();
    }

    public static IDocumentSession DocumentSession(this IAlbaHost host)
        => host.Services.GetRequiredService<IDocumentStore>().LightweightSession();

    public static async Task SeedDataAsync(this IAlbaHost host, params Action<IDocumentSession>[] seeders)
    {
        await using var session = DocumentSession(host);

        foreach (var seed in seeders)
        {
            seed(session);
            await session.SaveChangesAsync();
        }
    }
}
