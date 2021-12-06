using System;
using Microsoft.Extensions.DependencyInjection;

using Application.Interface;
using Application.ThirdPartyService;
using Application.Translator;
using Application.Pokemon;
using Application.ThirdPartyService.Interface;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddSingleton<ITranslatorFactory,TranslatorFactory>();
            services.AddTransient<IPokemon, PokemonFromThirdParty>();
            services.AddHttpClient<IPokeApi, PokeApi>();
            services.AddHttpClient<IFunTranslation, FunTranslations>();

            return services;
        }
    }
}
