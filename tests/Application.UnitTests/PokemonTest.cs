using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using FluentAssertions;

using Application.Pokemon;
using Application.ThirdPartyService.Interface;
using Application.ThirdPartyService;

namespace Application.UnitTests
{
    public class PokemonTest
    {
        private readonly Mock<IPokeApi> _pokeApi;

        public PokemonTest()
        {
            _pokeApi = new Mock<IPokeApi>();
        }

        [SetUp]
        public void Setup()
        {
            _pokeApi.Setup(p => p.GetPokemonSpecieAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ThirdPartyService.PokemonSpecie());
        }

        [Test]
        public async Task TestPokemon_Null()
        {
            var pokemon = new PokemonFromThirdParty(_pokeApi.Object);

            var pokemonDto = await pokemon.GetPokemonByNameAsync("ditto", new CancellationToken());

            pokemonDto.Should().NotBeNull();
            pokemonDto.Habitat.Should().BeNull();

            _pokeApi.Setup(p => p.GetPokemonSpecieAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(() => null);
            pokemonDto = await pokemon.GetPokemonByNameAsync("ditto", new CancellationToken());

            pokemonDto.Should().BeNull();
        }

        [Test]
        public async Task TestPokemon_Normal()
        {
            var pokemon = new PokemonFromThirdParty(_pokeApi.Object);

            var flavorEntry = new PokemonSpecie.FlavorTextEntry()
            {
                FlavorText = "I'm a test pokemon",
                Language = new PokemonSpecie.NameUrlItem()
                {
                    Name = "en"
                }
            };

            var newSpecie = new PokemonSpecie()
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

            _pokeApi.Setup(p => p.GetPokemonSpecieAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(newSpecie);

            var pokemonDto = await pokemon.GetPokemonByNameAsync("ditto", new CancellationToken());

            pokemonDto.Should().NotBeNull();
            pokemonDto.Habitat.Should().NotBeNullOrEmpty();
            pokemonDto.Habitat.Should().Be("urban");
        }
    }
}