using Core.DTOs;
using Core.Interfaces;
using Core.Services;
using FluentAssertions;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests;

public class FunTranslationTests
{
    Mock<IPokemonInfoProvider> _pokeInfoMock = new Mock<IPokemonInfoProvider>(MockBehavior.Strict);
    Mock<ITranslator> _translatorMock = new Mock<ITranslator>(MockBehavior.Strict);

    IPokemonService sut => new PokemonService(_pokeInfoMock.Object, _translatorMock.Object);

    [Fact]
    public async Task FunTranslation_WhenCalledForLegendaryPokemon_ShouldTranslateToYoda()
    {
        _pokeInfoMock.Setup(x => x.GetPokemonInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(new Pokemon("mewtwo", "eng description", "not-cave-habitat", isLegendary: true));
        _translatorMock.Setup(x => x.TranslateToYodaAsync(It.IsAny<string>()))
            .ReturnsAsync("yoda");

        var res = await sut.GetFunPokemonInfoAsync("mewtwo");

        res!.Description.Should().Be("yoda");
    }

    [Fact]
    public async Task FunTranslation_WhenCalledForNonLegendaryPokemonFromCaves_ShouldTranslateToYoda()
    {
        _pokeInfoMock.Setup(x => x.GetPokemonInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(new Pokemon("onix", "eng description", "cave", isLegendary: false));
        _translatorMock.Setup(x => x.TranslateToYodaAsync(It.IsAny<string>()))
            .ReturnsAsync("yoda");

        var res = await sut.GetFunPokemonInfoAsync("onix");

        res!.Description.Should().Be("yoda");
    }

    [Fact]
    public async Task FunTranslation_WhenCalledForNonLegendaryPokemonWhoIsNotFromCaves_ShouldTranslateToShakespearean()
    {
        _pokeInfoMock.Setup(x => x.GetPokemonInfoAsync(It.IsAny<string>()))
            .ReturnsAsync(new Pokemon("pikachu", "eng description", "forest", isLegendary: false));
        _translatorMock.Setup(x => x.TranslateToShakespeareAsync(It.IsAny<string>()))
            .ReturnsAsync("to be, or not to be");

        var res = await sut.GetFunPokemonInfoAsync("pikachu");

        res!.Description.Should().Be("to be, or not to be");
    }
}
