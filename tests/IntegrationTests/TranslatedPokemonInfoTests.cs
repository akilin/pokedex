using Core.DTOs;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests;

public class TranslatedPokemonInfoTests : PokedexTestBase
{
    public TranslatedPokemonInfoTests(PokedexWebApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task TranslatedPokemonInfo_WhenCalledForLegendaryPokemon_ShouldReturnYodaTranslation()
    {
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<Pokemon>("pokemon/translated/mewtwo");

        response.Should().NotBeNull();
        response!.Name.Should().Be("mewtwo");
        response!.Description.Should().Be("yoda translation");
        response!.IsLegendary.Should().BeTrue();
        response!.Habitat.Should().Be("rare");
    }

    [Fact]
    public async Task TranslatedPokemonInfo_WhenCalledForPokemonWhoLiveInACave_ShouldReturnYodaTranslation()
    {
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<Pokemon>("pokemon/translated/onix");

        response.Should().NotBeNull();
        response!.Name.Should().Be("onix");
        response!.Description.Should().Be("yoda translation");
        response!.IsLegendary.Should().BeFalse();
        response!.Habitat.Should().Be("cave");
    }

    [Fact]
    public async Task TranslatedPokemonInfo_WhenCalledForNonLegendaryPokemon_ShouldReturnShakespeareanTranslation()
    {
        var client = _factory.CreateClient();

        var response = await client.GetFromJsonAsync<Pokemon>("pokemon/translated/pikachu");

        response.Should().NotBeNull();
        response!.Name.Should().Be("pikachu");
        response!.Description.Should().Be("shakespeare translation");
        response!.IsLegendary.Should().BeFalse();
        response!.Habitat.Should().Be("forest");
    }

    [Fact]
    public async Task TranslatedPokemonInfo_WhenCalledForNotExistingPokemon_ShouldReturnNotFound()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("pokemon/translated/i-am-not-a-pokemon");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
