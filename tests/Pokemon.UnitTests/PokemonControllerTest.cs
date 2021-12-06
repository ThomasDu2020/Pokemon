using System;
using System.Threading;
using System.Threading.Tasks;

using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;

using FluentAssertions;
using Moq;

using Pokemon;
using Application.Pokemon;
using Application.Translator;
using Pokemon.Controllers;
using Application.Interface;
using Application.ThirdPartyService.Interface;

namespace Pokemon.UnitTests
{
    public class PokemonControllerTest
    {
        private readonly ILogger<PokemonController> _logger;

        private readonly Mock<IPokemon> _pokemon;

        private readonly Mock<ITranslatorFactory> _tranlatorFactory;

        public PokemonControllerTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _logger = factory.CreateLogger<PokemonController>();

            _pokemon = new Mock<IPokemon>();

            _tranlatorFactory = new Mock<ITranslatorFactory>();
        }


        [Test]
        public async Task TestGetPokemonByName_Normal()
        {
            var pokemonDto = new PokemonDto()
            {
                Name = "ditto",
                Description = "I'm a ditto",
                Habitat = "urban",
                IsLegendary = true
            };

            _pokemon.Setup(p => p.GetPokemonByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(pokemonDto);

            var controller = new PokemonController(_logger, _pokemon.Object, _tranlatorFactory.Object);
            var response = await controller.GetPokemonByName("ditto", new CancellationToken());

            response.Should().NotBeNull();

            var okObjectResult = response.Result as OkObjectResult;

            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType(typeof(PokemonDto));

            var result = okObjectResult.Value as PokemonDto;

            result.Name.Should().Be("ditto");
        }

        [Test]
        public async Task TestGetPokemonByName_Abnormal()
        {
            var pokemonDto = new PokemonDto()
            {
                Name = "ditto",
                Description = "I'm a ditto",
                Habitat = "urban",
                IsLegendary = true
            };

            _pokemon.Setup(p => p.GetPokemonByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Throws<Exception>();

            var controller = new PokemonController(_logger, _pokemon.Object, _tranlatorFactory.Object);
            var response = await controller.GetPokemonByName("ditto", new CancellationToken());

            response.Should().NotBeNull();

            var result = response.Result as NotFoundResult;
            result.Should().NotBeNull();          
            
        }

        [Test]
        public async Task TestGetTranslatedPokemonByName_Normal()
        {
            var pokemonDto = new PokemonDto()
            {
                Name = "ditto",
                Description = "I'm a ditto",
                Habitat = "urban",
                IsLegendary = true
            };

            _pokemon.Setup(p => p.GetPokemonByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(pokemonDto);

            var funTrans = new Mock<IFunTranslation>();
            funTrans.Setup(f => f.TranslateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync("I'm a ditto translated by yoda");

            _tranlatorFactory.Setup(f => f.GetITranslator(It.Is<string>(s => s == "yoda"))).Returns(new YodaTranslator(funTrans.Object));

            var controller = new PokemonController(_logger, _pokemon.Object, _tranlatorFactory.Object);
            var response = await controller.GetTranslatedPokemonByName("ditto", new CancellationToken());

            response.Should().NotBeNull();

            var okObjectResult = response.Result as OkObjectResult;

            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType(typeof(PokemonDto));

            var result = okObjectResult.Value as PokemonDto;

            result.Description.Should().Be("I'm a ditto translated by yoda");


            //
            pokemonDto.IsLegendary = false;
            
            _tranlatorFactory.Setup(f => f.GetITranslator(It.Is<string>(s => s == "shakespeare"))).Returns(new ShakespeareTranslator(funTrans.Object));

            _pokemon.Setup(p => p.GetPokemonByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(pokemonDto);

            funTrans.Setup(f => f.TranslateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync("I'm a ditto translated by shakespeare");

            response = await controller.GetTranslatedPokemonByName("ditto", new CancellationToken());

            response.Should().NotBeNull();

            okObjectResult = response.Result as OkObjectResult;

            okObjectResult.Should().NotBeNull();
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeOfType(typeof(PokemonDto));

            result = okObjectResult.Value as PokemonDto;

            result.Description.Should().Be("I'm a ditto translated by shakespeare");
        }

        [Test]
        public async Task TestGetTranslatedPokemonByName_Abnormal()
        {
            _pokemon.Setup(p => p.GetPokemonByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new PokemonDto());

            var funTrans = new Mock<IFunTranslation>();

            funTrans.Setup(f => f.TranslateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .Throws<Exception>();

            _tranlatorFactory.Setup(f => f.GetITranslator(It.Is<string>(s => s == "yoda"))).Returns(new YodaTranslator(funTrans.Object));

            var controller = new PokemonController(_logger, _pokemon.Object, _tranlatorFactory.Object);
            var response = await controller.GetTranslatedPokemonByName("ditto", new CancellationToken());

            response.Should().NotBeNull();

            var okObjectResult = response.Result as NotFoundResult;

            okObjectResult.Should().NotBeNull();
        }
    }
}