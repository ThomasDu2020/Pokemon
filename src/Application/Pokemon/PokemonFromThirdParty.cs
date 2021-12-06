using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Application.Interface;

using Application.ThirdPartyService.Interface;


namespace Application.Pokemon
{
    public class PokemonFromThirdParty : IPokemon
    {
        private readonly IPokeApi _pokeApi;

        public async Task<PokemonDto> GetPokemonByNameAsync(string name, CancellationToken token)
        {
            var pokemonSpecie = await _pokeApi.GetPokemonSpecieAsync(name, token);

            if (pokemonSpecie == null)
            {
                return null;
            }

            return pokemonSpecie.ConvertToDto();
        }

        public PokemonFromThirdParty(IPokeApi pokeApi)
        {
            _pokeApi = pokeApi;
        }
    }
}
