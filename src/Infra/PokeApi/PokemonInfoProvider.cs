using Core.DTOs;
using Core.Interfaces;
using System.Net.Http.Json;
using System.Security.Cryptography;

namespace Infra.PokeApi;

public class PokemonInfoProvider : IPokemonInfoProvider
{
    //taken from https://pokeapi.co/api/v2/language
    const int engLangId = 9;

    private readonly HttpClient _httpClient;

    public PokemonInfoProvider(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<Pokemon?> GetPokemonInfoAsync(string name)
    {
        var url = @"https://beta.pokeapi.co/graphql/v1beta";

        var query = GetQuery(name);
        var body = new { query };

        var httpResponse = await _httpClient.PostAsJsonAsync(url, body);
        httpResponse.EnsureSuccessStatusCode();

        var typedResponse = await httpResponse.Content.ReadFromJsonAsync<PokeApiResponse>();

        if (typedResponse!.data.pokemon_v2_pokemonspecies.Length == 0) return null;

        var obj = typedResponse!.data.pokemon_v2_pokemonspecies.Single();

        var descriptions = obj.pokemon_v2_pokemonspeciesflavortexts;
        var randomDescription = descriptions[RandomNumberGenerator.GetInt32(descriptions.Length)];

        return new Pokemon(obj.name, randomDescription.flavor_text, obj.pokemon_v2_pokemonhabitat.name, obj.is_legendary);
    }

    //can't use string.Format or `$` because of brackets. it becomes really ugly
    private string GetQuery(string pokemonName) => @"
query {
  pokemon_v2_pokemonspecies: pokemon_v2_pokemonspecies(where: {name: {_eq: """ + pokemonName +  @"""}}) {
    name
    is_legendary
    pokemon_v2_pokemonhabitat {
      name
    }
    pokemon_v2_pokemonspeciesflavortexts(where: { language_id: { _eq: " + engLangId + @"} }) {
      flavor_text
    }
  }
}
";
}
