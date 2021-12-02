using Xunit;

namespace IntegrationTests;

public class PokedexTestBase : IClassFixture<PokedexWebApplicationFactory>
{
    protected readonly PokedexWebApplicationFactory _factory;

    public PokedexTestBase(PokedexWebApplicationFactory factory) => _factory = factory;
}
