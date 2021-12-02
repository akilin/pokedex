using Core.DTOs;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests;

public class PokemonInfoTests : PokedexTestBase
{
    public PokemonInfoTests(PokedexWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task PokemonInfo_WhenCalledForExistingPokemon_ShouldReturnCorrectData()
    {
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<Pokemon>("/pokemon/mewtwo");

        response.Should().NotBeNull();
        response!.Name.Should().Be("mewtwo");
        response!.Description.Should().NotBeEmpty();
        response!.IsLegendary.Should().BeTrue();
        response!.Habitat.Should().Be("rare");
    }

    [Fact]
    public async Task PokemonInfo_WhenCalledForNotExistingPokemon_ShouldReturnNotFound()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/pokemon/i-am-not-a-pokemon");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
