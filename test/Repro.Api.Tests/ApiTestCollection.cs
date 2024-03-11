namespace Repro.Api.Tests;

[CollectionDefinition(Name)]
public class ApiTestCollection : ICollectionFixture<ApiFixture>
{
    public const string Name = nameof(ApiTestCollection);
}
