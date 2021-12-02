using Core.Interfaces;
using Infra.FunTranslations.Responses;
using Serilog;
using System.Net.Http.Json;

namespace Infra.FunTranslations;

public class FunTranslator : ITranslator
{
    HttpClient _httpClient;

    public FunTranslator(HttpClient httpClient) => _httpClient = httpClient;

    public Task<string> TranslateToShakespeareAsync(string text) =>
        TranslateAsync(text, "shakespeare");

    public Task<string> TranslateToYodaAsync(string text) =>
        TranslateAsync(text, "yoda");

    private async Task<string> TranslateAsync(string text, string dialect)
    {
        var url = $@"https://api.funtranslations.com/translate/{dialect}.json";

        var response = await _httpClient.PostAsJsonAsync(url, new { text });

        if (response.IsSuccessStatusCode)
        {
            var res = await response.Content.ReadFromJsonAsync<FunTranslationSuccessResponse>();
            return res!.contents.translated;
        }
        else
        {
            var res = await response.Content.ReadFromJsonAsync<FunTranslationErrorResponse>();
            Log.Error("Encountered an exception when attempting to translate. {@err}", res!.error);
            return text;
        }
    }
}
