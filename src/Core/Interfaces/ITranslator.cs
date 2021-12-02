namespace Core.Interfaces;

public interface ITranslator
{
    Task<string> TranslateToYodaAsync(string text);
    Task<string> TranslateToShakespeareAsync(string text);
}
