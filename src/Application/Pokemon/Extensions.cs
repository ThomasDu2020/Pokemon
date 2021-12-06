using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.ThirdPartyService;

namespace Application.Pokemon
{
    public static class Extensions
    {
        public static PokemonDto ConvertToDto(this PokemonSpecie pokemonSpecie)
        {
            var result = new PokemonDto();

            result.Name = pokemonSpecie.Name;
            result.Habitat = pokemonSpecie.Habitat?.Name;
            result.Description = pokemonSpecie.FlavorTextEntries?.FirstOrDefault(f => f.Language.Name == "en")?.FlavorText;
            result.IsLegendary = pokemonSpecie.IsLegendary;

            return result;
        }
    }
}
