using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace IntegrationTests;

public class PokedexWebApplicationFactory : WebApplicationFactory<Web.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(svc =>
        {
            var translatorMock = new Mock<ITranslator>(MockBehavior.Strict);
            translatorMock.Setup(x => x.TranslateToShakespeareAsync(It.IsAny<string>()))
                .ReturnsAsync("shakespeare translation");
            translatorMock.Setup(x => x.TranslateToYodaAsync(It.IsAny<string>()))
                .ReturnsAsync("yoda translation");

            svc.Replace(new ServiceDescriptor(typeof(ITranslator), translatorMock.Object));
        });
    }
}
