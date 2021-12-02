using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("[controller]")]
public class PokemonController : ControllerBase
{
    private readonly IPokemonService _service;

    public PokemonController(IPokemonService service) => _service = service;

    [HttpGet("{pokemonName?}")]
    public async Task<ActionResult<Pokemon>> GetPokemonInfo(string? pokemonName)
    {
        if (pokemonName.IsNullOrWhiteSpace()) return BadRequest("please provide pokemon name");
        var res = await _service.GetPokemonInfoAsync(pokemonName);
        if (res is null) return NotFound();
        return res;
        
    }

    [HttpGet("translated/{pokemonName?}")]
    public async Task<ActionResult<Pokemon>> GetFunPokemonInfo(string? pokemonName)
    {
        if (pokemonName.IsNullOrWhiteSpace()) return BadRequest("please provide pokemon name");
        var res = await _service.GetFunPokemonInfoAsync(pokemonName);
        if (res is null) return NotFound();
        return res;
    }
}
