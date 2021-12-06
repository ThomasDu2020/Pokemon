using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Application.Pokemon;

namespace Application.Interface
{
    public interface IPokemon
    {
        Task<PokemonDto> GetPokemonByNameAsync(string name, CancellationToken token);
    }
}
