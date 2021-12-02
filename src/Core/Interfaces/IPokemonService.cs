using Core.DTOs;

namespace Core.Interfaces;

public interface IPokemonService
{
    Task<Pokemon?> GetPokemonInfoAsync(string name);
    Task<Pokemon?> GetFunPokemonInfoAsync(string name);
}
