using Core.DTOs;

namespace Core.Interfaces;

public interface IPokemonInfoProvider
{
    Task<Pokemon?> GetPokemonInfoAsync(string name);
}
