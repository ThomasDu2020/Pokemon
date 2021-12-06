using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ThirdPartyService.Interface
{
    public interface IPokeApi
    {
        Task<PokemonSpecie> GetPokemonSpecieAsync(string name, CancellationToken token);
    }
}
