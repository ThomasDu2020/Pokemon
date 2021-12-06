using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;


using Newtonsoft.Json;

using Application.ThirdPartyService.Interface;

namespace Application.ThirdPartyService
{
    public class PokeApi : IPokeApi
    {
        const string Url = @"https://pokeapi.co/api/v2/pokemon-species/";

        private readonly HttpClient _httpClient;

        private readonly ILogger<PokeApi> _logger;

        public async Task<PokemonSpecie> GetPokemonSpecieAsync(string name, CancellationToken  token)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name is null or empty.");
            }

            name = name.Trim();

            var url = Url + name;

            PokemonSpecie result = null;
            try
            {
                var response = await _httpClient.GetStringAsync(url,token);

                result = JsonConvert.DeserializeObject<PokemonSpecie>(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e,"Get pokemon specie error");
            }

            return result;
        }

        public PokeApi(ILogger<PokeApi> logger ,HttpClient client)
        {
            _httpClient = client;
            _logger = logger;
        }
    }
}
