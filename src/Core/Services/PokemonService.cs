using Core.DTOs;
using Core.Interfaces;

namespace Core.Services;

public class PokemonService : IPokemonService
{
    readonly IPokemonInfoProvider _infoProvider;
    readonly ITranslator _translator;

    public PokemonService(IPokemonInfoProvider infoProvider, ITranslator translator) =>
        (_infoProvider, _translator) = (infoProvider, translator);

    public async Task<Pokemon?> GetFunPokemonInfoAsync(string name)
    {
        var pokemon = await _infoProvider.GetPokemonInfoAsync(name);
        if (pokemon is null) return null;

        if (pokemon.IsLegendary || pokemon.Habitat == "cave")
        {
            pokemon.Description = await _translator.TranslateToYodaAsync(pokemon.Description);
        }
        else
        {
            pokemon.Description = await _translator.TranslateToShakespeareAsync(pokemon.Description);
        }

        return pokemon;
    }

    public Task<Pokemon?> GetPokemonInfoAsync(string name) => _infoProvider.GetPokemonInfoAsync(name);
}
