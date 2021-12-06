using System;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

using Moq;
using Moq.Contrib.HttpClient;
using FluentAssertions;
using Newtonsoft.Json;

using Application.Pokemon;
using Application.ThirdPartyService.Interface;
using Application.ThirdPartyService;



namespace Application.UnitTests
{
    public class ThirdPartyServiceTest
    {
        private readonly ILogger<PokeApi> _logger;

        public ThirdPartyServiceTest()
        {
            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            _logger = factory.CreateLogger<PokeApi>();
        }

        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task TestPokeApi_Abnormal()
        {
            var handler = new Mock<HttpMessageHandler>();
            var client = handler.CreateClient();

            var pokeApi = new PokeApi(_logger, client);

            Func<Task> func = async () => await pokeApi.GetPokemonSpecieAsync(null, new CancellationToken());

            var result = await func.Should().ThrowAsync<ArgumentException>();

            handler.SetupRequest(HttpMethod.Get, "https://pokeapi.co/api/v2/pokemon-species/xxxx")
                    .ReturnsResponse("");

            var response = await pokeApi.GetPokemonSpecieAsync("ditto", new CancellationToken());
            response.Should().BeNull();
        }

            [Test]
        public async Task TestPokeApi_Normal()
        {
            var handler = new Mock<HttpMessageHandler>();
            var client = handler.CreateClient();

            var pokeApi = new PokeApi(_logger, client);

            var flavorEntry = new PokemonSpecie.FlavorTextEntry()
            {
                FlavorText = "I'm a test pokemon",
                Language = new PokemonSpecie.NameUrlItem()
                {
                    Name = "en"
                }
            };

            var pokemonSpecie = new PokemonSpecie()
             {
                 Name = "ditto",
                 Habitat = new PokemonSpecie.NameUrlItem()
                 {
                     Name = "urban",
                     Url = "ddd"
                 },
                 IsLegendary = true,
                 FlavorTextEntries = new PokemonSpecie.FlavorTextEntry[]
                {
                   flavorEntry
                }
             };

            handler.SetupRequest(HttpMethod.Get, "https://pokeapi.co/api/v2/pokemon-species/ditto")
                    .ReturnsResponse(JsonConvert.SerializeObject(pokemonSpecie));

            var pokeSpeice = await pokeApi.GetPokemonSpecieAsync("ditto", new CancellationToken());
            pokeSpeice.Should().NotBeNull();
            pokeSpeice.Habitat.Name.Should().Be("urban");
        }

        [Test]
        public async Task FunTranslation_Abnormal()
        {
            var handler = new Mock<HttpMessageHandler>();
            var client = handler.CreateClient();

            var funTrans = new FunTranslations(_logger, client);

            Func<Task> func = async () => await funTrans.TranslateAsync("", "", new CancellationToken());

            await func.Should().ThrowAsync<ArgumentException>();

            handler.SetupRequest("https://api.funtranslations.com/translate/xxx").
                ReturnsResponse(JsonConvert.SerializeObject(""));

            var result = await funTrans.TranslateAsync("yoda", "yoda", new CancellationToken());

            result.Should().BeNull();
        }

            [Test]
        public async Task FunTranslation_Normal()
        {
            var handler = new Mock<HttpMessageHandler>();
            var client = handler.CreateClient();

            var funTrans = new FunTranslations(_logger, client);

            var funTransResponse = new FunTranslationResponse()
            {
                Success = new FunTranslationResponse.SuccessItem
                {
                    Total = 1
                },

                Contents = new FunTranslationResponse.ContentItem()
                {
                    Text = "yoda to translated",
                    Translated = "yoda translated",
                    Translation = "yoda"
                }
            };

            handler.SetupRequest("https://api.funtranslations.com/translate/yoda").
                ReturnsResponse(JsonConvert.SerializeObject(funTransResponse));

            var result = await funTrans.TranslateAsync("yoda", "yoda", new CancellationToken());

            result.Should().Be("yoda translated");
        }
    }
}