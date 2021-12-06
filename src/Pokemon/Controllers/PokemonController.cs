using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Pokemon;
using Application.Interface;


namespace Pokemon.Controllers
{
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;

        private readonly IPokemon _pokemon;

        private readonly ITranslatorFactory _translatorFactory;

        public PokemonController(ILogger<PokemonController> logger, IPokemon pokemon, ITranslatorFactory translatorFactory)
        {
            _logger = logger;
            _pokemon = pokemon;
            _translatorFactory = translatorFactory;
        }

        [HttpGet]
        [Route("pokemon/{name}")]
        public async Task<ActionResult<PokemonDto>> GetPokemonByName(string name, CancellationToken cancellationToken)
        {
            PokemonDto result;
            try
            {
                result = await _pokemon.GetPokemonByNameAsync(name, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get pokemon error.");

                result = null;
            }

            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        [Route("pokemon/translated/{name}")]
        public async Task<ActionResult<PokemonDto>> GetTranslatedPokemonByName(string name, CancellationToken cancellationToken)
        {
            PokemonDto result;
            try
            {
                result = await _pokemon.GetPokemonByNameAsync(name, cancellationToken);

                if (result != null)
                {
                    ITranslator translator;

                    if (result.Habitat == "cave" || result.IsLegendary)
                        translator = _translatorFactory.GetITranslator("yoda");
                    else
                        translator = _translatorFactory.GetITranslator("shakespeare");

                    var translatedDesc = await translator.TranslateAsync(result.Description, cancellationToken);

                    if (!string.IsNullOrEmpty(translatedDesc))
                    {
                        result.Description = translatedDesc;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Get translated pokemon error.");

                result = null;
            }

            return result == null ? NotFound() : Ok(result);
        }
    }
}
