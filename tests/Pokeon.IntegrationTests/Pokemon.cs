using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using FluentAssertions;

using Pokemon;
using Application.Pokemon;
using Pokemon.Controllers;
using Application.Interface;

namespace Pokeon.IntegrationTests
{
    public class Tests
    {
        private static IServiceScopeFactory _scopeFactory;
        private static ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            var services = ConfigureServices();

            _serviceProvider = services.BuildServiceProvider();

            _scopeFactory = _serviceProvider.GetService<IServiceScopeFactory>();
        }

        private IServiceCollection ConfigureServices()
        {
            var configuration = BuildConfiguring();

            var startUp = new Startup(configuration);

            IServiceCollection services = new ServiceCollection();

            startUp.ConfigureServices(services);

            return services;
        }

        private IConfigurationRoot BuildConfiguring()
        {
            var builder = new ConfigurationBuilder();

            return builder.Build();
        }

        [Test]
        public async Task TestGetPokemonByName()
        {
            using var scope = _scopeFactory.CreateScope();

            var factory = _serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<PokemonController>();

            var pokemon = _serviceProvider.GetService<IPokemon>();

            var translatorFactory = _serviceProvider.GetService<ITranslatorFactory>();

            var controller = new PokemonController(logger, pokemon, translatorFactory);

            var response = await controller.GetPokemonByName("ditto", new CancellationToken());

            response.Should().NotBeNull();

            var okObjectResult = response.Result as OkObjectResult;

            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType(typeof(PokemonDto));

        }

        [Test]
        public async Task GetTranslatedPokemonByName()
        {
            using var scope = _scopeFactory.CreateScope();

            var factory = _serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<PokemonController>();

            var pokemon = _serviceProvider.GetService<IPokemon>();

            var translatorFactory = _serviceProvider.GetService<ITranslatorFactory>();

            var controller = new PokemonController(logger, pokemon, translatorFactory);

            var response = await controller.GetTranslatedPokemonByName("ditto", new CancellationToken());

            response.Should().NotBeNull();

            var okObjectResult = response.Result as OkObjectResult;

            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType(typeof(PokemonDto));
        }

    }
}